﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Spdy.UnitTests.Observability;
using Test.It.With.XUnit;
using Xunit.Abstractions;

namespace Spdy.IntegrationTests.TestFramework
{
    public abstract class XUnit2TestSpecificationAsync
        : XUnit2SpecificationAsync
    {
        private readonly List<IDisposable> _disposables =
            new();

        private readonly List<IAsyncDisposable> _asyncDisposables =
            new();

        static XUnit2TestSpecificationAsync()
        {
            LogFactoryExtensions.InitializeOnce();
            NLogBuilderExtensions.ConfigureNLogOnce(new ConfigurationBuilder()
                                                    .SetBasePath(Directory.GetCurrentDirectory())
                                                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                    .Build());
            NLogCapturingTargetExtensions.RegisterOutputOnce();
        }

        
        protected XUnit2TestSpecificationAsync(
            ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        protected override CancellationTokenSource CancellationTokenSource
        {
            get;
        } = new(TimeSpan.FromSeconds(3));

        protected T DisposeOnTearDown<T>(
            T disposable)
            where T : IDisposable
        {
            _disposables.Add(disposable);
            return disposable;
        }

        protected T DisposeAsyncOnTearDown<T>(
            T disposable)
            where T : IAsyncDisposable
        {
            _asyncDisposables.Add(disposable);
            return disposable;
        }

        protected override async Task DisposeAsync(
            bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            foreach (var asyncDisposable in _asyncDisposables)
            {
                await asyncDisposable.DisposeAsync()
                                     .ConfigureAwait(false);
            }

            await base.DisposeAsync(disposing)
                      .ConfigureAwait(false);
        }
    }
}