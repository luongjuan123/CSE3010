using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QuanLySinhVien.BUS;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.GUI
{
    public partial class FormLopHoc : Form
    {
        private readonly LopHocBUS lopHocBUS;

        public FormLopHoc()
        {
            InitializeComponent();
            lopHocBUS = new LopHocBUS();
        }

        private void FormLopHoc_Load(object sender, EventArgs e)
        {
            // Tầng View xử lý ngoại lệ khi tải danh sách lớp học
            try
            {
                LoadDanhSachLop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách lớp học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadDanhSachLop()
        {
            try
            {
                List<LopHoc> dsLop = lopHocBUS.GetAllLopHoc();
                dgvLopHoc.DataSource = null;
                dgvLopHoc.DataSource = dsLop;

                if (dgvLopHoc.Columns["MaLop"] != null)
                    dgvLopHoc.Columns["MaLop"].HeaderText = "Mã Lớp";
                if (dgvLopHoc.Columns["TenLop"] != null)
                    dgvLopHoc.Columns["TenLop"].HeaderText = "Tên Lớp Học";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp dữ liệu lớp học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Tầng View xử lý ngoại lệ cho thao tác Thêm lớp
            try
            {
                LopHoc lop = new LopHoc
                {
                    MaLop = txtMaLop.Text.Trim(),
                    TenLop = txtTenLop.Text.Trim()
                };

                lopHocBUS.AddLopHoc(lop);

                MessageBox.Show("Thêm lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachLop();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi thêm lớp học", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Tầng View xử lý ngoại lệ cho thao tác Sửa lớp
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaLop.Text))
                {
                    MessageBox.Show("Vui lòng chọn lớp học cần sửa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LopHoc lop = new LopHoc
                {
                    MaLop = txtMaLop.Text.Trim(),
                    TenLop = txtTenLop.Text.Trim()
                };

                lopHocBUS.UpdateLopHoc(lop);

                MessageBox.Show("Cập nhật thông tin lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachLop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật lớp học", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Tầng View xử lý ngoại lệ cho thao tác Xóa lớp
            try
            {
                string maLop = txtMaLop.Text.Trim();
                if (string.IsNullOrWhiteSpace(maLop))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã lớp học cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa lớp học '{maLop}'?",
                    "Xác nhận xóa lớp học", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Nếu lớp vẫn còn sinh viên, BUS sẽ ném ngoại lệ ngăn chặn xóa để đảm bảo toàn vẹn dữ liệu
                    lopHocBUS.DeleteLopHoc(maLop);

                    MessageBox.Show("Xóa lớp học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachLop();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi xóa lớp học", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtMaLop.Text = string.Empty;
            txtTenLop.Text = string.Empty;
            txtMaLop.Focus();
        }

        private void dgvLopHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvLopHoc.Rows.Count)
                {
                    DataGridViewRow row = dgvLopHoc.Rows[e.RowIndex];
                    txtMaLop.Text = row.Cells["MaLop"]?.Value?.ToString() ?? string.Empty;
                    txtTenLop.Text = row.Cells["TenLop"]?.Value?.ToString() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị chi tiết dòng dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
