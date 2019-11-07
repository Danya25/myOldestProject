using ConsoleApp2.Models;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Microsoft.Extensions.Logging;

namespace ConsoleApp2.Services
{
    internal sealed class IpicUploadService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<IpicUploadService> _logger;

        public IpicUploadService(ITelegramBotClient botClient,
            HttpClient httpClient,
            IServiceProvider serviceProvider,
            ILogger<IpicUploadService> logger)
        {
            _botClient = botClient;
            _httpClient = httpClient;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        internal async Task<IpicUploadResult> UploadImageToIpicAsync(string fileId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Running upload for file_id {0}", fileId);

            await using var fileStream = new MemoryStream();
            var file = await _botClient.GetInfoAndDownloadFileAsync(fileId, fileStream, cancellationToken);
            fileStream.Position = 0;
            var fileName = Path.GetFileName(file.FilePath);

            _logger.LogInformation("File name of {0} is {1}", fileId, fileName);

            cancellationToken.ThrowIfCancellationRequested();

            using var formData = new MultipartFormDataContent();

            using var fileContent = new StreamContent(fileStream);
            using var tnsize = new StringContent("150");
            using var name = new StringContent(fileName);
            using var quality = new StringContent("75");
            using var client = new StringContent("ipic.su");
            using var action = new StringContent("loadimg");
            using var link = new StringContent("/");

            formData.Add(fileContent, "image", fileName);
            formData.Add(tnsize, "tnsize");
            formData.Add(name, "name");
            formData.Add(quality, "quality");
            formData.Add(client, "client");
            formData.Add(action, "action");
            formData.Add(link, "link");

            using var response = await _httpClient.PostAsync("/", formData, cancellationToken);
            await using var responseStream = await response.Content.ReadAsStreamAsync();

            _logger.LogDebug("Response status code is {0}", response.StatusCode);

            using var scope = _serviceProvider.CreateScope();
            var ipicParseService = scope.ServiceProvider.GetRequiredService<IpicParseService>();

            return new IpicUploadResult
            {
                FileName = fileName,
                FileUrl = ipicParseService.ParseShortLinkToUploadedImage(responseStream)
            };
        }
    }
}
