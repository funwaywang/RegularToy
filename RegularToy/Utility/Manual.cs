using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace RegularToy
{
    public class ManualItem
    {
        public string Syntax { get; set; }
        public string Description { get; set; }
    }

    public static class Manual
    {
        static Manual()
        {
            Items = new List<ManualItem>();
        }
        
        public static List<ManualItem> Items { get; private set; }

        public static string Description { get; private set; }

        public static void Initialize()
        {
            Items.Clear();

            var streamInfo = Application.GetResourceStream(new Uri("/Resources/Manual.xml", UriKind.Relative));
            if (streamInfo == null || streamInfo.Stream == null)
                return;

            XmlDocument dom = new XmlDocument();
            dom.Load(streamInfo.Stream);

            //
            XmlNode descNode = dom.DocumentElement.SelectSingleNode("description");
            if (descNode != null)
            {
                Description = descNode.InnerText;
            }

            //
            XmlNode node = dom.DocumentElement.SelectSingleNode("items");
            if (node != null)
            {
                XmlElement item = (XmlElement)node.FirstChild;
                while (item != null)
                {
                    string syntax = GetNodeText(item, "syntax");
                    string function = GetNodeText(item, "function");
                    Items.Add(new ManualItem()
                    {
                        Syntax = syntax,
                        Description = function
                    });

                    item = (XmlElement)item.NextSibling;
                }
            }
        }
        
        private static string GetNodeText(XmlElement item, string name)
        {
            XmlNode node = item.SelectSingleNode(name);
            if (node != null)
                return node.InnerText.Trim();
            else
                return string.Empty;
        }
    }
}
