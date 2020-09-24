using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLtoXLSXcvt
{
    class TemplateNode
    {
        public string Path;
        public readonly Dictionary<string, string> Filters = new Dictionary<string, string>();
        public readonly Dictionary<string, string> Values = new Dictionary<string, string>();
        public readonly List<TemplateNode> SubNodes = new List<TemplateNode>();
        public TemplateNode Parrent = null;

        public TemplateNode() { }
        public TemplateNode(string[] lines)
        {
            TemplateNode current = null;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (line.Length <= 0) continue;


                if (line.StartsWith("(") && line.EndsWith(")"))
                {
                    if (current == null)
                    {
                        Path = line.Substring(1, line.Length - 2);
                        current = this;
                    }
                    else
                    {
                        var node = new TemplateNode();
                        node.Path = line.Substring(1, line.Length - 2);

                        current.SubNodes.Add(node);
                        node.Parrent = current;
                        current = node;
                    }
                }
                else if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    var expr = line.Substring(1, line.Length - 2);

                    var split_point = expr.IndexOf(':');
                    if (split_point < 0) throw new Exception(i + ": " + line);
                    var key = expr.Substring(0, split_point).Trim();
                    var value = expr.Substring(split_point + 1).Trim();

                    if (current.Filters.ContainsKey(key)) throw new Exception(i + ": " + line);
                    current.Filters.Add(key, value);
                }
                else if (line == "{")
                {
                    // Nothing to do here
                }
                else if (line == "}")
                {
                    if (current.Parrent != null) current = current.Parrent;
                }
                else if (line.Contains(':'))
                {
                    var split_point = line.IndexOf(':');
                    if (split_point < 0) throw new Exception(i + ": " + line);
                    var key = line.Substring(0, split_point).Trim();
                    var value = line.Substring(split_point + 1).Trim();

                    if (current.Values.ContainsKey(key)) throw new Exception(i + ": " + line);
                    current.Values.Add(key, value);
                }
                else throw new Exception(i + ": " + line);
            }
        }

        public void Apply(XmlNode root, Action<List<string>, int, int> action)
        {
            var nodes = root.SelectNodes(Path);
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                if (CheckFilters(node))
                {
                    var values = new List<string>();
                    FindValues(node, values);

                    foreach (var sub_node in SubNodes)
                        sub_node.Apply(node, values, values.Count);

                    action?.Invoke(values, i, nodes.Count);
                }
            }
        }
        public List<string> GetValueNames()
        {
            var list = new List<string>();
            foreach (var value in Values)
                list.Add(value.Key);

            foreach (var node in SubNodes)
                list.AddRange(node.GetValueNames());

            return list;
        }

        private void Apply(XmlNode root, List<string> values, int start)
        {
            var nodes = root.SelectNodes(Path);
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                if (CheckFilters(node))
                {
                    FindValues(node, values, start);

                    foreach (var sub_node in SubNodes)
                        sub_node.Apply(root, values, start + Values.Count);
                }
            }
        }
        private void FindValues(XmlNode node, List<string> values, int start = -1)
        {
            int pos = 0;
            foreach (var value in Values)
            {
                string result = "";
                if (start >= 0 && start + pos < values.Count) result = values[start + pos];

                foreach (XmlNode sub_node in node.SelectNodes(value.Value))
                {
                    var text = sub_node.FirstChild as XmlText;
                    if (text != null) result += (result.Length > 0 ? "; " : "") + WebUtility.HtmlDecode(text.Data);
                }

                if (start < 0 || start + pos >= values.Count) values.Add(result);
                else values[start + pos] = result;
                pos++;
            }
        }
        private bool CheckFilters(XmlNode node)
        {
            if (Filters.Count <= 0) return true;

            foreach (var filter in Filters)
            {
                foreach (XmlNode sub_node in node.SelectNodes(filter.Key))
                {
                    var text = sub_node.FirstChild as XmlText;
                    if (text != null && WebUtility.HtmlDecode(text.Data) == filter.Value) return true;
                }
            }
            return false;
        }
    }
}
