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

namespace GUI.Report.Nhap
{
    public partial class f_chitietnhapxuat : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        Boolean doubleclick1 = false;
        Boolean doubleclick2 = false;

        public f_chitietnhapxuat()
        {
            InitializeComponent();

            rTime.SetTime(thoigian);
        }

        private void f_chitietnhapkho_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "SỔ CHI TIẾT NHẬP XUẤT TỒN").ToString();

            changeFont.Translate(this);
             
            //translate text
            labelControl1.Text = LanguageHelper.TranslateMsgString(".reportKhoangThoiGian", "Khoảng Thời Gian").ToString();
            labelControl2.Text = LanguageHelper.TranslateMsgString(".reportTuNgay", "Từ Ngày").ToString();
            labelControl3.Text = LanguageHelper.TranslateMsgString(".reportDenNgay", "Đến Ngày").ToString();
            labelControl6.Text = LanguageHelper.TranslateMsgString(".reportDanhMuc", "Danh Mục").ToString();

            tungay.ReadOnly = true;
            denngay.ReadOnly = true;

            danhmuc.Text = "Kho - ສາງ";
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
            if (danhmuc.Text == "Hàng Hóa - ສິນຄ້າ")
            {
                try
                {
                    var lst = (from a in db.r_giasps where a.iddv == Biencucbo.donvi select new { id = a.idsp, name = a.tensp, key = a.id + danhmuc.Text + Biencucbo.idnv });

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
            else if (danhmuc.Text == "Kho - ສາງ")
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

        }
        public int test = 0;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (danhmuc.Text == "Hàng Hóa - ສິນຄ້າ")

                {
                    test = 0;
                    for (int i = 0; i < gridView2.DataRowCount; i++)
                    {

                        if (gridView2.GetRowCellValue(i, "loai").ToString() == "Hàng Hóa - ສິນຄ້າ")
                        {
                            test++;
                            if (test >= 1)
                            {
                                Lotus.MsgBox.ShowWarningDialog("Chỉ được chọn 1 trường Sản phẩm duy nhất");
                                return;
                            }
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
                if (danhmuc.Text == "Kho - ສາງ")
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
                    if (danhmuc.Text == "Hàng Hóa - ສິນຄ້າ")

                    {
                        test = 0;

                        for (int i = 0; i < gridView2.DataRowCount; i++)
                        {

                            if (gridView2.GetRowCellValue(i, "loai").ToString() == "Hàng Hóa - ສິນຄ້າ")
                            {
                                test++;
                                if (test >= 1)
                                {
                                    Lotus.MsgBox.ShowWarningDialog("Chỉ được chọn 1 trường Sản phẩm duy nhất");
                                    return;
                                }
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
                    if (danhmuc.Text == "Kho - ສາງ")
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
                    var lst = (from a in db.dk_rps where a.user==Biencucbo.idnv select a).Single(t => t.key == gridView2.GetFocusedRowCellValue("key").ToString());
                    db.dk_rps.DeleteOnSubmit(lst);
                    db.SubmitChanges();
                    var lst2 = from a in db.dk_rps where a.user == Biencucbo.idnv select a;
                    nhan.DataSource = lst2;

                }
                catch
                {

                }
                if (danhmuc.Text == "Hàng Hóa - ສິນຄ້າ")
                {
                    try
                    {
                        var lst = (from a in db.r_giasps where a.iddv == Biencucbo.donvi select new { id = a.idsp, name = a.tensp, key = a.id + danhmuc.Text + Biencucbo.idnv });

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
                else if (danhmuc.Text == "Kho - ສາງ")
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


            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                dk_rp dk = new dk_rp();
                var lst = (from a in db.dk_rps where a.user==Biencucbo.idnv select a).Single(t => t.key == gridView2.GetFocusedRowCellValue("key").ToString());
                db.dk_rps.DeleteOnSubmit(lst);
                db.SubmitChanges();
                var lst2 = from a in db.dk_rps where a.user == Biencucbo.idnv select a;
                nhan.DataSource = lst2;

            }
            catch
            {

            }
            if (danhmuc.Text == "Hàng Hóa - ສິນຄ້າ")
            {
                try
                {
                    var lst = (from a in db.r_giasps where a.iddv == Biencucbo.donvi select new { id = a.idsp, name = a.tensp, key = a.id + danhmuc.Text + Biencucbo.idnv });

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
            else if (danhmuc.Text == "Kho - ສາງ")
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
                if (danhmuc.Text == "Kho - ສາງ")
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
                var lst = from a in db.dk_rps where a.user == Biencucbo.idnv select a;
                db.dk_rps.DeleteAllOnSubmit(lst);
                db.SubmitChanges();
                nhan.DataSource = lst;

            }
            catch
            {

            }
            if (danhmuc.Text == "Hàng Hóa - ສິນຄ້າ")
            {
                try
                {
                    var lst = (from a in db.r_giasps where a.iddv == Biencucbo.donvi select new { id = a.idsp, name = a.tensp, key = a.id + danhmuc.Text + Biencucbo.idnv });

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
            else if (danhmuc.Text == "Kho - ສາງ")
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

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
            try
            {
                //reset db
                db.CommandTimeout = 0;

                Biencucbo.sp = "";
                Biencucbo.doituong = "";
                Biencucbo.congviec = "";
                Biencucbo.kho = "";
                int check = 0;
                int check1 = 0;
                int check2 = 0;
                int check3 = 0;

                for (int i = 0; i < gridView2.DataRowCount; i++)
                {
                    if (gridView2.GetRowCellValue(i, "loai").ToString() == "Kho - ສາງ")
                    {
                        check++;
                        Biencucbo.kho = Biencucbo.kho + gridView2.GetRowCellValue(i, "id").ToString() + "-" + gridView2.GetRowCellValue(i, "name").ToString() + ", ";
                    }
                    else if (gridView2.GetRowCellValue(i, "loai").ToString() == "Hàng Hóa - ສິນຄ້າ")
                    {
                        check1++;
                        Biencucbo.sp = "  " + Biencucbo.sp + gridView2.GetRowCellValue(i, "id").ToString() + "-" + gridView2.GetRowCellValue(i, "name").ToString() + ", ";
                    }

                }

                if (check == 0)
                {
                    Lotus.MsgBox.ShowWarningDialog("Cần phải chọn 1 trường dữ liệu bắt buộc: Kho");
                    return;
                }
                if (check1 == 0)
                {
                    Lotus.MsgBox.ShowWarningDialog("Cần phải chọn 1 trường dữ liệu bắt buộc: Hàng hoá");
                    return;
                }

                else
                {
                    if (Biencucbo.ngonngu.ToString() == "Vietnam")
                    {
                        if (check1 == 0)
                        {
                            Biencucbo.sp = "Tất cả";
                        }
                        if (check2 == 0)
                        {
                            Biencucbo.doituong = "Tất cả";
                        }
                        if (check3 == 0)
                        {
                            Biencucbo.congviec = "Tất cả";
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
                        if (check1 == 0)
                        {
                            Biencucbo.sp = "ທັງໝົດ";
                        }
                        if (check2 == 0)
                        {
                            Biencucbo.doituong = "ທັງໝົດ";
                        }
                        if (check3 == 0)
                        {
                            Biencucbo.congviec = "ທັງໝົດ";
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



                    key = 1;
                    var lst3a = from a in db.r_pxuats
                                join b in db.dk_rps on a.idsanpham equals b.id
                                where a.ngayhd < tungay.DateTime
                                && b.user == Biencucbo.idnv
                                select a;

                    var lst3 = (from a in lst3a
                                join b in db.dk_rps on a.iddv equals b.id
                                where b.user == Biencucbo.idnv
                                select a.soluong).Sum();

                    var lst4a = from a in db.r_pnhaps
                                join b in db.dk_rps on a.idsanpham equals b.id
                                where a.ngaynhap < tungay.DateTime
                                && b.user == Biencucbo.idnv
                                select a;

                    var lst4 = (from a in lst4a
                                join b in db.dk_rps on a.iddv equals b.id
                                where b.user == Biencucbo.idnv
                                select a.soluong).Sum();
                    if (lst3 == null)
                    {
                        lst3 = 0;
                    }
                    if (lst4 == null)
                    {
                        lst4 = 0;
                    }
                    Biencucbo.tondau = double.Parse(lst4.ToString()) - double.Parse(lst3.ToString());

                    lst3a = from a in db.r_pxuats
                            join b in db.dk_rps on a.idsanpham equals b.id
                            where a.ngayhd <= denngay.DateTime
                            && b.user == Biencucbo.idnv
                            select a;

                    lst3 = (from a in lst3a
                            join b in db.dk_rps on a.iddv equals b.id
                            where b.user == Biencucbo.idnv
                            select a.soluong).Sum();
                    lst4a = from a in db.r_pnhaps
                            join b in db.dk_rps on a.idsanpham equals b.id
                            where a.ngaynhap <= denngay.DateTime
                            && b.user == Biencucbo.idnv
                            select a;

                    lst4 = (from a in lst4a
                            join b in db.dk_rps on a.iddv equals b.id
                            where b.user == Biencucbo.idnv
                            select a.soluong).Sum();
                    if (lst3 == null)
                    {
                        lst3 = 0;
                    }
                    if (lst4 == null)
                    {
                        lst4 = 0;
                    }
                    Biencucbo.toncuoi = double.Parse(lst4.ToString()) - double.Parse(lst3.ToString());

                    lst3a = from a in db.r_pxuats
                            join b in db.dk_rps on a.idsanpham equals b.id
                            where a.ngayhd >= tungay.DateTime && a.ngayhd <= denngay.DateTime
                            && b.user == Biencucbo.idnv
                            select a;
                    lst4a = from a in db.r_pnhaps
                            join b in db.dk_rps on a.idsanpham equals b.id
                            where a.ngaynhap >= tungay.DateTime && a.ngaynhap <= denngay.DateTime
                            && b.user == Biencucbo.idnv
                            select a;


                    var lst2 = (from a in lst3a
                                join b in db.dk_rps on a.iddv equals b.id
                                where b.user == Biencucbo.idnv
                                select new
                                {
                                    idxuat = a.id,
                                    idnhap = "",
                                    idsp = a.idsanpham,
                                    tensp = a.tensp,
                                    dvt = a.dvt,
                                    ngay = a.ngayhd,
                                    diengiai = a.diengiai,
                                    slnhap = a.soluong - a.soluong,
                                    slxuat = a.soluong,
                                    dongia = a.thanhtien == 0 ? 0 : a.thanhtien / a.soluong,
                                    thanhtien = a.thanhtien,
                                }).Concat(from a in lst4a
                                          join b in db.dk_rps on a.iddv equals b.id
                                          where b.user == Biencucbo.idnv
                                          select new
                                          {
                                              idxuat = "",
                                              idnhap = a.id,
                                              idsp = a.idsanpham,
                                              tensp = a.tensp,
                                              dvt = a.dvt,
                                              ngay = a.ngaynhap,
                                              diengiai = a.diengiai,
                                              slnhap = a.soluong,
                                              slxuat = a.soluong - a.soluong,
                                              dongia = a.thanhtien == 0 ? 0 : a.thanhtien / a.soluong,
                                              thanhtien = a.thanhtien,

                                          }).OrderBy(t => t.ngay);
                    Biencucbo.tondau2 = Biencucbo.tondau;
                    key = 1;

                    var lst = (from a in lst2
                               select new

                               {
                                   idxuat = a.idxuat,
                                   idnhap = a.idnhap,
                                   idsp = a.idsp,
                                   tensp = a.tensp,
                                   dvt = a.dvt,
                                   ngay = a.ngay,
                                   diengiai = a.diengiai,
                                   slnhap = a.slnhap,
                                   slxuat = a.slxuat,
                                   dongia = a.dongia,
                                   thanhtien = a.thanhtien,

                                   tondau2 = timton(a.ngay),
                                   stt = stt(a.ngay),
                               });


                    r_chitietnhapxuat xtra = new r_chitietnhapxuat();
                    xtra.DataSource = lst;
                    xtra.ShowPreviewDialog();
                }
            }
            catch (Exception ex) 
            {
                XtraMessageBox.Show(ex.Message);
            }
            SplashScreenManager.CloseForm(false);
        }
        private double? timton(DateTime? a)
        {
            double? b = 0;
            b = Biencucbo.tondau2;
            Biencucbo.tondau2 = 0;
            return b;
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
