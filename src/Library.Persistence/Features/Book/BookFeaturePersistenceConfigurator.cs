using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Persistence.Features.Book
{
    public static class BookFeaturePersistenceConfigurator
    {
        public static void ConfigureBookPersistenceFeature(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddDbContext<BookContext>(builder => builder.UseInMemoryDatabase("Book"));

            serviceCollection.AddScoped<ICreateBookCommand, CreateBookCommand>();
            serviceCollection.AddSingleton<BookEntityFactory>();
        }
    }
}
