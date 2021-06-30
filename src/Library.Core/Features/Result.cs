using System.Collections.Generic;
using System.Linq;

namespace Library.Core.Features
{
    public class Result
    {
        public bool Success { get; }
        public IEnumerable<Error> Errors { get; }

        private Result(bool success, IEnumerable<Error> error = null)
        {
            Success = success;
            Errors = error ?? Enumerable.Empty<Error>();
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
}
