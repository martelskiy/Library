using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Persistence.Features.Book
{
    public class BookConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder
                .Property(entity => entity.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(entity => entity.Name)
                .IsRequired();

            builder
                .Property(entity => entity.Author)
                .IsRequired();

            builder
                .Property(entity => entity.IsAvailable)
                .IsRequired();
        }
    }
}
