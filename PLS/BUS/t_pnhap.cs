using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class t_pnhap
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        public void moipn(string id, DateTime ngaynhap, string iddt, string iddv, string idnv, string ghichu, int so, string loainhap, string tiente, float tygia, double tch)
        {
            pnhap pn = new pnhap();
            pn.id = id;
            pn.ngaynhap = ngaynhap;
            pn.iddt = iddt;
            pn.iddv = iddv;
            pn.idnv = idnv;
            pn.ghichu = ghichu;
            pn.so = so;
            pn.loainhap = loainhap;
            pn.tiente = tiente;
            pn.tygia = tygia;
            pn.tch = tch;
            db.pnhaps.InsertOnSubmit(pn);
            db.SubmitChanges();
        }
        public void moict(string idsp, string diengiai, float sl, float dongia, string idcv, string loaithue, float thue, float chietkhau, float thanhtien, string idpnhap, string id, string tiente, float tygia, float nguyente, double vhd, double tx, double vtt, double tn, double soluongcl)
        {
            pnhapct ct = new pnhapct();

            ct.idsanpham = idsp;
            ct.diengiai = diengiai;
            ct.soluong = sl;
            ct.dongia = dongia;
            ct.idcv = idcv;
            ct.loaithue = loaithue;
            ct.thue = thue;
            ct.chietkhau = chietkhau;
            ct.thanhtien = thanhtien;
            ct.idpnhap = idpnhap;
            ct.id = id;
            ct.tiente = tiente;
            ct.tygia = tygia;
            ct.nguyente = nguyente;
            ct.vhd = vhd;
            ct.tx = tx;
            ct.vtt = vtt;
            ct.tn = tn;
            ct.soluongcl = soluongcl;
            db.pnhapcts.InsertOnSubmit(ct);
            db.SubmitChanges();
        }


        public void suapn(string id, DateTime ngaynhap, string iddt, string ghichu, int so, string loainhap, string tiente, float tygia, double tch)
        {
            pnhap pn = (from c in db.pnhaps select c).Single(x => x.id == id);

            pn.ngaynhap = ngaynhap;
            pn.iddt = iddt;
            pn.ghichu = ghichu;
            pn.so = so;
            pn.tiente = tiente;
            pn.tygia = tygia;
            pn.loainhap = loainhap;
            pn.tch = tch;
            db.SubmitChanges();
        }
        public void suact(string idsp, string diengiai, float sl, float dongia, string idcv, string loaithue, float thue, float chietkhau, float thanhtien, string idpnhap, string id, string tiente, float tygia, float nguyente, double vhd, double tx, double vtt, double tn, double soluongcl)
        {
            pnhapct ct = (from c in db.pnhapcts select c).Single(x => x.id == id);

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
            ct.vhd = vhd;
            ct.tx = tx;
            ct.vtt = vtt;
            ct.tn = tn;
            ct.soluongcl = soluongcl;
            //ct.idpnhap = idpnhap;
            //ct.id = id;

            db.SubmitChanges();
        }

        public void xoaPN(string id)
        {
            pnhap pn = (from c in db.pnhaps select c).Single(x => x.id == id);
            db.pnhaps.DeleteOnSubmit(pn);
            db.SubmitChanges();
        }
        public void xoact(string id)
        {
            pnhapct ct = (from c in db.pnhapcts select c).Single(x => x.id == id);
            db.pnhapcts.DeleteOnSubmit(ct);
            db.SubmitChanges();
        }
    }
}
