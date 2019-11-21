using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DAL;
using BUS;
using ControlLocalizer;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraReports.UI;

namespace GUI.Report.Xuat
{
    public partial class f_thongketonkho : DevExpress.XtraEditors.XtraForm
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        t_thongke tk = new t_thongke();
        public f_thongketonkho()
        {
            InitializeComponent();

            rTime.SetTime(thoigian);
            rTime.SetTime2(thoigian);

            var lst = from d in db.donvis
                      where d.id.Length == 2
                      select new
                      {
                          d.id,
                          d.tendonvi,
                          d.iddv,
                          MaTim = LayMaTim(d),
                      };

            txtdonvi.Properties.DataSource = lst.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.dvTen + ".")).ToList();
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

        private void f_thongke_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Thống kê Tồn kho").ToString();

            changeFont.Translate(this);

            //tran
            if (Biencucbo.ngonngu.ToString() == "Vietnam")
            {
                gridColumn1.Caption = "ID";
                gridColumn2.Caption = "Tên Đơn Vị";
                gridColumn3.Caption = "Đơn Vị Quản Lý";
            }
            else
            {
                gridColumn1.Caption = "ລະຫັດຫົວໜ່ວຍ";
                gridColumn2.Caption = "ຊີ່ຫົວໜ່ວຍ";
                gridColumn3.Caption = "ຫົວໜ່ວຍຄຸ້ມຄອງ";
            }

            tungay.ReadOnly = true;
            denngay.ReadOnly = true;
        }
        public void loaddata(DateTime tungay, DateTime denngay)
        {
            //reset db
            db.CommandTimeout = 0;

            //xoa all theo iddv
            var lst00 = from a in db.thongke_tonkhos where a.id.StartsWith(txtdonvi.Text.Trim()) select a;
            db.thongke_tonkhos.DeleteAllOnSubmit(lst00.ToList());
            db.SubmitChanges();

            SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
            try
            {

                var lst2 = from a in db.r_pnhaps
                           join d in db.donvis on a.iddv equals d.id
                           where a.ngaynhap < denngay

                           select new
                           {
                               idsp = a.idsanpham,
                               tensp = a.tensp,
                               sl = a.soluong,
                               iddv = a.iddv,
                               //MaTim = LayMaTim(d),
                           };

                var nhaptong2 = lst2.GroupBy(x => new { x.iddv, x.idsp, x.tensp }).Select(y => new
                {
                    iddv = y.Key.iddv,
                    idsp = y.Key.idsp,
                    tensp = y.Key.tensp,

                    sl = y.Sum(t => t.sl)
                });
                var nhaptong = (from a in nhaptong2
                                join d in db.donvis on a.iddv equals d.id
                                select new
                                {
                                    a.iddv,
                                    d.tendonvi,
                                    a.idsp,
                                    a.tensp,
                                    a.sl,
                                    MaTim = LayMaTim(d)

                                }).ToList().Where(t => t.MaTim.Contains("." + txtdonvi.Text + "."));



                var lst3 = from a in db.r_pxuats
                           join d in db.donvis on a.iddv equals d.id
                           where a.ngayhd < denngay
                           select new
                           {
                               idsp = a.idsanpham,
                               tensp = a.tensp,
                               sl = a.soluong,
                               iddv = a.iddv,
                               //MaTim = LayMaTim(d),
                           };

                var xuattong2 = lst3.GroupBy(x => new { x.iddv, x.idsp, x.tensp }).Select(y => new
                {
                    iddv = y.Key.iddv,
                    idsp = y.Key.idsp,
                    tensp = y.Key.tensp,
                    sl = y.Sum(t => t.sl),
                    //y.Key.MaTim
                });

                var xuattong = (from a in xuattong2
                                join d in db.donvis on a.iddv equals d.id
                                select new
                                {
                                    a.iddv,
                                    d.tendonvi,
                                    a.idsp,
                                    a.tensp,
                                    a.sl,
                                    MaTim = LayMaTim(d)

                                }).ToList().Where(t => t.MaTim.Contains("." + txtdonvi.Text + "."));

                var lst = (from a in nhaptong
                           select new
                           {
                               a.iddv,
                               a.tendonvi,
                               a.idsp,
                               a.tensp,
                               nhap = a.sl,
                               xuat = a.sl - a.sl,
                           }).Concat(from b in xuattong
                                     select new
                                     {
                                         b.iddv,
                                         b.tendonvi,
                                         b.idsp,
                                         b.tensp,
                                         nhap = b.sl - b.sl,
                                         xuat = b.sl
                                     }).GroupBy(t => new { t.iddv, t.tendonvi, t.idsp, t.tensp }).Select(t => new
                                     {
                                         iddv = t.Key.iddv,
                                         //iddv = t.Key.iddv + " - " + t.Key.tendonvi,
                                         tendonvi = t.Key.tendonvi,
                                         t.Key.idsp,
                                         t.Key.tensp,
                                         //thanhtien = (t.Sum(z => z.nhap) == null ? 0 : t.Sum(z => z.nhap)) - (t.Sum(z => z.xuat) == null ? 0 : t.Sum(z => z.xuat))


                                         thanhtien = (t.Sum(z => z.nhap) == null ? 0 : t.Sum(z => z.nhap)) - (t.Sum(z => z.xuat) == null ? 0 : t.Sum(z => z.xuat))
                                     }).OrderBy(t => t.iddv);

                var count = lst.Count();
                if (count > 0)
                {
                    var list0 = (from a in lst
                                 select new data_thongke_tonkho()
                                 {
                                     id = a.iddv,
                                     iddv = a.iddv + "-" + a.tendonvi,
                                     tendonvi = a.tendonvi,
                                     tensp = a.tensp,
                                     thanhtien = a.thanhtien.Value
                                 }).ToList();

                    for (int i = 0; i < count; i++)
                    {
                        var row1 = list0.ElementAt(i) as data_thongke_tonkho;

                        tk.moi(row1.id + i + DateTime.Now, row1.iddv, row1.tendonvi, row1.idsp, row1.tensp, Math.Round(double.Parse(row1.thanhtien.ToString()), 3));
                    }
                }

                //chartControl1.DataSource = lst.ToList(); 
                chartControl1.DataSource = db.thongke_tonkhos.Where(t => t.iddv.StartsWith(txtdonvi.Text));


                //this.chartControl1.SeriesDataMember = "tensp";
                //this.chartControl1.SeriesTemplate.ArgumentDataMember = "iddv";

                //this.chartControl1.SeriesTemplate.ArgumentDataMember = "{" + "iddv" + ":" + "tendonvi" + "}";

                //this.chartControl1.SeriesTemplate.ValueDataMembersSerializable = "thanhtien";

                //chartControl1.Series["Series 1"].LegendTextPattern = "{iddv} : {thanhtien}";
                //chartControl1.Series["Series 1"].CrosshairLabelPattern = "{thanhtien:n3}";

                //series1.Name = "Series 1";{A} : {V:n3}
                //{ A: dd / MM / yyyy}: { V: n3}

                n1 = tungay;
                n2 = denngay;
            }
            catch (Exception ex)
            {
                Lotus.MsgBox.ShowErrorDialog(ex.ToString());
            }
            SplashScreenManager.CloseForm(false);
        }
        private void thoigian_EditValueChanged(object sender, EventArgs e)
        {
            chartControl1.DataSource = null;
            chartControl1.Series["Series 1"].DataSource = null;

            changeTime.thoigian_change3(thoigian, tungay, denngay);
            if (Biencucbo.gtime == 1)
            {
                loaddata(DateTime.Parse(tungay.Text), DateTime.Parse(denngay.Text));
            }
        }

        private void btnxem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //var items = from a in db.r_pxuats
            //            join d in db.donvis on a.iddv equals d.id
            //            where
            //            a.ngayhd >= tungay.DateTime && a.ngayhd <= denngay.DateTime
            //            select new
            //            {
            //                id = a.id,
            //                iddv = a.iddv + "-" + a.tendonvi,
            //                ngayhd = a.ngayhd,
            //                thanhtien = a.thanhtien,
            //                MaTim = LayMaTim(d)
            //            };
            //var lst = items.ToList().Where(t => t.MaTim.Contains("." + txtdonvi.Text + "."));




            //var lst2 = from a in db.r_pnhaps
            //           join d in db.donvis on a.iddv equals d.id
            //           where a.ngaynhap < denngay.DateTime

            //           select new
            //           {
            //               idsp = a.idsanpham,
            //               tensp = a.tensp,
            //               sl = a.soluong,
            //               iddv = a.iddv,
            //               //MaTim = LayMaTim(d),
            //           };

            //var nhaptong2 = lst2.GroupBy(x => new { x.iddv, x.idsp, x.tensp }).Select(y => new
            //{
            //    iddv = y.Key.iddv,
            //    idsp = y.Key.idsp,
            //    tensp = y.Key.tensp,

            //    sl = y.Sum(t => t.sl)
            //});
            //var nhaptong = (from a in nhaptong2
            //                join d in db.donvis on a.iddv equals d.id
            //                select new
            //                {
            //                    a.iddv,
            //                    d.tendonvi,
            //                    a.idsp,
            //                    a.tensp,
            //                    a.sl,
            //                    MaTim = LayMaTim(d)

            //                }).ToList().Where(t => t.MaTim.Contains("." + txtdonvi.Text + "."));



            //var lst3 = from a in db.r_pxuats
            //           join d in db.donvis on a.iddv equals d.id
            //           where a.ngayhd < denngay.DateTime
            //           select new
            //           {
            //               idsp = a.idsanpham,
            //               tensp = a.tensp,
            //               sl = a.soluong,
            //               iddv = a.iddv,
            //               //MaTim = LayMaTim(d),
            //           };

            //var xuattong2 = lst3.GroupBy(x => new { x.iddv, x.idsp, x.tensp }).Select(y => new
            //{
            //    iddv = y.Key.iddv,
            //    idsp = y.Key.idsp,
            //    tensp = y.Key.tensp,
            //    sl = y.Sum(t => t.sl),
            //    //y.Key.MaTim
            //});

            //var xuattong = (from a in xuattong2
            //                join d in db.donvis on a.iddv equals d.id
            //                select new
            //                {
            //                    a.iddv,
            //                    d.tendonvi,
            //                    a.idsp,
            //                    a.tensp,
            //                    a.sl,
            //                    MaTim = LayMaTim(d)

            //                }).ToList().Where(t => t.MaTim.Contains("." + txtdonvi.Text + "."));

            //var lst = (from a in nhaptong
            //           select new
            //           {
            //               a.iddv,
            //               a.tendonvi,
            //               a.idsp,
            //               a.tensp,
            //               nhap = a.sl,
            //               xuat = a.sl - a.sl,
            //           }).Concat(from b in xuattong
            //                     select new
            //                     {
            //                         b.iddv,
            //                         b.tendonvi,
            //                         b.idsp,
            //                         b.tensp,
            //                         nhap = b.sl - b.sl,
            //                         xuat = b.sl
            //                     }).GroupBy(t => new { t.iddv, t.tendonvi, t.idsp, t.tensp }).Select(t => new
            //                     {
            //                         iddv = t.Key.iddv,
            //                         //iddv = t.Key.iddv + " - " + t.Key.tendonvi,
            //                         tendonvi = t.Key.tendonvi,
            //                         t.Key.idsp,
            //                         t.Key.tensp,
            //                         //thanhtien = (t.Sum(z => z.nhap) == null ? 0 : t.Sum(z => z.nhap)) - (t.Sum(z => z.xuat) == null ? 0 : t.Sum(z => z.xuat))


            //                         thanhtien = (t.Sum(z => z.nhap) == null ? 0 : t.Sum(z => z.nhap)) - (t.Sum(z => z.xuat) == null ? 0 : t.Sum(z => z.xuat))
            //                     }).OrderBy(t => t.iddv);

            //var count = lst.Count();
            //if (count > 0)
            //{
            //    var list0 = (from a in lst
            //                 select new data_thongke_tonkho()
            //                 {
            //                     id = a.iddv,
            //                     iddv = a.iddv,
            //                     tendonvi = a.tendonvi,
            //                     tensp = a.tensp,
            //                     thanhtien = a.thanhtien.Value
            //                 }).ToList();

            //    for (int i = 0; i < count; i++)
            //    {
            //        var row1 = list0.ElementAt(i) as data_thongke_tonkho;

            //        tk.moi(row1.id + i + DateTime.Now, row1.iddv, row1.tendonvi, row1.idsp, row1.tensp, double.Parse(row1.thanhtien.ToString()));
            //    }
            //}

            var list00 = from a in db.thongke_tonkhos where a.iddv.StartsWith(txtdonvi.Text.Trim()) select a;
            //r_thongketonkho report = new r_thongketonkho(lst);
            r_thongketonkho report = new r_thongketonkho(list00);

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //xoa all theo iddv
            var lst = from a in db.thongke_tonkhos where a.id.StartsWith(txtdonvi.Text.Trim()) select a;

            db.thongke_tonkhos.DeleteAllOnSubmit(lst.ToList());
            db.SubmitChanges();

            loaddata(DateTime.Parse(tungay.Text), DateTime.Parse(denngay.Text));
        }

        public static string madv, tendv;
        public static DateTime n1, n2;
        private void txtdonvi_EditValueChanged(object sender, EventArgs e)
        {
            chartControl1.DataSource = null;
            chartControl1.Series["Series 1"].DataSource = null;

            var lst = (from a in db.donvis
                       where a.id == txtdonvi.Text
                       select new data_thongke()
                       {
                           tendonvi = a.tendonvi
                       }).ToList();

            var row1 = lst.ElementAt(0) as data_thongke;

            madv = txtdonvi.Text;
            tendv = row1.tendonvi;

            if (Biencucbo.gtime == 1)
            {
                loaddata(DateTime.Parse(tungay.Text), DateTime.Parse(denngay.Text));
            }
        }
    }
}