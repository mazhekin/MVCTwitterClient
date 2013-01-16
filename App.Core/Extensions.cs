using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace App.Core
{
    public static class Extensions
    {
        public static string Serialize<T>(this Object obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, obj);
            return writer.ToString();
        }

        public static T Deserialize<T>(this String xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                XmlReaderSettings settings = new XmlReaderSettings();
                // No settings need modifying here

                using (StringReader textReader = new StringReader(xml))
                {
                    using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                    {
                        return (T)serializer.Deserialize(xmlReader);
                    }
                }
            } 
            catch 
            {
                return default(T);
            }
        }

    }
}
