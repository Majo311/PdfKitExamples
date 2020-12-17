using System;
using System.IO;
using TallComponents.PDF;
using TallComponents.PDF.Shapes;

namespace pdfKitexamples
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (FileStream fileIn = new FileStream(Environment.CurrentDirectory + @"\sample1.pdf", FileMode.Open, FileAccess.Read))
            {
                Document pdfIn = new Document(fileIn);
                Document pdfOut = new Document();
                foreach (Page page in pdfIn.Pages)
                {
                    if (page.Index == 1)
                    {
                        ShapeCollection shapes = page.CreateShapes();
                        Page newPage = new Page(page.Width, page.Height);
                        newPage.Overlay.Add(shapes);
                        pdfOut.Pages.Add(newPage);
                        SavePdfDocument(pdfOut, null);
                    }
                }
            }
            Console.ReadLine();

        }
 
        public static void SavePdfDocument(Document document, string Path)
        {
            Path = Path == null ? Environment.CurrentDirectory : Path;
            using (FileStream fileOut = new FileStream(Path + @"\out.pdf", FileMode.Create, FileAccess.Write))
            {
                document.Write(fileOut);
            }
            Console.WriteLine("New pdf file was created!!!");
        }
    }
}
