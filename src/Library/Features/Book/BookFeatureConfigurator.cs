using FluentValidation;
using Library.Features.Book.Create;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Features.Book
{
    public static class BookFeatureConfigurator
    {
        public static void ConfigureBookFeature(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBookService, BookService>();
            serviceCollection.AddSingleton<IValidator<CreateBookDto>, CreateBookValidator>();
            serviceCollection.AddSingleton<BookFactory>();
        }
    }
}
