using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace DashTheDev.SDTD.ModCore;

public abstract class XmlModConfig : BaseModConfig
{
    public override void Load()
    {
        if (!File.Exists(FilePath))
        {
            Save();
            return;
        }

        try
        {
            Type configClassType = GetType();
            using StreamReader reader = new(FilePath);
            XmlSerializer serialiser = new(configClassType);
            object sourceConfig = serialiser.Deserialize(reader);

            foreach (PropertyInfo sourceProp in configClassType.GetProperties())
            {
                PropertyInfo targetProp = configClassType.GetProperty(sourceProp.Name);

                if (targetProp is null || !targetProp.CanWrite || !targetProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                {
                    continue;
                }

                targetProp.SetValue(this, sourceProp.GetValue(sourceConfig));
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"[{nameof(XmlModConfig)}] Unable to load {FileName}!", ex);
        }
        finally
        {
            Save();
        }
    }

    public override void Save()
    {
        try
        {
            XmlSerializer serialiser = new(GetType());
            XmlSerializerNamespaces namespaces = new();
            namespaces.Add(string.Empty, string.Empty);

            using StreamWriter writer = new(FilePath);
            serialiser.Serialize(writer, this, namespaces);
        }
        catch (Exception ex)
        {
            throw new Exception($"[{nameof(XmlModConfig)}] Unable to save {FileName}!", ex);
        }
    }
}