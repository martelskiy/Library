using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Features.Response
{
    public class NotFoundProblemDetails : ProblemDetails
    {
        private const string TitleConst = "Not Found";
        private const string DetailConst = "The requested resource could not be found";

        public NotFoundProblemDetails()
        {
            Status = StatusCodes.Status404NotFound;
            Title = TitleConst;
            Detail = DetailConst;
        }
    }
}
