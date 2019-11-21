using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Linq;

namespace BUS
{
    public class t_gia
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();

        public void moi(string id, string iddv, string idsp, double giasp)
        {
            giasp sp = new giasp();

            sp.id = id;
            sp.giaban = giasp;
            sp.iddv = iddv;
            sp.idsp = idsp;
            sp.kiemke = 0;
            db.giasps.InsertOnSubmit(sp);
            db.SubmitChanges();
        }
        public void sua(string id, string iddv, string idsp, double giasp)
        {
            giasp sp = (from c in db.giasps select c).Single(x => x.id == id);

            sp.giaban = giasp;
            sp.iddv = iddv;
            sp.idsp = idsp;
            sp.kiemke = 0;
            db.SubmitChanges();
        }
        public void suakiemke(string id, string iddv, string idsp, double kiemke)
        {
            giasp sp = (from tb in db.giasps select tb).Single(t => t.id == id);

            sp.id = id;
            sp.kiemke = kiemke;
            sp.iddv = iddv;
            sp.idsp = idsp;
            db.SubmitChanges();
        }
        public void xoa(string idsp, string iddv)
        {
            giasp sp = (from tb in db.giasps where tb.iddv == iddv select tb).Single(t => t.idsp == idsp);
            db.giasps.DeleteOnSubmit(sp);
            db.SubmitChanges();
        }
    }
}
