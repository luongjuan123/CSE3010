# BÀI TẬP VỀ ENTITY FRAMEWORK CORE (CODE FIRST) - QUẢN LÝ THƯ VIỆN

## 1. Giới thiệu dự án
Dự án được xây dựng bằng **C# (.NET 8.0)** sử dụng **Entity Framework Core (EF Core 8.0)** theo phương pháp **Code First** để ánh xạ (map) mô hình hướng đối tượng sang cơ sở dữ liệu quan hệ **SQL Server**.

### Các yêu cầu đã được đáp ứng đầy đủ:
1. **Mô hình lớp thực thể (Entities - ít nhất 2 lớp)**:
   - **`Author` (Tác giả)**: `AuthorId` (PK), `Name`, `Email`, `Bio`, cùng quan hệ 1-N với `Book`.
   - **`Category` (Thể loại)**: `CategoryId` (PK), `Name`, `Description`, cùng quan hệ 1-N với `Book`.
   - **`Book` (Sách)**: `BookId` (PK), `Title`, `ISBN`, `PublishedYear`, `Price`, khóa ngoại `AuthorId` (FK) liên kết với `Author`, khóa ngoại `CategoryId` (FK) liên kết với `Category`.
2. **Lớp ngữ cảnh dữ liệu `LibraryContext : DbContext`**:
   - Khai báo các `DbSet<Book>`, `DbSet<Author>`, `DbSet<Category>`.
   - Cấu hình quan hệ bảng (Relationship), ràng buộc toàn vẹn khóa ngoại (Foreign Keys, Cascade/Restrict) bằng Fluent API.
   - Hỗ trợ dữ liệu khởi tạo mẫu (Data Seeding) cho cả 3 bảng khi chạy migration.
   - Hỗ trợ cấu hình chuỗi kết nối linh hoạt qua `appsettings.json` (hỗ trợ cả SQL Server trên máy cục bộ, Docker, Windows SQL Server `LAPTOP-6SN65NCS\HUECTMDB`, LocalDB).
3. **Thực thi Migration & Cập nhật CSDL (Code First)**:
   - Đã tạo migration:
     ```bash
     dotnet ef migrations add InitialLibraryDB --context LibraryContext
     ```
   - Đã cập nhật cấu trúc và dữ liệu vào cơ sở dữ liệu:
     ```bash
     dotnet ef database update --context LibraryContext
     ```
   - Đã xuất script SQL tạo bảng và seed dữ liệu độc lập: `database.sql`.

---

## 2. Cấu trúc thư mục dự án
```
poject w6/
├── database.sql                  # Script SQL toàn bộ schema và seed data do EF Core xuất ra
├── README.md                     # Tài liệu hướng dẫn chi tiết dự án tuần 6
├── LibraryManagement.sln         # File Solution mở bằng Visual Studio
└── LibraryManagement/
    ├── LibraryManagement.csproj  # File project .NET 8.0 Console App
    ├── appsettings.json          # Cấu hình chuỗi kết nối SQL Server
    ├── Program.cs                # Entry point demo kết nối và truy vấn LINQ/EF Core
    ├── Entities/                 # Thư mục chứa các lớp thực thể (Entities)
    │   ├── Author.cs             # Lớp tác giả
    │   ├── Category.cs           # Lớp thể loại
    │   └── Book.cs               # Lớp sách (liên kết khóa ngoại đến Author & Category)
    ├── Data/
    │   └── LibraryContext.cs     # DbContext cấu hình mapping, quan hệ và Data Seeding
    └── Migrations/               # Thư mục lưu trữ lịch sử Migration Code First
        ├── 20261024035658_InitialLibraryDB.cs
        ├── 20261024035658_InitialLibraryDB.Designer.cs
        └── LibraryContextModelSnapshot.cs
```

---

## 3. Chi tiết mô hình quan hệ (Relationships)

```
+--------------------+              +--------------------+
|       Author       | 1          * |        Book        |
|--------------------|--------------|--------------------|
| PK  AuthorId       |<---\    /--->| PK  BookId         |
|     Name           |     \  /     |     Title          |
|     Email          |      \/      |     ISBN           |
|     Bio            |      /\      |     PublishedYear  |
+--------------------+     /  \     |     Price          |
                          /    \    | FK  AuthorId       |
+--------------------+   /      \   | FK  CategoryId     |
|      Category      |  /        \  +--------------------+
|--------------------| 1          * 
| PK  CategoryId     |-------------/
|     Name           |
|     Description    |
+--------------------+
```

- **Một tác giả (`Author`) có nhiều cuốn sách (`Books`)**: Xóa tác giả sẽ tự động xóa sách của tác giả đó (`Cascade Delete`).
- **Một thể loại (`Category`) có nhiều cuốn sách (`Books`)**: Không cho phép xóa thể loại nếu vẫn còn sách thuộc thể loại đó (`Restrict Delete`).

---

## 4. Các lệnh CLI đã sử dụng

```bash
# 1. Cài đặt công cụ EF Core CLI (nếu chưa có)
dotnet tool install --global dotnet-ef

# 2. Tạo Migration khởi tạo cơ sở dữ liệu
dotnet ef migrations add InitialLibraryDB --context LibraryContext

# 3. Cập nhật Migration vào Database
dotnet ef database update --context LibraryContext

# 4. Xuất script SQL tương đương
dotnet ef migrations script --idempotent -o ../database.sql --context LibraryContext

# 5. Chạy ứng dụng kiểm tra
dotnet run
```

---

## 5. Hướng dẫn chạy trên Visual Studio (Windows)

1. Mở file `poject w6/LibraryManagement.sln` bằng Visual Studio 2022.
2. Kiểm tra chuỗi kết nối trong file `appsettings.json` (mặc định đã có sẵn chuỗi kết nối cho máy bạn `LAPTOP-6SN65NCS\HUECTMDB` hoặc `LocalDb`).
3. Nếu muốn cập nhật DB từ Package Manager Console trong Visual Studio:
   ```powershell
   Update-Database -Context LibraryContext
   ```
4. Nhấn **F5** hoặc bấm nút **Run** để khởi chạy chương trình và xem kết quả truy vấn dữ liệu từ CSDL.
