namespace Spdy.Configuration
{
    public sealed class Configuration
    {
        internal Ping Ping { get; init; } = new();

        internal Metrics.Metrics Metrics { get; init; } = new();
    }
}