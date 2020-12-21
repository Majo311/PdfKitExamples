using pdfKitexamples.ShapeEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TallComponents.PDF;
using TallComponents.PDF.Shapes;
using pdfKitexamples.ShapeEditors;
using System.Reflection;

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
        public void EditPage(int pageIndex)
        {
            ShapeCollection oldshapeCollection=this.Document.Pages[pageIndex].CreateShapes();
            EditShapes(oldshapeCollection);
        }

        public void InsertPage(int Index)
        {

        }
        void EditShapes(ShapeCollection shapes, int dpi=10)
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape shape = shapes[i];
                string typeName=shape.GetType().Name;
                if (shape is ShapeCollection)
                {
                    // recurse
                    EditShapes(shape as ShapeCollection, dpi);
                }
                else
                {
                    foreach (Type type in Helper.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "pdfKitexamples.ShapeEditors"))
                    {
                        if(type.Name.StartsWith(typeName))
                        {
                            shapes.RemoveAt(i);
                            pdfKitexamples.ShapeEditors.IShapeEditor IShapeEditor = (IShapeEditor)Activator.CreateInstance(type);
                            Shape editedShape= IShapeEditor.GetEditedShape(shape);
                            shapes.Insert(i,editedShape);
                        }
                    }
                }
            }
        }
        public Shape EditShape(Shape shape)
        {
            Shape editedShape = null;
            return editedShape;
        }
        private ImageShape downScale(ImageShape image, int dpi=10)
        {
            Matrix matrix = image.Transform.CreateGdiMatrix();
            PointF[] points = new PointF[]
            {
            new PointF(0, 0),
            new PointF((float)image.Width, 0),
            new PointF(0, (float)image.Height)
            };
            matrix.TransformPoints(points);

            // real dimensions of the image in points as it appears on the page
            float realWidth = distance(points[0], points[1]);
            float realHeight = distance(points[0], points[2]);

            // given the desired resolution, these are the desired number of cols/rows of the optimized image
            int desiredColumns = (int)(realWidth * ((float)dpi / 72f));
            int desiredRows = (int)(realHeight * ((float)dpi / 72f));

            // create the new image and copy the source image to it (resampling happens here)
            using (Bitmap bitmap = image.CreateBitmap())
            {
                if (desiredColumns > bitmap.Width) return image; // prevent upscale
                if (desiredRows > bitmap.Width) return image; // prevent upscale

                Bitmap optimizedBitmap = new Bitmap(desiredColumns, desiredRows, PixelFormat.Format32bppArgb);

                //draw the image so the pixels can be resampled
                using (Graphics graphics = Graphics.FromImage(optimizedBitmap))
                {
                    graphics.DrawImage(bitmap, 0, 0, desiredColumns, desiredRows);
                }

                //create new imageshape and keep all of the settings the same
                ImageShape optimized = new ImageShape(optimizedBitmap, true);
                optimized.Compression = Compression.Jpeg;
                optimized.Width = image.Width;
                optimized.Height = image.Height;
                optimized.Transform = image.Transform;

                optimized.Opacity = image.Opacity;
                optimized.BlendMode = image.BlendMode;
                optimized.Transform = image.Transform;

                return optimized;
            }
        }
        float distance(PointF a, PointF b)
        {
            return (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
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
