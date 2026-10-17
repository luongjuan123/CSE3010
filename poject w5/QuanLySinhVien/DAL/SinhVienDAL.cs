using System;
using System.Collections.Generic;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.DAL
{
    /// <summary>
    /// Lớp SinhVienDAL kế thừa từ SinhVienDAO để đảm bảo tính tương thích với cả hai cách đặt tên (DAL / DAO)
    /// </summary>
    public class SinhVienDAL : SinhVienDAO
    {
        public SinhVienDAL() : base()
        {
        }

        public SinhVienDAL(DataHelper helper) : base(helper)
        {
        }

        public SinhVienDAL(string connectionString) : base(connectionString)
        {
        }

        // Các phương thức alias tương thích với đề bài gốc
        public List<SinhVien> getAllSinhVien1()
        {
            return GetAllSinhVienDataReader();
        }

        public List<SinhVien> GetAllSinhVien1()
        {
            return GetAllSinhVienDataReader();
        }
    }
}
