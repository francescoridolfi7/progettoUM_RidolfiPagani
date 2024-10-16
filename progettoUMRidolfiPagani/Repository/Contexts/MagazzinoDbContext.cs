using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using progettoUMRidolfiPagani.Models;

namespace progettoUMRidolfiPagani.Repository
{
    public class MagazzinoDbContext : DbContext
    {
        public MagazzinoDbContext(DbContextOptions<MagazzinoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Articolo> Articoli { get; set; }
        public DbSet<Movimento> Movimenti { get; set; }
        public DbSet<Posizione> Posizioni { get; set; }
       
        public DbSet<Utente> Utenti { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
