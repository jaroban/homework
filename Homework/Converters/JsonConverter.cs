namespace Homework.Converters;

using System.Text.Json;

public class JsonConverter: IDocumentConverter
{
    public Document? Deserialize(string input)
    {
        return JsonSerializer.Deserialize<Document>(input);
    }

    public string Serialize(Document input)
    {
        return JsonSerializer.Serialize(input);
    }
}
