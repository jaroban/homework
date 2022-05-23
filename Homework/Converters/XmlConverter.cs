namespace Homework.Converters;

using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

public class XmlConverter: IDocumentConverter
{
    public string Serialize(Document input)
    {
        var xmlSerializer = new XmlSerializer(typeof(Document));
        using var stringWriter = new StringWriter();
        using XmlWriter xmlWriter = XmlWriter.Create(stringWriter);
        xmlSerializer.Serialize(xmlWriter, input);
        return stringWriter.ToString();
    }

    public Document? Deserialize(string input)
    {
        try
        {
            var xdoc = XDocument.Parse(input);
            if (xdoc is not null)
            {
                return new Document
                {
                    Title = xdoc.Root?.Element("title")?.Value,
                    Text = xdoc.Root?.Element("text")?.Value
                };
            }
        }
        catch
        {
            // suppress exceptions
        }
        return null;
    }
}
