using Moq;
using Xunit;

namespace PopularNetLibraries.AutoMocker
{
    public class BasicAutoMockerTests
    {
        public interface ICarEngine { public void Start(); }
        
        public interface IAccelerator { public bool CanAccelerate(int speedMph); }
        
        class Car
        {
            private readonly ICarEngine _carEngine;
            private readonly IAccelerator _accelerator;
            public Car(ICarEngine carEngine, IAccelerator accelerator)
            {
                _carEngine = carEngine;
                _accelerator = accelerator;
            }
            public int Speed { get; set; }
            public ICarEngine GetCarEngine() => _carEngine;
            public void Start() { _carEngine.Start(); }

            public void Accelerate(int speedMph)
            {
                Speed = _accelerator.CanAccelerate(speedMph) ? speedMph : 50;
            }
        }

        [Fact]
        public void CreateCarWithEngine()
        {
            var mocker = new global::Moq.AutoMock.AutoMocker();
            var car = mocker.CreateInstance<Car>();

            var carEngineMock = mocker.GetMock<ICarEngine>();
            car.Start();
            
            Assert.NotNull(car.GetCarEngine());
            Assert.IsAssignableFrom<ICarEngine>(car.GetCarEngine());
            carEngineMock.Verify( engine => engine.Start(), Times.Once);
        }

        [Fact]
        public void CreateCarWithEngineAndAccelerator()
        {
            var mocker = new global::Moq.AutoMock.AutoMocker();
            var speedMph = 42;
            mocker.Use<IAccelerator>(x => x.CanAccelerate(speedMph) == true);

            var car = mocker.CreateInstance<Car>();
            car.Accelerate(speedMph);
            
            Assert.Equal(speedMph, car.Speed);
            mocker.VerifyAll(); // Verifies all mocks in the container (mocker)
        }
    }
}