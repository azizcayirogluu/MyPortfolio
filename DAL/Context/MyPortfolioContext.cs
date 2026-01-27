using Microsoft.EntityFrameworkCore;
using MyPortolioUdemy.DAL.Entities;

namespace MyPortolioUdemy.DAL.Context
{
    public class MyPortfolioContext : DbContext
    {
        public MyPortfolioContext(DbContextOptions<MyPortfolioContext> options) : base(options)
        {
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Features> AllFeatures { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure key lengths for string properties
            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.NameSurname).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Subject).HasMaxLength(200);
            });

            modelBuilder.Entity<About>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<Experience>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(200);
                entity.Property(e => e.Head).HasMaxLength(100);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.Value).HasDefaultValue(0);
            });

            modelBuilder.Entity<Portfolio>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<ToDoList>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(200);
                entity.Property(e => e.Status).HasDefaultValue(false);
            });
        }
    }
}