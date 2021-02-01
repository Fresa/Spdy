using System;

namespace Spdy.Configuration.Metrics
{
    public sealed class PingRoundTripTime
    {
        public Action<TimeSpan> Observe { get; init; } = _ => { };
    }
}