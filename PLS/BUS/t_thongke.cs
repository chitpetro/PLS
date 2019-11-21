using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BUS
{
    public class t_thongke
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        // Mới
        public void moi(string id, string iddv, string tendonvi, string idsp, string tensp, double thanhtien)
        {
            thongke_tonkho tk = new thongke_tonkho();
            tk.id = id;
            tk.iddv = iddv;
            tk.tendonvi = tendonvi;
            tk.idsp = idsp;
            tk.tensp = tensp;
            tk.thanhtien = thanhtien;

            db.thongke_tonkhos.InsertOnSubmit(tk);
            db.SubmitChanges();
        }

        public void sua(string id, string iddv, string tendonvi, string idsp, string tensp, double thanhtien)
        {
            thongke_tonkho tk = (from c in db.thongke_tonkhos select c).Single(x => x.id == id);

            tk.iddv = iddv;
            tk.tendonvi = tendonvi;
            tk.idsp = idsp;
            tk.tensp = tensp;
            tk.thanhtien = thanhtien;

            db.SubmitChanges();
        }
         
        public void xoa(string id)
        {
            thongke_tonkho tk = (from c in db.thongke_tonkhos select c).Single(x => x.id == id);
            db.thongke_tonkhos.DeleteOnSubmit(tk);
            db.SubmitChanges();
        }

        public void xoaAll(string id)
        {
            thongke_tonkho tk = (from c in db.thongke_tonkhos select c).Single(x => x.id.StartsWith(id));
            
            db.thongke_tonkhos.DeleteOnSubmit(tk);
            db.SubmitChanges();
        }
    }
}
