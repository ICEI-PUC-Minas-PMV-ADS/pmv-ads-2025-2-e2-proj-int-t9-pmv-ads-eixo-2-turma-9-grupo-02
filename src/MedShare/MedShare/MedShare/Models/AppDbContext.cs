using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MedShare.Models {
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Doador> Doadores { get; set; }
        public DbSet<Instituicao> Instituicoes { get; set; }
    }
}
