using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLtoXLSXcvt
{
    class TableRow : Dictionary<string, string>
    {
        private void VisitNode(XmlNode node, string full_name = "")
        {
            if (node.NodeType == XmlNodeType.Text)
            {
                var value = WebUtility.HtmlDecode((node as XmlText).Data);
                if (!ContainsKey(full_name))
                {
                    Add(full_name, value);
                }
                else
                {
                    this[full_name] += "; " + value;
                }
            }

            full_name += (full_name.Length > 0 ? "/" : "") + node.Name;

            foreach (XmlNode sub_node in node.ChildNodes)
            {
                VisitNode(sub_node, full_name);
            }
        }

        public TableRow(XmlNode node)
        {
            foreach (XmlNode sub_node in node.ChildNodes) VisitNode(sub_node);
        }
    }
}
