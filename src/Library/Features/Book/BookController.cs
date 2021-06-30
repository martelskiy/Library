using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Core.Features;
using Library.Features.Book.Create;
using Library.Features.Book.Fetch;
using Library.Features.Book.Loan;
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
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequestDto createBookRequest, CancellationToken cancellationToken)
        {
            var result = await _bookService.CreateBookAsync(createBookRequest, cancellationToken);

            if (result.Errors.Any(error => error.Type.Equals(ErrorType.Unspecified)))
                return new ObjectResult(new InternalServerErrorProblemDetails());

            if (result.Errors.Any(error => error.Type.Equals(ErrorType.Validation)))
                return new ObjectResult(new BadRequestProblemDetails(result.Errors.Select(error => string.Join(',', error.Message))));

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetBookResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(InternalServerErrorProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks([FromQuery] string author, CancellationToken cancellationToken)
        {
            var result = await _bookService.GetBooksAsync(author, cancellationToken);

            if (result.Errors.Any(error => error.Type.Equals(ErrorType.Unspecified)))
                return new ObjectResult(new InternalServerErrorProblemDetails());

            if (!result.Data.Any())
            {
                return NoContent();
            }
            return Ok(result.Data);
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BadRequestProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalServerErrorProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoanBook([FromRoute] int bookId, CancellationToken cancellationToken)
        {
            var result = await _bookService.LoanBookAsync(new LoanBookRequestDto{ BookId = bookId }, cancellationToken);

            if (result.Errors.Any(error => error.Type.Equals(ErrorType.Unspecified)))
                return new ObjectResult(new InternalServerErrorProblemDetails());

            if (result.Errors.Any(error => error.Type.Equals(ErrorType.Validation)))
                return new ObjectResult(new BadRequestProblemDetails(result.Errors.Select(error => string.Join(',', error.Message))));

            if (result.Errors.Any(error => error.Type.Equals(ErrorType.NotFound)))
                return new ObjectResult(new NotFoundProblemDetails());

            return Ok();
        }
    }
}
