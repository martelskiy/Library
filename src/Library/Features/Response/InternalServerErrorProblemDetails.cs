using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Features.Response
{
    public class InternalServerErrorProblemDetails : ProblemDetails
    {
        private const string TitleConst = "Internal Server Error";
        private const string DetailConst = "An unexpected error occurred on the server and has been logged";

        public InternalServerErrorProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError;
            Title = TitleConst;
            Detail = DetailConst;
        }
    }
}
