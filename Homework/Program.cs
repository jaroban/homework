using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using CommandLine;
using Homework.Converters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Homework
{
    class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file to be converted.")]
        public string InputFile { get; set; } = string.Empty;

        [Option('o', "output", Required = true, HelpText = "Output file.")]
        public string OutputFile { get; set; } = string.Empty;
    }

    class Program
    {
        static int Main(string[] args)
        {
            // setup DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Debug)
                        .AddFilter("System", LogLevel.Debug)
                        .AddFilter("Homework.Program", LogLevel.Debug)
                        .AddConsole();
                })
                .AddSingleton<IReadConvertWriteService, ReadConvertWriteService>()
                .BuildServiceProvider();

            using var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            if(loggerFactory is null)
            {
                return 1;
            }

            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogDebug("Starting application");

            var readConvertWriteService = serviceProvider.GetService<IReadConvertWriteService>();
            if(readConvertWriteService is null)
            {
                logger.LogError("DI not set up correctly");
                return 1;
            }
            
            int result = 0;

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(async options =>
                {
                    try
                    {
                        result = await readConvertWriteService.ReadConvertWrite(options.InputFile, options.OutputFile) ? 0 : 1;
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                })
                .WithNotParsed(errors =>
                {
                    // handle errors
                    foreach (var error in errors)
                    {
                        logger.LogError(error.ToString());
                    }
                });

            return result;
        }
    }
}