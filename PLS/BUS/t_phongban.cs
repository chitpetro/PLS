using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BUS
{
    public class t_phongban
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();

        public void moi(string id, string ten)
        {
            var dt = db.phongbans.FirstOrDefault(t => t.id == id);
            if (dt == null)
            {
                dt = new phongban();
                dt.id = id;
                dt.ten = ten;
              
                 
                db.phongbans.InsertOnSubmit(dt);
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
            phongban dt = (from d in db.phongbans select d).Single(t => t.id == id);
            db.phongbans.DeleteOnSubmit(dt);
            db.SubmitChanges();
        }
    }
}