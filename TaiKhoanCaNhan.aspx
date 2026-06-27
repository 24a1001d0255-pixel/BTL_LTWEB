<%@ Page Title="Tài khoản cá nhân" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="TaiKhoanCaNhan.aspx.cs" Inherits="WBS_BTL.TaiKhoanCaNhan" %>

<asp:Content ID="ContentAccount" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/TaiKhoan.css" />

    <div class="page-account">
        <div class="container">
            <h1 class="page-title">Tài khoản của bạn</h1>

            <div class="account-layout">
                <!-- Khu vực 1: Thông tin tài khoản -->
                <aside class="account-card">
                    <div class="avatar">
                        <span class="avatar-fallback">📚</span>
                    </div>

                    <div class="account-info">
                        <h2 class="account-name"><asp:Label ID="lblHoTen" runat="server" Text=""></asp:Label></h2>
                        <p class="account-meta"><asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></p>
                        <p class="account-meta"><asp:Label ID="lblSoDienThoai" runat="server" Text=""></asp:Label></p>
                        <p class="account-meta"><asp:Label ID="lblDiaChi" runat="server" Text=""></asp:Label></p>
                    </div>

                    <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật thông tin"
                        CssClass="btn btn-update" OnClick="btnCapNhat_Click" />

                    <asp:Panel ID="pnlEditForm" runat="server" CssClass="account-edit" Visible="false">
                        <div class="form-group">
                            <label>Họ tên</label>
                            <asp:TextBox ID="txtHoTen" runat="server" CssClass="input-control" />
                        </div>
                        <div class="form-group">
                            <label>Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="input-control" TextMode="Email" />
                        </div>
                        <div class="form-group">
                            <label>Số điện thoại</label>
                            <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="input-control" />
                        </div>
                        <div class="form-group">
                            <label>Địa chỉ</label>
                            <asp:TextBox ID="txtDiaChi" runat="server" CssClass="input-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        </div>
                        <div class="form-actions">
                            <asp:Button ID="btnLuu" runat="server" Text="Lưu" CssClass="btn btn-update" OnClick="btnLuu_Click" />
                            <asp:Button ID="btnHuy" runat="server" Text="Hủy" CssClass="btn btn-ghost" OnClick="btnHuy_Click" />
                        </div>
                    </asp:Panel>

                    <asp:Button ID="btnDangXuat" runat="server" Text="Đăng xuất" CssClass="btn btn-logout" OnClick="btnDangXuat_Click" />
                </aside>

                <!-- Khu vực 2: Lịch sử đơn hàng -->
                <section class="order-history">
                    <h2 class="section-heading">Lịch sử đơn hàng</h2>

                    <asp:Panel ID="pnlEmptyOrders" runat="server" CssClass="order-empty" Visible="false">
                        <p>Bạn chưa có đơn hàng nào tại BookVerse.</p>
                    </asp:Panel>

                    <asp:Repeater ID="rptDonHang" runat="server">
                        <ItemTemplate>
                            <div class="order-card">
                                <div class="order-header">
                                    <div class="order-title">
                                        <span class="order-id">#<%# Eval("MaDon") %></span>
                                        <span class="order-date"><%# Eval("NgayDat") %></span>
                                    </div>
                                    <span class="order-status status-<%# Eval("TrangThaiCode") %>"><%# Eval("TrangThai") %></span>
                                </div>

                                <div class="order-body">
                                    <div class="order-row">
                                        <span class="order-label">Tổng tiền</span>
                                        <span class="order-value"><%# Eval("TongTien", "{0:N0}₫") %></span>
                                    </div>
                                    <div class="order-row">
                                        <span class="order-label">Phương thức thanh toán</span>
                                        <span class="order-value"><%# Eval("PhuongThuc") %></span>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </section>
            </div>
        </div>
    </div>
</asp:Content>
