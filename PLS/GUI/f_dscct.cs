using BUS;
using DevExpress.XtraGrid.Views.Grid;
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
//using BUS;
using ControlLocalizer;
using DevExpress.XtraReports.UI;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraSplashScreen;

namespace GUI
{


    public partial class f_dscct : Form
    {

        KetNoiDBDataContext db = new KetNoiDBDataContext();
        Boolean doubleclick = false;
        string checkdv = "";
        double? checkchotcuoi = 0;
        string ccotbom = "";
        string cvoibom = "";
        public f_dscct()
        {
            InitializeComponent();

            rTime.SetTime(thoigian);
            rTime.SetTime2(thoigian);
        }

        public double? checkss(string iddv, double? cd, double? cc, string voibom, string cotbom)
        {
            double? ss = 0;
            if (iddv != checkdv)
            {
                checkdv = iddv;
                checkchotcuoi = cc;
                ccotbom = cotbom;
                cvoibom = voibom;
                ss = 0;
            }
            else
            {
                if (ccotbom != cotbom)
                {
                    checkchotcuoi = cc;
                    ccotbom = cotbom;
                    cvoibom = voibom;
                    ss = 0;
                }

                else
                {
                    if (cvoibom != voibom)
                    {
                        checkchotcuoi = cc;
                        cvoibom = voibom;
                        ss = 0;
                    }
                    else
                    {
                        if (cd == checkchotcuoi)
                        {
                            checkchotcuoi = cc;
                            ss = 0;
                        }
                        else
                        {
                            ss = cd - checkchotcuoi;
                            checkchotcuoi = cc;
                        }
                    }
                }
            }
            return ss;
        }

