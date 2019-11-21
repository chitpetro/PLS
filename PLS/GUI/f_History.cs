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
    public partial class f_History : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();

        public f_History()
        {
            InitializeComponent();

            //if (Biencucbo.ngonngu.ToString() == "Vietnam")
            //{
            //    thoigian.Properties.Items.Clear();
            //    thoigian.Properties.Items.Add("Tháng 1");
            //    thoigian.Properties.Items.Add("Tháng 2");
            //    thoigian.Properties.Items.Add("Tháng 3");
            //    thoigian.Properties.Items.Add("Tháng 4");
            //    thoigian.Properties.Items.Add("Tháng 5");
            //    thoigian.Properties.Items.Add("Tháng 6");
            //    thoigian.Properties.Items.Add("Tháng 7");
            //    thoigian.Properties.Items.Add("Tháng 8");
            //    thoigian.Properties.Items.Add("Tháng 9");
            //    thoigian.Properties.Items.Add("Tháng 10");
            //    thoigian.Properties.Items.Add("Tháng 11");
            //    thoigian.Properties.Items.Add("Tháng 12");
            //    thoigian.Properties.Items.Add("Quý 1");
            //    thoigian.Properties.Items.Add("Quý 2");
            //    thoigian.Properties.Items.Add("Quý 3");
            //    thoigian.Properties.Items.Add("6 Tháng Đầu");
            //    thoigian.Properties.Items.Add("6 Tháng Cuối");
            //    thoigian.Properties.Items.Add("Cả Năm");
            //    thoigian.Properties.Items.Add("Tùy ý");
            //}
            //else //Lao
            //{
            //    thoigian.Properties.Items.Clear();
            //    thoigian.Properties.Items.Add("ເດືອນ 1");
            //    thoigian.Properties.Items.Add("ເດືອນ 2");
            //    thoigian.Properties.Items.Add("ເດືອນ 3");
            //    thoigian.Properties.Items.Add("ເດືອນ 4");
            //    thoigian.Properties.Items.Add("ເດືອນ 5");
            //    thoigian.Properties.Items.Add("ເດືອນ 6");
            //    thoigian.Properties.Items.Add("ເດືອນ 7");
            //    thoigian.Properties.Items.Add("ເດືອນ 8");
            //    thoigian.Properties.Items.Add("ເດືອນ 9");
            //    thoigian.Properties.Items.Add("ເດືອນ 10");
            //    thoigian.Properties.Items.Add("ເດືອນ 11");
            //    thoigian.Properties.Items.Add("ເດືອນ 12");
            //    thoigian.Properties.Items.Add("ງວດ 1");
            //    thoigian.Properties.Items.Add("ງວດ 2");
            //    thoigian.Properties.Items.Add("ງວດ 3");
            //    thoigian.Properties.Items.Add("6 ເດືອນຕົ້ນປີ");
            //    thoigian.Properties.Items.Add("6 ເດືອນທ້າຍປີ");
            //    thoigian.Properties.Items.Add("ໝົດປີ");
            //    thoigian.Properties.Items.Add("ແລ້ວແຕ່");
            //}

            rTime.SetTime(thoigian);
            rTime.SetTime2(thoigian);
        }
        public void loaddata(DateTime tungay, DateTime denngay)
        {
            SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);
            //reset db
            db.CommandTimeout = 0;

            var lst = from a in db.histories
                      join d in db.donvis on a.donvi equals d.id
                      where
                      a.thoigian >= tungay && a.thoigian <= denngay
                      select new
                      {
                          ma = a.ma,
                          hoatdong = a.hoatdong,
                          nguoi = a.nguoi,
                          may = a.may,
                          thoigian = a.thoigian,
                          donvi = a.donvi,
                          MaTim = LayMaTim(d)
                      };
            var lst2 = lst.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

            gridControl1.DataSource = lst2;

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

            changeFont.Translate(this);
            changeFont.Translate(barManager1);

            tungay.ReadOnly = true;
            denngay.ReadOnly = true;
            if (Biencucbo.ngonngu.ToString() == "Vietnam")
            {
                thoigian.Text = "Tháng " + DateTime.Now.Month;
            }

            if (Biencucbo.ngonngu.ToString() == "Lao")
            {
                thoigian.Text = "ເດືອນ " + DateTime.Now.Month;
            }
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