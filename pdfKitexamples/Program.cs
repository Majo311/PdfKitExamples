using System;
using System.IO;
using TallComponents.PDF;
using TallComponents.PDF.Shapes;
using System.Linq;

namespace pdfKitexamples
{
    public class Program
    {
        static void Main(string[] args)
        {
            ShapeCollection shapes = null;
            using (PdfFile pdfFileIn = PdfFile.Read(Environment.CurrentDirectory + @"\sample1.pdf"))
            {
                Page secondPage = pdfFileIn.Document.Pages[1];
                shapes = secondPage.CreateShapes();
                using (PdfFile pdfOut = new PdfFile())
                {
                    pdfOut.AddPage(shapes, secondPage.Width, secondPage.Height);
                    pdfOut.Save(null);
                }
            }       
            Console.ReadLine();

        }
    }
}
