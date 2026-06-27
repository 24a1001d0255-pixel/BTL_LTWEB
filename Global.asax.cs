using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WBS_BTL
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Application["DanhSachSach"] = DataInitializer.GetDanhSachSach();
            Application["DanhSachTaiKhoan"] = new List<Account>();
            Application["SoNguoiTruyCap"] = 0;

            Application.Lock();
            try
            {
                var ds = Application["DanhSachTaiKhoan"] as List<Account>;
                if (ds == null)
                {
                    ds = new List<Account>();
                }

                bool hasAdmin = ds.Any(a => string.Equals(a.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase));
                if (!hasAdmin)
                {
                    ds.Add(new Account
                    {
                        TenDangNhap = "admin",
                        Email = "admin@bookverse.com",
                        MatKhau = "123456",
                        VaiTro = "Admin"
                    });
                }

                Application["DanhSachTaiKhoan"] = ds;
            }
            finally
            {
                Application.UnLock();
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}
