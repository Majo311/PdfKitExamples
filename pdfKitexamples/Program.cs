using System;
using System.IO;
using TallComponents.PDF;
using TallComponents.PDF.Shapes;
using System.Linq;
using TallComponents.PDF.Forms.Fields;
using TallComponents.PDF.Annotations.Widgets;
using TallComponents.PDF.Colors;
using TallComponents.PDF.Fonts;
using TallComponents.PDF.Security;

namespace pdfKitexamples
{
    public class Program
    {
         static void Main(string[] args)
        {

            //EitPages();
            //UseJavaScript();
            UseSecurity();
            Console.ReadLine();

        }

        private static void EditPages()
        {
            ShapeCollection shapes = null;
            using (PdfFile pdfFileIn = PdfFile.Read(Environment.CurrentDirectory + @"\sample1.pdf"))
            {
                Page secondPage = pdfFileIn.Pages[1];
                shapes = secondPage.CreateShapes();
                shapes = pdfFileIn.EditPage(secondPage.Index);
                using (PdfFile pdfOut = new PdfFile())
                {
                    pdfOut.AddPage(shapes, secondPage.Width, secondPage.Height);
                    pdfOut.Save(null);
                }
            }
        }

        private static void UseJavaScript()
        {
            string calculatorScript = File.ReadAllText(Directory.GetCurrentDirectory()+@"\Scripts\Calculator.js");
            using (PdfFile pdfOut = new PdfFile())
            {
                pdfOut.CreateJavaScript("calculatorScript", calculatorScript);
                //custom page size.
                Page page = new Page(280, 210);
                pdfOut.Pages.Add(page);

                //Create TextBox Field for the result
                {
                    TextShape textShape = new TextShape(25, page.Height - 40, "Result:", Font.Helvetica, 12);
                    page.VisualOverlay.Add(textShape);

                    TextField field = new TextField("result");
                    Widget widget = new Widget(70, page.Height - 40, 180, 15);

                    //prevent editing
                    field.ReadOnly = true;

                    widget.BorderColor = RgbColor.Blue;
                    widget.BorderWidth = 1;
                    widget.HorizontalAlignment = HorizontalAlignment.Right;

                    field.Widgets.Add(widget);
                    page.Widgets.Add(widget);
                    pdfOut.Fields.Add(field);
                }

                //Create TextBox Field for the current operator
                {
                    TextShape textShape = new TextShape(170, page.Height - 60, "Operator:", Font.Helvetica, 9);
                    page.VisualOverlay.Add(textShape);

                    TextField field = new TextField("operator");
                    Widget widget = new Widget(210, page.Height - 60, 40, 10);

                    //prevent editing
                    field.ReadOnly = true;

                    widget.BackgroundColor = System.Drawing.Color.Yellow;
                    widget.Font = Font.HelveticaOblique;
                    widget.FontSize = 6;
                    widget.HorizontalAlignment = HorizontalAlignment.Center;

                    field.Widgets.Add(widget);
                    page.Widgets.Add(widget);
                    pdfOut.Fields.Add(field);
                }

                //Place clear total (C) and clear element (CE) button
                Helper.placeCButton(pdfOut, page, 100, page.Height - 80);
                Helper.placeCeButton(pdfOut, page, 140, page.Height - 80);

                //Place number buttons
                Helper.placeNumberButton(pdfOut, page, 7, 100, page.Height - 100);
                Helper.placeNumberButton(pdfOut, page, 8, 140, page.Height - 100);
                Helper.placeNumberButton(pdfOut, page, 9, 180, page.Height - 100);

                Helper.placeNumberButton(pdfOut, page, 4, 100, page.Height - 120);
                Helper.placeNumberButton(pdfOut, page, 5, 140, page.Height - 120);
                Helper.placeNumberButton(pdfOut, page, 6, 180, page.Height - 120);

                Helper.placeNumberButton(pdfOut, page, 1, 100, page.Height - 140);
                Helper.placeNumberButton(pdfOut, page, 2, 140, page.Height - 140);
                Helper.placeNumberButton(pdfOut, page, 3, 180, page.Height - 140);

                Helper.placeNumberButton(pdfOut, page, 0, 100, page.Height - 160);

                //Place other buttons
                Helper.placeDotButton(pdfOut, page, 140, page.Height - 160);
                Helper.placePlusMinButton(pdfOut, page, 180, page.Height - 160);

                Helper.placePlusButton(pdfOut, page, 220, page.Height - 100);
                Helper.placeMinusButton(pdfOut, page, 220, page.Height - 120);
                Helper.placeDivideButton(pdfOut, page, 220, page.Height - 140);
                Helper.placeMultiplyButton(pdfOut, page, 220, page.Height - 160);

                Helper.placeEqualsButton(pdfOut, page, 220, page.Height - 180);
                pdfOut.Save();

            }     
        }
        private static void UseSecurity()
        {
            using (PdfFile pdfOut = new PdfFile())
            {
                Page page = new Page(280, 210);
                pdfOut.Pages.Add(page);
                User userX = new User("xzzzzzz");
                pdfOut.SecurityManager.AddPassword(userX);
                pdfOut.Save();
            }
        }
    }
}
