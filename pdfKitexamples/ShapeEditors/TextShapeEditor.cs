using System;
using TallComponents.PDF.Colors;
using TallComponents.PDF.Shapes;


namespace pdfKitexamples.ShapeEditors
{
    public class TextShapeEditor : IShapeEditor
    {
        public string Type 
        {
            get { return "TextShape"; }
        }
        public Shape GetEditedShape(Shape shape,dynamic objectValue)
        {
            TextShape textShape = shape as TextShape;
          //  textShape.Text = "This text was updated"; ;
            Shape newShape = UseDecoratedText(textShape);
            return newShape;
        } 

        public Shape UseDecoratedText(Shape shape)
        {
            // text with a vertical gradient
            var gradientText = new GradientText(x: 60, y: 500, width: 500, height: 68)
            {
                ColorPen = new RgbColor(0, 100, 0),
                ColorGradientTop = new RgbColor(240, 230, 140),
                ColorGradientBottom = new RgbColor(85, 107, 47)
            };
            Shape newShape=gradientText.GetDecoratedText("Twas brillig,");

            //// text with a vertical gradient and with a shadow
            //var shadowText = new ShadowText(x: 60, y: 400, width: 500, height: 68)
            //{
            //    ColorPen = new RgbColor(69, 65, 60),
            //    ColorGradientLight = new RgbColor(245, 245, 220),
            //    ColorGradientDark = new RgbColor(165, 42, 42)
            //};
            //page.Overlay.Add(shadowText.GetDecoratedText("and the slithy toves"));

            //// text embossed
            //var embossText = new EmbossText(x: 60, y: 300, width: 500, height: 48)
            //{
            //    ColorText = new RgbColor(72, 209, 204),
            //    ColorHighlighted = new RgbColor(224, 255, 255),
            //    ColorInShadow = new RgbColor(25, 25, 112)
            //};
            //page.Overlay.Add(embossText.GetDecoratedText("Did gyre and gimble"));

            //// text that glows
            //var glowText = new GlowText(x: 60, y: 200, width: 500, height: 68)
            //{
            //    ColorText = new RgbColor(224, 255, 255),
            //    ColorGlow = new RgbColor(73, 55, 109)
            //};
            //page.Overlay.Add(glowText.AddDecoratedTextShapes("in the wabe"));

            return newShape;
        }
    }
}
