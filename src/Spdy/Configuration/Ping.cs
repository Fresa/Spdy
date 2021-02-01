using System;
using System.Threading;

namespace Spdy.Configuration
{
    public sealed class Ping
    {
        /// <summary>
        /// The interval between pings
        /// </summary>
        public TimeSpan PingInterval { get; init; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Maximum number of unacknowledged outstanding pings
        /// </summary>
        public int MaxOutstandingPings { get; init; } = 10;

        public static Ping Disabled => new()
        {
            MaxOutstandingPings = 0,
            PingInterval = Timeout.InfiniteTimeSpan
        };
    }
}