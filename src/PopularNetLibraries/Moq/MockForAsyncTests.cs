using System.Threading.Tasks;
using Moq;
using Xunit;

namespace PopularNetLibraries.Moq
{
    public class MockForAsyncTests
    {
        public interface IRepository
        {
            Task<string> GetItem(int id);
        }
        
        [Fact]
        public async Task SetupAndVerify_AsyncMethod_InMock()
        {
            var repositoryMock = new Mock<IRepository>();

            repositoryMock.Setup(repo => repo.GetItem(0).Result).Returns("Zero item");
            repositoryMock.Setup(repo => repo.GetItem(1).Result).Returns("First item");
            var repository = repositoryMock.Object;
            
            Assert.Equal("Zero item",await repository.GetItem(0));
            Assert.Equal("First item",await repository.GetItem(1));
        }
    }
}