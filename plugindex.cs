using System.Collections.Generic;
using PluginContracts;
using PluginLoader;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.IO.Compression;
using System.Reflection;

namespace PlugIndex
{
    class plugindex
    {
        static void Main(string[] args)
        {

            SerializableDictionary<string, string> PluginDetails = new SerializableDictionary<string, string>();

            List<string> dllfiles = Directory.EnumerateFiles("Plugins", "*.dll",
                                              SearchOption.TopDirectoryOnly)
                       .Where(n => Path.GetExtension(n) == ".dll").ToList();

            

            ICollection<Plugin> plugins = GenericPluginLoader<Plugin>.UnsafeLoadPlugins("Plugins/");

            foreach (var plug in plugins)
            {
                if (!PluginDetails.ContainsKey(plug.PluginName))
                {
                    Console.WriteLine("\tPlugin Found: " + plug.PluginName + " (" + plug.PluginVersion + ")");
                    PluginDetails.Add(plug.PluginName, plug.PluginVersion);
                    System.Threading.Thread.Sleep(50);
                }
            }


            XmlSerializer serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));

            using (TextWriter textWriter = new StreamWriter("BUTTER-Installed-Plugins.xml"))
            {
                serializer.Serialize(textWriter, PluginDetails);
                textWriter.Close();
            }

        }
    }
}