        public void loaddata(DateTime tungay, DateTime denngay)
        {
            SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
            try
            {
                //reset db
                db.CommandTimeout = 0;

                var lst = (from a in db.r_chotcongtos

                           where
                           a.ngay >= tungay && a.ngay <= denngay
                           orderby a.so ascending
                           orderby a.ngay ascending
                           orderby a.voibom ascending
                           orderby a.cotbom ascending
                           orderby a.iddv ascending
                           select new
                           {
                               id = a.id,
                               ngay = a.ngay,
                               iddv = a.iddv,
                               tendonvi = a.tendonvi,
                               cotbom = a.cotbom,
                               voibom = a.voibom,
                               loaisp = a.loaisp,
                               chotdau = a.chotdau,
                               chotcuoi = a.chotcuoi,
                               thu = a.thu,
                               soluong = a.soluong,
                               dongia = a.dongia,
                               thanhtien = a.thanhtien,
                               //MaTim = LayMaTim(a.iddv),
                               saiso = checkss(a.iddv, a.chotdau, a.chotcuoi, a.voibom, a.cotbom)
                           });

                if (Biencucbo.donvi.Length == 2)
                {
                    if (Biencucbo.donvi != "00")
                    {
                        var lst2t = from a in db.donvis where a.iddv == Biencucbo.donvi || a.id == Biencucbo.donvi select a;
                        var lst2 = (from a in lst
                                    join b in lst2t on a.iddv equals b.id

                                    select a);
                        gridControl1.DataSource = lst2;
                    }
                    else
                    {
                        gridControl1.DataSource = lst;
                    }
                }
                else
                {
                    var lst3 = from a in lst where a.iddv == Biencucbo.donvi select a;
                    gridControl1.DataSource = lst3;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            SplashScreenManager.CloseForm(false);
        }


        private string LayMaTim(string da)
        {
            var d = (from a in db.donvis select a).Single(t => t.id == da);
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

            if (s.Contains("." + Biencucbo.donvi + "."))
                return s;
            else
                return "sai";
        }


        private void f_PN_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Danh Sách Chốt Công Tơ").ToString();

            changeFont.Translate(this);
            changeFont.Translate(barManager1);

            tungay.ReadOnly = true;
            denngay.ReadOnly = true;

            Biencucbo.getID = 0;
        }

        private void thoigian_EditValueChanged(object sender, EventArgs e)
        {
            changeTime.thoigian_change3(thoigian, tungay, denngay);
            if (Biencucbo.gtime == 1)
            {
                loaddata(DateTime.Parse(tungay.Text), DateTime.Parse(denngay.Text));
            }

        }

        private void timkiem_Click(object sender, EventArgs e)
        {
            loaddata(DateTime.Parse(tungay.Text), DateTime.Parse(denngay.Text));
        }

        private void gridView1_DoubleClick_1(object sender, EventArgs e)
        {
            doubleclick = true;

        }

        private void gridView1_CustomDrawRowIndicator_1(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridView1.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
            {
                if (e.Info.IsRowIndicator) //Nếu là dòng Indicator
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1; //Không hiển thị hình
                        e.Info.DisplayText = (e.RowHandle + 1).ToString(); //Số thứ tự tăng dần
                    }
                    SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font); //Lấy kích thước của vùng hiển thị Text
                    Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                    BeginInvoke(new MethodInvoker(delegate { cal(_Width, gridView1); })); //Tăng kích thước nếu Text vượt quá
                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1)); //Nhân -1 để đánh lại số thứ tự tăng dần
                SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                BeginInvoke(new MethodInvoker(delegate { cal(_Width, gridView1); }));
            }
        }
        bool cal(Int32 _Width, GridView _View)
        {
            _View.IndicatorWidth = _View.IndicatorWidth < _Width ? _Width : _View.IndicatorWidth;
            return true;
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            doubleclick = false;
        }

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            if (doubleclick == true)
            {
                Biencucbo.getID = 1;
                Biencucbo.ma = gridView1.GetFocusedRowCellValue("id").ToString();
                this.Close();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
            if (Biencucbo.ngonngu.ToString() == "Vietnam")
            {
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
            else //lao
            {
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

            //
            gridView1.Columns["iddv"].GroupIndex = 1;
            gridView1.Columns["cotbom"].GroupIndex = 2;
            gridView1.Columns["voibom"].GroupIndex = 3;
            gridView1.Columns["ngay"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            gridView1.Columns["id"].Visible = false;//.OptionsColumn.AllowShowHide;
            //gridView1.Columns["saiso"].Visible = false; //an cot sai so
            gridView1.ExpandAllGroups();

            gridView1.BestFitColumns();


            //check


            r_dscct report = new r_dscct();
            report.GridControl = gridControl1;

            ReportPrintTool printTool = new ReportPrintTool(report);
            //printTool.PrintingSystem.PageMargins.Right = 0;

            printTool.ShowPreviewDialog();
            gridView1.ClearGrouping();
            gridView1.ClearSorting();
            gridView1.Columns["id"].Visible = true;

            SplashScreenManager.CloseForm(false);
        }

        private void thu_CheckedChanged(object sender, EventArgs e)
        {
            if (thu.Checked == true)
            {
                SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
                try
                {
                    //reset db
                    db.CommandTimeout = 0;

                    var lst = (from a in db.r_chotcongtos
                               where a.ngay >= tungay.DateTime && a.ngay <= denngay.DateTime && a.thu > 0
                               orderby a.so ascending
                               orderby a.ngay ascending
                               orderby a.voibom ascending
                               orderby a.cotbom ascending
                               orderby a.iddv ascending
                               select new
                               {
                                   id = a.id,
                                   ngay = a.ngay,
                                   iddv = a.iddv,
                                   tendonvi = a.tendonvi,
                                   cotbom = a.cotbom,
                                   voibom = a.voibom,
                                   loaisp = a.loaisp,
                                   chotdau = a.chotdau,
                                   chotcuoi = a.chotcuoi,
                                   thu = a.thu,
                                   soluong = a.soluong,
                                   dongia = a.dongia,
                                   thanhtien = a.thanhtien,
                                   //MaTim = LayMaTim(a.iddv)
                               });

                    if (Biencucbo.donvi.Length == 2)
                    {
                        if (Biencucbo.donvi != "00")
                        {
                            var lst2t = from a in db.donvis where a.iddv == Biencucbo.donvi select a;
                            var lst2 = (from a in lst
                                        join b in lst2t on a.iddv equals b.id

                                        select a);
                            gridControl1.DataSource = lst2;
                        }
                        else
                        {
                            gridControl1.DataSource = lst;
                        }
                    }
                    else
                    {
                        var lst3 = from a in lst where a.iddv == Biencucbo.donvi select a;
                        gridControl1.DataSource = lst3;
                    }
                }
                catch (Exception ex)
                {
                    Lotus.MsgBox.ShowErrorDialog(ex.ToString());
                }
                SplashScreenManager.CloseForm(false);
            }
            else
            {
                loaddata(tungay.DateTime, denngay.DateTime);
            }
        }


        public static object result;
        private void btnCheckNgay_Click(object sender, EventArgs e)
        {
            timkiem_Click(sender, e);

            ShowMissDates();
        }

        void ShowMissDates()
        {
            SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);

            //reset db
            db.CommandTimeout = 0;

            var d1 = tungay.DateTime;
            var d2 = denngay.DateTime;

            var days = d2.DayOfYear - d1.DayOfYear + 1;
            var q1 = from iNum in Enumerable.Range(0, days)
                     let now = d1.AddDays(iNum)
                     select new
                     {
                         Ngay = now,
                         DonVi = string.Empty,
                         TenDonVi = string.Empty,
                         Tien = Convert.ToDouble(-1)
                     };

            var lst = from a in db.r_chotcongtos
                      join b in db.donvis on a.iddv equals b.id
                      where a.ngay >= tungay.DateTime && a.ngay <= denngay.DateTime
                      orderby a.ngay ascending
                      orderby a.iddv ascending
                      select new data_tmp()
                      {
                          id = a.id,
                          ngay = a.ngay,
                          iddv = a.iddv,
                          tendonvi = a.tendonvi,
                          thanhtien = a.thanhtien,
                          MaTim = LayMaTim(a.iddv)
                      };

            var lst2 = lst.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));


            long dem0 = 0;
            for (int i = 0; i < lst2.Count(); i++)
            {
                var row1 = lst2.ElementAt(i) as data_tmp;
                if (row1 == null) continue;

                if (row1.thanhtien == 0)
                { row1.thanhtien = 1; dem0++; continue; }
            }

            var q2 = from c in lst2
                     group c by new { c.ngay, c.iddv, c.tendonvi } into g
                     select new
                     {
                         Ngay = g.Key.ngay.Value,
                         DonVi = g.Key.iddv,
                         TenDonVi = g.Key.tendonvi,
                         Tien = Convert.ToDouble(g.Sum(t => t.thanhtien))
                     };
            foreach (var t in q2)
            {
                var qtmp = from iNum in Enumerable.Range(0, days)
                           let now = d1.AddDays(iNum)
                           select new
                           {
                               Ngay = now,
                               DonVi = t.DonVi,
                               TenDonVi = t.TenDonVi,
                               Tien = Convert.ToDouble(-1)
                           };
                q1 = q1.Union(qtmp);
            }

            q1 = q1.Where(t => t.DonVi != string.Empty);
            var source = q1.Union(q2);

            var x = from c in source
                    group c by new { c.Ngay, c.DonVi, c.TenDonVi } into g
                    select new
                    {
                        Ngay = g.Key.Ngay,
                        DonVi = g.Key.DonVi,
                        TenDonVi = g.Key.TenDonVi,
                        Tien = Convert.ToDouble(g.Sum(t => t.Tien))
                    };

            //danh sach ngay thieu
            var lt = x.Where(a => a.Tien == -1);

            f_missdate md = new f_missdate(lt);
            md.ShowDialog();

            SplashScreenManager.CloseForm(false);
        }

        public List<DateTime> DateRange(DateTime fromDate, DateTime toDate)
        {
            return Enumerable.Range(0, toDate.Subtract(fromDate).Days + 1)
                             .Select(d => fromDate.AddDays(d)).ToList();
        }

    }
}