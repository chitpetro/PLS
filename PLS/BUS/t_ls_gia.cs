using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Linq;

namespace BUS
{
    public class t_ls_gia
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();

        public void moi_sua(string id, string idsp, string iddv, double giacu, double giamoi, DateTime thoigian,string idnv, string ghichu,long so)
        {
            var sp = db.ls_gias.FirstOrDefault(t => t.id == id);
            if (sp == null)
            {
                sp = new ls_gia();
                sp.id = id;
                sp.idsp = idsp;
                sp.iddv = iddv;
                sp.giacu = giacu;
                sp.giamoi = giamoi;
                sp.thoigian = thoigian;
                sp.idnv = idnv;
                sp.ghichu = ghichu;
                sp.so = so;

                db.ls_gias.InsertOnSubmit(sp);
                db.SubmitChanges();
            }
            else
            {
                sp.id = id;
                sp.idsp = idsp;
                sp.iddv = iddv;
                sp.giacu = giacu;
                sp.giamoi = giamoi;
                sp.thoigian = thoigian;
                sp.idnv = idnv;
                sp.ghichu = ghichu;
                sp.so = so;

                db.SubmitChanges();
            }
        } 
        public void xoa(string idsp, string iddv)
        {
            giasp sp = (from tb in db.giasps where tb.iddv == iddv select tb).Single(t => t.idsp == idsp);
            db.giasps.DeleteOnSubmit(sp);
            db.SubmitChanges();
        } 
    }
}