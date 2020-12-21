using System;
using TallComponents.PDF.Shapes;

namespace pdfKitexamples.ShapeEditors
{
    interface IShapeEditor
    {
        string Type { get;}
        Shape GetEditedShape(Shape shape);
    }
}
