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
using DevExpress.XtraSplashScreen;

namespace GUI
{
    public partial class f_History_ChotCT : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();

        public f_History_ChotCT()
        {
            InitializeComponent();

            rTime.SetTime(thoigian);
            rTime.SetTime2(thoigian);
        }
        public void loaddata(DateTime tungay, DateTime denngay)
        {
            SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);

            //reset db
            db.CommandTimeout = 0;

            var lst = from a in db.View_histories
                      where
                      a.thoigian >= tungay && a.thoigian <= denngay
                      select new
                      {
                          a.donvi,
                          a.ngay,
                          a.thoigian,
                          a.songay,
                          songay2 = (a.thoigian.Value - a.ngay.Value).Days - 1,
                          a.nguoi,
                          a.may,
                          a.ma,
                          a.loaisp,
                          a.ssl,
                          a.stt,
                      };

            if (Biencucbo.donvi.Length == 2)
            {
                if (Biencucbo.donvi != "00")
                {
                    var lst2t = from a in db.donvis where a.iddv == Biencucbo.donvi select a;
                    var lst2 = (from a in lst
                                join b in lst2t on a.donvi equals b.id
                                select new
                                {
                                    a.donvi,
                                    a.ngay,
                                    a.thoigian,
                                    a.songay2, //songay2 = (a.thoigian.Value - a.ngay.Value).Days - 1,
                                    a.nguoi,
                                    a.ma,
                                    a.loaisp,
                                    a.ssl,
                                    a.stt
                                });
                    gridControl1.DataSource = lst2;//.OrderBy(t => t.ngay);

                    gridView1.Columns["donvi"].GroupIndex = 1;
                    gridView1.Columns["ngay"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
                //else  //Don vi: 00
                //{
                //    //    gridControl1.DataSource = lst.ToList();//.OrderBy(t => t.ngay);
                //    //    gridView1.Columns["donvi"].GroupIndex = 1;
                //    //    //gridView1.Columns["ngay"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                //}
            }
            else
            {
                var lst3 = from a in lst
                           where a.donvi == Biencucbo.donvi
                           select new
                           {
                               a.donvi,
                               a.ngay,
                               a.thoigian,
                               a.songay2,// = (a.thoigian.Value - a.ngay.Value).Days - 1,
                               a.nguoi,
                               a.ma,
                               a.loaisp,
                               a.ssl,
                               a.stt
                           };

                gridControl1.DataSource = lst3;//.OrderBy(t => t.ngay);
                gridView1.Columns["ngay"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            }

            SplashScreenManager.CloseForm(false);
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

        private void f_PN_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Lịch Sử Cập Nhật Chốt Công Tơ ").ToString();
            changeFont.Translate(this);
            changeFont.Translate(barManager1);

            tungay.ReadOnly = true;
            denngay.ReadOnly = true;

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
    }
}