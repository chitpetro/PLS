﻿using System;
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

namespace GUI.Report.Nhap
{
    public partial class f_thcnncc : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        Boolean doubleclick1 = false;
        Boolean doubleclick2 = false;

        public f_thcnncc()
        {
            InitializeComponent();

            rTime.SetTime(thoigian);
        }


        private void f_chitietnhapkho_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "TỔNG HỢP CÔNG NỢ NHÀ CUNG CẤP").ToString();

            changeFont.Translate(this);

            //translate text
            labelControl1.Text = LanguageHelper.TranslateMsgString(".reportKhoangThoiGian", "Khoảng Thời Gian").ToString();
            labelControl2.Text = LanguageHelper.TranslateMsgString(".reportTuNgay", "Từ Ngày").ToString();
            labelControl3.Text = LanguageHelper.TranslateMsgString(".reportDenNgay", "Đến Ngày").ToString();
            labelControl6.Text = LanguageHelper.TranslateMsgString(".reportDanhMuc", "Danh Mục").ToString();

            tungay.ReadOnly = true;
            denngay.ReadOnly = true;

            danhmuc.Text = "Đơn vị - ຫົວໜ່ວຍ";
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
            if (danhmuc.Text == "Đơn vị - ຫົວໜ່ວຍ")
            {
                try
                {


                    var list = (from a in db.donvis

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
            else if (danhmuc.Text == "Đối Tượng - ເປົ້້າໝາຍ")
            {
                try
                {
                    var lst = (from a in db.doituongs select new { id = a.id, name = a.ten, key = a.id + danhmuc.Text + Biencucbo.idnv });
                    nguon.DataSource = lst;

                    for (int i = gridView1.RowCount - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < gridView2.DataRowCount; j++)
                        {
                            if (gridView1.GetRowCellValue(i, "key").ToString() == gridView2.GetRowCellValue(j, "key").ToString())
                            {
                                gridView1.DeleteRow(i);
                                break;
                            }
                        }
                    };
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
                if (danhmuc.Text == "Đơn vị - ຫົວໜ່ວຍ")
                {
                    var list = (from a in db.donvis

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
                else
                {
                    gridView1.DeleteSelectedRows();
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

                    if (danhmuc.Text == "Đơn vị - ຫົວໜ່ວຍ")
                    {
                        var list = (from a in db.donvis

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
                    else
                    {
                        gridView1.DeleteSelectedRows();
                    }


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
                if (danhmuc.Text == "Đơn vị - ຫົວໜ່ວຍ")
                {
                    try
                    {
                        var list = (from a in db.donvis

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
                else if (danhmuc.Text == "Đối Tượng - ເປົ້້າໝາຍ")
                {
                    try
                    {
                        var lst = (from a in db.doituongs select new { id = a.id, name = a.ten, key = a.id + danhmuc.Text + Biencucbo.idnv });
                        nguon.DataSource = lst;

                        for (int i = gridView1.RowCount - 1; i >= 0; i--)
                        {
                            for (int j = 0; j < gridView2.DataRowCount; j++)
                            {
                                if (gridView1.GetRowCellValue(i, "key").ToString() == gridView2.GetRowCellValue(j, "key").ToString())
                                {
                                    gridView1.DeleteRow(i);
                                    break;
                                }
                            }
                        };
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
            if (danhmuc.Text == "Đơn vị - ຫົວໜ່ວຍ")
            {
                try
                {
                    var list = (from a in db.donvis

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
            else if (danhmuc.Text == "Đối Tượng - ເປົ້້າໝາຍ")
            {
                try
                {
                    var lst = (from a in db.doituongs select new { id = a.id, name = a.ten, key = a.id + danhmuc.Text + Biencucbo.idnv });
                    nguon.DataSource = lst;

                    for (int i = gridView1.RowCount - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < gridView2.DataRowCount; j++)
                        {
                            if (gridView1.GetRowCellValue(i, "key").ToString() == gridView2.GetRowCellValue(j, "key").ToString())
                            {
                                gridView1.DeleteRow(i);
                                break;
                            }
                        }
                    };
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
                if (danhmuc.Text == "Đơn vị - ຫົວໜ່ວຍ")
                {
                    var list = (from a in db.donvis

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
                else
                {

                    for (int i = gridView1.RowCount; i > 0; i--)
                    {
                        gridView1.DeleteSelectedRows();
                    }
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
            if (danhmuc.Text == "Đơn vị - ຫົວໜ່ວຍ")
            {
                try
                {
                    var list = (from a in db.donvis

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
            else if (danhmuc.Text == "Đối Tượng - ເປົ້້າໝາຍ")
            {
                try
                {
                    var lst = (from a in db.doituongs select new { id = a.id, name = a.ten, key = a.id + danhmuc.Text + Biencucbo.idnv });
                    nguon.DataSource = lst;

                    for (int i = gridView1.RowCount - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < gridView2.DataRowCount; j++)
                        {
                            if (gridView1.GetRowCellValue(i, "key").ToString() == gridView2.GetRowCellValue(j, "key").ToString())
                            {
                                gridView1.DeleteRow(i);
                                break;
                            }
                        }
                    };
                }
                catch

                {

                }
            }

        }

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
                int check = 0;

                int check2 = 0;


                for (int i = 0; i < gridView2.DataRowCount; i++)
                {
                    if (gridView2.GetRowCellValue(i, "loai").ToString() == "Đơn vị - ຫົວໜ່ວຍ")
                    {
                        check++;
                        Biencucbo.kho = Biencucbo.kho + gridView2.GetRowCellValue(i, "id").ToString() + "-" + gridView2.GetRowCellValue(i, "name").ToString() + ", ";
                    }

                    else if (gridView2.GetRowCellValue(i, "loai").ToString() == "Đối Tượng - ເປົ້້າໝາຍ")
                    {
                        check2++;
                        Biencucbo.doituong = "  " + Biencucbo.doituong + gridView2.GetRowCellValue(i, "id").ToString() + "-" + gridView2.GetRowCellValue(i, "name").ToString() + ", ";
                    }

                }

                if (check == 0)
                {
                    Lotus.MsgBox.ShowWarningDialog("Cần phải chọn 1 trường dữ liệu bắt buộc: Đơn vị");
                    return;
                }
                else
                {
                    if (Biencucbo.ngonngu.ToString() == "Vietnam")
                    {
                        if (check2 == 0)
                        {
                            Biencucbo.doituong = "Tất cả";
                        }

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
                    }
                    else
                    {
                        if (check2 == 0)
                        {
                            Biencucbo.doituong = "ທັງໝົດ";
                        }

                        if (thoigian.Text == "ແລ້ວແຕ່")
                        {
                            Biencucbo.time = "ແຕ່: " + tungay.Text + " ເຖິງ: " + denngay.Text;
                        }
                        else if (thoigian.Text == "ໝົດປີ")
                        {
                            Biencucbo.time = thoigian.Text + " " + DateTime.Now.Year;
                        }
                        else
                        {
                            Biencucbo.time = thoigian.Text + ", ປີ " + DateTime.Now.Year;
                        }
                    }


                    var lst2 = from a in db.pnhaps
                               join b in db.dk_rps on a.iddv equals b.id
                               where a.ngaynhap >= tungay.DateTime && a.ngaynhap <= denngay.DateTime && a.loainhap == "Nhập mua - ຊື້ນຳເຂົ້າ"
                               && b.user == Biencucbo.idnv
                               select a;
                    var lst3 = lst2;
                    if (check2 != 0)
                    {
                        lst3 = from a in lst2
                               join b in db.dk_rps on a.iddt equals b.id
                               where b.user == Biencucbo.idnv
                               select a;
                    }
                    else
                    {
                        lst3 = from a in lst2 select a;
                    }
                    var lst6 = (from a in lst3
                                join b in db.pnhapcts on a.id equals b.idpnhap into g
                                from sub in g.DefaultIfEmpty()

                                select new
                                {
                                    iddt = a.iddt,
                                    ban = sub.thanhtien == null ? 0 : sub.thanhtien
                                }).GroupBy(t => t.iddt).Select(c => new { iddts = c.Key, ban = c.Sum(t => t.ban) });

                    var lst4 = from a in db.pchis
                               join b in db.dk_rps on a.iddv equals b.id
                               where a.ngaychi >= tungay.DateTime && a.ngaychi <= denngay.DateTime && a.loaichi == "Chi phí nhà cung cấp - ຄ່າໃຊ້ຈ່າຍຜູ້ສະໜອງ"
                               && b.user == Biencucbo.idnv
                               select a;
                    var lst5 = lst4;
                    if (check2 != 0)
                    {

                        lst5 = from a in lst4
                               join b in db.dk_rps on a.iddt equals b.id
                               where b.user == Biencucbo.idnv
                               select a;
                    }
                    else
                    {
                        lst5 = from a in lst4 select a;
                    }
                    var lst7 = (from a in lst5
                                join b in db.pchicts on a.id equals b.idchi into g
                                from sub in g.DefaultIfEmpty()

                                select new
                                {
                                    iddt = a.iddt,
                                    thu = sub.sotien == null ? 0 : sub.sotien
                                }).GroupBy(t => t.iddt).Select(c => new { iddts = c.Key, thu = c.Sum(t => t.thu) });






                    lst2 = from a in db.pnhaps
                           join b in db.dk_rps on a.iddv equals b.id
                           where a.ngaynhap < tungay.DateTime && a.loainhap == "Nhập mua - ຊື້ນຳເຂົ້າ"
                           && b.user == Biencucbo.idnv
                           select a;

                    if (check2 != 0)
                    {
                        lst3 = from a in lst2
                               join b in db.dk_rps on a.iddt equals b.id
                                where b.user == Biencucbo.idnv
                               select a;
                    }
                    else
                    {
                        lst3 = from a in lst2 select a;
                    }
                    var lst6a = (from a in lst3
                                 join b in db.pnhapcts on a.id equals b.idpnhap into g
                                 from sub in g.DefaultIfEmpty()

                                 select new
                                 {
                                     iddt = a.iddt,
                                     ban = sub.thanhtien == null ? 0 : sub.thanhtien
                                 }).GroupBy(t => t.iddt).Select(c => new { iddts = c.Key, ban = c.Sum(t => t.ban) });

                    lst4 = from a in db.pchis
                           join b in db.dk_rps on a.iddv equals b.id
                           where a.ngaychi < tungay.DateTime && a.loaichi == "Chi phí nhà cung cấp - ຄ່າໃຊ້ຈ່າຍຜູ້ສະໜອງ"
                            && b.user == Biencucbo.idnv
                           select a;

                    if (check2 != 0)
                    {

                        lst5 = from a in lst4
                               join b in db.dk_rps on a.iddt equals b.id
                                where b.user == Biencucbo.idnv
                               select a;
                    }
                    else
                    {
                        lst5 = from a in lst4 select a;
                    }
                    var lst7a = (from a in lst5
                                 join b in db.pchicts on a.id equals b.idchi into g
                                 from sub in g.DefaultIfEmpty()

                                 select new
                                 {
                                     iddt = a.iddt,
                                     thu = sub.sotien == null ? 0 : sub.sotien
                                 }).GroupBy(t => t.iddt).Select(c => new { iddts = c.Key, thu = c.Sum(t => t.thu) });
                    var lst = from a in db.doituongs
                              join b in lst6 on a.id equals b.iddts into h
                              join c in lst7 on a.id equals c.iddts into k
                              join d in lst6a on a.id equals d.iddts into l
                              join f in lst7a on a.id equals f.iddts into m
                              from xuat in h.DefaultIfEmpty()
                              from thu in k.DefaultIfEmpty()
                              from xuatdau in l.DefaultIfEmpty()
                              from thudau in m.DefaultIfEmpty()
                              select new
                              {

                                  id = a.id,
                                  ten = a.ten,
                                  nodau = (xuatdau.ban == null ? 0 : xuatdau.ban) - (thudau.thu == null ? 0 : thudau.thu),

                                  ban = xuat.ban == null ? 0 : xuat.ban,
                                  thu = thu.thu == null ? 0 : thu.thu,

                                  notrongky = (xuat.ban == null ? 0 : xuat.ban) - (thu.thu == null ? 0 : thu.thu),
                                  nocuoiky = ((xuatdau.ban == null ? 0 : xuatdau.ban) - (thudau.thu == null ? 0 : thudau.thu)) + ((xuat.ban == null ? 0 : xuat.ban) - (thu.thu == null ? 0 : thu.thu)),
                              };
                    var lst8 = lst;

                    if (check2 != 0)
                    {

                        lst8 = from a in lst
                               join b in db.dk_rps on a.id equals b.id
                                where b.user == Biencucbo.idnv
                               select a;
                    }
                    else
                    {
                        lst8 = from a in lst select a;
                    }
                    var lst9 = from a in lst8 where a.nodau != 0 || a.notrongky != 0 || a.nocuoiky != 0 select a;
                    r_thcnncc xtra = new r_thcnncc();
                    xtra.DataSource = lst9;
                    xtra.ShowPreviewDialog();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            SplashScreenManager.CloseForm(false);
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