using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Features.Book
{
    public static class BookFeatureConfigurator
    {
        public static void ConfigureBookFeature(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBookService, BookService>();
            serviceCollection.AddSingleton<IValidator<BookDto>, CreateBookValidator>();
            serviceCollection.AddSingleton<BookFactory>();
        }
    }
}
