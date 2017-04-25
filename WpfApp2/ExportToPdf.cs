using iTextSharp.text;
using iTextSharp.text.pdf;
using MemberRegister;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WpfApp2
{
    class ExportToPdf
    {
        public ExportToPdf() {}
        public Boolean Export(List<jasenet> jasenetList)
        {
            Boolean success = false;
            
            List<jasenet> sortedList = jasenetList.OrderBy(o => o.snimi).ToList();

            try
            {
                // Luodaan tarvittavan pitkä pdfPTable
                PdfPTable pdfTable = new PdfPTable(11);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.DefaultCell.BorderWidth = 1;

                // Adding Header row
                List<PdfPCell> headerList = new List<PdfPCell>();
                headerList.Add(new PdfPCell(new Phrase("Hetu")));
                headerList.Add(new PdfPCell(new Phrase("Sukunimi")));
                headerList.Add(new PdfPCell(new Phrase("Etunimet")));
                headerList.Add(new PdfPCell(new Phrase("Osoite")));
                headerList.Add(new PdfPCell(new Phrase("Postinumero")));
                headerList.Add(new PdfPCell(new Phrase("Postitoimipaikka")));
                headerList.Add(new PdfPCell(new Phrase("Puhelinnumero")));
                headerList.Add(new PdfPCell(new Phrase("Email")));
                headerList.Add(new PdfPCell(new Phrase("Liittymispvm")));
                headerList.Add(new PdfPCell(new Phrase("Maksettu")));
                headerList.Add(new PdfPCell(new Phrase("Lisätietoa")));

                foreach (PdfPCell column in headerList)
                {
                    column.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                    pdfTable.AddCell(column);
                }
                
                // Adding DataRow
                foreach (jasenet jasen in sortedList)
                {
                    pdfTable.AddCell(jasen.hetu);
                    pdfTable.AddCell(jasen.snimi);
                    pdfTable.AddCell(jasen.enimet);
                    pdfTable.AddCell(jasen.osoite);
                    pdfTable.AddCell(jasen.postinumero.ToString());
                    pdfTable.AddCell(jasen.postitoimipaikka);
                    pdfTable.AddCell(jasen.puhelinnumero);
                    pdfTable.AddCell(jasen.email);
                    pdfTable.AddCell(jasen.liittymispv.ToString());
                    pdfTable.AddCell(jasen.maksettu.ToString());
                    pdfTable.AddCell(jasen.lisatietoa);

                }

                //Exporting to PDF
                string binPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string folderPath = Path.GetFullPath(Path.Combine(binPath, @"..\..\..\PDFs\"));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                String currentTime = DateTime.Now.ToString();
                using (FileStream stream = new FileStream(folderPath + "Namelist-" + currentTime + ".pdf", FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }
            } catch (Exception)
            {
                // Klup. Something went wrong..
            } finally
            {
                success = true;
            }

            return success;

        }
    }
}
