<%@ Page Title="Chi tiết sách" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="ChiTietSanPham.aspx.cs" Inherits="WBS_BTL.ChiTietSanPham" %>

<asp:Content ID="ContentDetail" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/ChiTietSanPham.css" />

    <section class="page-detail">
        <div class="container detail-layout">
            <div class="detail-cover-wrap">
                <asp:Image ID="imgBiaSach" runat="server" CssClass="detail-cover" AlternateText="Bìa sách" />
            </div>

            <div class="detail-info">
                <h1 class="detail-title"><asp:Label ID="lblTenSach" runat="server" Text="Đang tải..."></asp:Label></h1>

                <div class="detail-meta">
                    <div class="meta-item">
                        <span class="meta-label">Tác giả</span>
                        <span class="meta-value"><asp:Label ID="lblTacGia" runat="server" Text="-"></asp:Label></span>
                    </div>
                    <div class="meta-item">
                        <span class="meta-label">Nhà xuất bản</span>
                        <span class="meta-value"><asp:Label ID="lblNhaXuatBan" runat="server" Text="-"></asp:Label></span>
                    </div>
                    <div class="meta-item">
                        <span class="meta-label">Số trang</span>
                        <span class="meta-value"><asp:Label ID="lblSoTrang" runat="server" Text="-"></asp:Label></span>
                    </div>
                    <div class="meta-item">
                        <span class="meta-label">Tình trạng</span>
                        <span class="meta-value"><asp:Label ID="lblTinhTrang" runat="server" Text="-"></asp:Label></span>
                    </div>
                </div>

                <div class="detail-price-row">
                    <span class="price-label">Giá</span>
                    <span class="price-value"><asp:Label ID="lblGia" runat="server" CssClass="price-number" Text="-"></asp:Label></span>
                </div>

                <div class="detail-summary">
                    <h2 class="summary-heading">Tóm tắt nội dung</h2>
                    <p><asp:Label ID="lblMoTa" runat="server" Text="-"></asp:Label></p>
                </div>

                <div class="detail-actions">
                    <asp:Button ID="btnThemVaoGio" runat="server" Text="Thêm vào giỏ hàng"
                        CssClass="btn-add-cart"
                        OnClick="btnThemVaoGio_Click" />
                </div>

                <section class="review-section">
                    <h2 class="review-heading">Đánh giá sản phẩm</h2>

                    <div class="review-summary">
                        <div class="review-summary-score">
                            <asp:Label ID="lblTrungBinh" runat="server" CssClass="review-summary-number" Text="4.5"></asp:Label>
                            <asp:Label ID="lblSoLuongDanhGia" runat="server" CssClass="review-summary-count" Text="Dựa trên 12 đánh giá"></asp:Label>
                        </div>
                    </div>

                    <asp:Repeater ID="rptReviews" runat="server">
                        <ItemTemplate>
                            <article class="review-card">
                                <div class="review-card-header">
                                    <div class="review-user">
                                        <span class="review-avatar"><%# Eval("Initial") %></span>
                                        <span class="review-name"><%# Eval("TenNguoiDung") %></span>
                                    </div>
                                    <span class="review-date"><%# Eval("NgayDanhGia") %></span>
                                </div>

                                <p class="review-text"><%# Eval("NoiDung") %></p>
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>
                </section>
            </div>
        </div>
    </section>
</asp:Content>
