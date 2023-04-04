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
        public DbSet<ScientificPaper> ScientificPapers { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<MessageUser> MessageUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PaperTask> PaperTasks { get; set; }
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

            modelBuilder.Entity<MessageUser>()
                .HasOne(p => p.Message)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Section)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaperTask>()
                .HasOne(pT => pT.Section)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Comment)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Task)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}