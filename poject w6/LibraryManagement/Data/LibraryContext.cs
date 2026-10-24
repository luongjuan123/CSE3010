using System;
using System.IO;
using LibraryManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Category> Categories => Set<Category>();

        public LibraryContext()
        {
        }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Ưu tiên đọc chuỗi kết nối từ file appsettings.json
                string? connectionString = null;
                try
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .Build();

                    connectionString = configuration.GetConnectionString("DefaultConnection");
                }
                catch
                {
                    // Dự phòng nếu file appsettings chưa được load
                }

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    // Chuỗi kết nối mặc định SQL Server
                    connectionString = "Server=localhost,1433;Database=LibraryDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=true;";
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình bảng Author
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.AuthorId);
                entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Email).HasMaxLength(100);
                entity.Property(a => a.Bio).HasMaxLength(500);
            });

            // Cấu hình bảng Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.CategoryId);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Description).HasMaxLength(250);
            });

            // Cấu hình bảng Book và quan hệ 1-N
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.BookId);
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.ISBN).HasMaxLength(20);
                entity.Property(b => b.Price).HasColumnType("decimal(18,2)");

                // Quan hệ 1 Author - Nhiều Book
                entity.HasOne(b => b.Author)
                      .WithMany(a => a.Books)
                      .HasForeignKey(b => b.AuthorId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Quan hệ 1 Category - Nhiều Book
                entity.HasOne(b => b.Category)
                      .WithMany(c => c.Books)
                      .HasForeignKey(b => b.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seeding data mẫu ban đầu cho Database
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { AuthorId = 1, Name = "Nguyễn Nhật Ánh", Email = "anhnn@nxbtre.com.vn", Bio = "Nhà văn nổi tiếng của thiếu nhi Việt Nam" },
                new Author { AuthorId = 2, Name = "Nam Cao", Email = "namcao@classic.vn", Bio = "Nhà văn hiện thực xuất sắc đầu thế kỷ 20" },
                new Author { AuthorId = 3, Name = "J.K. Rowling", Email = "jkrowling@pottermore.com", Bio = "Tác giả bộ tiểu thuyết Harry Potter đình đám" }
            );

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Văn học thiếu nhi", Description = "Sách dành cho lứa tuổi thanh thiếu niên" },
                new Category { CategoryId = 2, Name = "Hiện thực phê phán", Description = "Tác phẩm văn học hiện thực xã hội" },
                new Category { CategoryId = 3, Name = "Giả tưởng / Fantasy", Description = "Tiểu thuyết kỳ ảo và phép thuật" }
            );

            // Seed Books
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = 1,
                    Title = "Cho tôi xin một vé đi tuổi thơ",
                    ISBN = "978-604-1-00001-1",
                    PublishedYear = 2008,
                    Price = 85000m,
                    AuthorId = 1,
                    CategoryId = 1
                },
                new Book
                {
                    BookId = 2,
                    Title = "Mắt biếc",
                    ISBN = "978-604-1-00002-8",
                    PublishedYear = 1990,
                    Price = 110000m,
                    AuthorId = 1,
                    CategoryId = 1
                },
                new Book
                {
                    BookId = 3,
                    Title = "Chí Phèo",
                    ISBN = "978-604-2-00003-5",
                    PublishedYear = 1941,
                    Price = 65000m,
                    AuthorId = 2,
                    CategoryId = 2
                },
                new Book
                {
                    BookId = 4,
                    Title = "Harry Potter và Hòn đá Phù thủy",
                    ISBN = "978-074-7-53269-9",
                    PublishedYear = 1997,
                    Price = 135000m,
                    AuthorId = 3,
                    CategoryId = 3
                }
            );
        }
    }
}
