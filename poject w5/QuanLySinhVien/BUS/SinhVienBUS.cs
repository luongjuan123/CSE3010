using System;
using System.Collections.Generic;
using QuanLySinhVien.Data.DAL;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.BUS
{
    public class SinhVienBUS
    {
        private readonly SinhVienDAO sinhVienDAO;
        private readonly LopHocDAO lopHocDAO;

        public SinhVienBUS()
        {
            sinhVienDAO = new SinhVienDAO();
            lopHocDAO = new LopHocDAO();
        }

        public SinhVienBUS(SinhVienDAO svDao, LopHocDAO lhDao)
        {
            sinhVienDAO = svDao;
            lopHocDAO = lhDao;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách sinh viên. Ném ngoại lệ lên tầng trên nếu có lỗi.
        /// </summary>
        public List<SinhVien> GetAllSinhVien()
        {
            try
            {
                return sinhVienDAO.GetAllSinhVien();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lấy danh sách sinh viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Tìm sinh viên theo mã. Ném ngoại lệ nếu mã không hợp lệ hoặc lỗi DAL.
        /// </summary>
        public SinhVien? GetSinhVienById(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
            {
                throw new ArgumentException("Mã sinh viên không được để trống!");
            }

            try
            {
                return sinhVienDAO.GetSinhVienById(maSV.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi tìm sinh viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy danh sách sinh viên theo lớp.
        /// </summary>
        public List<SinhVien> GetSinhViensByMaLop(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
            {
                throw new ArgumentException("Mã lớp không được để trống!");
            }

            try
            {
                return sinhVienDAO.GetSinhViensByMaLop(maLop.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lọc sinh viên theo lớp: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sinh viên theo từ khóa.
        /// </summary>
        public List<SinhVien> SearchSinhVien(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return sinhVienDAO.GetAllSinhVien();
                }
                return sinhVienDAO.SearchSinhVien(keyword.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi tìm kiếm sinh viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Thêm mới sinh viên với đầy đủ kiểm tra ràng buộc nghiệp vụ.
        /// Ném ngoại lệ khi dữ liệu không hợp lệ hoặc trùng mã.
        /// </summary>
        public void AddSinhVien(SinhVien sv)
        {
            if (sv == null)
            {
                throw new ArgumentNullException(nameof(sv), "Dữ liệu sinh viên không được để trống!");
            }

            // Kiểm tra rỗng các trường bắt buộc
            if (string.IsNullOrWhiteSpace(sv.MaSV))
            {
                throw new ArgumentException("Mã sinh viên không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(sv.HoTen))
            {
                throw new ArgumentException("Họ và tên sinh viên không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(sv.MaLop))
            {
                throw new ArgumentException("Vui lòng chọn lớp học cho sinh viên!");
            }

            // Kiểm tra khoảng điểm từ 0 đến 10
            if (sv.Diem < 0 || sv.Diem > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(sv.Diem), "Điểm sinh viên phải nằm trong khoảng từ 0.0 đến 10.0!");
            }

            try
            {
                string cleanMaSV = sv.MaSV.Trim().ToUpper();
                string cleanMaLop = sv.MaLop.Trim();

                // Kiểm tra lớp học có tồn tại hay không
                if (!lopHocDAO.CheckExists(cleanMaLop))
                {
                    throw new Exception($"Lớp học '{cleanMaLop}' không tồn tại trong hệ thống. Vui lòng tạo lớp học trước!");
                }

                // Kiểm tra mã sinh viên đã tồn tại chưa
                if (sinhVienDAO.CheckExists(cleanMaSV))
                {
                    throw new Exception($"Mã sinh viên '{cleanMaSV}' đã tồn tại trong hệ thống!");
                }

                sv.MaSV = cleanMaSV;
                sv.MaLop = cleanMaLop;
                sv.HoTen = sv.HoTen.Trim();
                sv.GioiTinh = sv.GioiTinh?.Trim() ?? string.Empty;
                sv.NgaySinh = sv.NgaySinh?.Trim() ?? string.Empty;
                sv.SoDienThoai = sv.SoDienThoai?.Trim() ?? string.Empty;
                sv.Email = sv.Email?.Trim() ?? string.Empty;

                sinhVienDAO.AddSinhVien(sv);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi thêm sinh viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin sinh viên với kiểm tra nghiệp vụ.
        /// Ném ngoại lệ khi dữ liệu không hợp lệ hoặc sinh viên không tồn tại.
        /// </summary>
        public void UpdateSinhVien(SinhVien sv)
        {
            if (sv == null)
            {
                throw new ArgumentNullException(nameof(sv), "Dữ liệu sinh viên không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(sv.MaSV))
            {
                throw new ArgumentException("Mã sinh viên không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(sv.HoTen))
            {
                throw new ArgumentException("Họ và tên sinh viên không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(sv.MaLop))
            {
                throw new ArgumentException("Vui lòng chọn lớp học cho sinh viên!");
            }

            if (sv.Diem < 0 || sv.Diem > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(sv.Diem), "Điểm sinh viên phải nằm trong khoảng từ 0.0 đến 10.0!");
            }

            try
            {
                string cleanMaSV = sv.MaSV.Trim().ToUpper();
                string cleanMaLop = sv.MaLop.Trim();

                // Kiểm tra sinh viên có tồn tại không
                if (!sinhVienDAO.CheckExists(cleanMaSV))
                {
                    throw new Exception($"Không tìm thấy sinh viên có mã '{cleanMaSV}' để cập nhật!");
                }

                // Kiểm tra lớp học
                if (!lopHocDAO.CheckExists(cleanMaLop))
                {
                    throw new Exception($"Lớp học '{cleanMaLop}' không tồn tại trong hệ thống!");
                }

                sv.MaSV = cleanMaSV;
                sv.MaLop = cleanMaLop;
                sv.HoTen = sv.HoTen.Trim();
                sv.GioiTinh = sv.GioiTinh?.Trim() ?? string.Empty;
                sv.NgaySinh = sv.NgaySinh?.Trim() ?? string.Empty;
                sv.SoDienThoai = sv.SoDienThoai?.Trim() ?? string.Empty;
                sv.Email = sv.Email?.Trim() ?? string.Empty;

                sinhVienDAO.UpdateSinhVien(sv);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi cập nhật sinh viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Xóa sinh viên. Ném ngoại lệ khi mã không hợp lệ hoặc sinh viên không tồn tại.
        /// </summary>
        public void DeleteSinhVien(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
            {
                throw new ArgumentException("Mã sinh viên không được để trống!");
            }

            try
            {
                string cleanMaSV = maSV.Trim().ToUpper();
                if (!sinhVienDAO.CheckExists(cleanMaSV))
                {
                    throw new Exception($"Không tìm thấy sinh viên có mã '{cleanMaSV}' để xóa!");
                }

                sinhVienDAO.DeleteSinhVien(cleanMaSV);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi xóa sinh viên: " + ex.Message, ex);
            }
        }
    }
}
