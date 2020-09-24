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
        private XmlDocument Document = new XmlDocument();
        private ExcelDocument ResultDocument;

        private string XMLPath;
        private string XLSXPath;
        private Dictionary<string, string> ValuesToExport;
        private Dictionary<string, string> ValuesFilter;
        private string NodesToParse;
        private bool AtLeastOne;

        private void CheckRow(TableRow row)
        {
            if (AtLeastOne)
            {
                bool found = false;
                foreach (var filter in ValuesFilter)
                {
                    if (row.ContainsKey(filter.Key) && row[filter.Key] == filter.Value)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) return;
            }
            else
            {
                foreach (var filter in ValuesFilter)
                {
                    if (!row.ContainsKey(filter.Key)) return;
                    if (row[filter.Key] != filter.Value) return;
                }
            }

            var result_row = new string[ValuesToExport.Count];
            int index = 0;
            foreach (var column in ValuesToExport)
            {
                var value = row.ContainsKey(column.Value) ? row[column.Value] : "";

                result_row[index++] = value;
            }

            ResultDocument.AddRow(result_row);
        }
        private void VisitNode(XmlNode node, string full_name = "")
        {
            full_name += "/" + node.Name;

            if (full_name == NodesToParse)
                CheckRow(new TableRow(node));
            else foreach (XmlNode sub_node in node.ChildNodes)
                VisitNode(sub_node, full_name);
        }

        public event EventHandler ProgressChanged;

        public int Progress { get; private set; } = 0;

        public Converter(string xml, string xlsx, Dictionary<string, string> vte, Dictionary<string, string> vf, string ntp, bool alo)
        {
            XMLPath = Path.GetFullPath(xml);
            XLSXPath = Path.GetFullPath(xlsx);
            ValuesToExport = vte;
            ValuesFilter = vf;
            NodesToParse = ntp;
            AtLeastOne = alo;
        }

        public bool Convert()
        {
            Progress = 0;
            Document.Load(XMLPath);
            using (ResultDocument = new ExcelDocument())
            {
                var header = new string[ValuesToExport.Count];
                int index = 0;
                foreach (var value in ValuesToExport)
                    header[index++] = value.Key;
                ResultDocument.AddRow(header);

                var element = Document.DocumentElement;
                for (int i = 0; i < element.ChildNodes.Count; i++)
                {
                    int progress = (i + 1) * 100 / element.ChildNodes.Count;
                    if (Progress != progress)
                    {
                        Progress = progress;
                        ProgressChanged?.Invoke(this, EventArgs.Empty);
                    }

                    XmlNode node = element.ChildNodes[i];
                    VisitNode(node, element.Name);
                }

                return ResultDocument.Save(XLSXPath);
            }
        }
    }
}
