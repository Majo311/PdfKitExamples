using System;
using TallComponents.PDF.Shapes;


namespace pdfKitexamples.ShapeEditors
{
    public class TextShapeEditor : IShapeEditor
    {
        public string Type 
        {
            get { return "TextShape"; }
        }
        public Shape GetEditedShape(Shape shape)
        {
            return null;
        } 
    }
}
