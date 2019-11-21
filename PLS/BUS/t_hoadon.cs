using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BUS
{
    public class t_hoadon
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        // Mới
        public void moihd(string id, DateTime ngayhd, string iddt, string idnv, string iddv, string ghichu, int so, string loaixuat, string tiente, double tygia, string vat)
        {
            hoadon hd = new hoadon();
            hd.id = id;
            hd.ngayhd = ngayhd;
            hd.iddt = iddt;
            hd.idnv = idnv;
            hd.iddv = iddv;
            hd.iddt = iddt;
            hd.ghichu = ghichu;
            hd.tiente = tiente;
            hd.tygia = tygia;
            hd.so = so;
            hd.soVAT = vat;

            hd.loaixuat = loaixuat;

            db.hoadons.InsertOnSubmit(hd);
            db.SubmitChanges();
        }
        public void moihd2(string id, DateTime ngayhd, string iddt, string idnv, string iddv, string ghichu, int so, string loaixuat, string tiente, double tygia, string link, string dv, string vat)
        {
            hoadon hd = new hoadon();
            hd.id = id;
            hd.ngayhd = ngayhd;
            hd.iddt = iddt;
            hd.idnv = idnv;
            hd.iddv = iddv;
            hd.iddt = iddt;
            hd.ghichu = ghichu;
            hd.tiente = tiente;
            hd.tygia = tygia;
            hd.so = so;
            hd.loaixuat = loaixuat;
            hd.link = link;
            hd.dv = dv;
            hd.soVAT = vat;
            db.hoadons.InsertOnSubmit(hd);
            db.SubmitChanges();
        }

        public void moihdct(string idhhoadon, string idsanpham, string diengiai, double soluong, double dongia, string idcv, string loaithue, double thue, double chietkhau, double thanhtien, string id, string tiente, double tygia, double nguyente)
        {
            hoadonct hdct = new hoadonct();
            hdct.idhoadon = idhhoadon;
            hdct.idsanpham = idsanpham;
            hdct.diengiai = diengiai;
            hdct.soluong = soluong;
            hdct.dongia = dongia;
            hdct.idcv = idcv;
            hdct.loaithue = loaithue;
            hdct.thue = thue;
            hdct.chietkhau = chietkhau;
            hdct.thanhtien = thanhtien;
            hdct.id = id;
            hdct.tiente = tiente;
            hdct.tygia = tygia;
            hdct.nguyente = nguyente;

            db.hoadoncts.InsertOnSubmit(hdct);
            db.SubmitChanges();
        }

        public void suahd(string id, DateTime ngaylap, string iddt, string ghichu, int so, string loaixuat, string tiente, double tygia, string vat)
        {
            hoadon hd = (from c in db.hoadons select c).Single(x => x.id == id);

            hd.ngayhd = ngaylap;
            hd.iddt = iddt;
            hd.ghichu = ghichu;
            hd.so = so;
            hd.tiente = tiente;
            hd.tygia = tygia;
            hd.loaixuat = loaixuat;
            hd.soVAT = vat;
            db.SubmitChanges();
        }
        public void suahdct(string idhoadon, string idsp, string diengiai, double sl, double dongia, string idcv, string loaithue, double thue, double chietkhau, double thanhtien, string id, string tiente, double tygia, double nguyente)
        {
            hoadonct ct = (from c in db.hoadoncts select c).Single(x => x.id == id);

            ct.idhoadon = idhoadon;
            ct.idsanpham = idsp;
            ct.diengiai = diengiai;
            ct.soluong = sl;
            ct.dongia = dongia;
            ct.idcv = idcv;
            ct.loaithue = loaithue;
            ct.thue = thue;
            ct.chietkhau = chietkhau;
            ct.thanhtien = thanhtien;
            ct.tiente = tiente;
            ct.tygia = tygia;
            ct.nguyente = nguyente;

            db.SubmitChanges();
        }

        public void xoahd(string id)
        {
            hoadon hd = (from c in db.hoadons select c).Single(x => x.id == id);
            db.hoadons.DeleteOnSubmit(hd);
            db.SubmitChanges();
        }
        public void xoact(string id)
        {
            hoadonct ct = (from c in db.hoadoncts select c).Single(x => x.id == id);
            db.hoadoncts.DeleteOnSubmit(ct);
            db.SubmitChanges();
        }
    }
}
