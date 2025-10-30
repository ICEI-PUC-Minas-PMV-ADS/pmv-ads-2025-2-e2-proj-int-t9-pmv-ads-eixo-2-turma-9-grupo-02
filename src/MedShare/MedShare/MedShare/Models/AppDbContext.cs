using Microsoft.EntityFrameworkCore;
using MedShare.Models.Data;

namespace MedShare.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Doador> Doadores { get; set; }
        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<Doacao> Doacoes { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("TEXT");
        }
    }
}