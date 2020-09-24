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
                if (line.Length <= 0 || line[0] == '#') continue;


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

        public List<string> GetValueNames()
        {
            var list = new List<string>();
            foreach (var value in Values)
                list.Add(value.Key);

            foreach (var node in SubNodes)
                list.AddRange(node.GetValueNames());

            return list;
        }
        public void Apply(XmlNode root, Action<Dictionary<string, string>, int, int> action)
        {
            var nodes = root.SelectNodes(Path);
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                if (CheckFilters(node))
                {
                    var values = new Dictionary<string, string>();
                    FindValues(node, values);

                    foreach (var sub_node in SubNodes)
                        sub_node.Apply(node, values);

                    action?.Invoke(values, i, nodes.Count);
                }
            }
        }

        private void Apply(XmlNode root, Dictionary<string, string> values)
        {
            var nodes = root.SelectNodes(Path);
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];

                if (CheckFilters(node))
                {
                    FindValues(node, values);

                    foreach (var sub_node in SubNodes)
                        sub_node.Apply(node, values);
                }
            }
        }
        private void FindValues(XmlNode node, Dictionary<string, string> values)
        {
            foreach (var value in Values)
            {
                var name = value.Key;
                var path = value.Value;

                if (name.StartsWith("$"))
                {
                    name = name.Substring(1);

                    foreach (XmlNode sub_node_name in node.SelectNodes(name))
                    {
                        var text_name = sub_node_name.FirstChild as XmlText;
                        if (text_name != null)
                        {
                            foreach (XmlNode sub_node in node.SelectNodes(path))
                            {
                                var text = sub_node.FirstChild as XmlText;
                                if (text != null)
                                {
                                    if (!values.ContainsKey(text_name.Data)) 
                                        values.Add(text_name.Data, WebUtility.HtmlDecode(text.Data));
                                    else values[text_name.Data] += "; " + WebUtility.HtmlDecode(text.Data);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (XmlNode sub_node in node.SelectNodes(path))
                    {
                        var text = sub_node.FirstChild as XmlText;
                        if (text != null)
                        {
                            if (!values.ContainsKey(name)) values.Add(name, WebUtility.HtmlDecode(text.Data));
                            else values[name] += "; " + WebUtility.HtmlDecode(text.Data);
                        }
                    }
                }
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
