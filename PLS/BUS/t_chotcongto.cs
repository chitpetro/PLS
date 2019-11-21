using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BUS
{
    public class t_chotcongto
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        // Mới
        public void moicongto(string id, DateTime ngay, string idnv, string iddv, int so)
        {
            chotcongto hd = new chotcongto();
            hd.id = id;
            hd.ngay = ngay;
            hd.idnv = idnv;
            hd.so = so;
            hd.iddv = iddv;
            db.chotcongtos.InsertOnSubmit(hd);
            db.SubmitChanges();
        }
        public void moicongtoct(string cotbom, string voibom, string idct, string loaisp, double chotdau, double chotcuoi, double soluong, double thu, double dongia, double thanhtien, string id, int so, string iddv, int stt)
        {
            chotcongtoct hdct = new chotcongtoct();
            hdct.cotbom = cotbom;
            hdct.voibom = voibom;
            hdct.idct = idct;
            hdct.loaisp = loaisp;
            hdct.chotdau = chotdau;
            hdct.chotcuoi = chotcuoi;
            hdct.soluong = soluong;
            hdct.thu = thu;
            hdct.dongia = dongia;
            hdct.thanhtien = thanhtien;
            hdct.id = id;
            hdct.so = so;
            hdct.stt = stt;
            hdct.iddv = iddv;
            db.chotcongtocts.InsertOnSubmit(hdct);
            db.SubmitChanges();
        }

        public void suachotcongto(string id, DateTime ngay)
        {
            chotcongto hd = (from c in db.chotcongtos select c).Single(x => x.id == id);
            hd.ngay = ngay;
            db.SubmitChanges();
        }
        public void suachotcongtoct(double chotdau, double chotcuoi, double thu, double dongia, string id)
        {
            chotcongtoct ct = (from c in db.chotcongtocts select c).Single(x => x.id == id);
            ct.chotdau = chotdau;
            ct.chotcuoi = chotcuoi;
            ct.dongia = dongia;
            ct.thu = thu;
            db.SubmitChanges();
        }

        //sửa số thứ tự chotcongto
        public void suaSOchotcongto(string id, int so)
        {
            chotcongto hd = (from c in db.chotcongtos select c).FirstOrDefault(x => x.id == id);
            hd.so = so;
            db.SubmitChanges();
        }
        //sửa số thứ tự chotcongtoct
        public void suaSOchotcongtoct(string id, int so)
        {
            var count = db.chotcongtocts.Where(x => x.idct == id).Count();

            for (int i = 0; i < count; i++)
            {
                //chotcongtoct hd = (from c in db.chotcongtocts select c).FirstOrDefault(x => x.idct == id & x.so != so);
                //hd.so = so;
                //db.SubmitChanges();

                chotcongtoct hd = (from c in db.chotcongtocts select c).FirstOrDefault(x => x.idct == id & x.so != so);
                if (hd != null)
                {
                    hd.so = so;
                    db.SubmitChanges();
                }
            }
        }

        public void xoachotcongto(string id)
        {
            chotcongto hd = (from c in db.chotcongtos select c).Single(x => x.id == id);
            db.chotcongtos.DeleteOnSubmit(hd);
            db.SubmitChanges();
        }
        public void xoact(string id)
        {
            chotcongtoct ct = (from c in db.chotcongtocts select c).Single(x => x.id == id);
            db.chotcongtocts.DeleteOnSubmit(ct);
            db.SubmitChanges();
        }
    }
}
