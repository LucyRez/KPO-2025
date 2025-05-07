using Grpc.Core;
using GrpcTestService;

namespace GrpcTestService.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly string[] messages = {"Hi!", "Hello!", "WUP?"};

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override async Task SayHelloServerStreaming(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
    {
        foreach (var item in messages)
        {
            await responseStream.WriteAsync(new HelloReply { Message = item});
            await Task.Delay(TimeSpan.FromSeconds(4));
        }
    }

    public override async Task<HelloReply> SayHelloClientStreaming(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
    {
        while (await requestStream.MoveNext()) {
            Console.WriteLine(requestStream.Current);
        }

        return new HelloReply { Message = "Finished!"};
    }

    public override Task SayHelloStreaming(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
    {
        return base.SayHelloStreaming(requestStream, responseStream, context);
    }
    
}
