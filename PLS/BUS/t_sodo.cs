using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BUS
{
   public class t_sodo
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();

        public void moi(string id, string iddv, string cotbom, string voibom, string idsp, int stt)
        {
            sodocotbom sp = new sodocotbom();
            sp.id = id;
            sp.cotbom = cotbom;
            sp.voibom = voibom;
            sp.iddv = iddv;
            sp.idsp = idsp;
            sp.stt = stt;
            db.sodocotboms.InsertOnSubmit(sp);
            db.SubmitChanges();
        }
        public void sua(string id, string iddv, string cotbom, string voibom, string idsp)
        {
            sodocotbom sp = (from tb in db.sodocotboms select tb).Single(t => t.id == id);

            sp.id = id;
            sp.cotbom = cotbom;
            sp.voibom = voibom;
            sp.iddv = iddv;
            sp.idsp = idsp;
          
            db.SubmitChanges();
        }
        public void xoa( string iddv, string id)
        {
            sodocotbom sp = (from tb in db.sodocotboms where tb.iddv == iddv  select tb).Single(t => t.id == id);
            db.sodocotboms.DeleteOnSubmit(sp);
            db.SubmitChanges();
        }
    }
}
