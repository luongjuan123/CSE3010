using System;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.DAL
{
    /// <summary>
    /// Lớp LopHocDAL kế thừa từ LopHocDAO để đảm bảo tính tương thích với cả hai cách đặt tên (DAL / DAO)
    /// </summary>
    public class LopHocDAL : LopHocDAO
    {
        public LopHocDAL() : base()
        {
        }

        public LopHocDAL(DataHelper helper) : base(helper)
        {
        }

        public LopHocDAL(string connectionString) : base(connectionString)
        {
        }
    }
}
