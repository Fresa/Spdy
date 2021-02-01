namespace Spdy.Configuration.Metrics
{
    public sealed record Metrics
    {
        /// <summary>
        /// Measures ping round trip
        /// </summary>
        public PingRoundTripTime PingRoundTripTime { get; init; } = new();
    }
}