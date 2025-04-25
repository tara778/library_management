using Microsoft.EntityFrameworkCore;
using library_management.Models;

namespace library_management.Data
{
    public class NeondbContext : DbContext
    {
        public NeondbContext(DbContextOptions<NeondbContext> options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<Loan> Loans { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            // Book-Author relationship
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasOne(b => b.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(b => b.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.ISBN).HasMaxLength(13);
                entity.HasIndex(b => b.ISBN).IsUnique();
            });

            // Loan relationships
            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasOne(l => l.Book)
                    .WithMany(b => b.Loans)
                    .HasForeignKey(l => l.BookId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                entity.HasOne(l => l.Member)
                    .WithMany(m => m.Loans)
                    .HasForeignKey(l => l.MemberId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                // PostgreSQL-compatible date functions
                entity.Property(l => l.LoanDate)
                    .HasDefaultValueSql("CURRENT_DATE");
                    
                entity.Property(l => l.DueDate)
                    .HasDefaultValueSql("CURRENT_DATE + INTERVAL '14 days'");
            });

            // Author configurations
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(a => a.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(a => a.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // Member configurations
            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(m => m.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(m => m.LastName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(m => m.Email)
                    .IsRequired();
                entity.Property(m => m.PhoneNumber)
                    .IsRequired();
                entity.Property(m => m.RegistrationDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Use UTC in PostgreSQL
                
                entity.HasIndex(m => m.Email).IsUnique();
            });
        }
    }
}