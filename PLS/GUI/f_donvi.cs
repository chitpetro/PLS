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
using DevExpress.XtraGrid.Views.Grid;
using ControlLocalizer;
using GUI.Libs;
using DevExpress.XtraReports.UI;

namespace GUI
{
    public partial class f_donvi : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        t_donvi dv = new t_donvi();
        public f_donvi()
        {
            InitializeComponent();
            gridControl1.DataSource = new DAL.KetNoiDBDataContext().donvis;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Biencucbo.hddv = 0;
            f_themdonvi frm = new f_themdonvi();
            frm.ShowDialog();
            gridControl1.DataSource = new DAL.KetNoiDBDataContext().donvis;
        }
        // phân quyền 
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            var q = Biencucbo.QuyenDangChon;
            if (q == null) return;

            if ((bool)q.Them == true)
            {
                barButtonItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                barButtonItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            if ((bool)q.Sua == true)
            {
                barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            if ((bool)q.Xoa == true)
            {
                barButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                barButtonItem4.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Biencucbo.hddv = 1;
            Biencucbo.ma = gridView1.GetFocusedRowCellValue("id").ToString();
            f_themdonvi frm = new f_themdonvi();
            frm.ShowDialog();
            gridControl1.DataSource = new DAL.KetNoiDBDataContext().donvis;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Lotus.MsgBox.ShowYesNoCancelDialog("Bạn có chắc chắn muốn xóa đơn vị này không?") == DialogResult.Yes)
            {
                dv.xoa(gridView1.GetFocusedRowCellValue("id").ToString());
                ShowAlert.Alert_Del_Success(this);
            }
            gridControl1.DataSource = new DAL.KetNoiDBDataContext().donvis;
        }

        bool cal(Int32 _Width, GridView _View)
        {
            _View.IndicatorWidth = _View.IndicatorWidth < _Width ? _Width : _View.IndicatorWidth;
            return true;
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
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

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControl1.DataSource = new DAL.KetNoiDBDataContext().donvis;
        }

        private void f_donvi_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Đơn Vị").ToString();

            changeFont.Translate(this);
            changeFont.Translate(barManager1);
        }

        private void btnin_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            r_ds_donvi report = new r_ds_donvi();
            report.GridControl = gridControl1;

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
            gridView1.ClearGrouping();
            gridView1.ClearSorting();
        }
    }
}