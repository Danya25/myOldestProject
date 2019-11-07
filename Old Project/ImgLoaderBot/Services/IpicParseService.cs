using HtmlAgilityPack;
using System.IO;

namespace ConsoleApp2.Services
{
    internal sealed class IpicParseService
    {
        private readonly HtmlDocument _htmlDocument;

        public IpicParseService(HtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        public string ParseShortLinkToUploadedImage(Stream input)
        {
            _htmlDocument.Load(input);
            var node = _htmlDocument.DocumentNode.SelectSingleNode("(//div[@class='padded rounded']/p)[6]/input[3]");
            return node.Attributes["value"].Value;
        }
    }
}
