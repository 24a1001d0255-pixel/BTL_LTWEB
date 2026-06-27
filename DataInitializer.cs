using System;
using System.Collections.Generic;
using System.Linq;

namespace WBS_BTL
{
    public static class DataInitializer
    {
        public static List<Sach> GetDanhSachSach()
        {
            var list = new List<Sach>
            {
                new Sach { MaSach = "S01", TenSach = "Đắc Nhân Tâm", TacGia = "Dale Carnegie", NhaXuatBan = "NXB Tổng hợp TP.HCM", SoTrang = 320, TinhTrangKho = "Còn hàng", Gia = 68000, Anh = "images/dacnhantam.webp", TheLoai = "Ky-nang", MoTa = "Cuốn sách kinh điển về cách thức ứng xử và thấu hiểu con người." },
                new Sach { MaSach = "S02", TenSach = "Nhà Giả Kim", TacGia = "Paulo Coelho", NhaXuatBan = "NXB Hà Nội", SoTrang = 228, TinhTrangKho = "Còn hàng", Gia = 79000, Anh = "images/nhagiakim.webp", TheLoai = "Van-hoc", MoTa = "Hành trình truy tìm ước mơ và ý nghĩa cuộc sống của chàng chăn cừu Santiago." },
                new Sach { MaSach = "S03", TenSach = "Sapiens: Lược Sử Loài Người", TacGia = "Yuval Noah Harari", NhaXuatBan = "NXB Tri Thức", SoTrang = 560, TinhTrangKho = "Còn hàng", Gia = 195000, Anh = "images/luocsuloainguoi.webp", TheLoai = "Khoa-hoc", MoTa = "Hành trình tiến hóa vĩ đại của loài người từ thời tiền sử đến thời hiện đại." },
                new Sach { MaSach = "S04", TenSach = "Tư Duy Nhanh Và Chậm", TacGia = "Daniel Kahneman", NhaXuatBan = "NXB Lao Động", SoTrang = 610, TinhTrangKho = "Còn hàng", Gia = 168000, Anh = "images/tuduynhanhvacham.webp", TheLoai = "Kinh-te", MoTa = "Giải mã cách con người ra quyết định thông qua hai hệ thống tư duy." },
                new Sach { MaSach = "S05", TenSach = "Dế Mèn Phiêu Lưu Ký", TacGia = "Tô Hoài", NhaXuatBan = "NXB Kim Đồng", SoTrang = 172, TinhTrangKho = "Còn hàng", Gia = 52000, Anh = "images/demenphieuluuky.jpg", TheLoai = "Thieu-nhi", MoTa = "Hành trình trưởng thành của chú Dế Mèn qua những cuộc phiêu lưu đáng nhớ." },
                new Sach { MaSach = "S06", TenSach = "Chiến Binh Cầu Vồng", TacGia = "Andrea Hirata", NhaXuatBan = "NXB Trẻ", SoTrang = 244, TinhTrangKho = "Còn hàng", Gia = 85000, Anh = "images/chienbinhcauvong.webp", TheLoai = "Van-hoc", MoTa = "Câu chuyện về tình bạn, ước mơ và nghị lực của những đứa trẻ nghèo huyện Belitong." },
                new Sach { MaSach = "S07", TenSach = "Atomic Habits", TacGia = "James Clear", NhaXuatBan = "NXB Thế Giới", SoTrang = 336, TinhTrangKho = "Còn hàng", Gia = 145000, Anh = "images/AtomicHabits.webp", TheLoai = "Ky-nang", MoTa = "Thay đổi nhỏ mỗi ngày tạo nên thành công lớn bền vững theo thời gian." },
                new Sach { MaSach = "S08", TenSach = "Tuổi Trẻ Đáng Giá Bao Nhiêu", TacGia = "Rosie Nguyễn", NhaXuatBan = "NXB Hà Nội", SoTrang = 288, TinhTrangKho = "Còn hàng", Gia = 61000, Anh = "images/tuoitredanggiabaonhieu.webp", TheLoai = "Ky-nang", MoTa = "Những suy ngẫm chân thành về học tập, yêu thương và trưởng thành." },
                new Sach { MaSach = "S09", TenSach = "Giáo Khoa Toán 12", TacGia = "Bộ GD&ĐT", NhaXuatBan = "NXB Giáo Dục Việt Nam", SoTrang = 196, TinhTrangKho = "Còn hàng", Gia = 32000, Anh = "images/toan12.png", TheLoai = "Giao-khoa", MoTa = "Sách giáo khoa chính thức theo chương trình Giáo dục phổ thông 2018." },
                new Sach { MaSach = "S10", TenSach = "Truyện Kiều", TacGia = "Nguyễn Du", NhaXuatBan = "NXB Văn Học", SoTrang = 152, TinhTrangKho = "Còn hàng", Gia = 45000, Anh = "images/truyenkieu.webp", TheLoai = "Van-hoc", MoTa = "Kiệt tác văn học dân tộc với những vần thơ bất hủ về nhân sinh quan." }
            };

            foreach (var s in list)
            {
                if (string.IsNullOrWhiteSpace(s.MoTa))
                    s.MoTa = BuildMoTaSach(s);
            }

            return list;
        }

        private static string BuildMoTaSach(Sach s)
        {
            string theLoai = string.IsNullOrWhiteSpace(s.TheLoai) ? "sách tham khảo" : s.TheLoai;
            return string.Format("{0} của {1} thuộc thể loại {2}. Tác phẩm phù hợp với độc giả yêu sách và muốn khám phá thêm kiến thức qua từng trang viết.", s.TenSach, string.IsNullOrWhiteSpace(s.TacGia) ? "tác giả" : s.TacGia, theLoai);
        }
    }
}
