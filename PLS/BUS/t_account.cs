using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Linq;

namespace BUS
{
    public class t_account
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        public List<account> dangnhap(string taikhoan, string matkhau)
        {
            var dn = (from tk in db.accounts where tk.uname == taikhoan & tk.pass == matkhau select tk).ToList();
            return dn;
        }
        public List<account> dangnhap2(string taikhoan, string matkhau)
        {
            var dn = (from tk in db.accounts where tk.uname == taikhoan & tk.pass == matkhau & tk.IsActived == true select tk).ToList();
            return dn;
        }

        public void moi(string id, string uname, string name, string pass, string phongban, string madonvi, bool ia)
        {
            var ac = db.accounts.FirstOrDefault(t => t.id == id);
            if (ac == null)
            {
                ac = new account();
                ac.id = id;
                ac.uname = uname;
                ac.name = name;
                ac.pass = pass;
                ac.phongban = phongban;
                ac.madonvi = madonvi;
                ac.IsActived = ia;
                db.accounts.InsertOnSubmit(ac);
                db.SubmitChanges();
            }
            else
            {
                ac.uname = uname;
                ac.name = name;
                ac.pass = pass;
                ac.phongban = phongban;
                ac.madonvi = madonvi;
                ac.IsActived = ia;

                db.SubmitChanges();
            }
        }

        public void xoa(string id)
        {
            account ac = (from tb in db.accounts select tb).Single(t => t.id == id);
            db.accounts.DeleteOnSubmit(ac);
            db.SubmitChanges();
        }
    }
}
