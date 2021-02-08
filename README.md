# Spdy
A client/server implementation of the [Spdy 3.1 protocol](https://www.chromium.org/spdy/spdy-protocol/spdy-protocol-draft3-1).

![Continuous Delivery](https://github.com/Fresa/Spdy/workflows/Continuous%20Delivery/badge.svg)

## Installation
```bash
dotnet add package Spdy
```

## Usage
The library can act both as a client and a server. Both are un-opiniated about what transport medium is used and accepts a `INetworkClient` that needs to be implemented.

### Client
The following example creates a client session and a stream, sends "Hello world!" to the server, closes its endpoint and then reads any responses from the server until it closes its endpoint.
```c#
var networkClient = ...;
await using var session = SpdySession.CreateClient(networkClient);
using var stream = session.CreateStream();

FlushResult flushResult;
do
{
    flushResult = await stream.SendLastAsync(
        new ReadOnlyMemory<byte>(
            Encoding.UTF8.GetBytes("Hello world!")));
} while (!flushResult.IsCompleted);

ReadResult readResult;
do
{
    readResult = await stream.ReceiveAsync();
    Console.WriteLine(
        Encoding.UTF8.GetString(readResult.Buffer.ToArray()));
} while (!readResult.IsCompleted);
```

### Server
The following example creates a server session which starts listening for a stream created by a client, sends "Hello world!", closes its endpoint and then reads any responses from the client until it closes its endpoint.
```c#
var networkClient = ...;
await using var session = SpdySession.CreateServer(networkClient);
using var stream = await session.ReceiveStreamAsync();

FlushResult flushResult;
do
{
    flushResult = await stream.SendLastAsync(
        new ReadOnlyMemory<byte>(
            Encoding.UTF8.GetBytes("Hello world!")));
} while (!flushResult.IsCompleted);

ReadResult readResult;
do
{
    readResult = await stream.ReceiveAsync();
    Console.WriteLine(
        Encoding.UTF8.GetString(readResult.Buffer.ToArray()));
} while (!readResult.IsCompleted);
```

### Logging
Logging can be enabled by calling `Spdy.Logging.LogFactory.TryInitializeOnce(ILogFactory logFactory)` with an implementation of `ILogFactory`. Since this method is static it will only set the first log factory it is called with and ignore all other calls.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://github.com/Fresa/Spdy/blob/master/LICENSE)
