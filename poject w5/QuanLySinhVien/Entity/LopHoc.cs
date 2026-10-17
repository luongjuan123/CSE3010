using System;

namespace QuanLySinhVien.Data.Entity
{
    public class LopHoc
    {
        public string MaLop { get; set; } = string.Empty;
        public string TenLop { get; set; } = string.Empty;

        public LopHoc()
        {
        }

        public LopHoc(string maLop, string tenLop)
        {
            MaLop = maLop;
            TenLop = tenLop;
        }

        public override string ToString()
        {
            return $"{MaLop} - {TenLop}";
        }
    }
}
