<%@ Page Title="Danh sách sản phẩm" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="DanhSachSp.aspx.cs" Inherits="WBS_BTL.DanhSachSp" %>

<asp:Content ID="ContentProductList" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/ProductList.css" />

    <div class="page-product-list">
        <div class="container">
            <div class="product-layout">
                <!-- Sidebar bộ lọc -->
                <aside class="filter-sidebar">
                    <h2 class="filter-title">Bộ lọc</h2>

                    <div class="filter-group">
                        <h3 class="filter-group-title">Khoảng giá</h3>
                        <asp:RadioButtonList ID="rblGia" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed">
                            <asp:ListItem Text="Tất cả" Value="all" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Dưới 50.000₫" Value="duoi50"></asp:ListItem>
                            <asp:ListItem Text="50.000₫ - 100.000₫" Value="50-100"></asp:ListItem>
                            <asp:ListItem Text="100.000₫ - 200.000₫" Value="100-200"></asp:ListItem>
                            <asp:ListItem Text="Trên 200.000₫" Value="tren200"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>

                    <div class="filter-group">
                        <h3 class="filter-group-title">Thể loại</h3>
                        <asp:RadioButtonList ID="rblTheLoai" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed">
                        </asp:RadioButtonList>
                    </div>

                    <div class="filter-group">
                        <h3 class="filter-group-title">Tình trạng kho</h3>
                        <asp:RadioButtonList ID="rblKho" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed">
                            <asp:ListItem Text="Tất cả" Value="all" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Còn hàng" Value="conhang"></asp:ListItem>
                            <asp:ListItem Text="Đã đặt" Value="dadat"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </aside>

                <!-- Vùng sản phẩm -->
                <section class="product-main">
                    <div class="product-header">
                        <h1 class="product-page-title">
                            <asp:Label ID="lblTieuDe" runat="server" Text="Tất cả sách"></asp:Label>
                        </h1>
                        <span class="product-count"><asp:Label ID="lblSoLuong" runat="server" Text="0"></asp:Label> cuốn sách</span>
                    </div>

                    <asp:Panel ID="pnlEmpty" runat="server" CssClass="product-empty" Visible="false">
                        <p>Không tìm thấy sách phù hợp với điều kiện lọc.</p>
                    </asp:Panel>

                    <div class="book-grid">
                        <asp:Repeater ID="rptSach" runat="server">
                            <ItemTemplate>
                                <article class="book-card">
                                    <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link">
                                        <img class="book-cover" src='<%# Eval("Anh") %>' alt='<%# Eval("TenSach") %>' />
                                    </a>
                                    <div class="book-body">
                                        <span class="book-category"><%# Eval("TheLoai") %></span>
                                        <h3 class="book-title">
                                            <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link"><%# Eval("TenSach") %></a>
                                        </h3>
                                        <p class="book-meta"><%# Eval("TacGia") %> · <%# Eval("NhaXuatBan") %></p>
                                        <div class="book-price"><%# Eval("Gia", "{0:N0}₫") %></div>
                                        <asp:Button ID="btnThemVaoGio" runat="server" Text="Thêm vào giỏ"
                                            CommandArgument='<%# Eval("MaSach") %>'
                                            OnCommand="btnThemVaoGio_Command"
                                            CssClass="btn-add-cart" />
                                    </div>
                                </article>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <!-- Pagination giả lập -->
                    <asp:Panel ID="pnlPagination" runat="server" CssClass="pagination" Visible="false">
                        <asp:LinkButton ID="lnkPrev" runat="server" CssClass="page-link" OnClick="Page_Changed" CommandArgument="prev">Trước</asp:LinkButton>
                        <span class="page-info">Trang <asp:Label ID="lblPage" runat="server" Text="1"></asp:Label></span>
                        <asp:LinkButton ID="lnkNext" runat="server" CssClass="page-link" OnClick="Page_Changed" CommandArgument="next">Sau</asp:LinkButton>
                    </asp:Panel>
                </section>
            </div>
        </div>
    </div>
</asp:Content>
