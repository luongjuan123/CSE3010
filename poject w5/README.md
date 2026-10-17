# BÀI TẬP VỀ ADO.NET (WinForms - Quản Lý Sinh Viên)

## 1. Giới thiệu dự án
Dự án được xây dựng theo kiến trúc 3 lớp (3-Tier Architecture) hoàn chỉnh bằng **C# Windows Forms** kết hợp thư viện **ADO.NET (`Microsoft.Data.SqlClient`)** thao tác với cơ sở dữ liệu **SQL Server**.

### Các yêu cầu đã được đáp ứng đầy đủ:
1. **Lớp `DataHelper`**:
   - Quản lý kết nối (`OpenConnection`, `CloseConnection`).
   - Cung cấp các phương thức thực thi ADO.NET: `ExecuteNonQuery`, `ExcuteReader` / `ExecuteReader`, `GetDataTable`, `ExecuteScalar`.
   - Kết nối mặc định: `Server=LAPTOP-6SN65NCS\HUECTMDB; Database=QLSV;Integrated Security=True; TrustServerCertificate=true` và có thể cấu hình linh hoạt qua file `App.config`.
2. **Thay đổi và hoàn thiện 2 lớp `LopHocDAO` và `SinhVienDAO` (đồng thời cung cấp tương thích `SinhVienDAL`, `LopHocDAL`)**:
   - Thao tác trực tiếp với cơ sở dữ liệu SQL Server hoàn toàn bằng ADO.NET.
   - Sử dụng **Parameterized Query (`SqlParameter`)** để phòng chống lỗi SQL Injection và an toàn dữ liệu.
   - Sửa toàn bộ các lỗi cú pháp câu lệnh SQL gốc (như lỗi cú pháp `UPDATE SinhVien where MaSV=... set...`).
   - Sửa phương thức xóa (`DeleteSinhVien`) và tìm kiếm (`GetSinhVienById`) từ việc xóa/tìm trong RAM sang thực thi lệnh `DELETE` và `SELECT` trực tiếp trên SQL Server.
3. **Quy tắc xử lý ngoại lệ (Exception Handling)**:
   - **Tầng DataAccessLayer (DAL / DAO)**: Ném ngoại lệ (`throw new Exception(...)`) khi xảy ra sự cố truy vấn CSDL, không nuốt lỗi và không hiển thị MessageBox.
   - **Tầng BusinessLogicLayer (BUS)**: Kiểm tra ràng buộc nghiệp vụ (mã không rỗng, điểm từ 0-10, kiểm tra trùng mã, kiểm tra ràng buộc khóa ngoại khi xóa lớp còn sinh viên). Nếu vi phạm hoặc có lỗi từ DAL, **ném ngoại lệ lên tầng trên**.
   - **Tầng View / GUI (WinForms)**: Mọi sự kiện của người dùng (Thêm, Sửa, Xóa, Tìm kiếm, Lọc, Load form) đều được bao bọc trong khối `try ... catch` để bắt toàn bộ ngoại lệ và hiển thị thông báo lỗi thân thiện cho người dùng thông qua `MessageBox.Show`.

---

## 2. Cấu trúc thư mục dự án
```
poject w5/
├── database.sql                  # Script SQL tạo CSDL QLSV và dữ liệu mẫu
├── README.md                     # Tài liệu hướng dẫn sử dụng
├── QuanLySinhVien.sln            # File Solution Visual Studio
└── QuanLySinhVien/
    ├── QuanLySinhVien.csproj     # File project .NET 8.0-windows
    ├── App.config                # Cấu hình chuỗi kết nối SQL Server
    ├── Program.cs                # Entry point của ứng dụng
    ├── Entity/                   # Tầng Thực thể / DTO
    │   ├── SinhVien.cs
    │   └── LopHoc.cs
    ├── DAL/                      # Tầng Data Access Layer (ADO.NET)
    │   ├── DataHelper.cs
    │   ├── SinhVienDAO.cs
    │   ├── SinhVienDAL.cs
    │   ├── LopHocDAO.cs
    │   └── LopHocDAL.cs
    ├── BUS/                      # Tầng Nghiệp vụ (Business Layer)
    │   ├── SinhVienBUS.cs
    │   └── LopHocBUS.cs
    └── GUI/                      # Tầng Giao diện (Presentation Layer)
        ├── MainForm.cs
        ├── MainForm.Designer.cs
        ├── FormLopHoc.cs
        └── FormLopHoc.Designer.cs
```

---

## 3. Hướng dẫn chạy chương trình trên Visual Studio

1. **Khởi tạo cơ sở dữ liệu**:
   - Mở file `database.sql` trong SQL Server Management Studio (SSMS).
   - Nhấn **Execute (F5)** để tạo Database `QLSV`, các bảng `LopHoc`, `SinhVien` và chèn dữ liệu mẫu.

2. **Cấu hình chuỗi kết nối (nếu cần đổi tên Server)**:
   - Mở file `QuanLySinhVien/App.config`.
   - Chỉnh sửa `connectionString` nếu tên SQL Server trên máy bạn khác với `LAPTOP-6SN65NCS\HUECTMDB`.

3. **Mở và chạy dự án**:
   - Mở file `QuanLySinhVien.sln` bằng Visual Studio 2022 (hoặc Rider).
   - Nhấn **F5** hoặc nút **Start** để biên dịch và chạy ứng dụng.
