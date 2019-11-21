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
using ControlLocalizer;
using DevExpress.XtraReports.UI;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraSplashScreen;

namespace GUI
{

    public partial class f_kt_banhang : Form
    {

        KetNoiDBDataContext db = new KetNoiDBDataContext();
        Boolean doubleclick = false;

        public f_kt_banhang()
        {
            InitializeComponent();

            rTime.SetTime(thoigian);
            rTime.SetTime2(thoigian);
        }
        public void loaddata(DateTime tungay, DateTime denngay)
        {
            SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
            try
            {
                //reset db
                db.CommandTimeout = 0;

                var lst = from a in db.kt_banhangs
                          join b in db.donvis on a.iddv equals b.id
                          where
                          a.ngay >= tungay && a.ngay <= denngay

                          orderby a.ngay ascending
                          orderby a.iddv ascending
                          select new
                          {
                              iddv = a.iddv,
                              ngay = a.ngay,
                              loaisp = a.loaisp,
                              sl = a.sl,
                              slxuat = a.slxuat,
                              tt = a.tt,
                              ttxuat = a.ttxuat,

                              sl2 = (a.slxuat - a.sl),// != null ? a.slxuat - a.sl : 0,
                              tt2 = (a.ttxuat - a.tt),// != null ? a.ttxuat - a.tt : 0

                              MaTim = LayMaTim(b)
                          };

                var lst2 = from a in lst where Math.Round((double)(a.sl2), 3, MidpointRounding.ToEven) != 0 || Math.Round(Convert.ToDecimal(a.tt2), 3, MidpointRounding.ToEven) != 0 select a;
                var lst3 = lst2.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                gridView1.Columns["iddv"].GroupIndex = 1;
                gridView1.Columns["ngay"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

                gridView1.ExpandAllGroups();
                gridView1.BestFitColumns();

                gridControl1.DataSource = lst3;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }

            SplashScreenManager.CloseForm(false);
        }
        #region code cu 
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
        #endregion


        private void f_PN_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Kiểm tra Số công tơ").ToString();

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

            gridView1.BestFitColumns();

            SplashScreenManager.CloseForm(false);
        }
    }
}