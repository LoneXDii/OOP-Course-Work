using System.Reflection;

namespace Server.API.SerialzationLib;

public class MySerializer<T> where T : class
{
    private static MySerializer<T>? instance = null;

    private MethodInfo? serializeJson;
    private MethodInfo? deserializeJson;
    private MethodInfo? serializeToFileJson;
    private MethodInfo? deserializeFromFileJson;
    private MethodInfo? serializeToFileXML;
    private MethodInfo? deserializeFromFileXML;

    private MySerializer()
    {
        Assembly asm = Assembly.LoadFrom("OOPSerializationLib.dll");

        Type? jsonSer = asm.GetType("OOPSerializationLib.MyJsonSerializer`1");
        Type? xmlSer = asm.GetType("OOPSerializationLib.MyXMLSerializer`1");

        if (jsonSer is null || xmlSer is null)
        {
            throw new DllNotFoundException();
        }

        jsonSer = jsonSer.MakeGenericType(typeof(T));

        serializeJson = jsonSer?.GetMethod("Serialize", [typeof(T)]);
        deserializeJson = jsonSer?.GetMethod("Deserialize", [typeof(string)]);
        serializeToFileJson = jsonSer?.GetMethod("SerializeToFile", [typeof(T), typeof(FileStream)]);
        deserializeFromFileJson = jsonSer?.GetMethod("DeserializeFromFile", [typeof(FileStream)]);

        xmlSer = xmlSer.MakeGenericType(typeof(T));

        serializeToFileXML = xmlSer?.GetMethod("Serialize", [typeof(T), typeof(FileStream)]);
        deserializeFromFileXML = xmlSer?.GetMethod("Deserialize", [typeof(FileStream)]);
    }

    public static MySerializer<T> GetInstance()
    {
        if(instance is null)
        {
            instance = new MySerializer<T>();
        }
        return instance;
    }

    public string? SerializeJson(T obj)
    {
        if (serializeJson is null)
        {
            throw new DllNotFoundException();
        }

        return (string?)serializeJson.Invoke(null, [obj]);
    }

    public T? DeserializeJson(string json)
    {
        if (deserializeJson is null)
        {
            throw new DllNotFoundException();
        }

        return (T?)deserializeJson.Invoke(null, [json]);
    }

    public void SerializeToFileJson(T obj, string filename)
    {
        using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
        {
            serializeToFileJson?.Invoke(null, [obj, fs]);
        }
    }

    public T? DeserializeFromFileJson(string filename)
    {
        T? obj = null;
        using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
        {
            obj = (T?)deserializeFromFileJson?.Invoke(null, [fs]);
        }
        return obj;
    }

    public void SerializeToFileXml(T obj, string filename)
    {
        using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
        {
            serializeToFileXML?.Invoke(null, [obj, fs]);
        }
    }

    public T? DeserializeFromFileXml(string filename)
    {
        T? obj = null;
        using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
        {
            obj = (T?)deserializeFromFileXML?.Invoke(null, [fs]);
        }
        return obj;
    }
}
