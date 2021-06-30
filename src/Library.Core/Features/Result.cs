using System.Collections.Generic;
using System.Linq;

namespace Library.Core.Features
{
    public class Result
    {
        public bool Success { get; }
        public IEnumerable<Error> Errors { get; }

        protected Result(bool success, IEnumerable<Error> errors = null)
        {
            Success = success;
            Errors = errors ?? Enumerable.Empty<Error>();
        }

        public static Result Ok()
        {
            return new(true);
        }

        public static Result Fail(IEnumerable<Error> error)
        {
            return new(false, error);
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; }

        public Result(bool success, T data, IEnumerable<Error> errors = null)
            : base(success, errors)
        {
            Data = data;
        }

        public new static Result<T> Fail(IEnumerable<Error> errors)
        {
            return new(false, default, errors);
        }

        public static Result<T> Ok(T data)
        {
            return new(true, data);
        }
    }
}
