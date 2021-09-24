using Moq;
using Xunit;

namespace PopularNetLibraries.Moq
{
    public class VerifyBehaviourTests
    {
        public interface IManager
        {
            void ExecuteAll();
            void ExecuteOne(string item);
        }

        public const string CrashParts = "crash-parts";
        public const string Tires = "tires";
        
        [Fact]
        public void ExecuteAll_WhenCalled()
        {
            var managerMock = new Mock<IManager>();
            
            managerMock.Object.ExecuteAll();
            
            managerMock.Verify(manager => manager.ExecuteAll());
        }
        
        [Fact]
        public void ExecuteAll_WhenNotCalled()
        {
            var managerMock = new Mock<IManager>();

            Assert.Throws<MockException>(() =>
                managerMock.Verify(manager => manager.ExecuteAll())
            );
        }

        [Fact]
        public void ExecuteOne_WhenItemMatching()
        {
            var managerMock = new Mock<IManager>();
            
            managerMock.Object.ExecuteOne(CrashParts);
            
            managerMock.Verify(manager => manager.ExecuteOne(CrashParts));
        }
        
        [Fact]
        public void ExecuteOne_WhenItemIsNotMatching()
        {
            var managerMock = new Mock<IManager>();
            
            managerMock.Object.ExecuteOne(Tires);
            
            Assert.Throws<MockException>(() =>
                managerMock.Verify(manager => manager.ExecuteOne(CrashParts))
            );
        }

        [Fact]
        public void Verify_ExecuteOne_WasNotCalled()
        {
            var managerMock = new Mock<IManager>();

            managerMock.Object.ExecuteAll();

            Assert.Throws<MockException>(() =>
                managerMock.Verify(manager => manager.ExecuteAll(), Times.Never())
            );
        }

        [Fact]
        public void Verify_BothExecuteMethods_WasCalledAtLeastOnce()
        {
            var managerMock = new Mock<IManager>();
            
            managerMock.Object.ExecuteAll();
            managerMock.Object.ExecuteOne(Tires);
            managerMock.Object.ExecuteOne(CrashParts);
            
            managerMock.Verify(manager => manager.ExecuteAll(), Times.AtLeastOnce());
            managerMock.Verify(manager => manager.ExecuteOne(It.IsAny<string>()), Times.AtLeastOnce());
        }
        
        [Fact]
        public void Verify_OnlyExecuteAll_WasCalled()
        {
            var managerMock = new Mock<IManager>();
            
            managerMock.Object.ExecuteAll();
            managerMock.Object.ExecuteAll();
            
            managerMock.Verify(manager => manager.ExecuteAll());
            managerMock.VerifyNoOtherCalls();
        }
        
    }
}