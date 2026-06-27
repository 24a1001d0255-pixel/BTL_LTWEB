using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class TaiKhoanCaNhan : System.Web.UI.Page
    {
        private const string SessionUser = "user";
        private const string SessionOrders = "TaiKhoanDonHang";

        protected void Page_Load(object sender, EventArgs e)
        {
            var acc = Session[SessionUser] as Account;
            if (acc == null)
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                EnsureDemoOrders(acc);
                HienThiThongTin(acc);
                HienThiDonHang(acc.Email);
            }
        }

        private void EnsureDemoOrders(Account acc)
        {
            var orders = Session[SessionOrders] as Dictionary<string, List<DonHangItem>>;
            if (orders == null)
            {
                orders = new Dictionary<string, List<DonHangItem>>();
                Session[SessionOrders] = orders;
            }

            string key = (acc.Email ?? string.Empty).Trim().ToLowerInvariant();

            if (!orders.ContainsKey(key))
            {
                var now = DateTime.Now;
                var demo = new List<DonHangItem>
                {
                    new DonHangItem
                    {
                        MaDon = "BV" + now.ToString("yyyyMMdd") + "001",
                        NgayDat = now.AddDays(-5).ToString("dd/MM/yyyy"),
                        TongTien = 285000,
                        PhuongThuc = "COD",
                        TrangThai = "Đang giao hàng",
                        TrangThaiCode = "shipping"
                    },
                    new DonHangItem
                    {
                        MaDon = "BV" + now.ToString("yyyyMMdd") + "002",
                        NgayDat = now.AddDays(-12).ToString("dd/MM/yyyy"),
                        TongTien = 198000,
                        PhuongThuc = "Chuyển khoản ngân hàng",
                        TrangThai = "Đã giao thành công",
                        TrangThaiCode = "completed"
                    },
                    new DonHangItem
                    {
                        MaDon = "BV" + now.ToString("yyyyMMdd") + "003",
                        NgayDat = now.AddDays(-20).ToString("dd/MM/yyyy"),
                        TongTien = 540000,
                        PhuongThuc = "Ví điện tử",
                        TrangThai = "Đang xử lý",
                        TrangThaiCode = "processing"
                    }
                };

                orders[key] = demo;
            }
        }

        private void HienThiThongTin(Account acc)
        {
            lblHoTen.Text = acc.TenDangNhap ?? string.Empty;
            lblEmail.Text = acc.Email ?? string.Empty;
            lblSoDienThoai.Text = string.IsNullOrWhiteSpace(acc.SoDienThoai) ? "Chưa cập nhật" : acc.SoDienThoai;
            lblDiaChi.Text = string.IsNullOrWhiteSpace(acc.DiaChi) ? "Chưa cập nhật" : acc.DiaChi;
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            var acc = Session[SessionUser] as Account;
            if (acc == null)
            {
                return;
            }

            txtHoTen.Text = acc.TenDangNhap ?? string.Empty;
            txtEmail.Text = acc.Email ?? string.Empty;
            txtSoDienThoai.Text = acc.SoDienThoai ?? string.Empty;
            txtDiaChi.Text = acc.DiaChi ?? string.Empty;

            pnlEditForm.Visible = true;
            btnCapNhat.Visible = false;
        }

        protected void btnLuu_Click(object sender, EventArgs e)
        {
            var acc = Session[SessionUser] as Account;
            if (acc == null)
            {
                return;
            }

            string hoTen = txtHoTen.Text?.Trim() ?? string.Empty;
            string email = txtEmail.Text?.Trim() ?? string.Empty;
            string soDienThoai = txtSoDienThoai.Text?.Trim() ?? string.Empty;
            string diaChi = txtDiaChi.Text?.Trim() ?? string.Empty;

            acc.TenDangNhap = string.IsNullOrWhiteSpace(hoTen) ? acc.TenDangNhap : hoTen;
            acc.Email = string.IsNullOrWhiteSpace(email) ? acc.Email : email;
            acc.SoDienThoai = soDienThoai;
            acc.DiaChi = diaChi;

            Session[SessionUser] = acc;
            HienThiThongTin(acc);

            pnlEditForm.Visible = false;
            btnCapNhat.Visible = true;
        }

        protected void btnHuy_Click(object sender, EventArgs e)
        {
            pnlEditForm.Visible = false;
            btnCapNhat.Visible = true;
        }

        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            Response.Redirect("~/TrangChu.aspx");
        }

        private void HienThiDonHang(string email)
        {
            var orders = Session[SessionOrders] as Dictionary<string, List<DonHangItem>>;
            var key = (email ?? string.Empty).Trim().ToLowerInvariant();
            var ds = orders != null && orders.ContainsKey(key) ? orders[key] : null;

            if (ds != null && ds.Count > 0)
            {
                pnlEmptyOrders.Visible = false;
                rptDonHang.DataSource = ds.OrderByDescending(x => x.NgayDat).ToList();
                rptDonHang.DataBind();
            }
            else
            {
                pnlEmptyOrders.Visible = true;
                rptDonHang.DataSource = null;
                rptDonHang.DataBind();
            }
        }
    }
}
