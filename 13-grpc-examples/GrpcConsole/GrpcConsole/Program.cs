using Grpc.Net.Client;
using TestGrpc2;

using var channel = GrpcChannel.ForAddress("http://localhost:5098");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(new HelloRequest { Name = "Test"});
var replyStream = client.SayHelloServerStream(new HelloRequest { Name = "Test"});
while (await replyStream.ResponseStream.MoveNext(CancellationToken.None)) {
    Console.WriteLine(replyStream.ResponseStream.Current);
}

Console.WriteLine(reply);