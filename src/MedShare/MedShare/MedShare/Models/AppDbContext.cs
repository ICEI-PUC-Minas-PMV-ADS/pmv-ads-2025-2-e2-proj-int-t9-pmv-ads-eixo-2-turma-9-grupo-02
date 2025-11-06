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
        // DbSet dos medicamentos disponíveis globalmente (catálogo base)
        public DbSet<Medicamento> Medicamentos { get; set; }
        // DbSet do relacionamento entre instituição e seus medicamentos (estoque individual)
        public DbSet<InstituicaoMedicamento> InstituicaoMedicamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Seed básico: insere medicamentos iniciais no catálogo global.
            // Facilita criação automática de estoque zerado para cada nova instituição.
            modelBuilder.Entity<Medicamento>().HasData(
                new Medicamento { MedicamentoId = 1, Nome = "Dipirona" },
                new Medicamento { MedicamentoId = 2, Nome = "Paracetamol" },
                new Medicamento { MedicamentoId = 3, Nome = "Amoxicilina" },
                new Medicamento { MedicamentoId = 4, Nome = "Omeprazol" }
            );
            // Garante que não existam duplicidades de um mesmo medicamento para a mesma instituição.
            modelBuilder.Entity<InstituicaoMedicamento>()
                .HasIndex(im => new { im.InstituicaoId, im.MedicamentoId })
                .IsUnique();
        }
    }
}
