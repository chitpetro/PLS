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
using GUI.Libs;

namespace GUI
{
    public partial class f_phongban : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        t_phongban dt = new t_phongban();
        public f_phongban()
        {
            InitializeComponent();
            gridControl1.DataSource = new DAL.KetNoiDBDataContext().phongbans;
        }
        // phân quyền 
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            var q = Biencucbo.QuyenDangChon;
            if (q == null) return;

            if ((bool)q.Them == true)
            {
                btnThem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                btnThem.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            if ((bool)q.Sua == true)
            {
                btnSua.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                btnSua.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            if ((bool)q.Xoa == true)
            {
                btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                btnXoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        //btnThem
        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Biencucbo.hdpb = 0;
            f_themphongban frm = new f_themphongban();
            frm.ShowDialog();
            gridControl1.DataSource = new DAL.KetNoiDBDataContext().phongbans;
        }
        //btnSua
        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Biencucbo.hdpb = 1;
            Biencucbo.ma = gridView1.GetFocusedRowCellValue("id").ToString();
            f_themphongban frm = new f_themphongban();
            frm.ShowDialog();
            gridControl1.DataSource = new DAL.KetNoiDBDataContext().phongbans;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            Biencucbo.hdpb = 1;
            Biencucbo.ma = gridView1.GetFocusedRowCellValue("id").ToString();
            var q = Biencucbo.QuyenDangChon;
            if (q == null) return;
            if (q.Sua == true)
            {
                f_themphongban frm = new f_themphongban();
                frm.ShowDialog();
            }
            gridControl1.DataSource = new DAL.KetNoiDBDataContext().phongbans;
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

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string id = gridView1.GetFocusedRowCellValue("id").ToString();
            var items = db.accounts.FirstOrDefault(t => t.phongban == id);
            if (items == null)
            {
                if (Lotus.MsgBox.ShowYesNoCancelDialog("Bạn có chắc chắn muốn xóa Phòng Ban này không?") == DialogResult.Yes)
                {
                    dt.xoa(id);
                    ShowAlert.Alert_Del_Success(this);
                }
                gridControl1.DataSource = new DAL.KetNoiDBDataContext().phongbans;
            }
            else
            {
                Lotus.MsgBox.ShowErrorDialog("Không Thể Xoá!");
            }
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridControl1.DataSource = new DAL.KetNoiDBDataContext().phongbans;
        }

        private void f_doituong_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Phòng Ban").ToString();

            changeFont.Translate(this);
            changeFont.Translate(barManager1);
        }
    }
}

