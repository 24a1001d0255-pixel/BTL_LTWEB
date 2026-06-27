using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class QuanLy : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var acc = Session["user"] as Account;
            if (acc == null || !string.Equals(acc.Email, "admin@bookverse.com", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("../DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblAdminInfo.Text = acc.TenDangNhap ?? acc.Email;
                LoadBooks();
                LoadOrders();
            }
        }

        protected void SwitchTab_Click(object sender, EventArgs e)
        {
            var btn = sender as LinkButton;
            if (btn == null) return;

            pnlTabSach.Visible = btn.CommandArgument == "sach";
            pnlTabDonHang.Visible = btn.CommandArgument == "donhang";

            lnkTabSach.CssClass = "admin-nav-item" + (pnlTabSach.Visible ? " active" : "");
            lnkTabDonHang.CssClass = "admin-nav-item" + (pnlTabDonHang.Visible ? " active" : "");
        }

        private void LoadBooks()
        {
            var ds = Application["DanhSachSach"] as List<Sach>;
            gvSach.DataSource = ds ?? new List<Sach>();
            gvSach.DataBind();

            lblBookCount.Text = (ds == null ? 0 : ds.Count) + " cuốn";
        }

        private void LoadOrders()
        {
            var ds = Application["DanhSachDonHang"] as List<DonHangItem>;
            if (ds == null)
            {
                ds = new List<DonHangItem>();
                Application["DanhSachDonHang"] = ds;
            }

            gvDonHang.DataSource = ds.OrderByDescending(x => x.NgayDat).ToList();
            gvDonHang.DataBind();

            lblOrderCount.Text = ds.Count + " đơn";
        }

        protected void btnThemSach_Click(object sender, EventArgs e)
        {
            ResetBookForm();
            ShowBookModal();
        }

        protected void btnLuuSach_Click(object sender, EventArgs e)
        {
            var ds = Application["DanhSachSach"] as List<Sach> ?? new List<Sach>();

            double gia;
            int soTrang;
            double.TryParse(txtGia.Text.Trim(), out gia);
            int.TryParse(txtSoTrang.Text.Trim(), out soTrang);

            string maSach = (txtMaSach.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(maSach))
            {
                maSach = "S" + (ds.Count + 1).ToString("D2");
            }

            var existing = ds.FirstOrDefault(s => string.Equals(s.MaSach, maSach, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.TenSach = txtTenSach.Text.Trim();
                existing.TacGia = txtTacGia.Text.Trim();
                existing.NhaXuatBan = txtNxb.Text.Trim();
                existing.Gia = gia > 0 ? gia : existing.Gia;
                existing.SoTrang = soTrang > 0 ? soTrang : existing.SoTrang;
                existing.TinhTrangKho = string.IsNullOrWhiteSpace(ddlTinhTrang.SelectedValue) ? existing.TinhTrangKho : ddlTinhTrang.SelectedValue;
                existing.Anh = LuuAnh(existing.Anh);
                existing.TheLoai = txtTheLoai.Text.Trim();
                existing.MoTa = txtMoTa.Text.Trim();
            }
            else
            {
                var sach = new Sach
                {
                    MaSach = maSach,
                    TenSach = txtTenSach.Text.Trim(),
                    TacGia = txtTacGia.Text.Trim(),
                    NhaXuatBan = txtNxb.Text.Trim(),
                    Gia = gia,
                    SoTrang = soTrang,
                    TinhTrangKho = string.IsNullOrWhiteSpace(ddlTinhTrang.SelectedValue) ? "Còn hàng" : ddlTinhTrang.SelectedValue,
                    Anh = LuuAnh("images/default.jpg"),
                    TheLoai = txtTheLoai.Text.Trim(),
                    MoTa = txtMoTa.Text.Trim()
                };

                ds.Add(sach);
            }

            Application["DanhSachSach"] = ds;
            LoadBooks();
            CloseBookModal();
        }

        protected void gvSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.CommandArgument.ToString()))
            {
                return;
            }

            string maSach = e.CommandArgument.ToString();
            var ds = Application["DanhSachSach"] as List<Sach>;
            if (ds == null)
            {
                return;
            }

            if (e.CommandName.Equals("EditBook", StringComparison.OrdinalIgnoreCase))
            {
                var sach = ds.FirstOrDefault(s => s.MaSach == maSach);
                if (sach == null)
                {
                    return;
                }

                txtMaSach.Text = sach.MaSach;
                txtTenSach.Text = sach.TenSach;
                txtTacGia.Text = sach.TacGia;
                txtNxb.Text = sach.NhaXuatBan;
                txtGia.Text = sach.Gia.ToString();
                txtSoTrang.Text = sach.SoTrang.ToString();
                ddlTinhTrang.SelectedValue = sach.TinhTrangKho;
                hfAnhCu.Value = sach.Anh;
                ltAnhHienTai.Text = !string.IsNullOrWhiteSpace(sach.Anh)
                    ? string.Format("<img src='{0}' style='max-width:120px;max-height:160px;border-radius:8px;margin-top:8px;border:1px solid #ccc;' />", ResolveAdminCoverUrl(sach.Anh))
                    : string.Empty;
                txtTheLoai.Text = sach.TheLoai;
                txtMoTa.Text = sach.MoTa;

                ShowBookModal();
            }
            else if (e.CommandName.Equals("DeleteBook", StringComparison.OrdinalIgnoreCase))
            {
                var sach = ds.FirstOrDefault(s => s.MaSach == maSach);
                if (sach == null)
                {
                    return;
                }

                ds.Remove(sach);
                Application["DanhSachSach"] = ds;
                LoadBooks();
            }
        }

        protected void gvDonHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!e.CommandName.Equals("UpdateStatus", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string maDon = e.CommandArgument.ToString();
            var ds = Application["DanhSachDonHang"] as List<DonHangItem>;
            if (ds == null)
            {
                return;
            }

            var don = ds.FirstOrDefault(d => d.MaDon == maDon);
            if (don == null)
            {
                return;
            }

            GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;
            if (row == null)
            {
                return;
            }

            var ddl = row.FindControl("ddlTrangThai") as DropDownList;
            if (ddl == null)
            {
                return;
            }

            don.TrangThaiCode = ddl.SelectedValue;
            don.TrangThai = ddl.SelectedItem.Text;
            Application["DanhSachDonHang"] = ds;
            LoadOrders();
        }

        public string GetStatusClass(string code)
        {
            if (string.Equals(code, "dagiao", StringComparison.OrdinalIgnoreCase)) return "badge badge-ok";
            if (string.Equals(code, "dahuy", StringComparison.OrdinalIgnoreCase)) return "badge badge-danger";
            if (string.Equals(code, "danggiao", StringComparison.OrdinalIgnoreCase)) return "badge badge-info";
            return "badge badge-warn";
        }

        public string ResolveAdminCoverUrl(string rawPath)
        {
            string path = (rawPath ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(path))
                return "../images/default.jpg";
            if (path.StartsWith("~/", StringComparison.Ordinal) || path.StartsWith("/", StringComparison.Ordinal))
                return path;
            if (path.StartsWith("images/", StringComparison.OrdinalIgnoreCase))
                return "../" + path;
            return "../images/" + path;
        }

        private void ResetBookForm()
        {
            hfMaSach.Value = string.Empty;
            txtMaSach.Text = string.Empty;
            txtTenSach.Text = string.Empty;
            txtTacGia.Text = string.Empty;
            txtNxb.Text = string.Empty;
            txtGia.Text = string.Empty;
            txtSoTrang.Text = string.Empty;
            ddlTinhTrang.SelectedValue = "Còn hàng";
            hfAnhCu.Value = string.Empty;
            ltAnhHienTai.Text = string.Empty;
            txtTheLoai.Text = string.Empty;
            txtMoTa.Text = string.Empty;
        }

        private void ShowBookModal()
        {
            bookModal.Attributes["class"] = "modal show";
            bookModal.Attributes["aria-hidden"] = "false";
        }

        private void CloseBookModal()
        {
            bookModal.Attributes["class"] = "modal";
            bookModal.Attributes["aria-hidden"] = "true";
        }

        protected void btnCloseModal_Click(object sender, EventArgs e)
        {
            CloseBookModal();
        }

        private string LuuAnh(string anhCu)
        {
            if (fuAnh.HasFile)
            {
                string fileName = Path.GetFileName(fuAnh.FileName);
                string ext = Path.GetExtension(fileName).ToLower();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".webp" && ext != ".gif")
                    return anhCu;

                string savePath = Server.MapPath("~/images/");
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                string newFileName = "book_" + Guid.NewGuid().ToString("N") + ext;
                string fullPath = Path.Combine(savePath, newFileName);
                fuAnh.SaveAs(fullPath);
                return "images/" + newFileName;
            }
            return string.IsNullOrWhiteSpace(anhCu) ? "images/default.jpg" : anhCu;
        }
    }
}
