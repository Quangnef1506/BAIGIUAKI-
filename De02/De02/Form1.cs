using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows.Forms;
using De02.DuLieu;

namespace De02
{
    public partial class FrmSanPham : Form
    {
        DulieuSanPham dulieuSP = new DulieuSanPham();

        public FrmSanPham()
        {
            InitializeComponent();
        }



      

        private void FrmSanPham_Load(object sender, EventArgs e)
        {
            LoadLoaiSP(); 
            LoadSanPham();
        }

        private void LoadSanPham()
        {
            try
            {
                // Lấy danh sách sản phẩm kèm loại sản phẩm
                var danhSachSanPham = dulieuSP.Sanphams.Include(sp => sp.LoaiSP).ToList();

                // Gắn dữ liệu vào DataGridView
                dataGridView1.DataSource = danhSachSanPham.Select(sp => new
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
                    NgayNhap = sp.NgayNhap,
                    LoaiSP = sp.LoaiSP.TenLoai, // Sử dụng TenLoai thay vì TenLoaiSP
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm: " + ex.Message);
            }
        }

     \
        private void LoadLoaiSP()
        {
            try
            {
                // Lấy danh sách loại sản phẩm
                var danhSachLoaiSP = dulieuSP.LoaiSPs.ToList();

                // Gắn dữ liệu vào ComboBox
                cboLoaiSP.DataSource = danhSachLoaiSP;
                cboLoaiSP.DisplayMember = "TenLoai"; // Hiển thị tên loại sản phẩm
                cboLoaiSP.ValueMember = "MaLoai";   // Giá trị là mã loại sản phẩm (Kiểu int)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách loại sản phẩm: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu người dùng chọn một dòng hợp lệ
            if (e.RowIndex >= 0)
            {
                // Lấy dữ liệu từ dòng đã chọn
                var row = dataGridView1.Rows[e.RowIndex];

                // Gán dữ liệu vào các control (TextBox, ComboBox, v.v.)
                txtMaSP.Text = row.Cells["MaSP"].Value.ToString(); // Gán mã sản phẩm
                txtTenSP.Text = row.Cells["TenSP"].Value.ToString(); // Gán tên sản phẩm
                dtNgaynhap.Value = Convert.ToDateTime(row.Cells["NgayNhap"].Value); // Gán ngày nhập

                string loaiSP = row.Cells["LoaiSP"].Value.ToString(); // Lấy tên loại sản phẩm
                cboLoaiSP.SelectedItem = cboLoaiSP.Items.Cast<LoaiSP>().FirstOrDefault(item =>
                    item.TenLoai == loaiSP); // Tìm và chọn loại sản phẩm tương ứng
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                int maSP = int.Parse(txtMaSP.Text); // Chắc chắn rằng MaSP là số nguyên
                var sanPham = dulieuSP.Sanphams.FirstOrDefault(sp => sp.MaSP == maSP);

                if (sanPham != null)
                {
                    sanPham.TenSP = txtTenSP.Text;
                    sanPham.NgayNhap = dtNgaynhap.Value;
                    sanPham.MaLoai = (int)cboLoaiSP.SelectedValue; // Sửa kiểu dữ liệu cho MaLoai
                    dulieuSP.SaveChanges();
                    LoadSanPham();
                    MessageBox.Show("Sửa sản phẩm thành công!");
                }
                else
                {
                    MessageBox.Show("Sản phẩm không tồn tại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa sản phẩm: " + ex.Message);
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int maSP = int.Parse(txtMaSP.Text); // Chắc chắn rằng MaSP là số nguyên
                var sanPham = dulieuSP.Sanphams.FirstOrDefault(sp => sp.MaSP == maSP);
                if (sanPham != null)
                {
                    dulieuSP.Sanphams.Remove(sanPham);
                    dulieuSP.SaveChanges();
                    LoadSanPham();
                    MessageBox.Show("Xóa sản phẩm thành công!");
                }
                else
                {
                    MessageBox.Show("Sản phẩm không tồn tại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        private void cboLoaiSP_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Khôi phục dữ liệu ban đầu (Không lưu gì)
                LoadSanPham();
                MessageBox.Show("Thông tin không được lưu.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy bỏ: " + ex.Message);
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra nếu có giá trị hợp lệ trong ComboBox
                if (cboLoaiSP.SelectedValue != null)
                {
                    var sanPhamMoi = new Sanpham
                    {
                        TenSP = txtTenSP.Text,
                        NgayNhap = dtNgaynhap.Value,
                        MaLoai = (int)btThem.SelectedValue // Sử dụng đúng tên ComboBox
                    };

                    dulieuSP.Sanphams.Add(sanPhamMoi);
                    dulieuSP.SaveChanges();
                    LoadSanPham();
                    MessageBox.Show("Thêm sản phẩm thành công!");
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn loại sản phẩm.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }
    }
}
