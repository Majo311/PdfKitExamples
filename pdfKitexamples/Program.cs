using System;
using System.IO;
using TallComponents.PDF;
using TallComponents.PDF.Shapes;

namespace pdfKitexamples
{
    public class Program
    {//wer
        static void Main(string[] args)
        {
            using (FileStream fileIn = new FileStream(Environment.CurrentDirectory + @"\sample1.pdf", FileMode.Open, FileAccess.Read))
            {
                Document pdfIn = new Document(fileIn);
                Document pdfOut = new Document();
                Page page = pdfIn.Pages[1]; // second page, they Pages array is ordered, and indexed 0-based 
                
                ShapeCollection shapes = page.CreateShapes();
                Page newPage = new Page(page.Width, page.Height);
                newPage.Overlay.Add(shapes);
                pdfOut.Pages.Add(newPage);
                SavePdfDocument(pdfOut, null);
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
