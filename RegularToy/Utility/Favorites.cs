using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml;

namespace RegularToy
{
    public class Favorite
    {
        public string Name { get; set; }

        public string Regular { get; set; }
        
        public string Description { get; set; }
    }

    public static class Favorites
    {
        static Favorites()
        {
            List = new ObservableCollection<Favorite>();
        }

        public static ObservableCollection<Favorite> List { get; private set; }

        public static bool IsLoaded { get; private set; }

        public static string DefaultFilename
        {
            get
            {
                return Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RegularToy\\Favorites.xml");
            }
        }

        public static void Initialize()
        {
            Load(DefaultFilename);
        }

        public static void Load(string filename)
        {
            if (!string.IsNullOrEmpty(filename) || File.Exists(filename))
            {
                XmlDocument document = new XmlDocument();
                try
                {
                    document.Load(filename);
                }
                catch
                {
                    return;
                }

                List.Clear();
                IsLoaded = true;

                XmlElement documentElement = document.DocumentElement;
                if (!(documentElement.Name != "favorites"))
                {
                    foreach (XmlElement element2 in documentElement.SelectNodes("item"))
                    {
                        Favorite item = new Favorite();
                        item.Name = Str.ReadTextNode(element2, "name");
                        item.Description = Str.ReadTextNode(element2, "description");
                        item.Regular = Str.ReadCDataNode(element2, "expression");
                        List.Add(item);
                    }
                }
            }
        }

        public static void Save()
        {
            Save(DefaultFilename);
        }

        public static void Save(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml("<?xml version='1.0' ?><favorites></favorites>");
                XmlElement documentElement = document.DocumentElement;
                foreach (var favorite in List)
                {
                    XmlElement parent = document.CreateElement("item");
                    Str.WriteTextNode(parent, "name", favorite.Name);
                    Str.WriteTextNode(parent, "description", favorite.Description);
                    Str.WriteCDataNode(parent, "expression", favorite.Regular);
                    documentElement.AppendChild(parent);
                }

                var dir = Path.GetDirectoryName(filename);
                if(!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                document.Save(filename);
            }
        }
    }
}
