using System;

namespace QuanLySinhVien.Data.Entity
{
    public class SinhVien
    {
        public string MaSV { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string GioiTinh { get; set; } = string.Empty;
        public string NgaySinh { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string MaLop { get; set; } = string.Empty;
        public double Diem { get; set; } = 0.0;
        public string Email { get; set; } = string.Empty;

        public SinhVien()
        {
        }

        public SinhVien(string maSV, string hoTen, string gioiTinh, string ngaySinh, 
                        string soDienThoai, string maLop, double diem, string email)
        {
            MaSV = maSV;
            HoTen = hoTen;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
            SoDienThoai = soDienThoai;
            MaLop = maLop;
            Diem = diem;
            Email = email;
        }

        public override string ToString()
        {
            return $"{MaSV} - {HoTen} ({MaLop})";
        }
    }
}
