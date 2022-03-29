using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Phonebook.Models;

namespace Phonebook.Data
{
    public class PhonebookContext : IdentityDbContext<User>
    {
        public PhonebookContext(DbContextOptions<PhonebookContext> options) : base(options)
        {
        }

        public DbSet<Record> Records { get; set; }
        public DbSet<Subdivision> Subdivisions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Record>().Property(r => r.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Record>().Property(r => r.Surname).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Record>().Property(r => r.FatherName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Record>().Property(r => r.Position).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Record>().Property(r => r.PersonalNumber).IsRequired();
            modelBuilder.Entity<Record>().Property(r => r.WorkNumber).IsRequired();
            modelBuilder.Entity<Record>().Property(r => r.WorkMobileNumber).IsRequired();
            modelBuilder.Entity<Record>().Property(r => r.WorkNumber).IsRequired();
            modelBuilder.Entity<Record>().Property(r => r.SubdivisionID).IsRequired();

            modelBuilder.Entity<Subdivision>().Property(s => s.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Subdivision>().Property(s => s.ParentId).IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
