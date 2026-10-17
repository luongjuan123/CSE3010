# CSE3010 - For CS3010 assignments storage

Kho lưu trữ bài tập và dự án thực hành môn học **CSE3010**.

---

## Danh mục nội dung

| Thư mục / File | Mô tả |
| :--- | :--- |
| `lecture0-0.cs`, `lecture0-1.cs` | Bài tập khởi động tuần 0 (Cơ bản về C#, nhập xuất, class hình học). |
| [`poject w5/`](./poject%20w5/) | **Dự án Quản lý Sinh viên - WinForms ADO.NET** theo kiến trúc 3 lớp (3-Tier). |

---

## Chi tiết Dự án Tuần 5: Quản lý Sinh viên (`poject w5`)

Dự án Windows Forms quản lý sinh viên và lớp học sử dụng **ADO.NET (`Microsoft.Data.SqlClient`)** kết nối **SQL Server**.

### 1. Kiến trúc 3 Lớp (3-Tier Architecture)
- **Tầng Thực thể (Entity / DTO)**:
  - `SinhVien`: Lưu trữ thông tin sinh viên (`MaSV`, `HoTen`, `GioiTinh`, `NgaySinh`, `SoDienThoai`, `MaLop`, `Diem`, `Email`).
  - `LopHoc`: Lưu trữ thông tin lớp học (`MaLop`, `TenLop`).
- **Tầng Truy cập Dữ liệu (DAL / DAO)**:
  - `DataHelper`: Quản lý kết nối SQL Server và thực thi câu lệnh SQL qua ADO.NET (`ExecuteNonQuery`, `ExecuteReader`, `GetDataTable`, `ExecuteScalar`).
  - `SinhVienDAO` / `SinhVienDAL`: Thực hiện truy vấn CRUD dữ liệu sinh viên trực tiếp trên CSDL với Parameterized Queries (`SqlParameter`).
  - `LopHocDAO` / `LopHocDAL`: Thực hiện thao tác CRUD danh mục lớp học.
  - **Quy tắc ngoại lệ**: Ném ngoại lệ (`throw new Exception(...)`) khi xảy ra lỗi SQL, không nuốt lỗi.
- **Tầng Nghiệp vụ (BUS / BLL)**:
  - `SinhVienBUS`: Kiểm tra hợp lệ dữ liệu (không để trống mã/tên, điểm từ 0.0 - 10.0, kiểm tra trùng mã khi thêm mới, kiểm tra tồn tại khi sửa/xóa, kiểm tra lớp tồn tại).
  - `LopHocBUS`: Kiểm tra dữ liệu lớp học, kiểm tra toàn vẹn dữ liệu (không cho phép xóa lớp học nếu vẫn còn sinh viên thuộc lớp đó).
  - **Quy tắc ngoại lệ**: Ném ngoại lệ (`throw new Exception(...)`) lên tầng trên khi vi phạm quy tắc nghiệp vụ hoặc nhận lỗi từ DAL.
- **Tầng Giao diện (View / GUI)**:
  - `MainForm`: Quản lý danh sách sinh viên, hiển thị DataGridView, các ô nhập liệu, tìm kiếm từ khóa, lọc theo lớp, thêm, sửa, xóa, làm mới.
  - `FormLopHoc`: Quản lý danh mục lớp học (thêm, sửa, xóa).
  - **Quy tắc ngoại lệ**: Mọi thao tác và sự kiện đều được bọc trong khối `try ... catch` để bắt toàn bộ ngoại lệ và hiển thị thông báo lỗi thân thiện cho người dùng bằng `MessageBox.Show`.

### 2. Cấu trúc thư mục `poject w5`
```
poject w5/
├── database.sql                  # Script SQL tạo CSDL QLSV và dữ liệu mẫu
├── README.md                     # Hướng dẫn chi tiết dự án tuần 5
├── QuanLySinhVien.sln            # Solution mở trong Visual Studio
└── QuanLySinhVien/
    ├── QuanLySinhVien.csproj     # Project .NET 8.0-windows
    ├── App.config                # Cấu hình chuỗi kết nối CSDL
    ├── Program.cs                # Điểm khởi chạy ứng dụng
    ├── Entity/                   # SinhVien.cs, LopHoc.cs
    ├── DAL/                      # DataHelper.cs, SinhVienDAO.cs, LopHocDAO.cs, ...
    ├── BUS/                      # SinhVienBUS.cs, LopHocBUS.cs
    └── GUI/                      # MainForm.cs, FormLopHoc.cs
```

### 3. Hướng dẫn chạy chương trình
1. **Tạo CSDL**: Chạy file `poject w5/database.sql` trong SQL Server Management Studio (SSMS).
2. **Cấu hình chuỗi kết nối**: Mở `poject w5/QuanLySinhVien/App.config` và điều chỉnh `Server` nếu cần.
3. **Chạy ứng dụng**: Mở `poject w5/QuanLySinhVien.sln` trên Visual Studio 2022 và bấm **F5**.
