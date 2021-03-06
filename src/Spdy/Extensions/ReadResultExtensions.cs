﻿using Spdy.Frames;
using Spdy.Frames.Readers;
using ReadResult = System.IO.Pipelines.ReadResult;

namespace Spdy.Extensions
{
    internal static class ReadResultExtensions
    {
        internal static ReadResult<Control> AsControl<T>(
            this ReadResult<T> readResult)
            where T : Control
            => readResult.Out(out var result, out var error)
                ? ReadResult<Control>.Ok(result)
                : ReadResult<Control>.Error(error);

        internal static bool HasMoreData(
            this ReadResult readResult)
            => readResult.IsCanceled == false &&
               readResult.IsCompleted == false;
    }
}