using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BUS
{
    public class t_cttk
    {

        KetNoiDBDataContext db = new KetNoiDBDataContext();

        public void moi(string id, string iddv, string loaichungtu, string machungtu, DateTime ngaychungtu, DateTime ngaylchungtu, int sochungtu, string doituong, string diengiai,
            string tk_no, string tk_co, double ps, string tiente, double tygia, double ps_nt, string idsp, string sanpham, string idnv,
            string loai, string idcv, string idmuccp)
        {
            ct_tk dt = new ct_tk();
            dt.id = id;
            dt.iddv = iddv;
            dt.loaichungtu = loaichungtu;
            dt.ngaychungtu = ngaychungtu;
            dt.ngaylchungtu = ngaylchungtu;
            dt.machungtu = machungtu;
            dt.sochungtu = sochungtu;
            dt.doituong = doituong;
            dt.diengiai = diengiai;
            dt.tk_no = tk_no;
            dt.tk_co = tk_co;
            dt.PS = ps;
            dt.tiente = tiente;
            dt.tygia = tygia;
            dt.PS_nt = ps_nt;
            dt.idsp = idsp;
            dt.sanpham = sanpham;
            dt.idnv = idnv;
            dt.loai = loai;
            dt.idcv = idcv;
            dt.idmuccp = idmuccp;


            db.ct_tks.InsertOnSubmit(dt);
            db.SubmitChanges();

        }
         
        public void xoa(string id)
        {
            var dt = (from d in db.ct_tks where d.machungtu == id select d);
            db.ct_tks.DeleteAllOnSubmit(dt);
       
            db.SubmitChanges();
        }
    }
}
