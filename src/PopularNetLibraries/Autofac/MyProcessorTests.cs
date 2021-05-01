using Autofac;
using PopularNetLibraries.Autofac.Sample;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace PopularNetLibraries.Autofac
{
    [ExcludeFromCodeCoverage]
    public class MyProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IDataRepository _dataRepository;

        public int RegisteredMessages { get; set; } = 0;

        public MyProcessor(IConfiguration configuration, ILogger logger, IDataRepository dataRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _dataRepository = dataRepository;
        }

        public void Execute(int id)
        {
            var data = _dataRepository.GetData(id);
            var isValid = data.Name.ToLower().Contains("code");
            var isLoggerEnabled = _configuration.GetOptionAsString("EnableLogger")
                .IfNone("no")
                .ToLower()
                .Contains("yes");
            if (!isValid && isLoggerEnabled)
            {
                _logger.Info($"[DataValidation] Non valid Id provided: {id}");
                RegisteredMessages++;
            }
        }
    }

    public class MyProcessorTests
    {
        [Fact]
        public void Execute_GivenID_1()
        {
            MyProcessor myProcessor;

            var containerBuilder = RegisterBasicTypes();

            var container = containerBuilder.Build();

            using (var lifetimeScope = container.BeginLifetimeScope())
            {
                myProcessor = lifetimeScope.Resolve<MyProcessor>();
            }

            myProcessor.Execute(1);

            Assert.Equal(0, myProcessor.RegisteredMessages);
        }

        [Fact]
        public void Execute_GivenID_2()
        {
            var dataId = 2;
            var containerBuilder = RegisterBasicTypes();
            var container = containerBuilder.Build();

            using var lifetimeScope = container.BeginLifetimeScope();
            var myProcessor = lifetimeScope.Resolve<MyProcessor>();
            myProcessor.Execute(dataId);

            Assert.Equal(1, myProcessor.RegisteredMessages);
        }

        [Fact] 
        public void Execute_ResolveDirect() 
        {
            // Direct recolving instances from container is NOT recomended
            // https://autofaccn.readthedocs.io/en/latest/lifetime/working-with-scopes.html
            CompositionRoot()
                .Resolve<MyProcessor>()
                .Execute(1);

            Assert.True(true);
        }

        private static IContainer CompositionRoot() => RegisterBasicTypes().Build();

        private static ContainerBuilder RegisterBasicTypes()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AppConfiguration>().As<IConfiguration>();
            builder.RegisterType<FakeLogger>().As<ILogger>().SingleInstance();
            builder.RegisterType<DataRepository>().As<IDataRepository>();
            builder.RegisterType<MyProcessor>();
            return builder;
        }
    }
}
