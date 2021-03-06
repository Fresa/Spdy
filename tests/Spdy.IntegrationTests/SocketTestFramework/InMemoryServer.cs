﻿using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Spdy.Network;

namespace Spdy.IntegrationTests.SocketTestFramework
{
    internal class InMemoryServer : INetworkServer
    {
        private readonly ConcurrentQueue<INetworkClient> _clients =
            new();

        private readonly BufferBlock<INetworkClient> _waitingClients;

        private InMemoryServer(
            BufferBlock<INetworkClient> waitingClients)
        {
            _waitingClients = waitingClients;
        }

        public async Task<INetworkClient> WaitForConnectedClientAsync(
            CancellationToken cancellationToken = default)
        {
            var client = await _waitingClients
                .ReceiveAsync(cancellationToken)
                .ConfigureAwait(false);
            _clients.Enqueue(client);
            return client;
        }

        internal static InMemoryServer Start(
            BufferBlock<INetworkClient> waitingClients)
        {
            var server = new InMemoryServer(waitingClients);
            return server;
        }

        public async ValueTask DisposeAsync()
        {
            _waitingClients.Complete();

            while (_clients.TryDequeue(out var client))
            {
                await client
                    .DisposeAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}