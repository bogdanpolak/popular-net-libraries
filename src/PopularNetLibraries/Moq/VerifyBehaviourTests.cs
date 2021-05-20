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
        
        // TODO: Add more samples for behaviour verification (see bellow)
        /*
        mock.Verify(foo => foo.DoSomething("ping"), Times.Never());
        mock.Verify(foo => foo.DoSomething("ping"), Times.AtLeastOnce());
        mock.VerifyNoOtherCalls();
        */
    }
}