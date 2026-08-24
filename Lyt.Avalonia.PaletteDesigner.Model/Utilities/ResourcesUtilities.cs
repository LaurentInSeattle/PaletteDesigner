namespace Lyt.Avalonia.PaletteDesigner.Model.Utilities;

using System.Text.Json.Serialization.Metadata;

public static class ResourcesUtilities
{
    private static string ResourcesPath = "Lyt.Avalonia.PaletteDesigner.Resources";
    private static Assembly ExecutingAssembly;

    public const string ResourcesExtension = ".json";

    static ResourcesUtilities()
    {
        ExecutingAssembly = Assembly.GetExecutingAssembly();
    }

    public static void SetResourcesPath(string resourcePath )
        => ResourcesUtilities.ResourcesPath = resourcePath;

    public static void SetExecutingAssembly(Assembly executingAssembly)
    {
        ResourcesUtilities.ExecutingAssembly = executingAssembly;
        DumpEmbeddedResourceNames();
    }

    public static List<string> EnumerateEmbeddedResourceNames(string filter)
    {
        List<string> resourceNames = [];
        var list = ExecutingAssembly.GetManifestResourceNames().ToList();
        foreach (string name in list)
        {
            // if (name.Contains(ResourcesPath) && name.EndsWith(ResourcesExtension))
            if (name.Contains(ResourcesPath))
            {
                Debug.WriteLine(name);
                if ( ! string.IsNullOrWhiteSpace(filter ))
                {
                    if ( ! name.Contains(filter))
                    {
                        continue;
                    }
                }

                resourceNames.Add(name);
            }
        }

        return resourceNames;
    }

    public static string? GetFullResourceName(string name)
    {
        var resourceNames = ExecutingAssembly.GetManifestResourceNames().ToList();
        return resourceNames.Single(str => str.EndsWith(name));
    }

    public static string LoadEmbeddedTextResource(string name, out string? resourceName)
    {
        resourceName = ResourcesUtilities.GetFullResourceName(name);
        if (!string.IsNullOrEmpty(resourceName))
        {
            var stream = ExecutingAssembly.GetManifestResourceStream(resourceName);
            if (stream is not null)
            {
                using (stream)
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        throw new Exception("Failed to load resource: " + name);
    }

    public static byte[] LoadEmbeddedBinaryResource(string name, out string? resourceName)
    {
        resourceName = ResourcesUtilities.GetFullResourceName(name);
        if (!string.IsNullOrEmpty(resourceName))
        {
            var stream = ExecutingAssembly.GetManifestResourceStream(resourceName);
            if (stream is not null)
            {
                using (stream)
                {
                    byte[] bytes = new byte[stream.Length];
                    int bytesRead = stream.Read(bytes, 0, bytes.Length);
                    if (bytesRead != bytes.Length)
                    {
                        throw new Exception("Failed to read resource stream: " + name);
                    }

                    return bytes;
                }
            }
        }

        throw new Exception("Failed to load resource: " + name);
    }

    public static T Deserialize<T>(string serialized, JsonTypeInfo<T> jsonTypeInfo) where T : class
    {
        try
        {
            object? deserialized = JsonSerializer.Deserialize<T>(serialized, jsonTypeInfo);
            if (deserialized is T binaryObject)
            {
                return binaryObject;
            }

            throw new Exception();
        }
        catch (Exception ex)
        {
            string msg = "Failed to deserialize " + typeof(T).FullName + "\n" + ex.ToString();
            throw new Exception(msg, ex);
        }
    }

    [Conditional("DEBUG")]
    public static void DumpEmbeddedResourceNames()
    {
        List<string> resourceNames = [];
        var list = ExecutingAssembly.GetManifestResourceNames().ToList();
        foreach (string name in list)
        {
            Debug.WriteLine(name);

            // if (name.Contains(ResourcesPath) && name.EndsWith(ResourcesExtension))
            if (name.Contains(ResourcesPath))
            {
                Debug.WriteLine(name);
                resourceNames.Add(name);
            }
        }
    }
}