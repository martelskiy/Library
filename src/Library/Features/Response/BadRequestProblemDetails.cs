using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Features.Response
{
    public class BadRequestProblemDetails : ProblemDetails
    {
        private const string TitleConst = "Bad Request";
        private const string DetailConst = "The request produced one or more errors";

        public BadRequestProblemDetails(IEnumerable<string> errors)
        {
            Title = TitleConst;
            Detail = DetailConst;
            Status = StatusCodes.Status400BadRequest;
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; }
    }
}
