using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TallComponents.PDF;
using TallComponents.PDF.Shapes;

namespace pdfKitexamples
{
    public class PdfFile:IDisposable
    {
        public Document Document { get; set; }
        public string Name 
        {
            get
            {
               return !String.IsNullOrEmpty(this.FullPath) ? this.FullPath.Split('\\').LastOrDefault().ToString():"";
            }
            set
            {
              if(String.IsNullOrEmpty(this.FullPath))
                {
                    this.FullPath = Environment.CurrentDirectory + @"\" + value;
                }
              else
                {
                    this.FullPath.Replace(this.FullPath.Split('\\').LastOrDefault(), value);
                }
            }
      
        }
        public string FullPath { get; private set; }
        public FileStream FileStream { get; set; }

       
        public PdfFile()
        {
            this.Document = new Document();
        }
        public static PdfFile Read(string Path)
        {
            PdfFile pdfFile = new PdfFile();
            FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read);
            pdfFile.FileStream = fs;
            pdfFile.FullPath = Path;
            pdfFile.Document = new Document(fs);
            return pdfFile;
        }
        public void AddPage(ShapeCollection shapes,double Width,double Height)
        {
            Page newPage = new Page(Width, Height);
            newPage.Overlay.Add(shapes);
            this.Document.Pages.Add(newPage);
        }
        public void Save(string Path)
        {
            Path = Path == null ? Environment.CurrentDirectory : Path;
            using (FileStream fileOut = new FileStream(Path + @"\out.pdf", FileMode.Create, FileAccess.Write))
            {
                this.Document.Write(fileOut);
            }
            Console.WriteLine("New pdf file was created!!!");
        }

        public void Dispose()
        {
            if(this.FileStream!=null)
            {
                this.FileStream.Close();
                this.FileStream.Dispose();
            }
        }
    }
}
