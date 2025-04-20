using Microsoft.EntityFrameworkCore;

namespace MedicinskiSustav.Models
{
    public class MedicinskiDbContext : DbContext
    {
        public MedicinskiDbContext(DbContextOptions<MedicinskiDbContext> options) : base(options)
        {
        }
        public DbSet<Pacijent> Pacijenti { get; set; }
        public DbSet<MedicinskaDokumentacija> Dokumentacije { get; set; }
        public DbSet<Bolest> Bolesti { get; set; }
        public DbSet<Pregled> Pregledi { get; set; }
        public DbSet<Slika> Slike { get; set; }
        public DbSet<Recept> Recepti { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1:1 veza Pacijent-Dokumentacija
            modelBuilder.Entity<Pacijent>()
                .HasOne(p => p.Dokumentacija)
                .WithOne(d => d.Pacijent)
                .HasForeignKey<MedicinskaDokumentacija>(d => d.PacijentId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:N veza Dokumentacija-Bolesti
            modelBuilder.Entity<MedicinskaDokumentacija>()
                .HasMany(d => d.Bolesti)
                .WithOne(b => b.MedicinskaDokumentacija)
                .HasForeignKey(b => b.DokumentacijaId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:N veza Pacijent-Pregledi
            modelBuilder.Entity<Pacijent>()
                .HasMany(p => p.Pregledi)
                .WithOne(p => p.Pacijent)
                .HasForeignKey(p => p.PacijentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracija enuma
            modelBuilder.Entity<Pregled>()
                .Property(p => p.SifraPregleda)
                .HasConversion<string>()
                .HasMaxLength(5);

            // Indeksi
            modelBuilder.Entity<Pacijent>()
                .HasIndex(p => p.OIB)
                .IsUnique();

            modelBuilder.Entity<Pacijent>()
                .HasIndex(p => p.Prezime);
        }
    }
}
