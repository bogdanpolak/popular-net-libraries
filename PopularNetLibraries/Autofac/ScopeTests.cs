using Autofac;
using System.Linq;
using Xunit;

namespace PopularNetLibraries.Autofac
{
    interface IElement
    {
        int Increment();
    }

    class Element : IElement
    {
        private int _counter = 0;

        public int Increment() => ++_counter;
    }

    public class ScopeTests
    {
        readonly ContainerBuilder _containerBuilder = new ContainerBuilder();

        [Fact] 
        public void InstancePerDependency()
        {
            _containerBuilder.RegisterType<Element>()
                .As<IElement>();
                // default: .InstancePerDependency;
            var container = _containerBuilder.Build();

            var actual = TestScopeTimes(container, 10);

            Assert.Equal(1, actual);
        }

        [Fact]
        public void SingleInstance()
        {
            _containerBuilder.RegisterType<Element>()
                .As<IElement>()
                .SingleInstance();
            var container =  _containerBuilder.Build();

            var _ = TestScopeTimes(container, 5);
            var actual = TestScopeTimes(container, 5);

            Assert.Equal(10, actual);
        }

        [Fact]
        public void InstancePerLifetimeScope()
        {
            _containerBuilder.RegisterType<Element>()
                .As<IElement>()
                .InstancePerLifetimeScope();
            var container = _containerBuilder.Build();

            var _ = TestScopeTimes(container, 5);
            var actual = TestScopeTimes(container, 5);

            Assert.Equal(5, actual);
        }
        

        private static int TestScopeTimes(IContainer container, int times)
        {
            using var scope = container.BeginLifetimeScope();
            var maxValue = Enumerable.Range(1, times).ToList()
                .Max(i => scope.Resolve<IElement>().Increment());
            return maxValue;
        }
    }
}
