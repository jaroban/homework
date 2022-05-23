namespace Homework.Converters;

public interface IDocumentConverter
{
    string Serialize(Document input);

    Document? Deserialize(string input);
}
