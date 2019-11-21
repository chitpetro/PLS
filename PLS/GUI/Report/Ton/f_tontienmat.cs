using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DevExpress.XtraReports.UI;
using BUS;
using ControlLocalizer;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;

namespace GUI.Report.Ton
{
    public partial class f_tontienmat : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        Boolean doubleclick1 = false;
        Boolean doubleclick2 = false;

        public f_tontienmat()
        {
            InitializeComponent();

            rTime.SetTime(thoigian);
        }


        private void f_chitietnhapkho_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "TỔNG HỢP TỒN TIỀN MẶT").ToString();

            changeFont.Translate(this);

            //translate text
            labelControl1.Text = LanguageHelper.TranslateMsgString(".reportKhoangThoiGian", "Khoảng Thời Gian").ToString();
            labelControl2.Text = LanguageHelper.TranslateMsgString(".reportTuNgay", "Từ Ngày").ToString();
            labelControl3.Text = LanguageHelper.TranslateMsgString(".reportDenNgay", "Đến Ngày").ToString();
            labelControl6.Text = LanguageHelper.TranslateMsgString(".reportDanhMuc", "Danh Mục").ToString();

            tungay.ReadOnly = true;
            denngay.ReadOnly = true;

            danhmuc.Text = "Chi Nhánh";
            rTime.SetTime2(thoigian);
            var lst = from a in db.dk_rps where a.user == Biencucbo.idnv select a;
            db.dk_rps.DeleteAllOnSubmit(lst);
            db.SubmitChanges();
            nhan.DataSource = lst;
        }
        private string LayMaTim(donvi d)
        {
            string s = "." + d.id + "." + d.iddv + ".";
            var find = db.donvis.FirstOrDefault(t => t.id == d.iddv);

            if (find != null)
            {

                string iddv = find.iddv;
                if (d.id != find.iddv)
                {
                    if (!s.Contains(iddv))
                        s += iddv + ".";
                }
                while (iddv != find.id)
                {
                    if (!s.Contains(find.id))
                        s += find.id + ".";
                    find = db.donvis.FirstOrDefault(t => t.id == find.iddv);
                }
            }
            return s;
        }

        private void thoigian_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeTime.thoigian_change3(thoigian, tungay, denngay);
        }

        private void danhmuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (danhmuc.Text == "Chi Nhánh")
            {
                try
                {
                    var list = (from a in db.donvis
                                where a.id.Length == 2 && a.id != "00"
                                select new
                                {
                                    id = a.id,
                                    name = a.tendonvi,
                                    key = a.id + danhmuc.Text + Biencucbo.idnv,
                                    MaTim = LayMaTim(a)
                                });
                    var lst2 = list.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        var lst3 = from a in lst2
                                   where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                   select a;
                        lst2 = lst3.ToList();
                    };
                    nguon.DataSource = lst2;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message);
                }
            }
            else if (danhmuc.Text == "Cửa Hàng")
            {
                try
                {
                    var list = (from a in db.donvis
                                where a.id.Length > 2
                                select new
                                {
                                    id = a.id,
                                    name = a.tendonvi,
                                    key = a.id + danhmuc.Text + Biencucbo.idnv,
                                    MaTim = LayMaTim(a)
                                });
                    var lst2 = list.ToList();

                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        var lst3 = from a in lst2
                                   where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                   select a;
                        lst2 = lst3.ToList();
                    };
                    nguon.DataSource = lst2;
                }
                catch
                {
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (danhmuc.Text == "Chi Nhánh")
                {
                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        if (gridView2.GetRowCellValue(j, "loai").ToString() == "Cửa Hàng")
                        {
                            MessageBox.Show("Đã chọn Cửa Hàng, Không được chọn Chi Nhánh", "THÔNG BÁO");

                            return;
                        }
                    }
                }
                else if (danhmuc.Text == "Cửa Hàng")
                {
                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        if (gridView2.GetRowCellValue(j, "loai").ToString() == "Chi Nhánh")
                        {

                            MessageBox.Show("Đã chọn Chi Nhánh, Không được chọn cửa hàng", "THÔNG BÁO");
                            return;
                        }
                    }
                }

                dk_rp dk = new dk_rp();
                dk.id = gridView1.GetFocusedRowCellValue("id").ToString();
                dk.name = gridView1.GetFocusedRowCellValue("name").ToString();
                dk.key = gridView1.GetFocusedRowCellValue("key").ToString();
                dk.loai = danhmuc.Text;
                dk.user = Biencucbo.idnv;
                db.dk_rps.InsertOnSubmit(dk);
                db.SubmitChanges();
                var lst = from a in db.dk_rps where a.user == Biencucbo.idnv select a;
                nhan.DataSource = lst;
                if (danhmuc.Text == "Chi Nhánh")
                {

                    var list = (from a in db.donvis
                                where a.id.Length == 2 && a.id != "00"
                                select new
                                {
                                    id = a.id,
                                    name = a.tendonvi,
                                    key = a.id + danhmuc.Text + Biencucbo.idnv,
                                    MaTim = LayMaTim(a)
                                });
                    var lst2 = list.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {

                        var lst3 = from a in lst2
                                   where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                   select a;
                        lst2 = lst3.ToList();
                    };
                    nguon.DataSource = lst2;
                }
                else if (danhmuc.Text == "Cửa Hàng")
                {
                    var list = (from a in db.donvis
                                where a.id.Length > 2
                                select new
                                {
                                    id = a.id,
                                    name = a.tendonvi,
                                    key = a.id + danhmuc.Text + Biencucbo.idnv,
                                    MaTim = LayMaTim(a)
                                });
                    var lst2 = list.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        var lst3 = from a in lst2
                                   where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                   select a;
                        lst2 = lst3.ToList();
                    };
                    nguon.DataSource = lst2;
                }

            }
            catch
            {
            }
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            doubleclick1 = false;
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (doubleclick1 == true)
            {

                try
                {
                    simpleButton1_Click(sender, e);
                }
                catch
                {
                }
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            doubleclick1 = true;
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            doubleclick2 = true;
        }

        private void gridView2_Click(object sender, EventArgs e)
        {
            doubleclick2 = false;
        }

        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (doubleclick2 == true)
            {
                try
                {
                    dk_rp dk = new dk_rp();
                    var lst = (from a in db.dk_rps where a.user == Biencucbo.idnv select a).Single(t => t.key == gridView2.GetFocusedRowCellValue("key").ToString());
                    db.dk_rps.DeleteOnSubmit(lst);
                    db.SubmitChanges();
                    var lst2 = from a in db.dk_rps where a.user == Biencucbo.idnv select a;
                    nhan.DataSource = lst2;

                }
                catch
                {
                }
                if (danhmuc.Text == "Chi Nhánh")
                {
                    try
                    {
                        var list = (from a in db.donvis
                                    where a.id.Length == 2 && a.id != "00"
                                    select new
                                    {
                                        id = a.id,
                                        name = a.tendonvi,
                                        key = a.id + danhmuc.Text + Biencucbo.idnv,
                                        MaTim = LayMaTim(a)
                                    });
                        var lst2 = list.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                        for (int j = 0; j < gridView2.DataRowCount; j++)
                        {
                            var lst3 = from a in lst2
                                       where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                       select a;
                            lst2 = lst3.ToList();
                        };
                        nguon.DataSource = lst2;
                    }
                    catch
                    {
                    }
                }
                else if (danhmuc.Text == "Cửa Hàng")
                {
                    try
                    {
                        var list = (from a in db.donvis
                                    where a.id.Length > 2
                                    select new
                                    {
                                        id = a.id,
                                        name = a.tendonvi,
                                        key = a.id + danhmuc.Text + Biencucbo.idnv,
                                        MaTim = LayMaTim(a)
                                    });
                        var lst2 = list.ToList();//.Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                        for (int j = 0; j < gridView2.DataRowCount; j++)
                        {
                            var lst3 = from a in lst2
                                       where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                       select a;
                            lst2 = lst3.ToList();
                        };
                        nguon.DataSource = lst2;
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                dk_rp dk = new dk_rp();
                var lst = (from a in db.dk_rps where a.user == Biencucbo.idnv select a).Single(t => t.key == gridView2.GetFocusedRowCellValue("key").ToString());
                db.dk_rps.DeleteOnSubmit(lst);
                db.SubmitChanges();
                var lst2 = from a in db.dk_rps where a.user == Biencucbo.idnv select a;
                nhan.DataSource = lst2;
            }
            catch
            {

            }
            if (danhmuc.Text == "Chi Nhánh")
            {
                try
                {
                    var list = (from a in db.donvis
                                where a.id.Length == 2 && a.id != "00"
                                select new
                                {
                                    id = a.id,
                                    name = a.tendonvi,
                                    key = a.id + danhmuc.Text + Biencucbo.idnv,
                                    MaTim = LayMaTim(a)
                                });
                    var lst2 = list.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        var lst3 = from a in lst2
                                   where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                   select a;
                        lst2 = lst3.ToList();
                    };
                    nguon.DataSource = lst2;
                }
                catch

                {

                }
            }
            else if (danhmuc.Text == "Cửa Hàng")
            {
                try
                {
                    var list = (from a in db.donvis
                                where a.id.Length > 2
                                select new
                                {
                                    id = a.id,
                                    name = a.tendonvi,
                                    key = a.id + danhmuc.Text + Biencucbo.idnv,
                                    MaTim = LayMaTim(a)
                                });
                    var lst2 = list.ToList();//.Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        var lst3 = from a in lst2
                                   where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                   select a;
                        lst2 = lst3.ToList();
                    };
                    nguon.DataSource = lst2;
                }
                catch
                {
                }
            }


        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (danhmuc.Text == "Chi Nhánh")
                {
                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        if (gridView2.GetRowCellValue(j, "loai").ToString() == "Cửa Hàng")
                        {
                            MessageBox.Show("Đã chọn Cửa Hàng, Không được chọn Chi Nhánh", "THÔNG BÁO");

                            return;
                        }
                    }
                }
                else if (danhmuc.Text == "Cửa Hàng")
                {
                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        if (gridView2.GetRowCellValue(j, "loai").ToString() == "Chi Nhánh")
                        {
                            MessageBox.Show("Đã chọn Chi Nhánh, Không được chọn cửa hàng", "THÔNG BÁO");
                            return;
                        }
                    }
                }


                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    dk_rp dk = new dk_rp();
                    dk.id = gridView1.GetRowCellValue(i, "id").ToString();
                    dk.name = gridView1.GetRowCellValue(i, "name").ToString();
                    dk.key = gridView1.GetRowCellValue(i, "key").ToString();
                    dk.loai = danhmuc.Text;
                    dk.user = Biencucbo.idnv;
                    db.dk_rps.InsertOnSubmit(dk);
                    db.SubmitChanges();
                    var lst = from a in db.dk_rps where a.user == Biencucbo.idnv select a;
                    nhan.DataSource = lst;
                }
                if (danhmuc.Text == "Chi Nhánh")
                {
                    var list = (from a in db.donvis
                                where a.id.Length == 2 && a.id != "00"
                                select new
                                {
                                    id = a.id,
                                    name = a.tendonvi,
                                    key = a.id + danhmuc.Text + Biencucbo.idnv,
                                    MaTim = LayMaTim(a)
                                });
                    var lst2 = list.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        var lst3 = from a in lst2
                                   where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                   select a;
                        lst2 = lst3.ToList();
                    };
                    nguon.DataSource = lst2;
                }
                else if (danhmuc.Text == "Cửa Hàng")
                {
                    var list = (from a in db.donvis
                                where a.id.Length > 2
                                select new
                                {
                                    id = a.id,
                                    name = a.tendonvi,
                                    key = a.id + danhmuc.Text + Biencucbo.idnv,
                                    MaTim = LayMaTim(a)
                                });
                    var lst2 = list.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        var lst3 = from a in lst2
                                   where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                   select a;
                        lst2 = lst3.ToList();
                    };
                    nguon.DataSource = lst2;
                }
            }
            catch
            {
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                var lst = (from a in db.dk_rps where a.user == Biencucbo.idnv select a);
                db.dk_rps.DeleteAllOnSubmit(lst);
                db.SubmitChanges();
                nhan.DataSource = lst;
            }
            catch
            {

            }
            if (danhmuc.Text == "Chi Nhánh")
            {
                try
                {
                    var list = (from a in db.donvis
                                where a.id.Length == 2 && a.id != "00"
                                select new
                                {
                                    id = a.id,
                                    name = a.tendonvi,
                                    key = a.id + danhmuc.Text + Biencucbo.idnv,
                                    MaTim = LayMaTim(a)
                                });
                    var lst2 = list.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        var lst3 = from a in lst2
                                   where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                   select a;
                        lst2 = lst3.ToList();
                    };
                    nguon.DataSource = lst2;
                }
                catch
                {
                }
            }
            else if (danhmuc.Text == "Cửa Hàng")
            {
                try
                {
                    var list = (from a in db.donvis
                                where a.id.Length > 2
                                select new
                                {
                                    id = a.id,
                                    name = a.tendonvi,
                                    key = a.id + danhmuc.Text + Biencucbo.idnv,
                                    MaTim = LayMaTim(a)
                                });
                    var lst2 = list.ToList();//.Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                    for (int j = 0; j < gridView2.DataRowCount; j++)
                    {
                        var lst3 = from a in lst2
                                   where a.key != gridView2.GetRowCellValue(j, "key").ToString()
                                   select a;
                        lst2 = lst3.ToList();
                    };
                    nguon.DataSource = lst2;
                }
                catch
                {
                }
            }
        }

        public string laytendv(string f)
        {
            string b = "";
            try
            {
                var lst = (from a in db.donvis select a).Single(t => t.id == f);
                b = lst.tendonvi;
            }
            catch
            { }
            return b;
        }

        //private void simpleButton5_Click(object sender, EventArgs e)
        //{
        //    SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
        //    try
        //    {
        //        reset db
        //        db.CommandTimeout = 0;

        //        Biencucbo.loai = "";
        //        Biencucbo.doituong = "";
        //        Biencucbo.congviec = "";
        //        Biencucbo.muccp = "";
        //        Biencucbo.kho = "";
        //        int checkch = 0;

        //        int checkcn = 0;


        //        if (thoigian.Text == "Tùy ý")
        //        {
        //            Biencucbo.time = "Từ ngày: " + tungay.Text + " Đến ngày: " + denngay.Text;
        //        }
        //        else if (thoigian.Text == "Cả Năm")
        //        {
        //            Biencucbo.time = thoigian.Text + " " + DateTime.Now.Year;
        //        }
        //        else
        //        {
        //            Biencucbo.time = thoigian.Text + ", năm " + DateTime.Now.Year;
        //        }

        //        for (int i = 0; i < gridView2.DataRowCount; i++)
        //        {
        //            if (gridView2.GetRowCellValue(i, "loai").ToString() == "Chi Nhánh")
        //            {
        //                checkcn++;
        //                Biencucbo.kho = Biencucbo.kho + gridView2.GetRowCellValue(i, "id").ToString() + "-" + gridView2.GetRowCellValue(i, "name").ToString() + ", ";
        //            }

        //            else if (gridView2.GetRowCellValue(i, "loai").ToString() == "Cửa Hàng")
        //            {
        //                checkch++;
        //                Biencucbo.doituong = "  " + Biencucbo.doituong + gridView2.GetRowCellValue(i, "id").ToString() + "-" + gridView2.GetRowCellValue(i, "name").ToString() + ", ";
        //            }
        //        }


        //        var lstTThu = (from a in db.pthus
        //                       join b in db.pthucts on a.id equals b.idthu into g
        //                       from sub in g.DefaultIfEmpty()
        //                       where a.ngaythu < tungay.DateTime //&& a.iddv == gridView2.GetRowCellValue(i, "id").ToString() //&& a.iddv.Length > 2
        //                       select new
        //                       {
        //                           a.iddv,
        //                           a.ngaythu,
        //                           tt = sub.thanhtien == null ? 0 : sub.thanhtien
        //                       }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, ttien = c.Sum(t => t.tt) });

        //        var lstTChi = (from a in db.pchis
        //                       join b in db.pchicts on a.id equals b.idchi into g
        //                       from sub in g.DefaultIfEmpty()
        //                       where a.ngaychi < tungay.DateTime //&& a.iddv == gridView2.GetRowCellValue(i, "id").ToString()//&& a.iddv.Length > 2
        //                       select new
        //                       {
        //                           a.iddv,
        //                           tt = sub.sotien == null ? 0 : sub.sotien
        //                       }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, ttien = c.Sum(t => t.tt) });


        //        var tondauki = (from a in db.donvis
        //                        join b in lstTThu on a.id equals b.iddv into k
        //                        join c in lstTChi on a.id equals c.iddv into l
        //                        from thud in k.DefaultIfEmpty()
        //                        from chid in l.DefaultIfEmpty()
        //                        select new
        //                        {
        //                            iddv = a.id,

        //                            tt = (thud.ttien == null ? 0 : thud.ttien) - (chid.ttien == null ? 0 : chid.ttien),
        //                        });

        //        thu trong ky
        //       var thu = (from a in db.donvis
        //                  join b in db.r_pthus on a.id equals b.iddv into g
        //                  from thutk in g.DefaultIfEmpty()
        //                  where thutk.ngaythu >= tungay.DateTime && thutk.ngaythu <= denngay.DateTime //&& a.iddv == gridView2.GetRowCellValue(i, "id").ToString()//&& a.id.Length > 2
        //                   select new
        //                  {
        //                      iddv = a.id,

        //                      gtthu = thutk.thanhtien,

        //                  }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, gt = c.Sum(t => t.gtthu) });

        //        chi trong ky
        //       var chi = (from a in db.donvis
        //                  join b in db.r_pchis on a.id equals b.iddv into g
        //                  from chitk in g.DefaultIfEmpty()
        //                  where chitk.ngaychi >= tungay.DateTime && chitk.ngaychi <= denngay.DateTime //&& a.iddv == gridView2.GetRowCellValue(i, "id").ToString()//&& a.id.Length > 2
        //                   select new
        //                  {
        //                      iddv = a.id,

        //                      gtchi = chitk.sotien,

        //                  }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, gt = c.Sum(t => t.gtchi) });

        //        if (check == 1)
        //        {
        //            var lst3 = from a in db.r_chotct_ngaymaxes select a;



        //            var lst1 = (from a in db.donvis
        //                        join b in tondauki on a.id equals b.iddv into k
        //                        join c in thu on a.id equals c.iddv into l
        //                        join d in chi on a.id equals d.iddv into m
        //                        join f in lst3 on a.id equals f.iddv into f
        //                        where a.id.Length > 2
        //                        from ba in k.DefaultIfEmpty()
        //                        from ca in l.DefaultIfEmpty()
        //                        from da in m.DefaultIfEmpty()
        //                        from fa in f.DefaultIfEmpty()

        //                        select new
        //                        {
        //                            a.id,
        //                            iddv = a.id + "-" + a.tendonvi,
        //                            tendonvi = a.tendonvi,
        //                            idcn = laytendv(a.iddv),
        //                            cn = a.iddv,
        //                            tondauki = ba.tt == null ? 0 : ba.tt,
        //                            thutk = ca.gt == null ? 0 : ca.gt,
        //                            chitk = da.gt == null ? 0 : da.gt,
        //                            toncuoiki = (ba.tt == null ? 0 : ba.tt) + (ca.gt == null ? 0 : ca.gt) - (da.gt == null ? 0 : da.gt),
        //                            ngaychuyen = layngaychuyentien(a.id),
        //                            MaTim = LayMaTim(a),
        //                            ngaycuoi = layngaymoinhat(a.id),
        //                        }).ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));
        //            var lst2 = lst1;

        //            if (checkch != 0)
        //            {

        //                lst2 = from a in lst1
        //                       join b in db.dk_rps on a.id equals b.id
        //                       where b.user == Biencucbo.idnv
        //                       select a;
        //            }

        //            lst1 = lst2;
        //            if (checkcn != 0)
        //            {
        //                lst1 = from a in lst2
        //                       join b in db.dk_rps on a.cn equals b.id
        //                       where b.user == Biencucbo.idnv
        //                       select a;
        //            }

        //            r_tontien xtra = new r_tontien();
        //            xtra.DataSource = lst1;
        //            xtra.ShowPreviewDialog();
        //        }
        //    catch (Exception ex)
        //    {
        //        XtraMessageBox.Show(ex.Message);
        //    }
        //    SplashScreenManager.CloseForm(false);
        //}


        private void simpleButton5_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
            try
            {
                //reset db
                db.CommandTimeout = 0;

                Biencucbo.loai = "";
                Biencucbo.doituong = "";
                Biencucbo.congviec = "";
                Biencucbo.muccp = "";
                Biencucbo.kho = "";
                int checkch = 0;

                int checkcn = 0;


                if (thoigian.Text == "Tùy ý")
                {
                    Biencucbo.time = "Từ ngày: " + tungay.Text + " Đến ngày: " + denngay.Text;
                }
                else if (thoigian.Text == "Cả Năm")
                {
                    Biencucbo.time = thoigian.Text + " " + DateTime.Now.Year;
                }
                else
                {
                    Biencucbo.time = thoigian.Text + ", năm " + DateTime.Now.Year;
                }

                for (int i = 0; i < gridView2.DataRowCount; i++)
                {
                    if (gridView2.GetRowCellValue(i, "loai").ToString() == "Chi Nhánh")
                    {
                        checkcn++;
                        //Biencucbo.kho = Biencucbo.kho + gridView2.GetRowCellValue(i, "id").ToString() + "-" + gridView2.GetRowCellValue(i, "name").ToString() + ", ";
                    }

                    else if (gridView2.GetRowCellValue(i, "loai").ToString() == "Cửa Hàng")
                    {
                        checkch++;
                        //Biencucbo.doituong = "  " + Biencucbo.doituong + gridView2.GetRowCellValue(i, "id").ToString() + "-" + gridView2.GetRowCellValue(i, "name").ToString() + ", ";
                    }
                }


                var lstTThu = (from a in db.pthus
                               join b in db.pthucts on a.id equals b.idthu into g
                               from sub in g.DefaultIfEmpty()
                               where a.ngaythu < tungay.DateTime //&& a.iddv == gridView2.GetRowCellValue(i, "id").ToString() //&& a.iddv.Length > 2
                               select new
                               {
                                   a.iddv,
                                   a.ngaythu,
                                   tt = sub.thanhtien == null ? 0 : sub.thanhtien
                               }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, ttien = c.Sum(t => t.tt) });

                var lstTChi = (from a in db.pchis
                               join b in db.pchicts on a.id equals b.idchi into g
                               from sub in g.DefaultIfEmpty()
                               where a.ngaychi < tungay.DateTime //&& a.iddv == gridView2.GetRowCellValue(i, "id").ToString()//&& a.iddv.Length > 2
                               select new
                               {
                                   a.iddv,
                                   tt = sub.sotien == null ? 0 : sub.sotien
                               }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, ttien = c.Sum(t => t.tt) });


                var tondauki = (from a in db.donvis
                                join b in lstTThu on a.id equals b.iddv into k
                                join c in lstTChi on a.id equals c.iddv into l
                                from thud in k.DefaultIfEmpty()
                                from chid in l.DefaultIfEmpty()
                                select new
                                {
                                    iddv = a.id,

                                    tt = (thud.ttien == null ? 0 : thud.ttien) - (chid.ttien == null ? 0 : chid.ttien),
                                });

                // thu trong ky
                var thu = (from a in db.donvis
                           join b in db.r_pthus on a.id equals b.iddv into g
                           from thutk in g.DefaultIfEmpty()
                           where thutk.ngaythu >= tungay.DateTime && thutk.ngaythu <= denngay.DateTime //&& a.iddv == gridView2.GetRowCellValue(i, "id").ToString()//&& a.id.Length > 2
                           select new
                           {
                               iddv = a.id,

                               gtthu = thutk.thanhtien,

                           }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, gt = c.Sum(t => t.gtthu) });

                // chi trong ky
                var chi = (from a in db.donvis
                           join b in db.r_pchis on a.id equals b.iddv into g
                           from chitk in g.DefaultIfEmpty()
                           where chitk.ngaychi >= tungay.DateTime && chitk.ngaychi <= denngay.DateTime //&& a.iddv == gridView2.GetRowCellValue(i, "id").ToString()//&& a.id.Length > 2
                           select new
                           {
                               iddv = a.id,

                               gtchi = chitk.sotien,

                           }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, gt = c.Sum(t => t.gtchi) });

                //if (check == 1)
                //{
                //var lst3 = from a in db.r_chotct_ngaymaxes select a;



                var lst1 = (from a in db.donvis
                            join b in tondauki on a.id equals b.iddv into k
                            join c in thu on a.id equals c.iddv into l
                            join d in chi on a.id equals d.iddv into m
                            //join f in lst3 on a.id equals f.iddv into f
                            where a.id.Length > 2
                            from ba in k.DefaultIfEmpty()
                            from ca in l.DefaultIfEmpty()
                            from da in m.DefaultIfEmpty()
                                //from fa in f.DefaultIfEmpty()

                            select new
                            {
                                a.id,
                                iddv = a.id + "-" + a.tendonvi,
                                tendonvi = a.tendonvi,
                                idcn = laytendv(a.iddv),
                                cn = a.iddv,
                                tondauki = ba.tt == null ? 0 : ba.tt,
                                thutk = ca.gt == null ? 0 : ca.gt,
                                chitk = da.gt == null ? 0 : da.gt,
                                toncuoiki = (ba.tt == null ? 0 : ba.tt) + (ca.gt == null ? 0 : ca.gt) - (da.gt == null ? 0 : da.gt),
                                ngaychuyen = layngaychuyentien(a.id),
                                // MaTim = LayMaTim(a),
                                ngaycuoi = layngaymoinhat(a.id),
                            });//.ToList();//.Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));
                var lst2 = lst1;
                
                if (checkch != 0)
                {
                    lst2 = from a in lst1
                           join b in db.dk_rps on a.id equals b.id
                           where b.user == Biencucbo.idnv
                           select a;
                }

                lst1 = lst2;
                if (checkcn != 0)
                {
                    lst1 = from a in lst2
                           join b in db.dk_rps on a.cn equals b.id
                           where b.user == Biencucbo.idnv
                           select a;
                }

                r_tontien xtra = new r_tontien();
                xtra.DataSource = lst1;
                xtra.ShowPreviewDialog();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            SplashScreenManager.CloseForm(false);
        }

        public DateTime layngaychuyentien(string iddv)
        {
            DateTime ab = DateTime.Parse("01/01/2018");
            try
            {
                var lst = (from a in db.pchis where a.iddv == iddv && a.loaichi == "Nộp Tiền Chi Nhánh - ມອບເງິນໃຫ້ສາຂາ" && a.ngaychi <= denngay.DateTime select a).Max(t => t.ngaychi);
                ab = DateTime.Parse(lst.ToString());
            }
            catch
            {

            }
            return ab;
        }
        public DateTime layngaymoinhat(string iddv)
        {
            DateTime ab = DateTime.Parse("01/01/2018");
            try
            {
                var lst = (from a in db.r_chotcongtos where a.iddv == iddv select a).Max(t => t.ngay);
                ab = DateTime.Parse(lst.ToString());
            }
            catch
            {

            }
            return ab;
        }
        public int key = 1;
        private int stt(DateTime? a)
        {
            int b = 0;
            b = key;
            key++;
            return b;
        }
    }
}
