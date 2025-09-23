using System;
using System.Collections.Generic;
using System.IO;

namespace QuanLyChiTieu
{
    public class DichVuQuanLyTapTin
    {
        private readonly string _duongDanTapTinChiTieu;
        private readonly string _duongDanTapTinNguoiDung;
        private readonly string _duongDanTapTinLoi;

        public DichVuQuanLyTapTin()
        {
            string duongDanDuLieuUngDung = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string thuMucUngDung = Path.Combine(duongDanDuLieuUngDung, "QuanLyChiTieu");
            Directory.CreateDirectory(thuMucUngDung);

            _duongDanTapTinChiTieu = Path.Combine(thuMucUngDung, "danh_sach_chi_tieu.txt");
            _duongDanTapTinNguoiDung = Path.Combine(thuMucUngDung, "danh_sach_nguoi_dung.txt");
            _duongDanTapTinLoi = Path.Combine(thuMucUngDung, "loi.log");
        }

        public void LuuDanhSachChiTieu(List<ChiTieu> danhSachChiTieu)
        {
            try
            {
                if (File.Exists(_duongDanTapTinChiTieu))
                {
                    string tenTapTinSaoLuu = $"danh_sach_chi_tieu_{DateTime.Now:yyyyMMddHHmmss}.txt";
                    string duongDanTapTinSaoLuu = Path.Combine(Path.GetDirectoryName(_duongDanTapTinChiTieu), tenTapTinSaoLuu);
                    File.Copy(_duongDanTapTinChiTieu, duongDanTapTinSaoLuu, overwrite: true);
                }
                File.WriteAllLines(_duongDanTapTinChiTieu, ChuyenDanhSachThanhChuoi(danhSachChiTieu));
            }
            catch (IOException ex) { GhiLoi($"Lỗi lưu {_duongDanTapTinChiTieu}: {ex.Message}"); throw; }
            catch (Exception ex) { GhiLoi($"Lỗi lưu: {ex.Message}"); throw; }
        }

        public List<ChiTieu> TaiDanhSachChiTieu()
        {
            try
            {
                if (!File.Exists(_duongDanTapTinChiTieu)) return new List<ChiTieu>();
                string[] lines = File.ReadAllLines(_duongDanTapTinChiTieu);
                return ChuyenChuoiThanhDanhSach(lines);
            }
            catch (IOException ex) { GhiLoi($"Lỗi đọc {_duongDanTapTinChiTieu}: {ex.Message}"); return new List<ChiTieu>(); }
            catch (Exception ex) { GhiLoi($"Lỗi đọc: {ex.Message}"); return new List<ChiTieu>(); }
        }

        public void LuuDanhSachNguoiDung(List<NguoiDung> danhSachNguoiDung)
        {
            try
            {
                if (File.Exists(_duongDanTapTinNguoiDung))
                {
                    string tenTapTinSaoLuu = $"danh_sach_nguoi_dung_{DateTime.Now:yyyyMMddHHmmss}.txt";
                    string duongDanTapTinSaoLuu = Path.Combine(Path.GetDirectoryName(_duongDanTapTinNguoiDung), tenTapTinSaoLuu);
                    File.Copy(_duongDanTapTinNguoiDung, duongDanTapTinSaoLuu, overwrite: true);
                }
                File.WriteAllLines(_duongDanTapTinNguoiDung, ChuyenDanhSachThanhChuoi(danhSachNguoiDung));
            }
            catch (IOException ex) { GhiLoi($"Lỗi lưu {_duongDanTapTinNguoiDung}: {ex.Message}"); throw; }
            catch (Exception ex) { GhiLoi($"Lỗi lưu: {ex.Message}"); throw; }
        }

        public List<NguoiDung> TaiDanhSachNguoiDung()
        {
            try
            {
                if (!File.Exists(_duongDanTapTinNguoiDung)) return new List<NguoiDung>();
                string[] lines = File.ReadAllLines(_duongDanTapTinNguoiDung);
                return ChuyenChuoiThanhDanhSach(lines);
            }
            catch (IOException ex) { GhiLoi($"Lỗi đọc {_duongDanTapTinNguoiDung}: {ex.Message}"); return new List<NguoiDung>(); }
            catch (Exception ex) { GhiLoi($"Lỗi đọc: {ex.Message}"); return new List<NguoiDung>(); }
        }

        private void GhiLoi(string thongBaoLoi)
        {
            try { File.AppendAllText(_duongDanTapTinLoi, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {thongBaoLoi}{Environment.NewLine}"); }
            catch { }
        }

        private string[] ChuyenDanhSachThanhChuoi<T>(List<T> danhSach)
        {
            var result = new List<string>();
            foreach (var item in danhSach)
            {
                if (item is ChiTieu ct) result.Add($"{ct.MaSo}|{ct.MoTa}|{ct.SoTien}|{ct.Ngay}");
                else if (item is NguoiDung nd) result.Add($"{nd.TenDangNhap}|{nd.MatKhau}");
            }
            return result.ToArray();
        }

        private List<T> ChuyenChuoiThanhDanhSach<T>(string[] lines)
        {
            var result = new List<T>();
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (typeof(T) == typeof(ChiTieu) && parts.Length == 4)
                    result.Add((T)(object)new ChiTieu { MaSo = int.Parse(parts[0]), MoTa = parts[1], SoTien = decimal.Parse(parts[2]), Ngay = DateTime.Parse(parts[3]) });
                else if (typeof(T) == typeof(NguoiDung) && parts.Length == 2)
                    result.Add((T)(object)new NguoiDung { TenDangNhap = parts[0], MatKhau = parts[1] });
            }
            return result;
        }
    }

    public class NguoiDung { public string TenDangNhap { get; set; } public string MatKhau { get; set; } }
    public class ChiTieu { public int MaSo { get; set; } public string MoTa { get; set; } public decimal SoTien { get; set; } public DateTime Ngay { get; set; } }
}