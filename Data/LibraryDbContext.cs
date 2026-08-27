using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {

        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Loan> Loans { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ----- Author -----
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.FirstName)
                        .HasMaxLength(100)
                        .IsRequired();

                entity.Property(a => a.LastName)
                        .HasMaxLength(100)
                        .IsRequired();
            });

            //  ----- Category -----
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name).HasMaxLength(50).IsRequired();

                entity.HasIndex(c => c.Name).IsUnique();
            });

            // ------ Book ------
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Title).HasMaxLength(200).IsRequired();

                entity.Property(b => b.ISBN).HasMaxLength(20).IsRequired(false);
                entity.HasIndex(b => b.ISBN).IsUnique();

                entity.HasOne(b => b.Category)
                        .WithMany(c => c.Books)
                        .HasForeignKey(b => b.CategoryId);
            });

            // ------ BookAuthor ------
            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasKey(ba => new { ba.BookId, ba.AuthorId });
                entity.HasOne(ba => ba.Book)
                      .WithMany(b => b.BookAuthors)
                      .HasForeignKey(ba => ba.BookId);

                entity.HasOne(ba => ba.Author)
                      .WithMany(a => a.BookAuthors)
                      .HasForeignKey(ba => ba.AuthorId);
            });

            // ------ BookCopy ------
            modelBuilder.Entity<BookCopy>(entity =>
            {
                entity.HasKey(bc => bc.Id);

                entity.Property(bc => bc.CopyNumber)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(bc => bc.Condition)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(bc => bc.IsAvailable)
                      .IsRequired()
                      .HasDefaultValue(true);

                entity.HasOne(bc => bc.Book)
                      .WithMany(b => b.BookCopies)
                      .HasForeignKey(bc => bc.BookId);
            });

            // ------ Member ------
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.FullName)
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(m => m.Email)
                      .HasMaxLength(150)
                      .IsRequired();

                entity.HasIndex(m => m.Email)
                      .IsUnique();

                entity.Property(m => m.JoinedAt)
                      .HasDefaultValueSql("GETDATE()");
            });

            // ------ Loan ------

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(l => l.BorrowedAt)
                      .IsRequired();

                entity.Property(l => l.DueAt)
                      .IsRequired();

                entity.Property(l => l.ReturnedAt)
                      .IsRequired(false);

                entity.HasOne(l => l.BookCopy)
                      .WithMany(bc => bc.Loans)
                      .HasForeignKey(l => l.BookCopyId);

                entity.HasOne(l => l.Member)
                      .WithMany(m => m.Loans)
                      .HasForeignKey(l => l.MemberId);
            });
        }
    }
}