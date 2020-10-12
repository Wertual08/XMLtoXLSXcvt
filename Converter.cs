using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;



namespace XMLtoXLSXcvt
{
    class Converter
    {
        private string XMLPath;
        private string XLSXPath;
        private List<TemplateNode> Templates;

        public event EventHandler ProgressChanged;

        public int Progress { get; private set; } = 0;
        public string Message { get; private set; } = "";

        public Converter(string xml, string xlsx, List<TemplateNode> templates)
        {
            XMLPath = xml;
            XLSXPath = xlsx;
            Templates = templates;
        }

        public bool Convert()
        {
            Progress = 0;
            var xml_doc = new XmlDocument();
            xml_doc.Load(XMLPath);

            using (var xlsx_doc = new ExcelDocument())
            {
                int offset = 0;
                for (int i = 0; i < Templates.Count; i++)
                {
                    Message = "Template: " + (i + 1) + " / " + Templates.Count;

                    var template = Templates[i];

                    offset += PerformTemplate(template, xml_doc, xlsx_doc, offset);
                }
                
                return xlsx_doc.Save(XLSXPath);
            }
        }

        private int PerformTemplate(TemplateNode template, XmlDocument xml_doc, ExcelDocument xlsx_doc, int offset)
        {
            xlsx_doc.RestartRows();
            xlsx_doc.AddRow();

            List<string> columns = new List<string>();

            var nodes = xml_doc.SelectNodes(template.Path);
            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];

                var values = template.Apply(node);

                if (values == null) continue;

                List<string> row = new List<string>(columns.Count);
                foreach (var value in values)
                {
                    int index = columns.IndexOf(value.Key);
                    if (index < 0)
                    {
                        index = columns.Count;
                        xlsx_doc.AddColumn(offset + columns.Count);
                        columns.Add(value.Key);
                    }
                    while (row.Count <= index) row.Add("");
                    row[index] = value.Value;
                }
                while (row.Count < columns.Count) row.Add("");
                xlsx_doc.AddRow(offset, row);

                int progress = (i + 1) * 100 / nodes.Count;
                if (Progress != progress)
                {
                    Progress = progress;
                    ProgressChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            for (int i = 0; i < columns.Count; i++)
                xlsx_doc[offset + i, 0] = columns[i];

            return columns.Count;
        }
    }
}
