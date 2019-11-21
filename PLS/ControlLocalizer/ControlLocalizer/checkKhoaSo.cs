using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
namespace ControlLocalizer
{
    public static class checkKhoaSo
    { 
        public static bool checkkhoaso(TextEdit txtdv, DateEdit txtngaynhap)
        {
            KetNoiDBDataContext db = new KetNoiDBDataContext();
            //check khoa so
            var khoasochungtu = db.khoasochungtus.FirstOrDefault(x => x.iddv == txtdv.Text); //ql2

            var ql1 = db.donvis.FirstOrDefault(t => t.id == txtdv.Text);
            var ql0 = db.donvis.FirstOrDefault(y => y.id == ql1.iddv);

            var khoaso1 = db.khoasochungtus.FirstOrDefault(x1 => x1.iddv == ql1.iddv);
            var khoaso0 = db.khoasochungtus.FirstOrDefault(x0 => x0.iddv == ql0.iddv);


            //checkks
            try
            {
                DateTime max = (DateTime)khoasochungtu.ngaykhoaso;
                if (khoaso1 != null)
                    if (khoaso1.ngaykhoaso > max) max = (DateTime)khoaso1.ngaykhoaso;
                if (khoaso0 != null)
                    if (khoaso0.ngaykhoaso > max) max = (DateTime)khoaso0.ngaykhoaso;

                if (txtngaynhap.DateTime <= max)
                {
                    XtraMessageBox.Show("Đã khoá sổ!");
                    return false;
                }
            }
            catch //chua co Khoa so thi Them Moi
            {
                var khoasochungtu2 = new khoasochungtu()
                {
                    iddv = txtdv.Text,
                    ngaykhoaso = DateTime.Parse("29/09/2017")
                };
                
                db.khoasochungtus.InsertOnSubmit(khoasochungtu2);
                db.SubmitChanges(); 
            }
            return true;
        }
    }
}