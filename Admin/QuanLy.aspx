<%@ Page Title="Quản trị hệ thống" Language="C#" AutoEventWireup="true" CodeBehind="QuanLy.aspx.cs" Inherits="WBS_BTL.QuanLy" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title><%: Page.Title %></title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="../Styles/style.css" />
    <link rel="stylesheet" href="Admin.css" />
</head>
<body>
    <form id="formAdmin" runat="server">
        <div class="admin-layout">
            <aside class="admin-sidebar">
                <div class="admin-brand">BookVerse Admin</div>
                <nav class="admin-nav">
                    <asp:LinkButton ID="lnkTabSach" runat="server" CssClass="admin-nav-item active" OnClick="SwitchTab_Click" CommandArgument="sach">📚 Quản lý sách</asp:LinkButton>
                    <asp:LinkButton ID="lnkTabDonHang" runat="server" CssClass="admin-nav-item" OnClick="SwitchTab_Click" CommandArgument="donhang">📦 Quản lý đơn hàng</asp:LinkButton>
                    <asp:LinkButton ID="lnkThoat" runat="server" CssClass="admin-nav-item" PostBackUrl="~/TrangChu.aspx">↩ Thoát quản trị</asp:LinkButton>
                </nav>
            </aside>

            <main class="admin-main">
                <header class="admin-topbar">
                    <h1 class="admin-page-title">Dashboard</h1>
                    <asp:Label ID="lblAdminInfo" runat="server" CssClass="admin-badge" Text="Admin"></asp:Label>
                </header>

                <asp:Panel ID="pnlTabSach" runat="server" CssClass="admin-tab">
                    <div class="toolbar">
                        <asp:Label ID="lblBookCount" runat="server" CssClass="admin-count" Text="0 cuốn"></asp:Label>
                        <asp:Button ID="btnThemSach" runat="server" Text="＋ Thêm sách mới" CssClass="btn btn-primary btn-sm" OnClick="btnThemSach_Click" />
                    </div>

                    <asp:GridView ID="gvSach" runat="server" AutoGenerateColumns="False" OnRowCommand="gvSach_RowCommand"
                        CssClass="table table-striped table-hover table-bordered admin-table" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="MaSach" HeaderText="Mã" />
                            <asp:TemplateField HeaderText="Ảnh">
                                <ItemTemplate>
                                    <img src='<%# ResolveAdminCoverUrl(Eval("Anh").ToString()) %>' class="admin-thumb" alt='<%# Eval("TenSach") %>' onerror="this.src='../images/default.jpg'" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TenSach" HeaderText="Tên sách" />
                            <asp:BoundField DataField="TacGia" HeaderText="Tác giả" />
                            <asp:BoundField DataField="Gia" HeaderText="Giá" DataFormatString="{0:N0}₫" />
                            <asp:BoundField DataField="SoTrang" HeaderText="Số trang" />
                            <asp:TemplateField HeaderText="Tình trạng">
                                <ItemTemplate>
                                    <span class='badge <%# Eval("TinhTrangKho").ToString() == "Còn hàng" ? "badge-ok" : "badge-warn" %>'><%# Eval("TinhTrangKho") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Chức năng">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" Text="Sửa" CssClass="btn btn-warning btn-sm" CommandName="EditBook" CommandArgument='<%# Eval("MaSach") %>' />
                                    <asp:Button ID="btnDelete" runat="server" Text="Xóa" CssClass="btn btn-danger btn-sm" CommandName="DeleteBook" CommandArgument='<%# Eval("MaSach") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>

                <asp:Panel ID="pnlTabDonHang" runat="server" CssClass="admin-tab" Visible="false">
                    <div class="toolbar">
                        <asp:Label ID="lblOrderCount" runat="server" CssClass="admin-count" Text="0 đơn"></asp:Label>
                    </div>

                    <asp:GridView ID="gvDonHang" runat="server" AutoGenerateColumns="False" OnRowCommand="gvDonHang_RowCommand"
                        CssClass="table table-striped table-hover table-bordered admin-table" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="MaDon" HeaderText="Mã đơn" />
                            <asp:BoundField DataField="KhachHang" HeaderText="Khách hàng" />
                            <asp:BoundField DataField="NgayDat" HeaderText="Ngày đặt" />
                            <asp:BoundField DataField="TongTien" HeaderText="Tổng tiền" DataFormatString="{0:N0}₫" />
                            <asp:BoundField DataField="PhuongThuc" HeaderText="Phương thức" />
                            <asp:TemplateField HeaderText="Trạng thái">
                                <ItemTemplate>
                                    <span class='<%# GetStatusClass(Eval("TrangThaiCode").ToString()) %>'><%# Eval("TrangThai") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cập nhật">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTrangThai" runat="server" CssClass="form-select form-select-sm d-inline-block w-auto">
                                        <asp:ListItem Value="dangxuly" Text="Đang xử lý"></asp:ListItem>
                                        <asp:ListItem Value="danggiao" Text="Đang giao hàng"></asp:ListItem>
                                        <asp:ListItem Value="dagiao" Text="Đã giao"></asp:ListItem>
                                        <asp:ListItem Value="dahuy" Text="Đã hủy"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật" CssClass="btn btn-success btn-sm" CommandName="UpdateStatus" CommandArgument='<%# Eval("MaDon") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </main>
        </div>

        <!-- Modal thêm/sửa sách -->
        <div id="bookModal" class="modal" runat="server" aria-hidden="true">
            <div class="modal-overlay" onclick="closeBookModal()"></div>
            <div class="modal-box" role="dialog" aria-modal="true" aria-label="Thông tin sách">
                <div class="modal-header">
                    <h3>Thông tin sách</h3>
                    <button type="button" class="modal-close" onclick="closeBookModal()" aria-label="Đóng">✕</button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfMaSach" runat="server" />

                    <div class="form-grid">
                        <label>Mã sách</label>
                        <asp:TextBox ID="txtMaSach" runat="server" CssClass="form-control" placeholder="Tự sinh nếu để trống" />

                        <label>Tên sách *</label>
                        <asp:TextBox ID="txtTenSach" runat="server" CssClass="form-control" />

                        <label>Tác giả</label>
                        <asp:TextBox ID="txtTacGia" runat="server" CssClass="form-control" />

                        <label>Nhà xuất bản</label>
                        <asp:TextBox ID="txtNxb" runat="server" CssClass="form-control" />

                        <label>Giá (VNĐ) *</label>
                        <asp:TextBox ID="txtGia" runat="server" CssClass="form-control" TextMode="Number" />

                        <label>Số trang</label>
                        <asp:TextBox ID="txtSoTrang" runat="server" CssClass="form-control" TextMode="Number" />

                        <label>Tình trạng</label>
                        <asp:DropDownList ID="ddlTinhTrang" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Còn hàng" Value="Còn hàng" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Đã đặt" Value="Đã đặt"></asp:ListItem>
                            <asp:ListItem Text="Hết hàng" Value="Hết hàng"></asp:ListItem>
                        </asp:DropDownList>

                        <label>Ảnh bìa</label>
                        <asp:FileUpload ID="fuAnh" runat="server" CssClass="form-control" accept="image/*" />
                        <asp:HiddenField ID="hfAnhCu" runat="server" />
                        <asp:Literal ID="ltAnhHienTai" runat="server"></asp:Literal>

                        <label>Thể loại</label>
                        <asp:TextBox ID="txtTheLoai" runat="server" CssClass="form-control" />

                        <label>Tóm tắt</label>
                        <asp:TextBox ID="txtMoTa" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnLuuSach" runat="server" Text="Lưu" CssClass="btn btn-primary" OnClick="btnLuuSach_Click" />
                    <button type="button" class="btn btn-ghost" onclick="closeBookModal()">Đóng</button>
                </div>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function closeBookModal() {
            var modal = document.getElementById('bookModal');
            if (modal) { modal.classList.remove('show'); modal.style.display = 'none'; modal.setAttribute('aria-hidden', 'true'); }
        }
        function showBookModal() {
            var modal = document.getElementById('bookModal');
            if (modal) { modal.classList.add('show'); modal.style.display = 'flex'; modal.setAttribute('aria-hidden', 'false'); }
        }
    </script>
</body>
</html>
