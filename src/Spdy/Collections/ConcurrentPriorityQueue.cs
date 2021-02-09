using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Spdy.Frames;

namespace Spdy.Collections
{
    internal sealed class ConcurrentPriorityQueue<T>
    {
        private readonly Dictionary<SynStream.PriorityLevel,
            ConcurrentQueue<(T, TaskCompletionSource)>> _priorityQueues =
            new(
                Enum.GetValues(typeof(SynStream.PriorityLevel))
                    .Cast<SynStream.PriorityLevel>()
                    .OrderBy(priority => priority)
                    .Select(
                        priority
                            => new KeyValuePair<SynStream.PriorityLevel,
                                ConcurrentQueue<(T, TaskCompletionSource)>>(
                                priority, new ConcurrentQueue<(T, TaskCompletionSource)>())));

        private readonly SemaphoreSlim _itemsAvailable = new(0);

        public void Enqueue(
            SynStream.PriorityLevel priority,
            T item)
        {
            SendAsync(priority, item);
        }

        internal Task SendAsync(
            SynStream.PriorityLevel priority,
            T item)
        {
            var sentCompletionSource = new TaskCompletionSource();
            _priorityQueues[priority]
                .Enqueue((item, sentCompletionSource));
            _itemsAvailable.Release();
            return sentCompletionSource.Task;
        }

        public async Task<(T, TaskCompletionSource)> DequeueAsync(
                CancellationToken cancellationToken = default)
        {
            await _itemsAvailable
                  .WaitAsync(cancellationToken)
                  .ConfigureAwait(false);

            foreach (var queue in _priorityQueues.Values)
            {
                if (queue.TryDequeue(out var frame))
                {
                    return frame;
                }
            }

            throw new InvalidOperationException("Gate is out of sync");
        }
    }
}