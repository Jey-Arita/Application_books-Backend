using Application_books.Database.Configuration;
using Application_books.Database.Entities;
using Application_books.Database.Entitties;
using Application_books.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application_books.Database
{
    public class ApplicationbooksContext : IdentityDbContext<UserEntity>
    {
        private readonly IAuditService _auditService;

        public ApplicationbooksContext(DbContextOptions options,
                IAuditService auditService)
                : base(options)
            {
            this._auditService = auditService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.HasDefaultSchema("security");

            modelBuilder.Entity<UserEntity>().ToTable("users");
            modelBuilder.Entity<IdentityRole>().ToTable("roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("users_roles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("users_claims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("users_logins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("roles_claims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("users_tokens");

            modelBuilder.ApplyConfiguration(new AutorConfiguration());
            modelBuilder.ApplyConfiguration(new CalificacionConfiguration());
            modelBuilder.ApplyConfiguration(new ComentarioConfiguration());
            modelBuilder.ApplyConfiguration(new LibroConfiguration());
            modelBuilder.ApplyConfiguration(new ListaFavoritoConfiguration());
            modelBuilder.ApplyConfiguration(new MebresiaConfiguration());

            // Set FKs OnRestrict
            var eTypes = modelBuilder.Model.GetEntityTypes();
            foreach (var type in eTypes)
            {
                var foreignKeys = type.GetForeignKeys();
                foreach (var foreignKey in foreignKeys)
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }

        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified
                ));

            foreach (var entry in entries)
            {
                var entity = entry.Entity as BaseEntity;
                if (entity != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = _auditService.GetUserId();
                        entity.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        entity.UpdatedBy = _auditService.GetUserId();
                        entity.UpdatedDate = DateTime.Now;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<AutorEntity> Autores { get; set; }
            public DbSet<CalificacionEntity> Calificaciones { get; set; }
            public DbSet<LibroEntity> Libros { get; set; }
            public DbSet<ListaFavoritoEntity> ListaFavoritos { get; set; }
            public DbSet<MembresiaEntity> Membresias { get; set; }
            public DbSet<UserEntity> Usuarios { get; set; }
            public DbSet<ComentarioEntity> Comentarios { get; set; }

    }
}
