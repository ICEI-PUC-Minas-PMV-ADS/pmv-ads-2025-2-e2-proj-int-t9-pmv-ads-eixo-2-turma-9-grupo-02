// DbContext principal para acesso ao banco de dados.

using Microsoft.EntityFrameworkCore;

namespace MedShare.Models {
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Doador> Doadores { get; set; }
        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<Doacao> Doacoes { get; set; }
        public DbSet<EstoqueMedicamento> EstoqueMedicamentos { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed admin user com senha comum (sem hash)
            modelBuilder.Entity<Usuario>().HasData(new Usuario
            {
                UsuarioId = 1,
                UsuarioEmail = "admin@medshare.com",
                UsuarioSenha = "admin123", // senha em texto puro
                Perfil = Perfil.Admin
            });
        }
    }
}
