using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MedShare.Models {
    
    /* AppDbContext vai herdar todas as caracteristicas de DbContext*/
    public class AppDbContext : DbContext
    {

        /*Construtor */
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 

        public DbSet<Doador> Doadors { get; set; }
        
    }
}
