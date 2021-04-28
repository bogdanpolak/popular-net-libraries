using Autofac;
using PopularNetLibraries.Autofac.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PopularNetLibraries.Autofac
{
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

        public void Execute(int Id)
        {
            var data = _dataRepository.GetData(Id);
            var isValid = data.Name.ToLower().Contains("code");
            var isLoggerEnabled = _configuration.GetOptionAsString("EnableLogger").ToLower().Contains("yes");
            if (!isValid && isLoggerEnabled)
            {
                _logger.Info($"[DataValidaton] Non valid Id provided: {Id}");
                RegisteredMessages++;
            }
        }
    }

    public class AutofacTests
    {
        [Fact]
        public void MyProcessExecute_WhenID_1()
        {
            MyProcessor _myProcessor;

            var containerBuilder = RegisterBasicTypes();

            var container = containerBuilder.Build();

            using (var lifetimeScope = container.BeginLifetimeScope())
            {
                _myProcessor = lifetimeScope.Resolve<MyProcessor>();
            }

            _myProcessor.Execute(1);

            Assert.Equal(0, _myProcessor.RegisteredMessages);
        }

        [Fact]
        public void MyProcessExecute_WhenID_2() 
        {
            var dataId = 2;
            var containerBuilder = RegisterBasicTypes();
            var container = containerBuilder.Build();

            using var lifetimeScope = container.BeginLifetimeScope();
            var myProcessor = lifetimeScope.Resolve<MyProcessor>();
            myProcessor.Execute(dataId);

            Assert.Equal(1, myProcessor.RegisteredMessages);
        }

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
