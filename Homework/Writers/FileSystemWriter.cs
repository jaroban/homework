namespace Homework.Writers;

public class FileSystemWriter: IFileWriter
{
    public Task WriteFile(string path, string content)
    {
        return File.WriteAllTextAsync(path, content);
    }
}
