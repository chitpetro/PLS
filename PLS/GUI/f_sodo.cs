using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DAL;
using DevExpress.XtraGrid.Views.Grid;
using ControlLocalizer;
using DevExpress.XtraEditors;
using GUI.Libs;

namespace GUI
{
    public partial class f_sodo : Form
    {
        t_sodo sp = new t_sodo();
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        public f_sodo()
        {
            InitializeComponent();

            var lst = new DAL.KetNoiDBDataContext().sodocotboms;
            try
            {
                var lst2 = (from a in lst where a.iddv == Biencucbo.donvi select a);
                gridControl1.DataSource = lst2;
                gridView1.ExpandAllGroups();
            }
            catch
            {
            }
        }

  
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Biencucbo.hdsd = 1;
                Biencucbo.ma = gridView1.GetFocusedRowCellValue("id").ToString();
                f_themsodo frm = new f_themsodo();
                frm.ShowDialog();

                var lst = new DAL.KetNoiDBDataContext().sodocotboms;

                var lst2 = (from a in lst where a.iddv == Biencucbo.donvi select a);
                gridControl1.DataSource = lst2;
                gridView1.ExpandAllGroups();
            }
            catch
            {
            }
        }
        // phân quyền 
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            var q = Biencucbo.QuyenDangChon;
            if (q == null) return;
             
            if ((bool)q.Sua == true)
            {
                btnsua.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                btnsua.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            if ((bool)q.Xoa == true)
            {
                btnxoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                btnxoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }
        private void btnthem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Biencucbo.hdsd = 0;
            f_themsodo frm = new f_themsodo();
            frm.ShowDialog();
            var lst = new DAL.KetNoiDBDataContext().sodocotboms;
            try
            {
                var lst2 = (from a in lst where a.iddv == Biencucbo.donvi select a);
                gridControl1.DataSource = lst2;
                gridView1.ExpandAllGroups();
            }
            catch
            {
            }
        }

        private void btnsua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Biencucbo.hdsd = 1;
            try
            {
                Biencucbo.ma = gridView1.GetFocusedRowCellValue("id").ToString();
                f_themsodo frm = new f_themsodo();
                frm.ShowDialog();
                var lst = new DAL.KetNoiDBDataContext().sodocotboms;

                var lst2 = (from a in lst where a.iddv == Biencucbo.donvi select a);
                gridControl1.DataSource = lst2;
                gridView1.ExpandAllGroups();
            }
            catch
            {
            }
        }

        private void btnxoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("Bạn có chắc chắn muốn xóa Sản phẩm này không?") == DialogResult.Yes)
                {
                    sp.xoa(Biencucbo.donvi, gridView1.GetFocusedRowCellValue("id").ToString());
                    ShowAlert.Alert_Del_Success(this);
                }
                var lst = new DAL.KetNoiDBDataContext().sodocotboms;

                var lst2 = (from a in lst where a.iddv == Biencucbo.donvi select a);
                gridControl1.DataSource = lst2;
                gridView1.ExpandAllGroups();
            }
            catch (Exception ex)
            {
                Lotus.MsgBox.ShowErrorDialog(ex.ToString());
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var lst = new DAL.KetNoiDBDataContext().sodocotboms;
            try
            {
                var lst2 = (from a in lst where a.iddv == Biencucbo.donvi select a);
                gridControl1.DataSource = lst2;
                gridView1.ExpandAllGroups();
            }
            catch
            {
            }
        }

        private void f_sodo_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Khai Báo Sơ Đồ Trụ Bơm").ToString();

            changeFont.Translate(this);
            changeFont.Translate(barManager1);
        }
    }
}

