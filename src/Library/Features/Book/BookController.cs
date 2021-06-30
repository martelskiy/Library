using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;
using Library.Features.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Features.Book
{
    [ApiController]
    [Route("/api/library/books")]
    [Produces("application/json")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalServerErrorProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBook([FromBody] BookDto book, CancellationToken cancellationToken)
        {
            var result = await _bookService.CreateBookAsync(book, cancellationToken);

            if (result.Errors.Any(error => error.Type.Equals(ErrorType.Unspecified)))
                return new ObjectResult(new InternalServerErrorProblemDetails());

            if (result.Errors.Any(error => error.Type.Equals(ErrorType.Validation)))
                return new ObjectResult(new BadRequestProblemDetails(result.Errors.Select(error => string.Join(',', error.Message))));

            return Ok();
        }
    }
}
