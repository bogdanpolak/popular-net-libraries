using Autofac;
using System;
using Xunit;

namespace PopularNetLibraries.Autofac
{
    internal class A { public B MyB { get; set; } }

    internal class B { }

    public class Calculator {
        public string Method { get; private set; }
        protected Calculator(string method) { Method = method; }
    }

    public class SimpleCalculator : Calculator { public SimpleCalculator() : base("simple") { } }

    public class ComplexCalculator : Calculator { public ComplexCalculator() : base("complex") {} }

    public class RegistrationTests
    {
        [Fact]
        public void Register_WithPropertyInjection()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<B>();
            builder.Register(
                context => new A() { MyB = context.Resolve<B>() });
                // use ResolveOptional if type could not been registered (returns dependency or NULL)

            var a = builder.Build().Resolve<A>();

            Assert.NotNull(a.MyB);
        }

        [Fact]
        public void Register_LambdaInjectByParameter()
        {
            var builder = new ContainerBuilder();
            builder.Register<Calculator>(
                (context, parameters) => parameters.Named<int>("CalculationMethod") switch
                    {
                        1 => new SimpleCalculator(),
                        2 => new ComplexCalculator(),
                        _ => throw new Exception("")
                    });
            // Better approach: use "delegate factory".

            var calculator = builder.Build().Resolve<Calculator>(new NamedParameter("CalculationMethod", 2));

            Assert.Equal("complex", calculator.Method);
        }
    }
}
