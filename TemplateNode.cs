using System;
using System.Collections.Generic;
using System.IO;
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
            return "$|&.*!".IndexOf(c) >= 0;
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
                        results.Add(current.Replace("\\\"", "\""));
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
        private static bool IsPatternPath(string pattern)
        {
            return pattern.StartsWith("(") && pattern.EndsWith(")");
        }
        private static bool IsPatternCondition(string pattern)
        {
            return pattern.StartsWith("[") && pattern.EndsWith("]");
        }
        private static bool IsPatternPrefix(string pattern)
        {
            return pattern.StartsWith("<") && pattern.EndsWith(">");
        }
        private static Predicate<string> GetPredicate(string filter_attribs, string filter_str)
        {
            if (filter_attribs.Contains('.')) return (string s) => Regex.IsMatch(s, filter_str);
            else if (filter_attribs.Contains('*')) return (string s) => Regex.IsMatch(s,
                "^" + Regex.Escape(filter_str).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            else return (string s) => s == filter_str;
        }
        private static bool MatchFilter(XmlNode node, string path, Predicate<string> predicate)
        {
            foreach (XmlNode sub_node in node.SelectNodes(path))
            {
                var text = sub_node.FirstChild as XmlText;
                if (text != null) if (predicate(WebUtility.HtmlDecode(text.Data))) return true;
            }
            return false;
        }
        private static string FindValue(XmlNode node, string attribs, string value)
        {
            if (attribs.Contains('!')) return value;
            else
            {
                string result = "";
                foreach (XmlNode sub_node in node.SelectNodes(value))
                {
                    var text = sub_node.FirstChild as XmlText;
                    if (text != null)
                    {
                        var data = WebUtility.HtmlDecode(text.Data);
                        if (result.Length == 0) result = data;
                        else result += "; " + data;
                    }
                }
                return result;
            }
        }

        private bool CheckFilters(XmlNode node)
        {
            if (Filters.Count <= 0) return true;

            bool any_found = false;
            bool any_value = false;

            foreach (var filter in Filters)
            {
                var path = filter.Key.Value;
                var filter_attribs = filter.Value.Attributes;
                var filter_str = filter.Value.Value;

                Predicate<string> predicate = GetPredicate(filter_attribs, filter_str);
                bool result = MatchFilter(node, path, predicate);

                if (!filter_attribs.Contains('&') && !filter_attribs.Contains('|')) return result;
                if (filter_attribs.Contains('&')) if (!result) return false;
                if (filter_attribs.Contains('|'))
                {
                    any_found = true;
                    if (result) any_value = true;
                }
            }

            if (any_found) return any_value;
            else return true;
        }
        private string ConstructPrefix(XmlNode node)
        {
            string result = "";
            foreach (var prefix in Prefixes)
            {
                if (prefix.Attributes.Contains('!')) result += prefix.Value;
                else
                {
                    foreach (XmlNode sub_node in node.SelectNodes(prefix.Value))
                    {
                        var text = sub_node.FirstChild as XmlText;
                        if (text != null) result += WebUtility.HtmlDecode(text.Data);
                    }
                }
            }
            return result;
        }
        private Dictionary<string, string> FindValues(XmlNode node, string prefix)
        {
            var result = new Dictionary<string, string>();
            foreach (var value in Values)
            {
                var name_attribs = value.Key.Attributes;
                var name = value.Key.Value;
                var data_attribs = value.Value.Attributes;
                var data = value.Value.Value;

                if (name_attribs.Contains('$'))
                {
                    foreach (XmlNode sub_node_name in node.SelectNodes(name))
                    {
                        var text_name = sub_node_name.FirstChild as XmlText;
                        if (text_name != null)
                        {
                            var header = prefix + WebUtility.HtmlDecode(text_name.Data);
                            var text = FindValue(node, data_attribs, data);

                            if (!result.ContainsKey(header)) result.Add(header, text);
                            else result[header] += "; " + text;
                        }
                    }
                }
                else
                {
                    var header = prefix + name;
                    var text = FindValue(node, data_attribs, data);

                    if (!result.ContainsKey(header)) result.Add(header, text);
                    else result[header] += "; " + text;
                }
            }

            return result;
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
        public readonly List<KeyValuePair<Unit, Unit>> Filters = new List<KeyValuePair<Unit, Unit>>();
        public readonly Dictionary<Unit, Unit> Values = new Dictionary<Unit, Unit>();
        public readonly List<Unit> Prefixes = new List<Unit>();
        public readonly List<TemplateNode> SubNodes = new List<TemplateNode>();
        public TemplateNode Parrent = null;

        public static List<TemplateNode> ParseLines(string[] lines)
        {
            var result = new List<TemplateNode>();
            TemplateNode current = null;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (line.Length <= 0 || line[0] == '#') continue;
                if (line == "===-===") break;

                var pattern = EscapeValues(line);
                var values = ExtractValues(line);
                var attribs = ExtractAttribs(line);

                if (attribs == null) throw new Exception("[Unclosed quotes]" + i + ": " + line);

                if (IsPatternPath(pattern))
                {
                    if (values.Length != 1) throw new Exception("[Values count]" + i + ": " + line);
                    if (attribs.Length != 1) throw new Exception("[Attributes count]" + i + ": " + line);
                    if (attribs[0] != "") throw new Exception("[Attributes]" + i + ": " + line);

                    var node = new TemplateNode();
                    node.Path = values[0];

                    if (current == null)
                    {
                        result.Add(node);
                    }
                    else
                    {
                        current.SubNodes.Add(node);
                        node.Parrent = current;
                    }

                    current = node;
                }
                else if (IsPatternCondition(pattern))
                {
                    if (current == null) throw new Exception("[No current node]" + i + ": " + line);
                    if (values.Length != 2) throw new Exception("[Values count]" + i + ": " + line);
                    if (attribs.Length != 2) throw new Exception("[Attributes count]" + i + ": " + line);
                    if (attribs[0] != "") throw new Exception("[Attributes]" + i + ": " + line);
                    if (attribs[1].Contains('$')) throw new Exception("[Attributes]" + i + ": " + line);
                    if (attribs[1].Contains('.') && attribs[1].Contains('*')) throw new Exception("[Attributes]" + i + ": " + line);

                    current.Filters.Add(new KeyValuePair<Unit, Unit>(new Unit(attribs[0], values[0]), new Unit(attribs[1], values[1])));
                }
                else if (IsPatternPrefix(pattern))
                {
                    if (current == null) throw new Exception("[No current node]" + i + ": " + line);
                    foreach (var attrib in attribs)
                        if (attrib.Length != 0 && (attrib.Length != 1 || attrib[0] != '!')) 
                            throw new Exception("[Attributes]" + i + ": " + line);
                    for (int a = 0; a < attribs.Length; a++)
                        current.Prefixes.Add(new Unit(attribs[a], values[a]));
                }
                else if (line.Contains(':'))
                {
                    if (current == null) throw new Exception("[No current node]" + i + ": " + line);
                    if (values.Length != 2) throw new Exception("[Values count]" + i + ": " + line);
                    if (attribs.Length != 2) throw new Exception("[Attributes count]" + i + ": " + line);
                    if (attribs[0].Length != 0 && (attribs[0].Length != 1 || attribs[0][0] != '$'))
                        throw new Exception("[Attributes]" + i + ": " + line);
                    if (attribs[1].Length != 0 && (attribs[1].Length != 1 || attribs[1][0] != '!'))
                        throw new Exception("[Attributes]" + i + ": " + line);
                    if (current.Values.ContainsKey(new Unit(values[0])))
                        throw new Exception("[Duplicated key]" + i + ": " + line);

                    current.Values.Add(new Unit(attribs[0], values[0]), new Unit(attribs[1], values[1]));
                }
                else if (line == "{")
                {
                    // Nothing to do here
                }
                else if (line == "}")
                {
                    current = current.Parrent;
                }
                else throw new Exception("[Bad format]" + i + ": " + line);
            }

            return result;
        }
        public TemplateNode() { }

        public Dictionary<string, string> Apply(XmlNode root)
        {
            if (!CheckFilters(root)) return null;

            var prefix = ConstructPrefix(root);
            var result = FindValues(root, prefix);
            

            foreach (var sub_node in SubNodes)
            {
                var nodes = root.SelectNodes(sub_node.Path);
                foreach (XmlNode node in nodes)
                {
                    var sub_result = sub_node.Apply(node);
                    if (sub_result == null) continue;
                    foreach (var found in sub_result)
                    {
                        if (!result.ContainsKey(found.Key)) result.Add(found.Key, found.Value);
                        else result[found.Key] += "; " + found.Value;
                    }
                }
            }

            return result;
        }
    }
}
