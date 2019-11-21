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
using DevExpress.XtraGrid;
using DevExpress.XtraReports.UI;

namespace GUI
{
    public partial class f_PN : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        Boolean doubleclick = false;

        public f_PN()
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

                var lst = from a in db.r_pnhaps
                          join d in db.donvis on a.iddv equals d.id
                          where
                          a.ngaynhap >= tungay && a.ngaynhap <= denngay
                          select new
                          {
                              id = a.id,
                              ngaynhap = a.ngaynhap,
                              iddt = a.iddt,
                              ten = a.ten,
                              idnv = a.idnv,
                              iddv = a.iddv,
                              idcv = a.idcv,
                              dv = a.dv,
                              ghichu = a.ghichu,
                              loainhap = a.loainhap,
                              idsanpham = a.idsanpham,
                              soluong = a.soluong,
                              thanhtien = a.thanhtien,
                              noidung = a.diengiai,
                              tiente = a.tiente,
                              nguyente = a.nguyente,
                              MaTim = LayMaTim(d)
                          };
                var lst2 = lst.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

                gridControl1.DataSource = lst2;
            }
            catch (Exception ex)
            {
                Lotus.MsgBox.ShowErrorDialog(ex.ToString());
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
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Danh Sách Phiếu Nhập Kho").ToString();

            changeFont.Translate(this);
            changeFont.Translate(barManager1);

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

        private void btnIN_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);

            //
            gridView1.Columns["iddv"].GroupIndex = 1;
            gridView1.Columns["idsanpham"].GroupIndex = 2;
            //gridView1.Columns["voibom"].GroupIndex = 3;
            //gridView1.Columns["ngay"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            //gridView1.Columns["id"].Visible = false;//.OptionsColumn.AllowShowHide;
            gridView1.Columns["tiente"].Visible = false; //an cot
            gridView1.Columns["nguyente"].Visible = false; //an cot
            gridView1.Columns["thanhtien"].Visible = false; //an cot
            gridView1.Columns["idcv"].Visible = false; //an cot
            gridView1.ExpandAllGroups();

            gridView1.BestFitColumns();

            //check 
            r_dspn report = new r_dspn();
            report.GridControl = gridControl1;

            ReportPrintTool printTool = new ReportPrintTool(report);

            printTool.ShowPreviewDialog();
            gridView1.ClearGrouping();
            gridView1.ClearSorting();

            SplashScreenManager.CloseForm(false);
        }
    }
}