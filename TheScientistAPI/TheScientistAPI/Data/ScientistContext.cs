using System;
using Microsoft.EntityFrameworkCore;
using TheScientistAPI.Model;

namespace TheScientistAPI.Data
{
    public class ScientistContext : DbContext
    {
        public ScientistContext(DbContextOptions<ScientistContext> options):base(options)
        { }
        DbSet<User> Users { get; set; }
        DbSet<UserType> UserTypes { get; set; }
        DbSet<ScientificPaper> ScientificPapers { get; set; }
        DbSet<PaperUser> PaperUsers { get; set; }
        DbSet<Section> Sections { get; set; }
        DbSet<CodeSegment> CodeSegments { get; set; }
        DbSet<TextSegment> TextSegments { get; set; }
        DbSet<Image> Imege { get; set; }
        DbSet<ToDoList> ToDoLists { get; set; }
        DbSet<ToDoItem> ToDoItems { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<NotificationUser> NotificationUsers { get; set; }
        DbSet<NotificationType> NotificationTypes { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<MessageUser> MessageUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageUser>()
            .HasOne(m => m.User)
            .WithMany(t => t.Messages)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}