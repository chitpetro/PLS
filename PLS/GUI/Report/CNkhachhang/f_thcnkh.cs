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
    public partial class f_thcnkh : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        Boolean doubleclick1 = false;
        Boolean doubleclick2 = false;

        public f_thcnkh()
        {
            InitializeComponent();

            rTime.SetTime(thoigian);
        }

        private void f_chitietnhapkho_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "TỔNG HỢP CÔNG NỢ KHÁCH HÀNG").ToString();

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
                var lst = from a in db.dk_rps where a.user == Biencucbo.idnv select a;
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

                    var lstdv1 = (from a in db.donvis select a);
                    var lstdv2 = (from a in db.donvis select a);
                    var lstdv3 = (from a in db.donvis select a);

                    var lstlaydv = (from a in db.dk_rps where a.user == Biencucbo.idnv && a.loai == "Đơn vị - ຫົວໜ່ວຍ" && a.id.Length == 2 && a.id != "00" select a);

                    if (lstlaydv.Count() > 0)
                    {
                        lstdv1 = (from a in lstdv3
                                  join b in lstlaydv on a.id.Substring(0, 2) equals b.id
                                  select a);
                    }
                    else
                    {
                        lstdv1 = null;
                    }
                    lstlaydv = (from a in db.dk_rps where a.user == Biencucbo.idnv && a.loai == "Đơn vị - ຫົວໜ່ວຍ" && a.id.Length > 2 select a);
                    if (lstlaydv.Count() > 0)
                    {
                        lstdv3 = (from a in lstdv2
                                  join b in lstlaydv on a.id equals b.id
                                  select a);
                    }
                    else
                    {
                        lstdv3 = null;
                    }
                    if (lstdv1 != null || lstdv3 != null)
                    {
                        if (lstdv1 == null)
                            lstdv2 = lstdv3;
                        else if (lstdv3 == null)
                            lstdv2 = lstdv1;
                        else /*if (lstdv1 != null && lstdv3 != null)*/
                            lstdv2 = lstdv1.Concat(lstdv3);
                    }

                    var lstcno = (from a in db.r_pcantrus
                                  join b in db.doituongs on a.dtcantru equals b.id
                                  select new
                                  {
                                      ngayhd = a.ngayct,
                                      a.id,
                                      iddt = a.dtcantru,
                                      b.ten,
                                      a.ghichu,
                                      a.diengiai,
                                      a.thanhtien,
                                      a.dv
                                  });
                    var lsttno = (from a in db.r_pcantrus
                                  join b in db.doituongs on a.dttruno equals b.id
                                  select new
                                  {
                                      ngaythu = a.ngayct,
                                      a.id,
                                      iddt = a.dttruno,
                                      b.ten,
                                      a.ghichu,
                                      a.diengiai,
                                      a.thanhtien,
                                      a.dv
                                  });

                    var lst2 = (from a in db.r_pxuats
                                join b in lstdv2 on a.iddv equals b.id
                                where a.ngayhd >= tungay.DateTime && a.ngayhd <= denngay.DateTime && (a.loaixuat == "Xuất bán - ຈ່າຍອອກຂາຍ" || a.loaixuat == "Xuất Khác - ເບີກຈ່າຍອື່ນໆ")

                                select new
                                {
                                    a.iddt,
                                    a.ten,
                                    a.thanhtien,
                                    a.iddv
                                }).Concat(from a in lstcno
                                          join b in lstdv2 on a.dv equals b.id
                                          where a.ngayhd >= tungay.DateTime && a.ngayhd <= denngay.DateTime

                                          select new
                                          {
                                              a.iddt,
                                              a.ten,
                                              a.thanhtien,
                                              iddv = a.dv
                                          });
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
                                select new
                                {
                                    iddt = a.iddt,
                                    ban = a.thanhtien,
                                    a.iddv
                                }).GroupBy(t => new { iddt = t.iddt, iddv = t.iddv }).Select(c => new { iddt = c.Key.iddt, iddv = c.Key.iddv, ban = c.Sum(t => t.ban) });

                    var lst4 = (from a in db.r_pthus
                                join b in lstdv2 on a.iddv equals b.id
                                where a.ngaythu >= tungay.DateTime && a.ngaythu <= denngay.DateTime && (a.loaithu == "Thu tiền bán hàng - ຮັບເງິນຂາຍສິນຄ້າ" || a.loaithu == "Thu tiền Nợ - ຮັບເງິນໜີ້")

                                select new
                                {
                                    a.iddt,
                                    a.iddv,
                                    a.ten,
                                    a.thanhtien,

                                }).Concat(from a in db.r_pttoans
                                          join b in lstdv2 on a.dv equals b.id
                                          where a.ngaythu >= tungay.DateTime && a.ngaythu <= denngay.DateTime

                                          select new
                                          {
                                              a.iddt,
                                              iddv = a.dv,
                                              a.ten,
                                              a.thanhtien,

                                          }).Concat(from a in lsttno
                                                    join b in lstdv2 on a.dv equals b.id
                                                    where a.ngaythu >= tungay.DateTime && a.ngaythu <= denngay.DateTime

                                                    select new
                                                    {
                                                        a.iddt,
                                                        iddv = a.dv,
                                                        a.ten,
                                                        a.thanhtien,

                                                    });
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

                                select new
                                {
                                    iddt = a.iddt,
                                    thu = a.thanhtien,
                                    a.iddv
                                }).GroupBy(t => new { t.iddt, t.iddv }).Select(c => new { iddt = c.Key.iddt, iddv = c.Key.iddv, thu = c.Sum(t => t.thu) });



                    lst2 = (from a in db.r_pxuats
                            join b in lstdv2 on a.iddv equals b.id
                            where a.ngayhd < tungay.DateTime && (a.loaixuat == "Xuất bán - ຈ່າຍອອກຂາຍ" || a.loaixuat == "Xuất Khác - ເບີກຈ່າຍອື່ນໆ")

                            select new
                            {
                                a.iddt,
                                a.ten,
                                a.thanhtien,
                                a.iddv
                            }).Concat(from a in lstcno
                                      join b in lstdv2 on a.dv equals b.id
                                      where a.ngayhd < tungay.DateTime

                                      select new
                                      {
                                          a.iddt,
                                          a.ten,
                                          a.thanhtien,
                                          iddv = a.dv
                                      });

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
                                 select new
                                 {
                                     iddt = a.iddt,
                                     ban = a.thanhtien,
                                     a.iddv
                                 }).GroupBy(t => new { iddt = t.iddt, iddv = t.iddv }).Select(c => new { iddt = c.Key.iddt, iddv = c.Key.iddv, ban = c.Sum(t => t.ban) });

                    lst4 = (from a in db.r_pthus
                            join b in lstdv2 on a.iddv equals b.id
                            where a.ngaythu < tungay.DateTime && (a.loaithu == "Thu tiền bán hàng - ຮັບເງິນຂາຍສິນຄ້າ" || a.loaithu == "Thu tiền Nợ - ຮັບເງິນໜີ້")

                            select new
                            {
                                a.iddt,
                                a.iddv,
                                a.ten,
                                a.thanhtien,

                            }).Concat(from a in db.r_pttoans
                                      join b in lstdv2 on a.dv equals b.id
                                      where a.ngaythu < tungay.DateTime

                                      select new
                                      {
                                          a.iddt,
                                          iddv = a.dv,
                                          a.ten,
                                          a.thanhtien,

                                      }).Concat(from a in lsttno
                                                join b in lstdv2 on a.dv equals b.id
                                                where a.ngaythu < tungay.DateTime

                                                select new
                                                {
                                                    a.iddt,
                                                    iddv = a.dv,
                                                    a.ten,
                                                    a.thanhtien,

                                                });

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

                                 select new
                                 {
                                     iddt = a.iddt,
                                     thu = a.thanhtien,
                                     a.iddv
                                 }).GroupBy(t => new { t.iddt, t.iddv }).Select(c => new { iddt = c.Key.iddt, iddv = c.Key.iddv, thu = c.Sum(t => t.thu) });




                    // tồn dầu kỳ

                    var tondau = (from a in lst6a
                                  select new
                                  {
                                      a.iddv,
                                      id = a.iddt,
                                      a.ban,
                                      thu = a.ban - a.ban,
                                  }).Concat(from a in lst7a
                                            select new
                                            {
                                                a.iddv,
                                                id = a.iddt,
                                                ban = a.thu - a.thu,
                                                thu = a.thu,
                                            }).GroupBy(t => new { t.id, t.iddv }).Select(c => new { c.Key.id,
                                                c.Key.iddv,
                                                nodk = c.Sum(t => t.ban) - c.Sum(t => t.thu),
                                                ban = c.Sum(t => t.ban) - c.Sum(t => t.ban),
                                                thu = c.Sum(t => t.thu) - c.Sum(t => t.thu),
                                                notk = c.Sum(t => t.thu) - c.Sum(t => t.thu),
                                                nock = c.Sum(t => t.ban) - c.Sum(t => t.thu) });
                    var tontk = (from a in lst6
                                 select new
                                 {
                                     a.iddv,
                                     id = a.iddt,
                                     a.ban,
                                     thu = a.ban - a.ban,
                                 }).Concat(from a in lst7
                                           select new
                                           {
                                               a.iddv,
                                               id = a.iddt,
                                               ban = a.thu - a.thu,
                                               thu = a.thu,
                                           }).GroupBy(t => new { t.id, t.iddv }).Select(c => new
                                           {
                                               c.Key.id,
                                               c.Key.iddv,
                                               nodk = c.Sum(t => t.ban) - c.Sum(t => t.ban),
                                               ban = c.Sum(t => t.ban),
                                               thu = c.Sum(t => t.thu),
                                               notk = c.Sum(t => t.ban) - c.Sum(t => t.thu),
                                               nock = c.Sum(t => t.ban) - c.Sum(t => t.thu)
                                           });

                    var lst = (from a in tondau select a).Concat(from a in tontk select a).GroupBy(t => new { t.id, t.iddv }).Select(c => new
                    {
                        c.Key.id,
                        c.Key.iddv,
                        nodk = c.Sum(t => t.nodk),
                        ban = c.Sum(t => t.ban),
                        thu = c.Sum(t => t.thu),
                        notk = c.Sum(t => t.notk),
                        nock = c.Sum(t => t.nock),
                    });


                    //var lst = from a in db.doituongs
                    //          join u in db.dk_rps on a.id equals u.id
                    //          where u.user == Biencucbo.idnv
                    //          join b in lst6 on a.id equals b.iddt into h
                    //          join c in lst7 on a.id equals c.iddt into k
                    //          join d in lst6a on a.id equals d.iddt into l
                    //          join f in lst7a on a.id equals f.iddt into m

                    //          from xuat in h.DefaultIfEmpty()
                    //          from thu in k.DefaultIfEmpty()
                    //          from xuatdau in l.DefaultIfEmpty()
                    //          from thudau in m.DefaultIfEmpty()

                    //          select new
                    //          {

                    //              id = a.id,
                    //              ten = a.ten,
                    //              nodau = (xuatdau.ban == null ? 0 : xuatdau.ban) - (thudau.thu == null ? 0 : thudau.thu),
                    //              ban = xuat.ban == null ? 0 : xuat.ban,
                    //              thu = thu.thu == null ? 0 : thu.thu,
                    //              //iddv = xuatcuoi.ban != null ? xuatcuoi.iddv : (thucuoi.thu != null ? thucuoi.iddv : ""),
                    //              notrongky = (xuat.ban == null ? 0 : xuat.ban) - (thu.thu == null ? 0 : thu.thu),
                    //              nocuoiky = ((xuatdau.ban == null ? 0 : xuatdau.ban) - (thudau.thu == null ? 0 : thudau.thu)) + ((xuat.ban == null ? 0 : xuat.ban) - (thu.thu == null ? 0 : thu.thu)),
                    //          };


                    //if (check2 != 0)
                    //{

                    //    lst8 = from a in lst
                    //           join b in db.dk_rps on a.id equals b.id
                    //           where b.user == Biencucbo.idnv
                    //           select a;
                    //}
                    //else
                    //{
                    //    lst8 = from a in lst select a;
                    //}
                    var lst8 = (from a in lst
                                join b in db.doituongs on a.id equals b.id
                                select new
                                {
                                    a.id,
                                    b.ten,
                                    nodau = a.nodk,
                                    a.ban,
                                    a.thu,
                                    a.iddv,
                                    notrongky = a.notk,
                                    nocuoiky = a.nock,
                                });

                    var lst9 = from a in lst8
                           join b in db.donvis on a.iddv equals b.id
                           where a.nodau != 0 || a.notrongky != 0 || a.nocuoiky != 0
                           select new
                           {
                               a.id,
                               a.ten,
                               a.nodau,
                               a.ban,
                               a.thu,
                               iddv = a.iddv + "-" + b.tendonvi,
                               a.notrongky,
                               a.nocuoiky,
                           };
                r_thcnkh xtra = new r_thcnkh();
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