﻿using Spdy.Logging;

namespace Spdy.UnitTests.Logging
{
    internal class FakeLoggerFactory
    {
        internal static ILogFactory Create()
        {
            return new FakeLogFactory();
        }
        
        private class FakeLogFactory : ILogFactory
        {
            public ILogger Create(
                string logger)
                => new FakeLogger(logger);

            public ILogger Create<T>()
                => new FakeLogger(nameof(FakeLoggerFactory));
        }
    }
}