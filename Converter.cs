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
                result_document.AddRow();
                List<string> columns = new List<string>();

                Template.Apply(document, (Dictionary<string, string> values, int i, int count) => 
                {
                    List<string> row = new List<string>(values.Count);
                    foreach (var value in values)
                    {
                        int index = columns.IndexOf(value.Key);
                        if (index < 0)
                        {
                            index = columns.Count;
                            columns.Add(value.Key);
                        }
                        while (row.Count <= index) row.Add("");
                        row[index] = value.Value;
                    }
                    result_document.AddRow(row);

                    int progress = (i + 1) * 100 / count;
                    if (Progress != progress)
                    {
                        Progress = progress;
                        ProgressChanged?.Invoke(this, EventArgs.Empty);
                    }
                });

                for (int i = 0; i < columns.Count; i++)
                    result_document[i, 0] = columns[i];

                return result_document.Save(XLSXPath);
            }
        }
    }
}
