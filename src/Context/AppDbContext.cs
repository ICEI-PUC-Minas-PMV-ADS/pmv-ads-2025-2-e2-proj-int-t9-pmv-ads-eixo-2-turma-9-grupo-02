using MedShare.Models;
using Microsoft.EntityFrameworkCore;

namespace MedShare.Context {
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Doador>  Doadors { get; set; }
        public DbSet<Doacao> Doacaos { get; set; }
        public DbSet<Notificacao> Notificacaos { get; set; }
        public DbSet<HistoricoDoacao> HistoricoDoacaos { get; set; }
        public DbSet<Instituicao> Instituicaos { get; set; }
        public DbSet<MedicamentoEstoque> MedicamentoEstoques { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Remove cascade delete da FK DoadorId em Notificacao
            modelBuilder.Entity<Notificacao>()
                .HasOne(n => n.Doador)
                .WithMany()
                .HasForeignKey(n => n.DoadorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
