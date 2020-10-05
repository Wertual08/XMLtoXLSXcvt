using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace XMLtoXLSXcvt
{
    class TemplateNode
    {
        private static bool IsAttribute(char c)
        {
            return "$|&.*".IndexOf(c) >= 0;
        }
        private static string EscapeValues(string line)
        {
            string result = "";

            bool in_value = false;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '\"' && (i == 0 || line[i - 1] != '\\')) in_value = !in_value;
                else if (!in_value) result += line[i];
            }

            return result;
        }
        private static string[] ExtractValues(string line)
        {
            List<string> results = new List<string>(3);

            bool in_value = false;
            string current = "";
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '\"' && (i == 0 || line[i - 1] != '\\'))
                {
                    if (in_value)
                    {
                        results.Add(current);
                        current = "";
                    }
                    in_value = !in_value;
                }
                else if (in_value) current += line[i];
            }

            if (in_value) return null;
            return results.ToArray();
        }
        private static string[] ExtractAttribs(string line)
        {
            List<string> results = new List<string>(3);

            bool in_value = false;
            string current = "";
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '\"' && (i == 0 || line[i - 1] != '\\'))
                {
                    if (!in_value)
                    {
                        results.Add(current);
                        current = "";
                    }
                    in_value = !in_value;
                }
                else
                {
                    if (IsAttribute(line[i])) current += line[i];
                    else current = "";
                }
            }

            return results.ToArray();
        }
        private bool IsPatternPath(string pattern)
        {
            return pattern.StartsWith("(") && pattern.EndsWith(")");
        }
        private bool IsPatternCondition(string pattern)
        {
            return pattern.StartsWith("[") && pattern.EndsWith("]");
        }

        public struct Unit
        {
            public string Attributes;
            public string Value;
            public Unit(string a, string v)
            {
                Attributes = a;
                Value = v;
            }
            public Unit(string v)
            {
                Attributes = "";
                Value = v;
            }

            public override bool Equals(object obj)
            {
                return (obj is Unit) && Attributes == ((Unit)obj).Attributes && Value == ((Unit)obj).Value;
            }
        }

        public string Path;
        public readonly Dictionary<Unit, Unit> Filters = new Dictionary<Unit, Unit>();
        public readonly Dictionary<Unit, Unit> Values = new Dictionary<Unit, Unit>();
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

                var pattern = EscapeValues(line);
                var values = ExtractValues(line);
                var attribs = ExtractAttribs(line);
                
                if (attribs == null) throw new Exception("[Unclosed quotes]" + i + ": " + line);

                if (IsPatternPath(pattern))
                {
                    if (values.Length != 1) throw new Exception("[Values count]" + i + ": " + line);
                    if (attribs.Length != 1) throw new Exception("[Attributes count]" + i + ": " + line);
                    if (attribs[0] != "") throw new Exception("[Attributes]" + i + ": " + line); 

                    if (current == null)
                    {
                        Path = values[0];
                        current = this;
                    }
                    else
                    {
                        var node = new TemplateNode();
                        node.Path = values[0];

                        current.SubNodes.Add(node);
                        node.Parrent = current;
                        current = node;
                    }
                }
                else if (IsPatternCondition(pattern))
                {
                    if (values.Length != 2) throw new Exception("[Values count]" + i + ": " + line);
                    if (attribs.Length != 2) throw new Exception("[Attributes count]" + i + ": " + line);
                    if (attribs[0] != "") throw new Exception("[Attributes]" + i + ": " + line);
                    if (attribs[1].Contains('$')) throw new Exception("[Attributes]" + i + ": " + line);
                    if (current.Filters.ContainsKey(new Unit(values[0]))) 
                        throw new Exception("[Duplicated key]" + i + ": " + line);

                    current.Filters.Add(new Unit(values[0]), new Unit(attribs[1], values[1]));
                }
                else if (line.Contains(':'))
                {
                    if (values.Length != 2) throw new Exception("[Values count]" + i + ": " + line);
                    if (attribs.Length != 2) throw new Exception("[Attributes count]" + i + ": " + line);
                    if (attribs[0].Contains('&') || attribs[0].Contains('|') || attribs[0].Contains('.')) 
                        throw new Exception("[Attributes]" + i + ": " + line);
                    if (attribs[1] != "") throw new Exception("[Attributes]" + i + ": " + line);
                    if (current.Values.ContainsKey(new Unit(values[0])))
                        throw new Exception("[Duplicated key]" + i + ": " + line);

                    current.Values.Add(new Unit(attribs[0], values[0]), new Unit(values[1]));
                }
                else if (line == "{")
                {
                    // Nothing to do here
                }
                else if (line == "}")
                {
                    if (current.Parrent != null) current = current.Parrent;
                }
                else throw new Exception("[Bad format]" + i + ": " + line);
            }
        }

        public Dictionary<string, string> Apply(XmlNode root)
        {
            if (!CheckFilters(root)) return null;

            var result = FindValues(root);

            foreach (var sub_node in SubNodes)
            {
                var nodes = root.SelectNodes(sub_node.Path);
                foreach (XmlNode node in nodes)
                {
                    var sub_result = sub_node.Apply(node);
                    if (sub_result == null) continue;
                    foreach (var found in sub_result)
                    {
                        if (result.ContainsKey(found.Key)) result[found.Key] += "; " + found.Value;
                        else result.Add(found.Key, found.Value);
                    }
                }
            }

            return result;
        }

        private bool CheckFilters(XmlNode node)
        {
            if (Filters.Count <= 0) return true;

            bool any_found = false;
            bool any_value = false;

            foreach (var filter in Filters)
            {
                var path = filter.Key.Value;
                var value_attribs = filter.Value.Attributes;
                var value = filter.Value.Value;

                bool result = false;
                if (value_attribs.Contains('.'))
                {
                    foreach (XmlNode sub_node in node.SelectNodes(path))
                    {
                        var text = sub_node.FirstChild as XmlText;
                        if (text != null) result = Regex.IsMatch(WebUtility.HtmlDecode(text.Data), value);
                    }
                }
                else if (value_attribs.Contains('*'))
                {
                    var regex = new Regex(
                        "^" + Regex.Escape(value).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline
                        );

                    foreach (XmlNode sub_node in node.SelectNodes(path))
                    {
                        var text = sub_node.FirstChild as XmlText;
                        if (text != null) result = regex.IsMatch(WebUtility.HtmlDecode(text.Data));
                    }
                }
                else
                {
                    foreach (XmlNode sub_node in node.SelectNodes(path))
                    {
                        var text = sub_node.FirstChild as XmlText;
                        if (text != null) result = WebUtility.HtmlDecode(text.Data) == value;
                    }
                }

                if (!value_attribs.Contains('&') && !value_attribs.Contains('|')) return result;
                if (value_attribs.Contains('&')) if (!result) return false;
                if (value_attribs.Contains('|'))
                {
                    any_found = true;
                    if (result) any_value = true;
                }
            }

            if (any_found) return any_value;
            else return true;
        }

        private Dictionary<string, string> FindValues(XmlNode node)
        {
            var result = new Dictionary<string, string>();
            foreach (var value in Values)
            {
                var name_attribs = value.Key.Attributes;
                var name = value.Key.Value;
                var path = value.Value.Value;

                if (name_attribs.Contains('$'))
                {
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
                                    var header = WebUtility.HtmlDecode(text_name.Data);
                                    var data = WebUtility.HtmlDecode(text.Data);
                                    if (!result.ContainsKey(header))
                                        result.Add(header, data);
                                    else result[header] += "; " + data;
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
                            var data = WebUtility.HtmlDecode(text.Data);
                            if (!result.ContainsKey(name)) result.Add(name, data);
                            else result[name] += "; " + data;
                        }
                    }
                }
            }

            return result;
        }
    }
}
