<%@ Page Title="Trang chủ" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="TrangChu.aspx.cs" Inherits="WBS_BTL.TrangChu" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/TrangChu.css" />

    <section class="page-home">
        <div class="container">
            <h2 class="section-title">Sách nổi bật</h2>
            <div class="book-grid">
                <asp:Repeater ID="rptSachNoiBat" runat="server">
                    <ItemTemplate>
                        <article class="book-card">
                                <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link">
                                    <img class="book-cover" src='<%# Eval("Anh") %>' alt='<%# Eval("TenSach") %>' />
                                </a>
                                <div class="book-body">
                                    <h3 class="book-title"><a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link"><%# Eval("TenSach") %></a></h3>
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
        </div>
    </section>
</asp:Content>
 