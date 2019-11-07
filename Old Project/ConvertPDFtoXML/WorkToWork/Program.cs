using iTextSharp.text.pdf.parser;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using SautinSoft;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WorkToWork.Model;

namespace WorkToWork
{
    class Program
    {
        public static List<string> nameFiles = new List<string>();
        static async Task Main(string[] args)
        {
            Console.WriteLine("Введите название файла: ");
            var path = Console.ReadLine();
            ConvertToPdf(path);
           await GetInformation();
        }
        public static void ConvertToPdf(string file)
        {
            PdfMetamorphosis pm = new PdfMetamorphosis();
            string pdfPath = "";
            if (pm != null)
            {
                string rtfPath = file;
                pdfPath = System.IO.Path.ChangeExtension(rtfPath, ".pdf");
                if (pm.RtfToPdfConvertFile(rtfPath, pdfPath) == 0)
                {
                    Console.WriteLine("Convert RTF to PDF");
                }
            }

            PdfDocument inputDocument = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Import);

            string name = System.IO.Path.GetFileNameWithoutExtension(pdfPath);
            for (int i = 0; i < inputDocument.PageCount; i++)
            {
                PdfDocument outputDocuemnt = new PdfDocument();
                outputDocuemnt.Version = inputDocument.Version;
                outputDocuemnt.Info.Title =
                    $"Page {i + 1} of {inputDocument.Info.Title}";
                outputDocuemnt.Info.Creator = inputDocument.Info.Creator;
                outputDocuemnt.AddPage(inputDocument.Pages[i]);
                var fileP = $"{name}-Page{i + 1}.pdf";
                nameFiles.Add(fileP);
                outputDocuemnt.Save(fileP);
                
            }
        }
        public static void ReadPdfToXml()
        {
            foreach(var filePdf in nameFiles )
            {
                StringBuilder sb = new StringBuilder();
                iTextSharp.text.pdf.PdfReader reader = 
                    new iTextSharp.text.pdf.PdfReader((System.IO.Path.Combine(Directory.GetCurrentDirectory()+$@"/{filePdf}")));
            }
        }
        public static async Task GetInformation()
        {
            foreach (var filePdf in nameFiles)
            {
                StringBuilder sb = new StringBuilder();
                string text = "";
                iTextSharp.text.pdf.PdfReader reader =
                    new iTextSharp.text.pdf.PdfReader((System.IO.Path.Combine(Directory.GetCurrentDirectory() + $@"/{filePdf}")));
                text = PdfTextExtractor.GetTextFromPage(reader, 1);
                XmlConverter xmlConv = new XmlConverter();
                await xmlConv.Convert(text, filePdf);
            }

        }
    }
}
