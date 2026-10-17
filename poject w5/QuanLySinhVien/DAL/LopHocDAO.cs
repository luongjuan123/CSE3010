using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.DAL
{
    public class LopHocDAO
    {
        protected DataHelper dataHelper;

        public LopHocDAO()
        {
            dataHelper = new DataHelper();
        }

        public LopHocDAO(DataHelper helper)
        {
            dataHelper = helper;
        }

        public LopHocDAO(string connectionString)
        {
            dataHelper = new DataHelper(connectionString);
        }

        /// <summary>
        /// Lấy toàn bộ danh sách lớp học từ CSDL
        /// </summary>
        public virtual List<LopHoc> GetAllLopHoc()
        {
            try
            {
                List<LopHoc> list = new List<LopHoc>();
                string sql = "SELECT MaLop, TenLop FROM LopHoc";
                DataTable dt = dataHelper.GetDataTable(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    LopHoc lop = new LopHoc
                    {
                        MaLop = dr["MaLop"]?.ToString() ?? string.Empty,
                        TenLop = dr["TenLop"]?.ToString() ?? string.Empty
                    };
                    list.Add(lop);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DataAccess (GetAllLopHoc): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin lớp học theo Mã Lớp
        /// </summary>
        public virtual LopHoc? GetLopHocById(string maLop)
        {
            try
            {
                string sql = "SELECT MaLop, TenLop FROM LopHoc WHERE MaLop = @MaLop";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaLop", SqlDbType.VarChar) { Value = maLop }
                };

                DataTable dt = dataHelper.GetDataTable(sql, parameters);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    return new LopHoc
                    {
                        MaLop = dr["MaLop"]?.ToString() ?? string.Empty,
                        TenLop = dr["TenLop"]?.ToString() ?? string.Empty
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi tìm lớp học có mã '{maLop}': " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Thêm mới lớp học vào CSDL bằng ADO.NET
        /// </summary>
        public virtual void AddLopHoc(LopHoc lop)
        {
            try
            {
                string sql = "INSERT INTO LopHoc (MaLop, TenLop) VALUES (@MaLop, @TenLop)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaLop", SqlDbType.VarChar) { Value = lop.MaLop },
                    new SqlParameter("@TenLop", SqlDbType.NVarChar) { Value = lop.TenLop }
                };

                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi thêm lớp học '{lop.MaLop}': " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin lớp học bằng ADO.NET
        /// </summary>
        public virtual void UpdateLopHoc(LopHoc lop)
        {
            try
            {
                string sql = "UPDATE LopHoc SET TenLop = @TenLop WHERE MaLop = @MaLop";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaLop", SqlDbType.VarChar) { Value = lop.MaLop },
                    new SqlParameter("@TenLop", SqlDbType.NVarChar) { Value = lop.TenLop }
                };

                int rows = dataHelper.ExecuteNonQuery(sql, parameters);
                if (rows == 0)
                {
                    throw new Exception($"Không tìm thấy lớp học có mã '{lop.MaLop}' để cập nhật trong CSDL.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi cập nhật lớp học '{lop.MaLop}': " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Xóa lớp học theo Mã Lớp bằng ADO.NET
        /// </summary>
        public virtual void DeleteLopHoc(string maLop)
        {
            try
            {
                string sql = "DELETE FROM LopHoc WHERE MaLop = @MaLop";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaLop", SqlDbType.VarChar) { Value = maLop }
                };

                int rows = dataHelper.ExecuteNonQuery(sql, parameters);
                if (rows == 0)
                {
                    throw new Exception($"Không tìm thấy lớp học có mã '{maLop}' để xóa trong CSDL.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi xóa lớp học '{maLop}': " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Kiểm tra lớp học có tồn tại hay không
        /// </summary>
        public virtual bool CheckExists(string maLop)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM LopHoc WHERE MaLop = @MaLop";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaLop", SqlDbType.VarChar) { Value = maLop }
                };

                object? result = dataHelper.ExecuteScalar(sql, parameters);
                int count = result != null ? Convert.ToInt32(result) : 0;
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi kiểm tra tồn tại của lớp '{maLop}': " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Đếm số lượng sinh viên đang thuộc lớp học
        /// </summary>
        public virtual int CountSinhVienInLop(string maLop)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM SinhVien WHERE MaLop = @MaLop";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaLop", SqlDbType.VarChar) { Value = maLop }
                };

                object? result = dataHelper.ExecuteScalar(sql, parameters);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi đếm số sinh viên lớp '{maLop}': " + ex.Message, ex);
            }
        }
    }
}
