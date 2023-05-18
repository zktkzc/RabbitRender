using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace Rabbit_core.Tools;

public static class SerializeHelper
{
    public static void Serialize<T>(T t, string path)
    {
        DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
        using XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        dataContractJsonSerializer.WriteObject(writer, t);
    }

    public static void Deserialize<T>(string path, out T? t)
    {
        DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
        using XmlTextReader reader = new XmlTextReader(path);
        t = (T?)dataContractJsonSerializer.ReadObject(reader);
    }
}