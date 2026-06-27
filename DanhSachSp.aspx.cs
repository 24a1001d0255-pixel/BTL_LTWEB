using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class DanhSachSp : System.Web.UI.Page
    {
        private const int PageSize = 6;
        private int CurrentPage
        {
            get { return ViewState["CurrentPage"] as int? ?? 1; }
            set { ViewState["CurrentPage"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFilterOptions();
            }
            ApplyFilters();
        }

        private void LoadFilterOptions()
        {
            var ds = Application["DanhSachSach"] as List<Sach>;
            if (ds == null)
            {
                return;
            }

            var theLoais = ds
                .Where(s => !string.IsNullOrWhiteSpace(s.TheLoai))
                .Select(s => s.TheLoai)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            rblTheLoai.Items.Clear();
            rblTheLoai.Items.Add(new System.Web.UI.WebControls.ListItem("Tất cả", "all"));
            foreach (var tl in theLoais)
            {
                rblTheLoai.Items.Add(new System.Web.UI.WebControls.ListItem(tl, tl));
            }

            string type = (Request.QueryString["type"] ?? string.Empty).Trim();
            string value = (Request.QueryString["value"] ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
            {
                string decoded = value;
                if (!string.Equals(type, "theloai", StringComparison.OrdinalIgnoreCase) || !theLoais.Contains(decoded))
                {
                    decoded = theLoais.Contains(value) ? value : decoded;
                }

                if (type.Equals("theloai", StringComparison.OrdinalIgnoreCase))
                {
                    rblTheLoai.SelectedValue = decoded;
                }
            }

            string q = (Request.QueryString["q"] ?? string.Empty).Trim();
            if (!string.IsNullOrEmpty(q))
            {
                lblTieuDe.Text = "Kết quả tìm kiếm: " + q;
                rblTheLoai.SelectedValue = "all";
            }
            else if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
            {
                lblTieuDe.Text = type.Equals("tacgia", StringComparison.OrdinalIgnoreCase) ? "Tác giả: " + value :
                                 type.Equals("nxb", StringComparison.OrdinalIgnoreCase) ? "Nhà xuất bản: " + value :
                                 "Thể loại: " + value;
            }
        }

        protected void Filter_Changed(object sender, EventArgs e)
        {
            CurrentPage = 1;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var ds = Application["DanhSachSach"] as List<Sach>;
            if (ds == null)
            {
                pnlEmpty.Visible = true;
                rptSach.DataSource = null;
                rptSach.DataBind();
                lblSoLuong.Text = "0";
                pnlPagination.Visible = false;
                return;
            }

            IEnumerable<Sach> result = ds;

            string type = (Request.QueryString["type"] ?? string.Empty).Trim();
            string value = (Request.QueryString["value"] ?? string.Empty).Trim();
            string q = (Request.QueryString["q"] ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
            {
                if (type.Equals("theloai", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Where(s => string.Equals(s.TheLoai, value, StringComparison.OrdinalIgnoreCase));
                }
                else if (type.Equals("tacgia", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Where(s => string.Equals(s.TacGia, value, StringComparison.OrdinalIgnoreCase));
                }
                else if (type.Equals("nxb", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Where(s => string.Equals(s.NhaXuatBan, value, StringComparison.OrdinalIgnoreCase));
                }
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                string keyword = q.Trim().ToLowerInvariant();
                result = result.Where(s =>
                    (s.TenSach ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (s.TacGia ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (s.MoTa ?? string.Empty).ToLowerInvariant().Contains(keyword));
            }

            string gia = rblGia.SelectedValue ?? "all";
            if (gia == "duoi50")
            {
                result = result.Where(s => s.Gia < 50000);
            }
            else if (gia == "50-100")
            {
                result = result.Where(s => s.Gia >= 50000 && s.Gia <= 100000);
            }
            else if (gia == "100-200")
            {
                result = result.Where(s => s.Gia > 100000 && s.Gia <= 200000);
            }
            else if (gia == "tren200")
            {
                result = result.Where(s => s.Gia > 200000);
            }

            string theLoaiLoc = rblTheLoai.SelectedValue ?? "all";
            if (!string.Equals(theLoaiLoc, "all", StringComparison.OrdinalIgnoreCase))
            {
                result = result.Where(s => string.Equals(s.TheLoai, theLoaiLoc, StringComparison.OrdinalIgnoreCase));
            }

            string kho = rblKho.SelectedValue ?? "all";
            if (!string.Equals(kho, "all", StringComparison.OrdinalIgnoreCase))
            {
                if (kho.Equals("conhang", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Where(s => string.Equals(s.TinhTrangKho, "Còn hàng", StringComparison.OrdinalIgnoreCase));
                }
                else if (kho.Equals("dadat", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Where(s => string.Equals(s.TinhTrangKho, "Đã đặt", StringComparison.OrdinalIgnoreCase));
                }
            }

            var list = result.OrderBy(s => s.TenSach).ToList();
            lblSoLuong.Text = list.Count.ToString();

            if (list.Count == 0)
            {
                pnlEmpty.Visible = true;
                rptSach.DataSource = null;
                rptSach.DataBind();
                pnlPagination.Visible = false;
                return;
            }

            pnlEmpty.Visible = false;
            int totalPages = (int)Math.Ceiling(list.Count / (double)PageSize);
            if (CurrentPage > totalPages)
            {
                CurrentPage = totalPages;
            }
            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }

            var pageItems = list.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            rptSach.DataSource = pageItems;
            rptSach.DataBind();

            if (totalPages > 1)
            {
                pnlPagination.Visible = true;
                lblPage.Text = CurrentPage + "/" + totalPages;
                lnkPrev.Enabled = CurrentPage > 1;
                lnkNext.Enabled = CurrentPage < totalPages;
            }
            else
            {
                pnlPagination.Visible = false;
            }
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            var ds = Application["DanhSachSach"] as List<Sach>;
            if (ds == null)
            {
                return;
            }

            IEnumerable<Sach> result = ds;
            string type = (Request.QueryString["type"] ?? string.Empty).Trim();
            string value = (Request.QueryString["value"] ?? string.Empty).Trim();
            string q = (Request.QueryString["q"] ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
            {
                if (type.Equals("theloai", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Where(s => string.Equals(s.TheLoai, value, StringComparison.OrdinalIgnoreCase));
                }
                else if (type.Equals("tacgia", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Where(s => string.Equals(s.TacGia, value, StringComparison.OrdinalIgnoreCase));
                }
                else if (type.Equals("nxb", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Where(s => string.Equals(s.NhaXuatBan, value, StringComparison.OrdinalIgnoreCase));
                }
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                string keyword = q.Trim().ToLowerInvariant();
                result = result.Where(s =>
                    (s.TenSach ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (s.TacGia ?? string.Empty).ToLowerInvariant().Contains(keyword) ||
                    (s.MoTa ?? string.Empty).ToLowerInvariant().Contains(keyword));
            }

            string gia = rblGia.SelectedValue ?? "all";
            if (gia == "duoi50")
            {
                result = result.Where(s => s.Gia < 50000);
            }
            else if (gia == "50-100")
            {
                result = result.Where(s => s.Gia >= 50000 && s.Gia <= 100000);
            }
            else if (gia == "100-200")
            {
                result = result.Where(s => s.Gia > 100000 && s.Gia <= 200000);
            }
            else if (gia == "tren200")
            {
                result = result.Where(s => s.Gia > 200000);
            }

            string theLoaiLoc = rblTheLoai.SelectedValue ?? "all";
            if (!string.Equals(theLoaiLoc, "all", StringComparison.OrdinalIgnoreCase))
            {
                result = result.Where(s => string.Equals(s.TheLoai, theLoaiLoc, StringComparison.OrdinalIgnoreCase));
            }

            string kho = rblKho.SelectedValue ?? "all";
            if (!string.Equals(kho, "all", StringComparison.OrdinalIgnoreCase))
            {
                if (kho.Equals("conhang", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Where(s => string.Equals(s.TinhTrangKho, "Còn hàng", StringComparison.OrdinalIgnoreCase));
                }
                else if (kho.Equals("dadat", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.Where(s => string.Equals(s.TinhTrangKho, "Đã đặt", StringComparison.OrdinalIgnoreCase));
                }
            }

            var list = result.OrderBy(s => s.TenSach).ToList();
            int totalPages = (int)Math.Ceiling(list.Count / (double)PageSize);
            if (totalPages < 1)
            {
                totalPages = 1;
            }

            if (string.Equals((sender as System.Web.UI.WebControls.LinkButton)?.CommandArgument, "next", StringComparison.OrdinalIgnoreCase))
            {
                CurrentPage = CurrentPage + 1;
            }
            else
            {
                CurrentPage = CurrentPage - 1;
            }

            if (CurrentPage > totalPages)
            {
                CurrentPage = totalPages;
            }
            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }

            var pageItems = list.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            rptSach.DataSource = pageItems;
            rptSach.DataBind();
            lblPage.Text = CurrentPage + "/" + totalPages;
            lnkPrev.Enabled = CurrentPage > 1;
            lnkNext.Enabled = CurrentPage < totalPages;
        }

        protected void btnThemVaoGio_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }

            string maSach = e.CommandArgument.ToString();
            var dsSanPham = Application["DanhSachSach"] as List<Sach>;
            var sach = dsSanPham?.FirstOrDefault(sp => sp.MaSach == maSach);
            if (sach == null)
            {
                return;
            }

            List<GioHangItem> gioHang;
            if (Session["giohang"] == null)
            {
                gioHang = new List<GioHangItem>();
            }
            else
            {
                gioHang = Session["giohang"] as List<GioHangItem> ?? new List<GioHangItem>();
            }

            var existingItem = gioHang.FirstOrDefault(item => item.MaSach == maSach);
            if (existingItem != null)
            {
                existingItem.SoLuong++;
            }
            else
            {
                gioHang.Add(new GioHangItem
                {
                    MaSach = sach.MaSach,
                    TenSach = sach.TenSach,
                    Gia = sach.Gia,
                    Anh = sach.Anh,
                    SoLuong = 1
                });
            }

            Session["giohang"] = gioHang;
            var m = this.Master as Site;
            m?.UpdateCartCountFromSession();
        }
    }
}
