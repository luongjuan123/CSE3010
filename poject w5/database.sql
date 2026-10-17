-- Script tạo cơ sở dữ liệu Quản Lý Sinh Viên (QLSV)
-- Sử dụng cho bài tập ADO.NET WinForms

USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'QLSV')
BEGIN
    CREATE DATABASE QLSV;
END
GO

USE QLSV;
GO

-- Xóa bảng nếu đã tồn tại để tạo lại
IF OBJECT_ID('SinhVien', 'U') IS NOT NULL
    DROP TABLE SinhVien;
GO

IF OBJECT_ID('LopHoc', 'U') IS NOT NULL
    DROP TABLE LopHoc;
GO

-- 1. Tạo bảng LopHoc
CREATE TABLE LopHoc (
    MaLop VARCHAR(20) NOT NULL PRIMARY KEY,
    TenLop NVARCHAR(100) NOT NULL
);
GO

-- 2. Tạo bảng SinhVien
CREATE TABLE SinhVien (
    MaSV VARCHAR(20) NOT NULL PRIMARY KEY,
    MaLop VARCHAR(20) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10) NULL,
    NgaySinh NVARCHAR(20) NULL,
    Diem FLOAT NULL DEFAULT 0.0,
    SoDienThoai VARCHAR(20) NULL,
    Email VARCHAR(100) NULL,
    CONSTRAINT FK_SinhVien_LopHoc FOREIGN KEY (MaLop) REFERENCES LopHoc(MaLop)
);
GO

-- 3. Chèn dữ liệu mẫu vào bảng LopHoc
INSERT INTO LopHoc (MaLop, TenLop) VALUES 
('CNTT01', N'Công nghệ thông tin 01'),
('CNTT02', N'Công nghệ thông tin 02'),
('KTPM01', N'Kỹ thuật phần mềm 01'),
('KHMT01', N'Khoa học máy tính 01');
GO

-- 4. Chèn dữ liệu mẫu vào bảng SinhVien
INSERT INTO SinhVien (MaSV, MaLop, HoTen, GioiTinh, NgaySinh, Diem, SoDienThoai, Email) VALUES
('SV001', 'CNTT01', N'Nguyễn Văn An', N'Nam', '2003-05-15', 8.5, '0912345678', 'an.nv@example.com'),
('SV002', 'CNTT01', N'Trần Thị Bích', N'Nữ', '2003-08-20', 9.0, '0987654321', 'bich.tt@example.com'),
('SV003', 'CNTT02', N'Lê Hoàng Cường', N'Nam', '2002-12-10', 7.2, '0905123456', 'cuong.lh@example.com'),
('SV004', 'KTPM01', N'Phạm Minh Dũng', N'Nam', '2003-03-25', 8.0, '0934567890', 'dung.pm@example.com'),
('SV005', 'KHMT01', N'Đỗ Mai Hương', N'Nữ', '2003-11-02', 8.8, '0978112233', 'huong.dm@example.com');
GO

-- Kiểm tra dữ liệu
SELECT * FROM LopHoc;
SELECT * FROM SinhVien;
GO
