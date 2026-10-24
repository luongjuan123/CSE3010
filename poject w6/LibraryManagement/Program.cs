using System;
using System.Linq;
using LibraryManagement.Data;
using LibraryManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("==========================================================");
            Console.WriteLine("  QUẢN LÝ THƯ VIỆN - ENTITY FRAMEWORK CORE (CODE FIRST)   ");
            Console.WriteLine("==========================================================\n");

            using (var context = new LibraryContext())
            {
                // Kiểm tra kết nối CSDL
                Console.WriteLine("Đang kiểm tra kết nối CSDL...");
                if (!context.Database.CanConnect())
                {
                    Console.WriteLine("[LỖI] Không thể kết nối đến cơ sở dữ liệu!");
                    Console.WriteLine("Hãy đảm bảo bạn đã chạy lệnh migration:");
                    Console.WriteLine("  dotnet ef migrations add InitialLibraryDB --context LibraryContext");
                    Console.WriteLine("  dotnet ef database update --context LibraryContext");
                    return;
                }

                Console.WriteLine("[THÀNH CÔNG] Đã kết nối đến CSDL LibraryDB!\n");

                // 1. Hiển thị danh sách Thể loại (Categories)
                Console.WriteLine("--- DANH SÁCH THỂ LOẠI (CATEGORIES) ---");
                var categories = context.Categories.ToList();
                foreach (var cat in categories)
                {
                    Console.WriteLine($"[{cat.CategoryId}] {cat.Name} - {cat.Description}");
                }

                // 2. Hiển thị danh sách Tác giả (Authors)
                Console.WriteLine("\n--- DANH SÁCH TÁC GIẢ (AUTHORS) ---");
                var authors = context.Authors.ToList();
                foreach (var author in authors)
                {
                    Console.WriteLine($"[{author.AuthorId}] {author.Name} | Email: {author.Email ?? "N/A"} | Tiểu sử: {author.Bio}");
                }

                // 3. Hiển thị danh sách Sách kèm Tác giả và Thể loại (Eager Loading với Include)
                Console.WriteLine("\n--- DANH SÁCH SÁCH (BOOKS) KÈM THÔNG TIN LIÊN KẾT ---");
                var books = context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .ToList();

                foreach (var book in books)
                {
                    Console.WriteLine($"- ID: {book.BookId} | Tựa đề: {book.Title}");
                    Console.WriteLine($"  Tác giả: {book.Author.Name} | Thể loại: {book.Category.Name}");
                    Console.WriteLine($"  Năm XB: {book.PublishedYear} | Giá: {book.Price:N0} VNĐ | ISBN: {book.ISBN}");
                    Console.WriteLine("  --------------------------------------------------------");
                }

                Console.WriteLine("\nỨng dụng Entity Framework Core Code First hoạt động hoàn hảo!");
            }
        }
    }
}
