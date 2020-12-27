using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TallComponents.PDF;
using TallComponents.PDF.Actions;
using TallComponents.PDF.Annotations;
using TallComponents.PDF.Annotations.Widgets;
using TallComponents.PDF.Forms.Fields;

namespace pdfKitexamples
{
    public class Helper
    {
        public static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }

        public static void placeNumberButton(PdfFile document, Page page, int number, double left, double bottom)
        {
            string javaScript = string.Format("number_pressed({0});", number.ToString());
            placeButton(document, page, left, bottom, "num" + number.ToString(), number.ToString(), javaScript);
        }

        public static void placeDotButton(Document document, Page page, double left, double bottom)
        {
            string javaScript = "dot_pressed();";
            placeButton(document, page, left, bottom, "dot", ".", javaScript);
        }

        public static void placePlusMinButton(Document document, Page page, double left, double bottom)
        {
            string javaScript = "plusMin_pressed();";
            placeButton(document, page, left, bottom, "plus_min", "+/-", javaScript);
        }

        public static void placePlusButton(Document document, Page page, double left, double bottom)
        {
            string javaScript = "operator_pressed('PLUS');";
            placeButton(document, page, left, bottom, "plus", "+", javaScript);
        }

        public static void placeMinusButton(Document document, Page page, double left, double bottom)
        {
            string javaScript = "operator_pressed('MINUS');";
            placeButton(document, page, left, bottom, "minus", "-", javaScript);
        }

        public static void placeDivideButton(Document document, Page page, double left, double bottom)
        {
            string javaScript = "operator_pressed('DIVIDE');";
            placeButton(document, page, left, bottom, "divide", "/", javaScript);
        }

        public static void placeMultiplyButton(Document document, Page page, double left, double bottom)
        {
            string javaScript = "operator_pressed('MULTIPLY');";
            placeButton(document, page, left, bottom, "multiply", "*", javaScript);
        }

        public static void placeEqualsButton(Document document, Page page, double left, double bottom)
        {
            string javaScript = "equal_pressed();";
            placeButton(document, page, left, bottom, "equals", "=", javaScript);
        }

        public static void placeCButton(Document document, Page page, double left, double bottom)
        {
            string javaScript = "clear_all();";
            placeButton(document, page, left, bottom, "c", "C", javaScript, true);
        }

        public static void placeCeButton(Document document, Page page, double left, double bottom)
        {
            string javaScript = "clear_entry();\nresult.value = 0;";
            placeButton(document, page, left, bottom, "Ce", "CE", javaScript, true);
        }

        public static void placeButton(Document document, Page page, double left, double bottom, string fieldName, string label, string javaScript)
        {
            placeButton(document, page, left, bottom, fieldName, label, javaScript, false);
        }

        public static void placeButton(Document document, Page page, double left, double bottom, string fieldName, string label, string javaScript, bool displayRed)
        {
            PushButtonField field = new PushButtonField(fieldName);
            PushButtonWidget widget = new PushButtonWidget(left, bottom, 30, 15);

            if (displayRed)
            {
                widget.BackgroundColor = System.Drawing.Color.Red;
                widget.BorderColor = System.Drawing.Color.Gray;
                widget.TextColor = System.Drawing.Color.White;
            }
            else
            {
                widget.BackgroundColor = System.Drawing.Color.LightGray;
                widget.BorderColor = System.Drawing.Color.Gray;
            }
            widget.BorderStyle = BorderStyle.Beveled;

            widget.Label = label;

            field.Widgets.Add(widget);
            page.Widgets.Add(widget);
            document.Fields.Add(field);

            //include action
            widget.MouseUpActions.Add(new JavaScriptAction(javaScript));
        }
    }
}
