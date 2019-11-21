using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BUS
{
    public class t_pttoan
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        public void moipttoan(string id, DateTime ngaythu, string iddt, string iddv, string idnv, string ghichu, int so, string dv, string tiente, double tygia)
        {
            pttoan pt = new pttoan();
            pt.id = id;
            pt.ngaythu = ngaythu;
            pt.iddt = iddt;
            pt.iddv = iddv;
            pt.tiente = tiente;
            pt.tygia = tygia;
            pt.idnv = idnv;
            pt.ghichu = ghichu;
            pt.so = so;
            pt.dv = dv;

            db.pttoans.InsertOnSubmit(pt);
            db.SubmitChanges();
        }
        public void moict(string diengiai, string idcv, string idmuccp, double thanhtien, string idttoan, string id, double nguyente)/* string tk)*/
        {
            pttoanct ct = new pttoanct();

            ct.diengiai = diengiai;

            ct.idcv = idcv;
            ct.idmuccp = idmuccp;

            ct.thanhtien = thanhtien;
            ct.idttoan = idttoan;
            ct.id = id;
            ct.nguyente = nguyente;
            //ct.tkco = tk;
            db.pttoancts.InsertOnSubmit(ct);
            db.SubmitChanges();
        }

        public void suapt(string id, DateTime ngaythu, string iddt, string ghichu, int so, string dv, string tiente, double tygia)
        {
            pttoan pn = (from c in db.pttoans select c).Single(x => x.id == id);

            pn.ngaythu = ngaythu;
            pn.iddt = iddt;
            pn.ghichu = ghichu;
            pn.so = so;
            pn.tiente = tiente;
            pn.tygia = tygia;
            pn.dv = dv;

            db.SubmitChanges();
        }

        public void xoapttoan(string id)
        {
            pttoan pt = (from c in db.pttoans select c).Single(x => x.id == id);
            db.pttoans.DeleteOnSubmit(pt);
            db.SubmitChanges();
        }
        public void xoact(string id)
        {
            pttoanct ct = (from c in db.pttoancts select c).Single(x => x.id == id);
            db.pttoancts.DeleteOnSubmit(ct);
            db.SubmitChanges();
        }
    }
}
