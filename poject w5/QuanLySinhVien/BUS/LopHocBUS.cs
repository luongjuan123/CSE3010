using System;
using System.Collections.Generic;
using QuanLySinhVien.Data.DAL;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.BUS
{
    public class LopHocBUS
    {
        private readonly LopHocDAO lopHocDAO;

        public LopHocBUS()
        {
            lopHocDAO = new LopHocDAO();
        }

        public LopHocBUS(LopHocDAO dao)
        {
            lopHocDAO = dao;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách lớp học. Ném ngoại lệ khi có lỗi.
        /// </summary>
        public List<LopHoc> GetAllLopHoc()
        {
            try
            {
                return lopHocDAO.GetAllLopHoc();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lấy danh sách lớp học: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin lớp học theo mã.
        /// </summary>
        public LopHoc? GetLopHocById(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
            {
                throw new ArgumentException("Mã lớp học không được để trống!");
            }

            try
            {
                return lopHocDAO.GetLopHocById(maLop.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi tìm lớp học: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Thêm mới lớp học. Ném ngoại lệ nếu vi phạm ràng buộc dữ liệu hoặc trùng mã.
        /// </summary>
        public void AddLopHoc(LopHoc lop)
        {
            if (lop == null)
            {
                throw new ArgumentNullException(nameof(lop), "Dữ liệu lớp học không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(lop.MaLop))
            {
                throw new ArgumentException("Mã lớp học không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(lop.TenLop))
            {
                throw new ArgumentException("Tên lớp học không được để trống!");
            }

            try
            {
                string cleanMaLop = lop.MaLop.Trim().ToUpper();
                if (lopHocDAO.CheckExists(cleanMaLop))
                {
                    throw new Exception($"Mã lớp học '{cleanMaLop}' đã tồn tại trong hệ thống!");
                }

                lop.MaLop = cleanMaLop;
                lop.TenLop = lop.TenLop.Trim();

                lopHocDAO.AddLopHoc(lop);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi thêm lớp học: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin lớp học.
        /// </summary>
        public void UpdateLopHoc(LopHoc lop)
        {
            if (lop == null)
            {
                throw new ArgumentNullException(nameof(lop), "Dữ liệu lớp học không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(lop.MaLop))
            {
                throw new ArgumentException("Mã lớp học không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(lop.TenLop))
            {
                throw new ArgumentException("Tên lớp học không được để trống!");
            }

            try
            {
                string cleanMaLop = lop.MaLop.Trim().ToUpper();
                if (!lopHocDAO.CheckExists(cleanMaLop))
                {
                    throw new Exception($"Không tìm thấy lớp học có mã '{cleanMaLop}' để cập nhật!");
                }

                lop.MaLop = cleanMaLop;
                lop.TenLop = lop.TenLop.Trim();

                lopHocDAO.UpdateLopHoc(lop);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi cập nhật lớp học: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Xóa lớp học. Kiểm tra ràng buộc toàn vẹn: nếu còn sinh viên trong lớp thì ném ngoại lệ không cho xóa.
        /// </summary>
        public void DeleteLopHoc(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
            {
                throw new ArgumentException("Mã lớp học không được để trống!");
            }

            try
            {
                string cleanMaLop = maLop.Trim().ToUpper();
                if (!lopHocDAO.CheckExists(cleanMaLop))
                {
                    throw new Exception($"Không tìm thấy lớp học có mã '{cleanMaLop}' để xóa!");
                }

                int countSV = lopHocDAO.CountSinhVienInLop(cleanMaLop);
                if (countSV > 0)
                {
                    throw new Exception($"Không thể xóa lớp '{cleanMaLop}' vì hiện đang có {countSV} sinh viên thuộc lớp này! Hãy xóa hoặc chuyển lớp cho các sinh viên trước.");
                }

                lopHocDAO.DeleteLopHoc(cleanMaLop);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi xóa lớp học: " + ex.Message, ex);
            }
        }
    }
}
