using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.Data.DAL
{
    public class SinhVienDAO
    {
        protected DataHelper dataHelper;

        public SinhVienDAO()
        {
            dataHelper = new DataHelper();
        }

        public SinhVienDAO(DataHelper helper)
        {
            dataHelper = helper;
        }

        public SinhVienDAO(string connectionString)
        {
            dataHelper = new DataHelper(connectionString);
        }

        /// <summary>
        /// Lấy toàn bộ danh sách sinh viên bằng DataTable
        /// </summary>
        public virtual List<SinhVien> GetAllSinhVien()
        {
            try
            {
                List<SinhVien> list = new List<SinhVien>();
                string sql = "SELECT MaSV, HoTen, GioiTinh, NgaySinh, SoDienThoai, MaLop, Diem, Email FROM SinhVien";
                DataTable dt = dataHelper.GetDataTable(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    SinhVien sv = new SinhVien
                    {
                        MaSV = dr["MaSV"]?.ToString() ?? string.Empty,
                        HoTen = dr["HoTen"]?.ToString() ?? string.Empty,
                        GioiTinh = dr["GioiTinh"]?.ToString() ?? string.Empty,
                        NgaySinh = dr["NgaySinh"]?.ToString() ?? string.Empty,
                        SoDienThoai = dr["SoDienThoai"]?.ToString() ?? string.Empty,
                        MaLop = dr["MaLop"]?.ToString() ?? string.Empty,
                        Diem = dr["Diem"] != DBNull.Value ? Convert.ToDouble(dr["Diem"]) : 0.0,
                        Email = dr["Email"]?.ToString() ?? string.Empty
                    };
                    list.Add(sv);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DataAccess (GetAllSinhVien): " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách sinh viên bằng SqlDataReader
        /// </summary>
        public virtual List<SinhVien> GetAllSinhVienDataReader()
        {
            SqlDataReader? dr = null;
            try
            {
                List<SinhVien> list = new List<SinhVien>();
                string sql = "SELECT MaSV, HoTen, GioiTinh, NgaySinh, SoDienThoai, MaLop, Diem, Email FROM SinhVien";
                dr = dataHelper.ExcuteReader(sql);

                while (dr.Read())
                {
                    SinhVien sv = new SinhVien
                    {
                        MaSV = dr["MaSV"]?.ToString() ?? string.Empty,
                        HoTen = dr["HoTen"]?.ToString() ?? string.Empty,
                        GioiTinh = dr["GioiTinh"]?.ToString() ?? string.Empty,
                        NgaySinh = dr["NgaySinh"]?.ToString() ?? string.Empty,
                        SoDienThoai = dr["SoDienThoai"]?.ToString() ?? string.Empty,
                        MaLop = dr["MaLop"]?.ToString() ?? string.Empty,
                        Diem = dr["Diem"] != DBNull.Value ? Convert.ToDouble(dr["Diem"]) : 0.0,
                        Email = dr["Email"]?.ToString() ?? string.Empty
                    };
                    list.Add(sv);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DataAccess (GetAllSinhVienDataReader): " + ex.Message, ex);
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
                dataHelper.CloseConnection();
            }
        }

        /// <summary>
        /// Lấy thông tin sinh viên theo Mã SV trực tiếp từ CSDL bằng ADO.NET
        /// </summary>
        public virtual SinhVien? GetSinhVienById(string maSV)
        {
            try
            {
                string sql = "SELECT MaSV, HoTen, GioiTinh, NgaySinh, SoDienThoai, MaLop, Diem, Email FROM SinhVien WHERE MaSV = @MaSV";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaSV", SqlDbType.VarChar) { Value = maSV }
                };

                DataTable dt = dataHelper.GetDataTable(sql, parameters);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    return new SinhVien
                    {
                        MaSV = dr["MaSV"]?.ToString() ?? string.Empty,
                        HoTen = dr["HoTen"]?.ToString() ?? string.Empty,
                        GioiTinh = dr["GioiTinh"]?.ToString() ?? string.Empty,
                        NgaySinh = dr["NgaySinh"]?.ToString() ?? string.Empty,
                        SoDienThoai = dr["SoDienThoai"]?.ToString() ?? string.Empty,
                        MaLop = dr["MaLop"]?.ToString() ?? string.Empty,
                        Diem = dr["Diem"] != DBNull.Value ? Convert.ToDouble(dr["Diem"]) : 0.0,
                        Email = dr["Email"]?.ToString() ?? string.Empty
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi tìm sinh viên có mã '{maSV}': " + ex.Message, ex);
            }
        }

        public virtual SinhVien? GetSinhVienByMaSV(string maSV)
        {
            return GetSinhVienById(maSV);
        }

        /// <summary>
        /// Lấy danh sách sinh viên theo Mã Lớp bằng ADO.NET
        /// </summary>
        public virtual List<SinhVien> GetSinhViensByMaLop(string maLop)
        {
            try
            {
                List<SinhVien> list = new List<SinhVien>();
                string sql = "SELECT MaSV, HoTen, GioiTinh, NgaySinh, SoDienThoai, MaLop, Diem, Email FROM SinhVien WHERE MaLop = @MaLop";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaLop", SqlDbType.VarChar) { Value = maLop }
                };

                DataTable dt = dataHelper.GetDataTable(sql, parameters);
                foreach (DataRow dr in dt.Rows)
                {
                    SinhVien sv = new SinhVien
                    {
                        MaSV = dr["MaSV"]?.ToString() ?? string.Empty,
                        HoTen = dr["HoTen"]?.ToString() ?? string.Empty,
                        GioiTinh = dr["GioiTinh"]?.ToString() ?? string.Empty,
                        NgaySinh = dr["NgaySinh"]?.ToString() ?? string.Empty,
                        SoDienThoai = dr["SoDienThoai"]?.ToString() ?? string.Empty,
                        MaLop = dr["MaLop"]?.ToString() ?? string.Empty,
                        Diem = dr["Diem"] != DBNull.Value ? Convert.ToDouble(dr["Diem"]) : 0.0,
                        Email = dr["Email"]?.ToString() ?? string.Empty
                    };
                    list.Add(sv);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi lấy danh sách sinh viên theo lớp '{maLop}': " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Thêm mới sinh viên vào CSDL bằng ADO.NET Parameterized Query
        /// </summary>
        public virtual void AddSinhVien(SinhVien sv)
        {
            try
            {
                string sql = @"INSERT INTO SinhVien (MaSV, MaLop, HoTen, GioiTinh, NgaySinh, Diem, SoDienThoai, Email)
                               VALUES (@MaSV, @MaLop, @HoTen, @GioiTinh, @NgaySinh, @Diem, @SoDienThoai, @Email)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaSV", SqlDbType.VarChar) { Value = sv.MaSV },
                    new SqlParameter("@MaLop", SqlDbType.VarChar) { Value = sv.MaLop },
                    new SqlParameter("@HoTen", SqlDbType.NVarChar) { Value = sv.HoTen },
                    new SqlParameter("@GioiTinh", SqlDbType.NVarChar) { Value = (object?)sv.GioiTinh ?? DBNull.Value },
                    new SqlParameter("@NgaySinh", SqlDbType.NVarChar) { Value = (object?)sv.NgaySinh ?? DBNull.Value },
                    new SqlParameter("@Diem", SqlDbType.Float) { Value = sv.Diem },
                    new SqlParameter("@SoDienThoai", SqlDbType.VarChar) { Value = (object?)sv.SoDienThoai ?? DBNull.Value },
                    new SqlParameter("@Email", SqlDbType.VarChar) { Value = (object?)sv.Email ?? DBNull.Value }
                };

                dataHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi thêm sinh viên '{sv.MaSV}': " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin sinh viên bằng ADO.NET (khắc phục câu SQL lỗi cú pháp)
        /// </summary>
        public virtual void UpdateSinhVien(SinhVien sv)
        {
            try
            {
                string sql = @"UPDATE SinhVien 
                               SET MaLop = @MaLop, 
                                   HoTen = @HoTen, 
                                   GioiTinh = @GioiTinh, 
                                   NgaySinh = @NgaySinh, 
                                   Diem = @Diem, 
                                   SoDienThoai = @SoDienThoai, 
                                   Email = @Email 
                               WHERE MaSV = @MaSV";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaSV", SqlDbType.VarChar) { Value = sv.MaSV },
                    new SqlParameter("@MaLop", SqlDbType.VarChar) { Value = sv.MaLop },
                    new SqlParameter("@HoTen", SqlDbType.NVarChar) { Value = sv.HoTen },
                    new SqlParameter("@GioiTinh", SqlDbType.NVarChar) { Value = (object?)sv.GioiTinh ?? DBNull.Value },
                    new SqlParameter("@NgaySinh", SqlDbType.NVarChar) { Value = (object?)sv.NgaySinh ?? DBNull.Value },
                    new SqlParameter("@Diem", SqlDbType.Float) { Value = sv.Diem },
                    new SqlParameter("@SoDienThoai", SqlDbType.VarChar) { Value = (object?)sv.SoDienThoai ?? DBNull.Value },
                    new SqlParameter("@Email", SqlDbType.VarChar) { Value = (object?)sv.Email ?? DBNull.Value }
                };

                int rowsAffected = dataHelper.ExecuteNonQuery(sql, parameters);
                if (rowsAffected == 0)
                {
                    throw new Exception($"Không tìm thấy sinh viên có mã '{sv.MaSV}' để cập nhật trong CSDL.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi cập nhật sinh viên '{sv.MaSV}': " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Xóa sinh viên trực tiếp trong CSDL bằng ADO.NET (thay thế xóa trong list bộ nhớ)
        /// </summary>
        public virtual void DeleteSinhVien(string maSV)
        {
            try
            {
                string sql = "DELETE FROM SinhVien WHERE MaSV = @MaSV";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaSV", SqlDbType.VarChar) { Value = maSV }
                };

                int rows = dataHelper.ExecuteNonQuery(sql, parameters);
                if (rows == 0)
                {
                    throw new Exception($"Không tìm thấy sinh viên có mã '{maSV}' để xóa trong CSDL.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi xóa sinh viên '{maSV}': " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Kiểm tra sinh viên có tồn tại theo MaSV hay không
        /// </summary>
        public virtual bool CheckExists(string maSV)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM SinhVien WHERE MaSV = @MaSV";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaSV", SqlDbType.VarChar) { Value = maSV }
                };

                object? result = dataHelper.ExecuteScalar(sql, parameters);
                int count = result != null ? Convert.ToInt32(result) : 0;
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi kiểm tra tồn tại của sinh viên '{maSV}': " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sinh viên theo từ khóa (Mã SV, Họ tên, Email, Số ĐT)
        /// </summary>
        public virtual List<SinhVien> SearchSinhVien(string keyword)
        {
            try
            {
                List<SinhVien> list = new List<SinhVien>();
                string sql = @"SELECT MaSV, HoTen, GioiTinh, NgaySinh, SoDienThoai, MaLop, Diem, Email 
                               FROM SinhVien 
                               WHERE MaSV LIKE @kw OR HoTen LIKE @kw OR Email LIKE @kw OR SoDienThoai LIKE @kw";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@kw", SqlDbType.NVarChar) { Value = $"%{keyword}%" }
                };

                DataTable dt = dataHelper.GetDataTable(sql, parameters);
                foreach (DataRow dr in dt.Rows)
                {
                    SinhVien sv = new SinhVien
                    {
                        MaSV = dr["MaSV"]?.ToString() ?? string.Empty,
                        HoTen = dr["HoTen"]?.ToString() ?? string.Empty,
                        GioiTinh = dr["GioiTinh"]?.ToString() ?? string.Empty,
                        NgaySinh = dr["NgaySinh"]?.ToString() ?? string.Empty,
                        SoDienThoai = dr["SoDienThoai"]?.ToString() ?? string.Empty,
                        MaLop = dr["MaLop"]?.ToString() ?? string.Empty,
                        Diem = dr["Diem"] != DBNull.Value ? Convert.ToDouble(dr["Diem"]) : 0.0,
                        Email = dr["Email"]?.ToString() ?? string.Empty
                    };
                    list.Add(sv);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DataAccess khi tìm kiếm sinh viên với từ khóa '{keyword}': " + ex.Message, ex);
            }
        }
    }
}
