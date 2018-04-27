using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml;

namespace RegularToy
{
    class Str
    {
        public static string DecodeBase64(string code)
        {
            byte[] bytes = Convert.FromBase64String(code);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string EncodeBase64(string code)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(code));
        }

        public static bool GetBoolean(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                switch (value.ToLower())
                {
                    case "true":
                    case "yes":
                    case "1":
                        return true;

                    case "false":
                    case "no":
                    case "0":
                        return true;
                }
            }
            return false;
        }

        public static int GetInteger(object obj)
        {
            int num;
            if (obj is int)
            {
                return (int)obj;
            }
            if (obj is decimal)
            {
                return (int)((decimal)obj);
            }
            if (obj is float)
            {
                return (int)((float)obj);
            }
            if (obj is double)
            {
                return (int)((double)obj);
            }
            if (obj is float)
            {
                return (int)((float)obj);
            }
            if (obj is short)
            {
                return (short)obj;
            }
            if (obj is long)
            {
                return (int)((long)obj);
            }
            if ((obj is string) && int.TryParse((string)obj, out num))
            {
                return num;
            }
            return 0;
        }

        public static Size GetSizeDefault(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                int num;
                int num2;
                string[] strArray = value.Split(new char[] { ',' });
                if (strArray.Length != 2)
                {
                    return Size.Empty;
                }
                if (int.TryParse(strArray[0].Trim(), out num) && int.TryParse(strArray[1].Trim(), out num2))
                {
                    return new Size(num, num2);
                }
            }
            return Size.Empty;
        }

        public static string JoinString(IEnumerable<string> value, string separator)
        {
            if (value == null)
                return null;

            var sb = new StringBuilder();
            foreach (var line in value)
            {
                if (sb.Length > 0)
                    sb.Append(separator);
                sb.Append(line);
            }

            return sb.ToString();
        }

        public static Size? GetSize(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var regex = new Regex(@"(\d+),\s*(\d+)");
            var match = regex.Match(value);
            if (match.Success)
            {
                return new Size()
                {
                    Width = int.Parse(match.Groups[1].Value),
                    Height = int.Parse(match.Groups[2].Value)
                };
            }

            return null;
        }

        public static string ReadCDataNode(XmlElement parent, string name)
        {
            XmlNode node = parent.SelectSingleNode(name);
            if ((node != null) && node.HasChildNodes)
            {
                for (XmlNode node2 = node.FirstChild; node2 != null; node2 = node2.NextSibling)
                {
                    if (node2 is XmlCDataSection)
                    {
                        return node2.InnerText;
                    }
                }
            }
            return string.Empty;
        }

        public static string ReadTextNode(XmlElement parent, string name)
        {
            XmlNode node = parent.SelectSingleNode(name);
            if (node != null)
            {
                return node.InnerText;
            }
            return string.Empty;
        }

        public static void WriteCDataNode(XmlElement parent, string name, string value)
        {
            XmlElement newChild = parent.OwnerDocument.CreateElement(name);
            XmlCDataSection section = parent.OwnerDocument.CreateCDataSection(value);
            newChild.AppendChild(section);
            parent.AppendChild(newChild);
        }

        public static void WriteTextNode(XmlElement parent, string name, string value)
        {
            XmlElement newChild = parent.OwnerDocument.CreateElement(name);
            newChild.InnerText = value;
            parent.AppendChild(newChild);
        }
    }
}
