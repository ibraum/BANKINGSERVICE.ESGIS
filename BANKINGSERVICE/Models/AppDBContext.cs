using BANKING_SYSTEME.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BANKING_SYSTEM.Models
{
    public class AppDBContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Compte> Comptes { get; set; }
        public DbSet<Livret> Livrets { get; set; }
        public DbSet<PEL> PELs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            OptionsBuilder.UseSqlServer("Server=TLAN ;Database=CoursASPNet ;Trusted_Connection=True; TrustServerCertificate=True; Encrypt=True ;");
        }
    }
}
