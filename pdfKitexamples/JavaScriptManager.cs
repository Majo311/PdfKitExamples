using System;
using TallComponents.PDF;
using TallComponents.PDF.JavaScript;

namespace pdfKitexamples
{
    public static class JavaScriptManager
    { 
        public static void CreateJavaScript(this PdfFile pdfFile,string nameOfScript,string script)
        {
            JavaScript js = new JavaScript(script);
            pdfFile.JavaScripts.Add(nameOfScript, js);
        }
    }
}
