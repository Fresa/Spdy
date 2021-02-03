using System;

namespace Spdy.Configuration.Metrics
{
    public sealed record PingRoundTripTime
    {
        public Action<TimeSpan> Observe { get; init; } = _ => { };
    }
}