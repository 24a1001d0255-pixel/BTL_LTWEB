<%@ Page Title="Đăng ký" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="DangKy.aspx.cs" Inherits="WBS_BTL.DangKy" %>

<asp:Content ID="ContentRegister" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/Auth.css" />

    <div class="page-auth">
        <div class="auth-card">
            <div class="auth-header">
                <h1 class="auth-title">Tạo tài khoản BookVerse</h1>
                <p class="auth-subtitle">Đăng ký để theo dõi đơn hàng và nhận ưu đãi sách hay.</p>
            </div>

            <div class="auth-body">
                <div class="form-group">
                    <label for="<%= txtHoTen.ClientID %>">Họ tên</label>
                    <asp:TextBox ID="txtHoTen" runat="server" CssClass="input-control" placeholder="Nhập họ tên của bạn"></asp:TextBox>
                    <span class="field-error" id="errHoTen">Vui lòng nhập họ tên.</span>
                </div>

                <div class="form-group">
                    <label for="<%= txtEmail.ClientID %>">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="input-control" placeholder="vidu@email.com" TextMode="Email"></asp:TextBox>
                    <span class="field-error" id="errEmail">Email không hợp lệ.</span>
                </div>

                <div class="form-group">
                    <label for="<%= txtSoDienThoai.ClientID %>">Số điện thoại</label>
                    <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="input-control" placeholder="0909 123 456"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="<%= txtMatKhau.ClientID %>">Mật khẩu</label>
                    <asp:TextBox ID="txtMatKhau" runat="server" CssClass="input-control" placeholder="Tối thiểu 6 ký tự" TextMode="Password"></asp:TextBox>
                    <span class="field-error" id="errMatKhau">Mật khẩu phải từ 6 ký tự trở lên.</span>
                </div>

                <div class="form-group">
                    <label for="<%= txtNhapLaiMatKhau.ClientID %>">Nhập lại mật khẩu</label>
                    <asp:TextBox ID="txtNhapLaiMatKhau" runat="server" CssClass="input-control" placeholder="Nhập lại mật khẩu" TextMode="Password"></asp:TextBox>
                    <span class="field-error" id="errNhapLai">Mật khẩu không trùng khớp.</span>
                </div>

                <asp:Button ID="btnDangKy" runat="server" Text="Đăng ký"
                    CssClass="btn btn-primary btn-full"
                    OnClientClick="return validateRegisterForm();"
                    OnClick="btnDangKy_Click" />

                <p class="auth-footer-text">
                    Đã có tài khoản?
                    <asp:HyperLink ID="lnkToDangNhap" runat="server" NavigateUrl="DangNhap.aspx" CssClass="auth-link">Đăng nhập ngay</asp:HyperLink>
                </p>
            </div>
        </div>
    </div>

    <script>
        function validateRegisterForm() {
            var hoTen = document.getElementById('<%= txtHoTen.ClientID %>').value.trim();
            var email = document.getElementById('<%= txtEmail.ClientID %>').value.trim();
            var matKhau = document.getElementById('<%= txtMatKhau.ClientID %>').value;
            var nhapLai = document.getElementById('<%= txtNhapLaiMatKhau.ClientID %>').value;

            var valid = true;

            if (hoTen.length < 2) {
                document.getElementById('errHoTen').style.display = 'block';
                valid = false;
            } else {
                document.getElementById('errHoTen').style.display = 'none';
            }

            if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
                document.getElementById('errEmail').style.display = 'block';
                valid = false;
            } else {
                document.getElementById('errEmail').style.display = 'none';
            }

            if (matKhau.length < 6) {
                document.getElementById('errMatKhau').style.display = 'block';
                valid = false;
            } else {
                document.getElementById('errMatKhau').style.display = 'none';
            }

            if (matKhau !== nhapLai || nhapLai.length === 0) {
                document.getElementById('errNhapLai').style.display = 'block';
                valid = false;
            } else {
                document.getElementById('errNhapLai').style.display = 'none';
            }

            return valid;
        }
    </script>
</asp:Content>
