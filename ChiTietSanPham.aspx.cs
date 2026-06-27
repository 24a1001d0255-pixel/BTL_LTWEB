using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace WBS_BTL
{
    public partial class ChiTietSanPham : Page
    {
        private static readonly Random _random = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string maSach = Request.QueryString["id"];
                if (string.IsNullOrWhiteSpace(maSach))
                {
                    Response.Redirect("~/TrangChu.aspx");
                    return;
                }

                var ds = Application["DanhSachSach"] as List<Sach>;
                var sach = ds?.FirstOrDefault(s => string.Equals(s.MaSach, maSach, StringComparison.OrdinalIgnoreCase));
                if (sach == null)
                {
                    Response.Redirect("~/TrangChu.aspx");
                    return;
                }

                imgBiaSach.ImageUrl = ResolveCoverUrl(sach.Anh);
                lblTenSach.Text = sach.TenSach;
                lblTacGia.Text = sach.TacGia;
                lblNhaXuatBan.Text = sach.NhaXuatBan;
                lblSoTrang.Text = sach.SoTrang > 0 ? sach.SoTrang + " trang" : "-";
                lblTinhTrang.Text = sach.TinhTrangKho;
                lblGia.Text = sach.Gia.ToString("N0") + "₫";
                lblMoTa.Text = sach.MoTa ?? string.Empty;

                var reviews = GetSampleReviews(sach.TenSach).ToList();
                var avg = Math.Round(reviews.Average(x => x.SoSao), 1);
                lblTrungBinh.Text = avg.ToString("0.0");
                lblSoLuongDanhGia.Text = "Dựa trên " + reviews.Count + " đánh giá";

                rptReviews.DataSource = reviews;
                rptReviews.DataBind();
            }
        }

        protected void btnThemVaoGio_Click(object sender, EventArgs e)
        {
            string maSach = Request.QueryString["id"];
            var ds = Application["DanhSachSach"] as List<Sach>;
            var sach = ds?.FirstOrDefault(s => string.Equals(s.MaSach, maSach, StringComparison.OrdinalIgnoreCase));
            if (sach == null)
            {
                Response.Redirect("~/TrangChu.aspx");
                return;
            }

            List<GioHangItem> gioHang = Session["giohang"] as List<GioHangItem> ?? new List<GioHangItem>();
            var existing = gioHang.FirstOrDefault(x => string.Equals(x.MaSach, sach.MaSach, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.SoLuong++;
            }
            else
            {
                gioHang.Add(new GioHangItem
                {
                    MaSach = sach.MaSach,
                    TenSach = sach.TenSach,
                    Gia = sach.Gia,
                    Anh = sach.Anh,
                    SoLuong = 1
                });
            }

            Session["giohang"] = gioHang;
            var m = this.Master as Site;
            m?.UpdateCartCountFromSession();

            string script = "alert('Đã thêm vào giỏ hàng.');";
            ScriptManager.RegisterStartupScript(this, GetType(), "addedToCart", script, true);
        }

        private string ResolveCoverUrl(string rawPath)
        {
            string path = (rawPath ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(path))
            {
                return "~/images/default.jpg";
            }

            if (path.StartsWith("~/", StringComparison.Ordinal) || path.StartsWith("/", StringComparison.Ordinal))
            {
                return path;
            }

            if (path.StartsWith("images/", StringComparison.OrdinalIgnoreCase))
            {
                return "~/" + path;
            }

            return "~/images/" + path;
        }

        private IEnumerable<ReviewModel> GetSampleReviews(string tenSach)
        {
            string tenSachLocal = string.IsNullOrWhiteSpace(tenSach) ? "cuốn sách" : tenSach;
            var reviewers = new[]
            {
                new { Ten = "Minh Anh" },
                new { Ten = "Hoàng Nam" },
                new { Ten = "Thu Hà" },
                new { Ten = "Đức Anh" },
                new { Ten = "Linh Đan" }
            };

            var contents = new[]
            {
                $"Bìa đẹp, nội dung hay, mình đọc {tenSachLocal} trong hai ngày và cảm thấy rất đáng tiền.",
                $"Sách {tenSachLocal} giao nhanh, đóng gói cẩn thận. Tuy nhiên mình mong tái bản sửa một số lỗi in.",
                $"Nội dung sâu sắc, cách viết dễ hiểu, phù hợp cho người mới bắt đầu đọc {tenSachLocal}.",
                $"Đây là một trong những cuốn {tenSachLocal} mà mình muốn giữ lại trên kệ sách lâu dài.",
                $"Cuốn {tenSachLocal} phù hợp đọc cuối tuần, nhẹ nhàng mà vẫn truyền tải được thông điệp tốt."
            };

            var now = DateTime.Now;
            for (int i = 0; i < reviewers.Length; i++)
            {
                yield return new ReviewModel
                {
                    TenNguoiDung = reviewers[i].Ten,
                    Initial = char.ToUpperInvariant(reviewers[i].Ten[0]).ToString(),
                    SoSao = _random.Next(4, 6),
                    NoiDung = contents[i % contents.Length],
                    NgayDanhGia = now.AddDays(-i - 1).ToString("dd MMM, yyyy")
                };
            }
        }

        private class ReviewModel
        {
            public string TenNguoiDung { get; set; }
            public string Initial { get; set; }
            public int SoSao { get; set; }
            public string NoiDung { get; set; }
            public string NgayDanhGia { get; set; }
        }
    }
}
