using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheScientistAPI.Model;

namespace TheScientistAPI.Data
{
    public class ScientistContext : IdentityDbContext<ApplicationUser>
    {
        public ScientistContext(DbContextOptions<ScientistContext> options) : base(options)
        { }
        DbSet<ScientificPaper> ScientificPapers { get; set; }
        DbSet<Section> Sections { get; set; }
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<Keyword> Keywords { get; set; }
        DbSet<Reference> References { get; set; }
        DbSet<SectionType> SectionTypes { get; set; }
        DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.ScientificPaper)
                .WithMany(sp => sp.UserRoles)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(p => p.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}