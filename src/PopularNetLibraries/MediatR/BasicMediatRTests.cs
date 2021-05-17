using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Xunit;

namespace PopularNetLibraries.MediatR
{
    public class BasicMediatRTests
    {
        public class PingRequest : IRequest<PongResponse> { public string Message; }

        public class PongResponse { public string Message; }
        
        public class PingHandler : IRequestHandler<PingRequest, PongResponse>
        {
            public Task<PongResponse> Handle(PingRequest request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new PongResponse { Message = request.Message + "-pong" });
            }
        }

        private readonly Assembly _myAssembly = typeof(PingRequest).GetTypeInfo().Assembly;
        
        [Fact]
        public async Task SendPing_WithResultPingPong()
        {
            var mediator = MediatorBuilder.Build(_myAssembly);
            var result = await mediator.Send(new PingRequest{Message = "ping"});
            Assert.Equal("ping-pong",result.Message);
        }
    }
}