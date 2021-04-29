using LanguageExt;
using static LanguageExt.Prelude;
using PopularNetLibraries.Autofac.Sample;
using System;
using System.IO;
using Autofac;
using Xunit;

namespace PopularNetLibraries.Autofac
{
    class AppConfiguration2 : IConfiguration
    {
        private readonly string _initialStartDate;
        private readonly string _raportingFolder;

        public AppConfiguration2(DateTime initialStartDate, string raportingFolder)
        {
            _initialStartDate = initialStartDate.ToString("yyyy-MM-dd");
            _raportingFolder = raportingFolder;
        }

        public Option<string> GetOptionAsString(string optionName)
        {
            return optionName.ToLower() switch
            {
                "connectionstring" => @"Data Source=.\SQLEXPRESS;Initial Catalog=TirexDB;Integrated Security=True",
                "initialstartdate" => _initialStartDate,
                "reportsuploadfolder" => _raportingFolder,
                _ => None,
            };
        }
    }

    class Processor
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
            _configuration.GetOptionAsString("ReportSuploadFolder")
                .Match(Some: folderPath => WiteDataToTextFile(folderPath),
                        None: () => _logger.Warning("Not able to read Configuration[ReportSuploadFolder]"));
        }

        private void WiteDataToTextFile(string folderPath)
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
        private static string RaportingTempFolder => Path.Combine(TestFolderPath, "Reports");

        public RegistrationOfInstanceTests()
        {
            Directory.CreateDirectory(RaportingTempFolder);
        }

        ~RegistrationOfInstanceTests()
        {
            Directory.Delete(TestFolderPath, true);
        }

        [Fact]
        public void Processor_GivenInstanceConfiguration()
        {
            var appConfiguration = new AppConfiguration2(new DateTime(2020,11,30), RaportingTempFolder);

            var containerBuilder = RegisterBasicTypes();
            // ------------------------------------------------------------------------
            containerBuilder.RegisterInstance(appConfiguration).As<IConfiguration>();
            // ------------------------------------------------------------------------
            containerBuilder
                .Build()
                .Resolve<Processor>()
                .Run();

            var expectedFile = Path.Combine(RaportingTempFolder, "store.txt");
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
