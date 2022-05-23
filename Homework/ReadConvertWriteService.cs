namespace Homework;

using Homework.Converters;
using Homework.Readers;
using Homework.Writers;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class ReadConvertWriteService: IReadConvertWriteService
{
    private ILogger<ReadConvertWriteService> logger;

    public ReadConvertWriteService(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<ReadConvertWriteService>();
    }

    public static IDocumentConverter? GetConverter(string fileName)
    {
        var extension = fileName.Split('.').LastOrDefault()?.ToLower();

        switch (extension)
        {
            case "xml":
                return new XmlConverter();

            case "json":
                return new JsonConverter();
        }
        return null;
    }

    public async Task<bool> ReadConvertWrite(string inputFilePath, string outputFilePath)
    {
        IFileReader reader;

        // read
        if(inputFilePath.StartsWith("http"))
        {
            reader = new WebReader();
        }
        else
        {
            reader = new FileSystemReader();
        }

        string fileContent;

        try
        {
            fileContent = await reader.ReadFile(inputFilePath);
        }
        catch(Exception ex)
        {
            logger.LogError(ex.Message);
            return false;
        }

        // convert
        var inputConverter = GetConverter(inputFilePath);
        if(inputConverter is null)
        {
            logger.LogError("Can't find converter for input file {InputFile}", inputFilePath);
            return false;
        }

        var outputConverter = GetConverter(outputFilePath);
        if (outputConverter is null)
        {
            logger.LogError("Can't find converter for output file {OutputFile}", outputFilePath);
            return false;
        }

        var document = inputConverter.Deserialize(fileContent);
        if(document is null)
        {
            logger.LogError("Couldn't deserialize document from {InputFile}", inputFilePath);
            return false;
        }

        var converted = outputConverter.Serialize(document);

        // write
        var writer = new FileSystemWriter();

        try
        {
            await writer.WriteFile(outputFilePath, converted);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return false;
        }

        return true;
    }
}
