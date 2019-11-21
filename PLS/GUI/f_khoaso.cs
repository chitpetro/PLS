using ControlLocalizer;
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
using BUS;
using DevExpress.XtraGrid.Views.Grid;
using GUI.Libs;

namespace GUI
{
    public partial class f_khoaso : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        public f_khoaso()
        {
            InitializeComponent();

            var list = from d in db.donvis
                       select new
                       {
                           id = d.id,
                           tendonvi = d.tendonvi,
                           iddv = d.iddv,
                           MaTim = LayMaTim(d),
                       };

            var lst1 = list.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.dvTen + "."));

            txtiddv.Properties.DataSource = lst1.ToList();
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
        private void f_khoaso_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Khoá Sổ Chứng Từ").ToString();

            changeFont.Translate(this);

            loaddata();
        }
        void loaddata()
        {
            var list = from a in db.khoasochungtus
                       join b in db.donvis
                       on a.iddv equals b.id
                       select new
                       {
                           iddv = a.iddv,
                           tendonvi = b.tendonvi,
                           ngaykhoaso = a.ngaykhoaso,
                           MaTim = LayMaTim(b),
                       };

            var lst1 = list.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.dvTen + "."));

            gridControl1.DataSource = lst1.ToList();
        }

        t_tudong td = new t_tudong();

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridView1.IsGroupRow(e.RowHandle))
            {
                if (e.Info.IsRowIndicator)
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1;
                        e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    }
                    SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                    Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                    BeginInvoke(new MethodInvoker(delegate { cal(_Width, gridView1); }));
                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));
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

        private void btnok_Click(object sender, EventArgs e)
        {
            if (txtiddv.Text == "" || txtdate.Text == "")
            {
                Lotus.MsgBox.ShowWarningDialog("Thông tin chưa đầy đủ. Vui lòng kiểm tra lại");
                return;
            }

            db = new KetNoiDBDataContext();

            t_history hs = new t_history();

            var lst = from a in db.khoasochungtus where a.iddv == txtiddv.Text select a;

            if (lst.Count() <= 0) //them
            {
                var khoasochungtu2 = new khoasochungtu()
                {
                    iddv = txtiddv.Text,
                    ngaykhoaso = txtdate.DateTime
                };
                db.khoasochungtus.InsertOnSubmit(khoasochungtu2);
                db.SubmitChanges();
                txtiddv.Text = "";
                txtdate.Text = "";
                hs.add(txtiddv.Text, "Thêm mới Khoá Sổ - ... " + txtiddv.Text + " " + txtdate.Text);

                ShowAlert.Alert_Add_Success(this);
            }
            else //sua
            {
                var khoasochungtu = db.khoasochungtus.FirstOrDefault(x => x.iddv == txtiddv.Text);
                khoasochungtu.ngaykhoaso = txtdate.DateTime;
                db.SubmitChanges(); 
                hs.add(txtiddv.Text, "Sửa Khoá Sổ - ... " + txtiddv.Text + " " + txtdate.Text);

                ShowAlert.Alert_Edit_Success(this);
            }
            loaddata();
        }
    }
}
