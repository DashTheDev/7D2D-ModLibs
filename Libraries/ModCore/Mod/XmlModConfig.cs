using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
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
            object? sourceConfig = serialiser.Deserialize(reader);

            foreach (PropertyInfo sourceProp in configClassType.GetProperties())
            {
                PropertyInfo? targetProp = configClassType.GetProperty(sourceProp.Name);

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
            if (File.Exists(FilePath))
            {
                SaveExisting();
            }
            else
            {
                SaveNew();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"[{nameof(XmlModConfig)}] Unable to save {FileName}!", ex);
        }
    }

    private void SaveNew()
    {
        XmlSerializer serialiser = new(GetType());
        XmlSerializerNamespaces namespaces = new();
        namespaces.Add(string.Empty, string.Empty);

        using StreamWriter writer = new(FilePath);
        serialiser.Serialize(writer, this, namespaces);
    }

    private void SaveExisting()
    {
        XDocument document = XDocument.Load(FilePath, LoadOptions.PreserveWhitespace);
        XElement root = document.Root ?? throw new InvalidOperationException($"{FileName} has no root element.");

        XmlSerializationUtility.UpdateElement(root, this);

        using StreamWriter writer = new(FilePath);
        document.Save(writer, SaveOptions.DisableFormatting);
    }
}

internal static class XmlSerializationUtility
{
    private static readonly HashSet<Type> SimpleTypes =
    [
        typeof(string), typeof(decimal), typeof(DateTime), typeof(DateTimeOffset), typeof(Guid), typeof(TimeSpan)
    ];

    public static void UpdateElement(XElement element, object instance)
    {
        foreach (PropertyInfo property in instance.GetType().GetProperties())
        {
            if (!property.CanRead || !property.CanWrite || Attribute.IsDefined(property, typeof(XmlIgnoreAttribute)))
            {
                continue;
            }

            object? value = property.GetValue(instance);
            XElement? existing = element.Element(property.Name);

            if (existing is not null && value is not null && IsComplexObjectType(property.PropertyType))
            {
                UpdateElement(existing, value);
                continue;
            }

            XElement propertyElement = SerialiseElement(property.PropertyType, property.Name, value);

            if (existing is not null)
            {
                existing.ReplaceWith(propertyElement);
            }
            else
            {
                element.Add(propertyElement);
            }
        }
    }

    private static bool IsComplexObjectType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        if (type.IsPrimitive || type.IsEnum || SimpleTypes.Contains(type))
        {
            return false;
        }

        if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
        {
            return false;
        }

        return true;
    }

    private static XElement SerialiseElement(Type propertyType, string name, object? value)
    {
        XmlSerializer serialiser = new(propertyType, new XmlRootAttribute(name));
        XmlSerializerNamespaces namespaces = new();
        namespaces.Add(string.Empty, string.Empty);

        using MemoryStream stream = new();
        using (XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings { OmitXmlDeclaration = true }))
        {
            serialiser.Serialize(writer, value, namespaces);
        }

        stream.Position = 0;
        return XElement.Load(stream);
    }
}