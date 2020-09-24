using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;



namespace XMLtoXLSXcvt
{
    class Converter
    {
        private string XMLPath;
        private string XLSXPath;
        private TemplateNode Template;

        public event EventHandler ProgressChanged;

        public int Progress { get; private set; } = 0;

        public Converter(string xml, string xlsx, TemplateNode template)
        {
            XMLPath = xml;
            XLSXPath = xlsx;
            Template = template;
        }

        public bool Convert()
        {
            Progress = 0;
            var document = new XmlDocument();
            document.Load(XMLPath);
            using (var result_document = new ExcelDocument())
            {
                result_document.AddRow(Template.GetValueNames());

                Template.Apply(document, (List<string> values, int i, int count) => 
                {
                    result_document.AddRow(values);
                    int progress = (i + 1) * 100 / count;
                    if (Progress != progress)
                    {
                        Progress = progress;
                        ProgressChanged?.Invoke(this, EventArgs.Empty);
                    }
                });

                return result_document.Save(XLSXPath);
            }
        }
    }
}
