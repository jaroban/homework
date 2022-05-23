using FluentAssertions;
using Homework;
using Homework.Readers;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace HomeworkTests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("Document1.xml")]
        [InlineData("Document1.json")]
        public async Task ItShouldReadInputFile(string fileName)
        {
            var reader = new FileSystemReader();
            var result = await reader.ReadFile(fileName);

            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData("Document1.xml", "../../../ConvertedXmlToJson.json")]
        [InlineData("Document1.json", "../../../ConvertedJsonToXml.xml")]
        [InlineData("https://www.w3schools.com/xml/cd_catalog.xml", "../../../DownloadedXmlToJson.json")]
        [InlineData("https://raw.githubusercontent.com/LearnWebCode/json-example/master/pet-of-the-day.json", "../../../DownloadedJsonToXml.xml")]
        public async Task ItShouldConvertFile(string input, string output)
        {
            File.Delete(output);

            var readConvertWriteService = new ReadConvertWriteService(new NullLoggerFactory());
            var result = await readConvertWriteService.ReadConvertWrite(input, output);

            result.Should().Be(true);
            File.Exists(output).Should().Be(true);
        }
    }
}