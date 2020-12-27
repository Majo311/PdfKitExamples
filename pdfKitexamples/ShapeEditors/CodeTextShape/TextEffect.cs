using System.IO;
using TallComponents.PDF;
using TallComponents.PDF.Brushes;
using TallComponents.PDF.Colors;
using TallComponents.PDF.Fonts;
using TallComponents.PDF.Shapes;
using TallComponents.PDF.Transforms;
using AxialGradientBrush = TallComponents.PDF.Brushes.AxialGradientBrush;
using Pen = TallComponents.PDF.Pens.Pen;
using SolidBrush = TallComponents.PDF.Brushes.SolidBrush;

//internal class AddTextField
//{
//    private static void Main(string[] args)
//    {
//        var document = new Document();
//        var page = new Page(PageSize.Letter);
//        document.Pages.Add(page);

//        // text with a vertical gradient
//        var gradientText = new GradientText(x: 60, y: 500, width: 500, height: 68)
//        {
//            ColorPen = new RgbColor(0, 100, 0),
//            ColorGradientTop = new RgbColor(240, 230, 140),
//            ColorGradientBottom = new RgbColor(85, 107, 47)
//        };
//        page.Overlay.Add(gradientText.GetDecoratedText("Twas brillig,"));

//        // text with a vertical gradient and with a shadow
//        var shadowText = new ShadowText(x: 60, y: 400, width: 500, height: 68)
//        {
//            ColorPen = new RgbColor(69, 65, 60),
//            ColorGradientLight = new RgbColor(245, 245, 220),
//            ColorGradientDark = new RgbColor(165, 42, 42)
//        };
//        page.Overlay.Add(shadowText.GetDecoratedText("and the slithy toves"));

//        // text embossed
//        var embossText = new EmbossText(x: 60, y: 300, width: 500, height: 48)
//        {
//            ColorText = new RgbColor(72, 209, 204),
//            ColorHighlighted = new RgbColor(224, 255, 255),
//            ColorInShadow = new RgbColor(25, 25, 112)
//        };
//        page.Overlay.Add(embossText.GetDecoratedText("Did gyre and gimble"));

//        // text that glows
//        var glowText = new GlowText(x: 60, y: 200, width: 500, height: 68)
//        {
//            ColorText = new RgbColor(224, 255, 255),
//            ColorGlow = new RgbColor(73, 55, 109)
//        };
//        page.Overlay.Add(glowText.AddDecoratedTextShapes("in the wabe"));

//        using (var file = new FileStream(@"..\..\addtexteffects.pdf", FileMode.Create, FileAccess.Write))
//        {
//            document.Write(file);
//        }
//    }
//}


// class that draws text with gradient colors
public class GradientText : DecoratedTextBase
{
    public Color ColorPen { get; set; }
    public Color ColorGradientTop { get; set; }
    public Color ColorGradientBottom { get; set; }

    public GradientText(double x, double y, double width, double height) : base(x, y, width, height)
    {
        this.Font = Font.HelveticaBold;
        this.Size = 48;
    }

    public Shape GetDecoratedText(string text)
    {
        this.Pen = new Pen(this.ColorPen, 1);
        this.Brush = new AxialGradientBrush(this.ColorGradientBottom, this.ColorGradientTop, 0, 0, 0, Size);
        this.AddDecoratedTextShapes(text);
        return this.Shapes;
    }
}


// class that draws a gradient filled text with a shadow
public class ShadowText : DecoratedTextBase
{
    public Color ColorPen { get; set; }
    public Color ColorGradientLight { get; set; }
    public Color ColorGradientDark { get; set; }
    const double ShadowDistanceX = 2;
    const double ShadowDistanceY = 3;

    public ShadowText(double x, double y, double width, double height) : base(x, y, width, height)
    {
        this.Font = Font.HelveticaBold;
        this.Size = 48;
    }

    public Shape GetDecoratedText(string text)
    {
        var textPen = this.Pen;

        // draw shadow growing right down, so scaled that the left-up parts of the text is aligned
        this.Scale = (this.Size + ShadowDistanceX) / this.Size;
        this.Pen = new Pen(new RgbColor(220, 220, 220), ShadowDistanceY / 2);
        this.Brush = new SolidBrush(new RgbColor(133, 133, 133));
        this.TextOffsetX = ShadowDistanceX;
        this.TextOffsetY = -ShadowDistanceY;
        this.AddDecoratedTextShapes(text);

        // draw gradient text, light source left up
        this.Scale = 1;
        this.Pen = textPen;
        this.Brush = new AxialGradientBrush(this.ColorGradientDark, this.ColorGradientLight, Size / 2, 0, 0, Size);
        this.TextOffsetX = 0;
        this.TextOffsetY = 0;
        this.AddDecoratedTextShapes(text);

        return this.Shapes;
    }
}


