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

            repositoryMock.Setup(repo => repo.GetItem(0)).ReturnsAsync("Zero item");
            repositoryMock.Setup(repo => repo.GetItem(1)).ReturnsAsync("First item");
            repositoryMock.Setup(repo => repo.GetItem(2).Result).Returns("Second item");
            var repository = repositoryMock.Object;
            
            Assert.Equal("Zero item",await repository.GetItem(0));
            Assert.Equal("First item",await repository.GetItem(1));
            Assert.Equal("Second item",await repository.GetItem(2));
        }
    }
}