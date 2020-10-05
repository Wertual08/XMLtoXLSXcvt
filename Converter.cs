﻿using System;
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

                var nodes = document.SelectNodes(Template.Path);
                for (int i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];

                    var values = Template.Apply(node);

                    if (values == null) continue;

                    List<string> row = new List<string>(columns.Count);
                    foreach (var value in values)
                    {
                        int index = columns.IndexOf(value.Key);
                        if (index < 0)
                        {
                            index = columns.Count;
                            result_document.AddColumn(columns.Count);
                            columns.Add(value.Key);
                        }
                        while (row.Count <= index) row.Add("");
                        row[index] = value.Value;
                    }
                    while (row.Count < columns.Count) row.Add("");
                    result_document.AddRow(row);

                    int progress = (i + 1) * 100 / nodes.Count;
                    if (Progress != progress)
                    {
                        Progress = progress;
                        ProgressChanged?.Invoke(this, EventArgs.Empty);
                    }
                }

                for (int i = 0; i < columns.Count; i++)
                    result_document[i, 0] = columns[i];

                return result_document.Save(XLSXPath);
            }
        }
    }
}