// Draw embossed text
public class EmbossText : DecoratedTextBase
{
    public Color ColorText { get; set; }
    public Color ColorHighlighted { get; set; }
    public Color ColorInShadow { get; set; }
    const double BorderSize = 0.8;

    public EmbossText(double x, double y, double width, double height) : base(x, y, width, height)
    {
        this.Font = Font.TimesRoman;
        this.Size = 48;
    }

    public Shape GetDecoratedText(string text)
    {
        // draw a backgroud rectangle around the text using the text color.
        var backgroud = new RectangleShape(0, 0, this.Width + (this.Margin * 2), this.Height + (this.Margin * 2));
        backgroud.Brush = new SolidBrush(this.ColorText);
        this.Shapes.Add(backgroud);

        // Text in the shadow: Oversize the text by using a fat pen, this will be the shadow part. 
        this.Pen = new Pen(this.ColorInShadow, BorderSize * 2);
        this.AddDecoratedTextShapes(text);

        // Text in the light: overwrite parts of the shadow with in highlight color, slightly moved down-right.
        this.Brush = new SolidBrush(this.ColorHighlighted);
        this.Pen = null;
        this.TextOffsetX = BorderSize;
        this.TextOffsetY = -BorderSize;
        this.AddDecoratedTextShapes(text);

        // The actual text: draw the text on top with solid color
        this.Brush = new SolidBrush(this.ColorText);
        this.Pen = null;
        this.TextOffsetX = 0;
        this.TextOffsetY = 0;
        this.AddDecoratedTextShapes(text);

        return this.Shapes;
    }
}


// Class that draws solid text that glows
// Draw several layes of the same text, the lower layers are increasingly
// fattened (using a thicker pensize), and are increasingly more transparent
public class GlowText : DecoratedTextBase
{
    public Color ColorText { get; set; }
    public Color ColorGlow { get; set; }
    const int NrOfLayers = 7;
    public readonly double[] Opacities = { 255, 128, 64, 32, 16, 8, 4 };

    public GlowText(double x, double y, double width, double height) : base(x, y, width, height)
    {
        this.Font = Font.TimesBoldItalic;
        this.Size = 48;
    }

    public new Shape AddDecoratedTextShapes(string text)
    {
        const double OverSizeStep = 1.4;
        double offsetY = this.Font.BaselineOffset * this.Size;
        double offsetX = this.Margin;
        foreach (char c in text)
        {
            var glyphPaths = this.Font.CreatePaths(c, this.Size);
            for (int i = NrOfLayers - 1; i >= 0; i--)
            {
                var charStack = new FreeHandShape();
                if (i == 0) charStack.Brush = new SolidBrush(this.ColorText);
                charStack.Opacity = Opacities[i];
                charStack.Pen = new Pen(this.ColorGlow, 1 + (i * OverSizeStep));
                charStack.Pen.MiterLimit = 4;
                charStack.Paths.AddRange(glyphPaths);
                charStack.Transform = new TranslateTransform(offsetX, offsetY);
                Shapes.Add(charStack);
            }
            offsetX += this.Font.CalculateWidth(c.ToString(), this.Size);
        }
        return this.Shapes;
    }
}


// base clase for the decoration
public class DecoratedTextBase
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double TextOffsetX { get; set; }
    public double TextOffsetY { get; set; }
    public Font Font { get; set; }
    public double Size { get; set; }
    public Pen Pen { get; set; }
    public Brush Brush { get; set; }
    public double Scale { get; set; }
    public double Margin { get; set; }
    public ShapeCollection Shapes { get; set; }

    protected DecoratedTextBase(double x, double y, double width, double height)
    {
        this.X = x;
        this.Y = y;
        this.Width = width;
        this.Height = height;
        this.Scale = 1;
        this.Margin = 10;
        this.TextOffsetX = 0;
        this.TextOffsetY = 0;
        this.Shapes = new ShapeCollection(this.X, this.Y, this.Width + (2 * this.Margin), this.Height + (2 * this.Margin));
    }

    protected void AddDecoratedTextShapes(string text)
    {
        double offsetX = this.TextOffsetX + this.Margin;
        double offsetY = this.TextOffsetY + this.Margin + (this.Font.BaselineOffset * this.Size);
        foreach (char c in text)
        {
            if (!char.IsWhiteSpace(c))
            {
                var charAsShape = new FreeHandShape { Pen = this.Pen, Brush = this.Brush };
                charAsShape.Paths.AddRange(this.Font.CreatePaths(c, this.Size));
                charAsShape.Transform = new TransformCollection
                    {
                        new ScaleTransform(this.Scale, this.Scale),
                        new TranslateTransform(offsetX, offsetY)
                    };
                this.Shapes.Add(charAsShape);
            }
            offsetX += this.Font.CalculateWidth(c.ToString(), this.Size);
        }
    }
}


