using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class TrangChu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var ds = Application["DanhSachSach"] as List<Sach>;
                if (ds != null)
                {
                    var top10 = ds
                        .OrderByDescending(sp => sp.Gia)
                        .Take(10)
                        .ToList();

                    rptSachNoiBat.DataSource = top10;
                    rptSachNoiBat.DataBind();
                }
            }
        }

        protected void btnThemVaoGio_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            string maSach = e.CommandArgument.ToString();
            var dsSanPham = Application["DanhSachSach"] as List<Sach>;
            var sach = dsSanPham?.FirstOrDefault(sp => sp.MaSach == maSach);
            if (sach == null)
            {
                return;
            }

            List<GioHangItem> gioHang;
            if (Session["giohang"] == null)
            {
                gioHang = new List<GioHangItem>();
            }
            else
            {
                gioHang = Session["giohang"] as List<GioHangItem> ?? new List<GioHangItem>();
            }

            var existingItem = gioHang.FirstOrDefault(item => item.MaSach == maSach);
            if (existingItem != null)
            {
                existingItem.SoLuong++;
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
        }
    }
}
