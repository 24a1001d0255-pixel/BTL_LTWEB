using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class DangKy : System.Web.UI.Page
    {
        private const string SessionUser = "user";
        private const string AppAccounts = "DanhSachTaiKhoan";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnDangKy_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text?.Trim() ?? string.Empty;
            string email = txtEmail.Text?.Trim() ?? string.Empty;
            string soDienThoai = txtSoDienThoai.Text?.Trim() ?? string.Empty;
            string matKhau = txtMatKhau.Text ?? string.Empty;
            string nhapLai = txtNhapLaiMatKhau.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(hoTen) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(matKhau))
            {
                return;
            }

            if (!IsValidEmail(email) || matKhau.Length < 6 || matKhau != nhapLai)
            {
                return;
            }

            var accounts = GetAccounts();
            if (accounts.Any(a => string.Equals(a.Email, email, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            var newAccount = new Account
            {
                TenDangNhap = hoTen,
                Email = email,
                SoDienThoai = soDienThoai,
                MatKhau = matKhau,
                VaiTro = "User"
            };

            accounts.Add(newAccount);
            Application[AppAccounts] = accounts;

            Session[SessionUser] = newAccount;
            Response.Redirect("~/TrangChu.aspx");
        }

        private List<Account> GetAccounts()
        {
            var existing = Application[AppAccounts] as List<Account>;
            if (existing != null && existing.Count > 0)
            {
                return existing;
            }

            var defaults = new List<Account>
            {
                new Account { TenDangNhap = "Admin BookVerse", Email = "admin@bookverse.vn", MatKhau = "123456", SoDienThoai = "0909123456", VaiTro = "Admin" },
                new Account { TenDangNhap = "Nguyễn Văn A", Email = "nguyenvana@email.com", MatKhau = "123456", SoDienThoai = "0912345678", VaiTro = "User" }
            };

            Application[AppAccounts] = defaults;
            return defaults;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
