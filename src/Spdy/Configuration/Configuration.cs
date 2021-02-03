namespace Spdy.Configuration
{
    public sealed record Configuration
    {
        public Ping Ping { get; init; } = new();

        public Metrics.Metrics Metrics { get; init; } = new();
    }
}