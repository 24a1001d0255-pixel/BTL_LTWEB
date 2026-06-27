using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var acc = Session["user"] as Account;
                bool isLogged = acc != null;
                lnkDangNhap.Visible = !isLogged;
                lnkTaiKhoan.Visible = isLogged;
                pnlUserArea.Visible = isLogged;

                if (isLogged)
                {
                    lblUserName.Text = acc.TenDangNhap ?? string.Empty;
                }

                bool isAdmin = acc != null && string.Equals(acc.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase);
                menuAdmin.Visible = isAdmin;
            }

            UpdateCartCountFromSession();
        }

        public void UpdateCartCountFromSession()
        {
            var cart = Session["giohang"] as List<GioHangItem>;
            int count = cart?.Sum(x => x.SoLuong) ?? 0;
            lblCartCount.Text = count.ToString();
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            var q = txtTimKiem.Text?.Trim();
            if (string.IsNullOrWhiteSpace(q))
            {
                Response.Redirect("~/TrangChu.aspx");
                return;
            }

            Response.Redirect("~/DanhSachSp.aspx?q=" + Server.UrlEncode(q));
        }

        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            Response.Redirect("~/TrangChu.aspx");
        }
    }
}