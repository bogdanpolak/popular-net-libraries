using Moq;
using Xunit;

namespace PopularNetLibraries.Moq
{
    public class VerifyMockSequence
    {
        public interface IMultiManager
        {
            void MethodA();
            void MethodB();
            void MethodC();
        }
        
        [Fact]
        public void Verify_ThreeMethodsSequence_WhenCallsWasInOrder()
        {
            var managerMock = new Mock<IMultiManager>(MockBehavior.Strict);
            var sequence = new MockSequence();
            
            managerMock.InSequence(sequence).Setup(manager => manager.MethodA());
            managerMock.InSequence(sequence).Setup(manager => manager.MethodB());
            managerMock.InSequence(sequence).Setup(manager => manager.MethodC());
            
            managerMock.Object.MethodA();
            managerMock.Object.MethodB();
            managerMock.Object.MethodC();

            managerMock.VerifyAll();
        }
        
        [Fact]
        public void Verify_ThreeMethodsSequence_Exception_WhenOrderIncorrect()
        {
            var managerMock = new Mock<IMultiManager>(MockBehavior.Strict);
            var sequence = new MockSequence();
            
            managerMock.InSequence(sequence).Setup(manager => manager.MethodA());
            managerMock.InSequence(sequence).Setup(manager => manager.MethodB());
            managerMock.InSequence(sequence).Setup(manager => manager.MethodC());

            managerMock.Object.MethodA();
            Assert.Throws<MockException>(() =>
                managerMock.Object.MethodC());
        }
    }
}