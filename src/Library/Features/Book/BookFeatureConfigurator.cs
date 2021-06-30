using FluentValidation;
using Library.Features.Book.Create;
using Library.Features.Book.Loan;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Features.Book
{
    public static class BookFeatureConfigurator
    {
        public static void ConfigureBookFeature(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBookService, BookService>();
            serviceCollection.AddSingleton<IValidator<CreateBookRequestDto>, CreateBookRequestDtoValidator>();
            serviceCollection.AddSingleton<IValidator<LoanBookRequestDto>, LoanBookRequestDtoValidator>();
            serviceCollection.AddSingleton<BookFactory>();
        }
    }
}
