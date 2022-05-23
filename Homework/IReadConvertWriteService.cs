namespace Homework;

public interface IReadConvertWriteService
{
    Task<bool> ReadConvertWrite(string inputFilePath, string outputFilePath);
}
