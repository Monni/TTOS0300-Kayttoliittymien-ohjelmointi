using iTextSharp.text;
using iTextSharp.text.pdf;
using MemberRegister;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WpfApp2
{
    class ExportToMailinglabel
    {
        public ExportToMailinglabel() { }
        public Boolean Export(List<jasenet> jasenetList)
        {
            Boolean success = false;

            try
            {
                List<jasenet> sortedList = jasenetList.OrderBy(o => o.postinumero).ToList();

                //Exporting to PDF
                string binPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string folderPath = Path.GetFullPath(Path.Combine(binPath, @"..\..\..\PDFs\"));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                String currentTime = DateTime.Now.ToString();
                using (FileStream stream = new FileStream(folderPath + "Postitustarrat-" + currentTime + ".pdf", FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();


                    foreach (jasenet jasen in sortedList)
                    {
                        // Luodaan tarvittavan pitkä pdfPTable
                        PdfPTable pdfTable1 = new PdfPTable(2);
                        pdfTable1.DefaultCell.Padding = 3;
                        pdfTable1.WidthPercentage = 30;
                        pdfTable1.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfTable1.DefaultCell.BorderWidth = 0;

                        PdfPTable pdfTable2 = new PdfPTable(3);
                        pdfTable2.DefaultCell.Padding = 3;
                        pdfTable2.WidthPercentage = 30;
                        pdfTable2.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfTable2.DefaultCell.BorderWidth = 0;

                        // Adding Header row
                        List<PdfPCell> headerList1 = new List<PdfPCell>();
                        headerList1.Add(new PdfPCell(new Phrase("Sukunimi")));
                        headerList1.Add(new PdfPCell(new Phrase("Etunimet")));
                        foreach (PdfPCell column in headerList1)
                        {
                            column.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                            pdfTable1.AddCell(column);
                        }
                        pdfTable1.AddCell(jasen.snimi);
                        pdfTable1.AddCell(jasen.enimet);

                        List<PdfPCell> headerList2 = new List<PdfPCell>();
                        headerList2.Add(new PdfPCell(new Phrase("Osoite")));
                        headerList2.Add(new PdfPCell(new Phrase("Postinumero")));
                        headerList2.Add(new PdfPCell(new Phrase("Postitoimipaikka")));
                        foreach (PdfPCell column in headerList2)
                        {
                            column.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                            pdfTable2.AddCell(column);
                        }
                        pdfTable2.AddCell(jasen.osoite);
                        pdfTable2.AddCell(jasen.postinumero.ToString());
                        pdfTable2.AddCell(jasen.postitoimipaikka);

                        // Create empty space in between
                        PdfPTable pdfTable3 = new PdfPTable(3);
                        PdfPCell cellBlankRow = new PdfPCell(new Phrase(" "));
                        pdfTable3.DefaultCell.Padding = 3;
                        pdfTable3.WidthPercentage = 30;
                        pdfTable3.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfTable3.DefaultCell.BorderWidth = 0;
                        pdfTable3.AddCell(" ");
                        pdfTable3.AddCell(" ");
                        pdfTable3.AddCell(" ");

                        pdfDoc.Add(pdfTable1);
                        pdfDoc.Add(pdfTable2);
                        pdfDoc.Add(pdfTable3);

                    }
                    // Close everything opened..
                    pdfDoc.Close();
                    stream.Close();
                }
            }
            catch (Exception)
            {
                // Klup. Something went wrong.
            }
            finally
            {
                success = true;
            }
            return success;
        }
    }
}
