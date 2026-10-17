using System;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLySinhVien.Data.DAL
{
    public class DataHelper
    {
        private SqlConnection con;
        private string connectionString;

        public string ConnectionString
        {
            get => connectionString;
            set
            {
                connectionString = value;
                con = new SqlConnection(connectionString);
            }
        }

        public DataHelper()
        {
            // Ưu tiên đọc chuỗi kết nối từ App.config nếu có, ngược lại dùng chuỗi mặc định
            string? configConn = ConfigurationManager.ConnectionStrings["QLSVConnectionString"]?.ConnectionString;
            if (!string.IsNullOrWhiteSpace(configConn))
            {
                this.connectionString = configConn;
            }
            else
            {
                this.connectionString = @"Server=LAPTOP-6SN65NCS\HUECTMDB; Database=QLSV;Integrated Security=True; TrustServerCertificate=true";
            }
            con = new SqlConnection(this.connectionString);
        }

        public DataHelper(string customConnectionString)
        {
            this.connectionString = customConnectionString;
            con = new SqlConnection(this.connectionString);
        }

        public void OpenConnection()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể kết nối đến cơ sở dữ liệu: " + ex.Message, ex);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đóng kết nối cơ sở dữ liệu: " + ex.Message, ex);
            }
        }

        public void ExecuteNonQuery(string sqlExcuteQuery)
        {
            ExecuteNonQuery(sqlExcuteQuery, null);
        }

        public int ExecuteNonQuery(string sql, SqlParameter[]? parameters)
        {
            try
            {
                OpenConnection();
                using (SqlCommand comand = new SqlCommand(sql, con))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        comand.Parameters.AddRange(parameters);
                    }
                    int rows = comand.ExecuteNonQuery();
                    return rows;
                }
            }
            catch (Exception exx)
            {
                throw new Exception("Lỗi khi thực thi câu lệnh SQL (ExecuteNonQuery): " + exx.Message, exx);
            }
            finally
            {
                CloseConnection();
            }
        }

        // Tên phương thức theo mẫu của đề bài (ExcuteReader)
        public SqlDataReader ExcuteReader(string sqlSelect)
        {
            return ExecuteReader(sqlSelect, null);
        }

        public SqlDataReader ExecuteReader(string sqlSelect, SqlParameter[]? parameters = null)
        {
            try
            {
                OpenConnection();
                SqlCommand comand = new SqlCommand(sqlSelect, con);
                if (parameters != null && parameters.Length > 0)
                {
                    comand.Parameters.AddRange(parameters);
                }
                SqlDataReader red = comand.ExecuteReader();
                return red;
            }
            catch (Exception ex)
            {
                CloseConnection();
                throw new Exception("Lỗi đọc dữ liệu từ SQL (ExecuteReader): " + ex.Message, ex);
            }
        }

        public DataTable GetDataTable(string sqlSelect)
        {
            return GetDataTable(sqlSelect, null);
        }

        public DataTable GetDataTable(string sqlSelect, SqlParameter[]? parameters)
        {
            try
            {
                OpenConnection();
                using (SqlCommand cmd = new SqlCommand(sqlSelect, con))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi truy vấn dữ liệu vào DataTable: " + ex.Message, ex);
            }
            finally
            {
                CloseConnection();
            }
        }

        public object? ExecuteScalar(string sql, SqlParameter[]? parameters = null)
        {
            try
            {
                OpenConnection();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thực thi câu lệnh SQL (ExecuteScalar): " + ex.Message, ex);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
