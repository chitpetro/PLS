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
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;

namespace GUI
{
    public partial class f_dshd : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        Boolean doubleclick = false;

        public f_dshd()
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

                var lst = from a in db.r_pxuats
                              //join d in db.donvis on a.iddv equals d.id
                          where
                          a.ngayhd >= tungay && a.ngayhd <= denngay

                          select new
                          {
                              id = a.id,
                              ngayhd = a.ngayhd,
                              iddt = a.iddt,
                              idnv = a.idnv,
                              iddv = a.iddv,
                              idcv = a.idcv,
                              ten = a.ten,
                              diengiai = a.diengiai,
                              //noidung =  (a.chietkhau * a.soluong) +a.thanhtien,
                              dv = a.dv,
                              link = a.link,
                              ghichu = a.ghichu,
                              loaixuat = a.loaixuat,
                              idsanpham = a.idsanpham,
                              soluong = a.soluong,
                              thanhtien = a.thanhtien,
                              tiente = a.tiente,
                              dongia = a.dongia,
                              nguyente = a.nguyente,
                              chietkhau = a.chietkhau * a.soluong,
                              //MaTim = LayMaTim(d)
                          };
                //var lst2 = lst.ToList().Where(t => t.MaTim.Contains("." + Biencucbo.donvi + "."));

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

                //gridControl1.DataSource = lst2;
            }
            catch (Exception ex)
            {
                Lotus.MsgBox.ShowErrorDialog(ex.ToString());
            }
            SplashScreenManager.CloseForm(false);
        }

        //private string LayMaTim(donvi d)
        //{
        //    string s = "." + d.id + "." + d.iddv + ".";
        //    var find = db.donvis.FirstOrDefault(t => t.id == d.iddv);
        //    if (find != null)
        //    {
        //        string iddv = find.iddv;
        //        if (d.id != find.iddv)
        //        {
        //            if (!s.Contains(iddv))
        //                s += iddv + ".";
        //        }
        //        while (iddv != find.id)
        //        {
        //            if (!s.Contains(find.id))
        //                s += find.id + ".";
        //            find = db.donvis.FirstOrDefault(t => t.id == find.iddv);
        //        }
        //    }
        //    return s;
        //}

        private void f_PN_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Danh Sách Hoá Đơn").ToString();

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

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            doubleclick = true;
        }

        private void btnin_Click(object sender, EventArgs e)
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

            ////
            //gridView1.Columns["iddv"].GroupIndex = 1;
            //gridView1.Columns["ngayhd"].GroupIndex = 2;
            ////gridView1.Columns["voibom"].GroupIndex = 3;
            ////gridView1.Columns["ngay"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            ////gridView1.Columns["id"].Visible = false;//.OptionsColumn.AllowShowHide;
            //gridView1.Columns["idcv"].Visible = false; //an cot sai so
            //gridView1.Columns["link"].Visible = false;
            //gridView1.Columns["ten"].Width = 100;
            ////gridView1.ExpandAllGroups();

            //gridView1.BestFitColumns();


            ////check 
            //r_dshd report = new r_dshd();
            //report.GridControl = gridControl1;

            //ReportPrintTool printTool = new ReportPrintTool(report);
            ////printTool.PrintingSystem.PageMargins.Right = 0;

            //printTool.ShowPreviewDialog();
            //gridView1.ClearGrouping();
            //gridView1.ClearSorting();
            //gridView1.Columns["id"].Visible = true;

            try
            {
                //reset db
                db.CommandTimeout = 0;

                var lst = from a in db.r_pxuats
                          join d in db.donvis on a.iddv equals d.id
                          where
                          a.ngayhd >= tungay.DateTime && a.ngayhd <= denngay.DateTime
                          select new
                          {
                              id = a.id,
                              ngayhd = a.ngayhd,
                              iddt = a.iddt,
                              idnv = a.idnv,
                              iddv = a.iddv,
                              idcv = a.idcv,
                              ten = a.ten,
                              //noidung = a.diengiai,
                              diengiai = a.diengiai,
                              dv = a.dv,
                              link = a.link,
                              ghichu = a.ghichu,
                              loaixuat = a.loaixuat,
                              idsanpham = a.idsanpham,
                              soluong = a.soluong,
                              thanhtien = a.thanhtien,
                              tiente = a.tiente,
                              dongia = a.dongia,
                              nguyente = a.nguyente,
                              chietkhau = a.chietkhau * a.soluong,
                              tendonvi2 = a.iddv + " - " + d.tendonvi
                          };

                if (Biencucbo.donvi.Length == 2)
                {
                    if (Biencucbo.donvi != "00")
                    {
                        var lst2t = from a in db.donvis where a.iddv == Biencucbo.donvi select a;
                        var lst2 = (from a in lst
                                    join b in lst2t on a.iddv equals b.id
                                    select a);

                        //loc du lieu
                        if (gridView1.FilterPanelText != "")
                        {
                            gridView1.BestFitColumns();

                            //check 
                            gridView1.Columns["tendonvi2"].GroupIndex = 1;
                            gridView1.Columns["iddv"].Visible = false;
                            gridView1.Columns["tiente"].Visible = false;
                            gridView1.Columns["idcv"].Visible = false;
                            gridView1.Columns["link"].Visible = false;

                            r_dshd3 report = new r_dshd3();
                            report.GridControl = gridControl1;
                            ReportPrintTool printTool = new ReportPrintTool(report);
                            gridView1.ExpandAllGroups();
                            printTool.ShowPreviewDialog();
                        }
                        else
                        {
                            r_dshd2 r = new r_dshd2();
                            r.DataSource = lst2;
                            r.ShowPreviewDialog();
                        }
                    }
                    else
                    {
                        //loc du lieu
                        if (gridView1.FilterPanelText != "")
                        {
                            gridView1.BestFitColumns();

                            //check 
                            gridView1.Columns["tendonvi2"].GroupIndex = 1;
                            gridView1.Columns["iddv"].Visible = false;
                            gridView1.Columns["tiente"].Visible = false;
                            gridView1.Columns["idcv"].Visible = false;
                            gridView1.Columns["link"].Visible = false;

                            r_dshd3 report = new r_dshd3();
                            report.GridControl = gridControl1;
                            ReportPrintTool printTool = new ReportPrintTool(report);
                            gridView1.ExpandAllGroups();
                            printTool.ShowPreviewDialog();
                        }
                        else
                        {
                            r_dshd2 r = new r_dshd2();
                            r.DataSource = lst;
                            r.ShowPreviewDialog();
                        }
                    }
                }
                else
                {
                    //loc du lieu
                    if (gridView1.FilterPanelText != "")
                    {
                        gridView1.BestFitColumns();

                        //check 
                        gridView1.Columns["tendonvi2"].GroupIndex = 1;
                        gridView1.Columns["iddv"].Visible = false;
                        gridView1.Columns["tiente"].Visible = false;
                        gridView1.Columns["idcv"].Visible = false;
                        gridView1.Columns["link"].Visible = false;
                        r_dshd3 report = new r_dshd3();
                        report.GridControl = gridControl1;
                        ReportPrintTool printTool = new ReportPrintTool(report);
                        gridView1.ExpandAllGroups();
                        printTool.ShowPreviewDialog();
                    }
                    else
                    {
                        var lst3 = from a in lst where a.iddv == Biencucbo.donvi select a;

                        r_dshd2 r = new r_dshd2();
                        r.DataSource = lst3;
                        r.ShowPreviewDialog();
                    }
                }
            }
            catch { }

            SplashScreenManager.CloseForm(false);
        }
    }
}