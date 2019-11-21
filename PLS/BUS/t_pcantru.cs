using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BUS
{
    public class t_pcantru
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        public void moipcantru(string id, DateTime ngayct, string dttruno, string dtcantru, string iddv, string idnv, string ghichu, int so, string dv, string tiente, double tygia)
        {
            pcantru pt = new pcantru();
            pt.id = id;
            pt.ngayct = ngayct;
            pt.dttruno = dttruno;
            pt.dtcantru = dtcantru;
            pt.iddv = iddv;
            pt.tiente = tiente;
            pt.tygia = tygia;
            pt.idnv = idnv;
            pt.ghichu = ghichu;
            pt.so = so;
            pt.dv = dv;
            db.pcantrus.InsertOnSubmit(pt);
            db.SubmitChanges();
        }
        public void moict(string diengiai, string idcv, string idmuccp, double thanhtien, string idcantru, string id, double nguyente)/* string tk)*/
        {
            pcantruct ct = new pcantruct();

            ct.diengiai = diengiai;

            ct.idcv = idcv;
            ct.idmuccp = idmuccp;

            ct.thanhtien = thanhtien;
            ct.idcantru = idcantru;
            ct.id = id;
            ct.nguyente = nguyente;
            db.pcantructs.InsertOnSubmit(ct);
            db.SubmitChanges();
        }

        public void suapt(string id, DateTime ngayct, string dttruno, string dtcantru, string ghichu, int so, string dv, string tiente, double tygia)
        {
            pcantru pn = (from c in db.pcantrus select c).Single(x => x.id == id);

            pn.ngayct = ngayct;
            pn.dttruno = dttruno;
            pn.dtcantru = dtcantru;
            pn.ghichu = ghichu;
            pn.so = so;
            pn.tiente = tiente;
            pn.tygia = tygia;
            pn.dv = dv;
            db.SubmitChanges();
        }

        public void xoapcantru(string id)
        {
            pcantru pt = (from c in db.pcantrus select c).Single(x => x.id == id);
            db.pcantrus.DeleteOnSubmit(pt);
            db.SubmitChanges();
        }
        public void xoact(string id)
        {
            pcantruct ct = (from c in db.pcantructs select c).Single(x => x.id == id);
            db.pcantructs.DeleteOnSubmit(ct);
            db.SubmitChanges();
        }
    }
}
