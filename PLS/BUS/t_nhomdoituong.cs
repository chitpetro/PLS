using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class t_nhomdoituong
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();

        public void moi(string id, string ten)
        {
            var dt = db.nhomdoituongs.FirstOrDefault(t => t.id == id);
            if (dt == null)
            {
                dt = new nhomdoituong();
                dt.id = id;
                dt.ten = ten;

                db.nhomdoituongs.InsertOnSubmit(dt);
                db.SubmitChanges();
            }
            else
            {
                dt.ten = ten;
                db.SubmitChanges();
            }
        }
        public void xoa(string id)
        {
            nhomdoituong dt = (from d in db.nhomdoituongs select d).Single(t => t.id == id);
            db.nhomdoituongs.DeleteOnSubmit(dt);
            db.SubmitChanges();
        }
    }
}