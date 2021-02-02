using System.IO.Pipelines;

namespace Spdy.Extensions
{
    internal static class FlushResultExtensions
    {
        internal static bool HasMore(
            this FlushResult readResult)
            => readResult.IsCanceled == false &&
               readResult.IsCompleted == false;
    }
}