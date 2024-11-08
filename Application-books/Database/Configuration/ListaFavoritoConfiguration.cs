using Application_books.Database.Entitties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application_books.Database.Configuration
{
    public class ListaFavoritoConfiguration : IEntityTypeConfiguration<ListaFavoritoEntity>
    {
        public void Configure(EntityTypeBuilder<ListaFavoritoEntity> builder)
        {
            builder.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .HasPrincipalKey(e => e.Id);

            builder.HasOne(e => e.UpdatedByUser)
                .WithMany()
                .HasForeignKey(e => e.UpdatedBy)
                .HasPrincipalKey(e => e.Id);
        }
    }
}
