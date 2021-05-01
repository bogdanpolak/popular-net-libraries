using LanguageExt;
using static LanguageExt.Prelude;
using PopularNetLibraries.Autofac.Sample;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Autofac;
using Xunit;

namespace PopularNetLibraries.Autofac
{
    class AppConfiguration2 : IConfiguration
    {
        private readonly string _initialStartDate;
        private readonly string _reportingFolder;

        public AppConfiguration2(DateTime initialStartDate, string reportingFolder)
        {
            _initialStartDate = initialStartDate.ToString("yyyy-MM-dd");
            _reportingFolder = reportingFolder;
        }

        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public Option<string> GetOptionAsString(string optionName)
        {
            return optionName.ToLower() switch
            {
                "connectionstring" => @"Data Source=.\SQLEXPRESS;Initial Catalog=TirexDB;Integrated Security=True",
                "initialstartdate" => _initialStartDate,
                "reportsuploadfolder" => _reportingFolder,
                _ => None,
            };
        }
    }

    public class Processor
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public Processor(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Run()
        {
            _configuration.GetOptionAsString("ReportsUploadFolder")
                .Match(Some: WriteDataToTextFile,
                        None: () => _logger.Warning("Not able to read Configuration[ReportsUploadFolder]"));
        }

        private void WriteDataToTextFile(string folderPath)
        {
            var initialStartDate = _configuration.GetOptionAsString("InitialStartDate")
                .IfNone("");
            if (!Directory.Exists(folderPath))
            {
                _logger.Warning($"Folder doesn't exists. Can't write to: {folderPath}");
                return;
            }
            File.WriteAllText(Path.Combine(folderPath,"store.txt"), $"InitialDate={initialStartDate}");
        }
    }

    public class RegistrationOfInstanceTests
    {
        private static string TestFolderPath => Path.Combine(Path.GetTempPath(), "AdaptiveSoft");
        private static string ReportingTempFolder => Path.Combine(TestFolderPath, "Reports");

        public RegistrationOfInstanceTests()
        {
            Directory.CreateDirectory(ReportingTempFolder);
        }

        ~RegistrationOfInstanceTests()
        {
            Directory.Delete(TestFolderPath, true);
        }

        [Fact]
        public void Processor_GivenInstanceConfiguration()
        {
            var appConfiguration = new AppConfiguration2(new DateTime(2020,11,30), ReportingTempFolder);

            var containerBuilder = RegisterBasicTypes();
            // ------------------------------------------------------------------------
            containerBuilder.RegisterInstance(appConfiguration).As<IConfiguration>();
            // ------------------------------------------------------------------------
            containerBuilder
                .Build()
                .Resolve<Processor>()
                .Run();

            var expectedFile = Path.Combine(ReportingTempFolder, "store.txt");
            var isExists = File.Exists(expectedFile);
            Assert.True( isExists );
        }

        private static ContainerBuilder RegisterBasicTypes()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FakeLogger>().As<ILogger>().SingleInstance();
            builder.RegisterType<Processor>();
            return builder;
        }
    }
}
