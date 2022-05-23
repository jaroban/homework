namespace Homework.Readers;

internal class WebReader: IFileReader
{
    public Task<string> ReadFile(string path)
    {
        var client = new HttpClient();
        return client.GetStringAsync(path);
    }
}
