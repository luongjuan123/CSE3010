using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using QuanLySinhVien.BUS;
using QuanLySinhVien.Data.Entity;

namespace QuanLySinhVien.GUI
{
    public partial class MainForm : Form
    {
        private readonly SinhVienBUS sinhVienBUS;
        private readonly LopHocBUS lopHocBUS;

        public MainForm()
        {
            InitializeComponent();
            sinhVienBUS = new SinhVienBUS();
            lopHocBUS = new LopHocBUS();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Tầng View xử lý ngoại lệ khi Form được nạp
            try
            {
                cboGioiTinh.SelectedIndex = 0;
                LoadDanhSachLop();
                LoadDanhSachSinhVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo dữ liệu: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Nạp danh sách lớp học vào ComboBox Thông tin và ComboBox Lọc
        /// </summary>
        public void LoadDanhSachLop()
        {
            try
            {
                List<LopHoc> dsLop = lopHocBUS.GetAllLopHoc();

                // Binding vào ComboBox lớp học ở phần nhập liệu
                cboLopHoc.DataSource = null;
                cboLopHoc.DisplayMember = "TenLop";
                cboLopHoc.ValueMember = "MaLop";
                cboLopHoc.DataSource = new List<LopHoc>(dsLop);

                // Binding vào ComboBox lọc theo lớp
                List<LopHoc> dsLopLoc = new List<LopHoc>
                {
                    new LopHoc { MaLop = "ALL", TenLop = "-- Tất cả các lớp --" }
                };
                dsLopLoc.AddRange(dsLop);

                cboLocLop.SelectedIndexChanged -= cboLocLop_SelectedIndexChanged;
                cboLocLop.DataSource = null;
                cboLocLop.DisplayMember = "TenLop";
                cboLocLop.ValueMember = "MaLop";
                cboLocLop.DataSource = dsLopLoc;
                cboLocLop.SelectedIndex = 0;
                cboLocLop.SelectedIndexChanged += cboLocLop_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách lớp học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Nạp toàn bộ danh sách sinh viên lên DataGridView
        /// </summary>
        public void LoadDanhSachSinhVien()
        {
            try
            {
                List<SinhVien> dsSV = sinhVienBUS.GetAllSinhVien();
                HienThiDanhSachSinhVien(dsSV);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Định dạng hiển thị DataGridView
        /// </summary>
        private void HienThiDanhSachSinhVien(List<SinhVien> ds)
        {
            dgvSinhVien.DataSource = null;
            dgvSinhVien.DataSource = ds;

            if (dgvSinhVien.Columns["MaSV"] != null)
                dgvSinhVien.Columns["MaSV"].HeaderText = "Mã SV";
            if (dgvSinhVien.Columns["HoTen"] != null)
                dgvSinhVien.Columns["HoTen"].HeaderText = "Họ và tên";
            if (dgvSinhVien.Columns["GioiTinh"] != null)
                dgvSinhVien.Columns["GioiTinh"].HeaderText = "Giới tính";
            if (dgvSinhVien.Columns["NgaySinh"] != null)
                dgvSinhVien.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            if (dgvSinhVien.Columns["SoDienThoai"] != null)
                dgvSinhVien.Columns["SoDienThoai"].HeaderText = "Số ĐT";
            if (dgvSinhVien.Columns["MaLop"] != null)
                dgvSinhVien.Columns["MaLop"].HeaderText = "Mã Lớp";
            if (dgvSinhVien.Columns["Diem"] != null)
                dgvSinhVien.Columns["Diem"].HeaderText = "Điểm";
            if (dgvSinhVien.Columns["Email"] != null)
                dgvSinhVien.Columns["Email"].HeaderText = "Email";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Tầng View xử lý ngoại lệ cho thao tác Thêm
            try
            {
                if (cboLopHoc.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn hoặc tạo lớp học trước khi thêm sinh viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!double.TryParse(txtDiem.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double diem))
                {
                    if (!double.TryParse(txtDiem.Text.Trim(), out diem))
                    {
                        MessageBox.Show("Điểm phải là định dạng số hợp lệ (ví dụ: 8.5)!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDiem.Focus();
                        return;
                    }
                }

                SinhVien sv = new SinhVien
                {
                    MaSV = txtMaSV.Text.Trim(),
                    HoTen = txtHoTen.Text.Trim(),
                    GioiTinh = cboGioiTinh.SelectedItem?.ToString() ?? "Nam",
                    NgaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd"),
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    MaLop = cboLopHoc.SelectedValue.ToString() ?? string.Empty,
                    Diem = diem,
                    Email = txtEmail.Text.Trim()
                };

                // Gọi tầng Business, nếu có lỗi (trùng mã, điểm ngoài 0-10, ...) BUS sẽ throw exception
                sinhVienBUS.AddSinhVien(sv);

                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachSinhVien();
                ResetForm();
            }
            catch (Exception ex)
            {
                // Bắt và hiển thị lỗi từ tầng Business hoặc DataAccess
                MessageBox.Show(ex.Message, "Lỗi thêm sinh viên", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Tầng View xử lý ngoại lệ cho thao tác Sửa
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSV.Text))
                {
                    MessageBox.Show("Vui lòng chọn sinh viên cần sửa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboLopHoc.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn lớp học!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!double.TryParse(txtDiem.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double diem))
                {
                    if (!double.TryParse(txtDiem.Text.Trim(), out diem))
                    {
                        MessageBox.Show("Điểm phải là định dạng số hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDiem.Focus();
                        return;
                    }
                }

                SinhVien sv = new SinhVien
                {
                    MaSV = txtMaSV.Text.Trim(),
                    HoTen = txtHoTen.Text.Trim(),
                    GioiTinh = cboGioiTinh.SelectedItem?.ToString() ?? "Nam",
                    NgaySinh = dtpNgaySinh.Value.ToString("yyyy-MM-dd"),
                    SoDienThoai = txtSoDienThoai.Text.Trim(),
                    MaLop = cboLopHoc.SelectedValue.ToString() ?? string.Empty,
                    Diem = diem,
                    Email = txtEmail.Text.Trim()
                };

                sinhVienBUS.UpdateSinhVien(sv);

                MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachSinhVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật sinh viên", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Tầng View xử lý ngoại lệ cho thao tác Xóa
            try
            {
                string maSV = txtMaSV.Text.Trim();
                if (string.IsNullOrWhiteSpace(maSV))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã sinh viên cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên có mã '{maSV}'?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    sinhVienBUS.DeleteSinhVien(maSV);

                    MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachSinhVien();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi xóa sinh viên", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtMaSV.Text = string.Empty;
            txtHoTen.Text = string.Empty;
            cboGioiTinh.SelectedIndex = 0;
            dtpNgaySinh.Value = DateTime.Now;
            txtSoDienThoai.Text = string.Empty;
            txtDiem.Text = string.Empty;
            txtEmail.Text = string.Empty;
            if (cboLopHoc.Items.Count > 0)
                cboLopHoc.SelectedIndex = 0;
            txtMaSV.Focus();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // Tầng View xử lý ngoại lệ cho thao tác Tìm kiếm
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                List<SinhVien> ketQua = sinhVienBUS.SearchSinhVien(keyword);
                HienThiDanhSachSinhVien(ketQua);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void cboLocLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Tầng View xử lý ngoại lệ khi lọc theo lớp
            try
            {
                if (cboLocLop.SelectedValue == null) return;

                string maLop = cboLocLop.SelectedValue.ToString() ?? string.Empty;
                if (maLop == "ALL" || string.IsNullOrWhiteSpace(maLop))
                {
                    LoadDanhSachSinhVien();
                }
                else
                {
                    List<SinhVien> dsTheoLop = sinhVienBUS.GetSinhViensByMaLop(maLop);
                    HienThiDanhSachSinhVien(dsTheoLop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi lọc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTatCa_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = string.Empty;
            if (cboLocLop.Items.Count > 0)
                cboLocLop.SelectedIndex = 0;
            LoadDanhSachSinhVien();
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Tầng View xử lý ngoại lệ khi chọn dòng DataGridView
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvSinhVien.Rows.Count)
                {
                    DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                    txtMaSV.Text = row.Cells["MaSV"]?.Value?.ToString() ?? string.Empty;
                    txtHoTen.Text = row.Cells["HoTen"]?.Value?.ToString() ?? string.Empty;

                    string gt = row.Cells["GioiTinh"]?.Value?.ToString() ?? "Nam";
                    int idx = cboGioiTinh.FindStringExact(gt);
                    cboGioiTinh.SelectedIndex = idx >= 0 ? idx : 0;

                    string nsStr = row.Cells["NgaySinh"]?.Value?.ToString() ?? string.Empty;
                    if (DateTime.TryParse(nsStr, out DateTime ns))
                    {
                        dtpNgaySinh.Value = ns;
                    }

                    txtSoDienThoai.Text = row.Cells["SoDienThoai"]?.Value?.ToString() ?? string.Empty;
                    txtDiem.Text = row.Cells["Diem"]?.Value?.ToString() ?? "0";
                    txtEmail.Text = row.Cells["Email"]?.Value?.ToString() ?? string.Empty;

                    string maLop = row.Cells["MaLop"]?.Value?.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(maLop))
                    {
                        cboLopHoc.SelectedValue = maLop;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị chi tiết dòng dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQuanLyLop_Click(object sender, EventArgs e)
        {
            // Tầng View xử lý ngoại lệ khi mở form Quản lý lớp
            try
            {
                using (FormLopHoc frm = new FormLopHoc())
                {
                    frm.ShowDialog();
                    // Khi đóng form Quản lý lớp, nạp lại danh sách lớp cho MainForm
                    LoadDanhSachLop();
                    LoadDanhSachSinhVien();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở giao diện Quản lý Lớp học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
