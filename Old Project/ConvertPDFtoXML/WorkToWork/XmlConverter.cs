using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WorkToWork
{
    class XmlConverter
    {
        public async Task Convert(string text, string filePdf)
        {
            var lines = text.Split('\n');
            var dt = lines[6].Substring(3);
            var adoptionDate = lines[23].Split(' ').LastOrDefault();
            var imagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), filePdf);
            var line16 = lines[16].Split(' ');
            var notificationNumber = line16[1];
            var documentDt = line16[2];
            var operationCode = line16[4];
            var currencyCode = line16[5];
            var documentNumber = line16[line16.Length - 1];
            var sum = string.Join("", line16.Skip(6).Reverse().Skip(1).Reverse());
            XDocument xdoc = new XDocument();

            XElement document = new XElement("Document");

            XAttribute AdoptionDateXml = new XAttribute("AdoptionDate", adoptionDate);
            XAttribute DtXml = new XAttribute("Dt", dt);
            XAttribute ImagePathXml = new XAttribute("ImagePath", imagePath);

            XElement DocumentRow = new XElement("DocumentRow");

            XElement NotificationNumberXml = new XElement("NotificationNumber", notificationNumber);
            XElement DocumentDtXml = new XElement("Dt", documentDt);
            XElement OperationCodeXml = new XElement("OperationCode", operationCode);
            XElement CurrencyCodeXml = new XElement("CurrencyCode", currencyCode);
            XElement SumXml = new XElement("Sum", sum);
            XElement DocumentNumberXml = new XElement("DocumentNumber", documentNumber);

            DocumentRow.Add(NotificationNumberXml, DocumentDtXml, OperationCodeXml, CurrencyCodeXml, SumXml, DocumentNumberXml);
            document.Add(AdoptionDateXml, DtXml, ImagePathXml, DocumentRow);
            xdoc.Add(document);
            xdoc.Save(Path.GetFileNameWithoutExtension(filePdf) + ".xml");
        }
    }
}
