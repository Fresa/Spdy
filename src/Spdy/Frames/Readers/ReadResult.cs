using System;
using System.Diagnostics.CodeAnalysis;

namespace Spdy.Frames.Readers
{
    internal readonly struct ReadResult<T>
        where T : Frame
    {
        private readonly T? _result;
        private readonly RstStream? _error;

        private ReadResult(T result)
        {
            _result = result;
            _error = null;
        }

        private ReadResult(RstStream error)
        {
            _result = null;
            _error = error;
        }

        public bool Out([NotNullWhen(true)] out T? result, [NotNullWhen(false)] out RstStream? error)
        {
            if (_result == null)
            {
                result = null;
                error = _error ?? throw new NullReferenceException();
                return false;
            }

            result = _result;
            error = _error;
            return true;
        }

        public T Result
        {
            get
            {
                if (Out(out var result, out var error))
                {
                    return result;
                }

                throw error;
            }
        }

        public static implicit operator ReadResult<Frame>(ReadResult<T> stream) =>
            new(
                stream._result is not null ? 
                    stream._result as Frame : 
                    stream._error ?? throw new NullReferenceException());

        public static ReadResult<T> Error(
            RstStream error)
        {
            return new(error);
        }

        public static ReadResult<T> Ok(
            T result)
        {
            return new(result);
        }
    }

    internal static class ReadResult
    {
        public static ReadResult<T> Ok<T>(
            T result) where T : Control
        {
            return ReadResult<T>.Ok(result);
        }

        public static ReadResult<Data> Ok(
            Data result)
        {
            return ReadResult<Data>.Ok(result);
        }
    }
}