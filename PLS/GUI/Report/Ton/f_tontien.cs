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
using DevExpress.XtraReports.UI;
using BUS;
using ControlLocalizer;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace GUI.Report.Nhap
{
    public partial class f_tontien : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        Boolean doubleclick1 = false;
        Boolean doubleclick2 = false;

        public f_tontien()
        {
            InitializeComponent();
            rTime.SetTime(thoigian);
        }

        private void f_chitietnhapkho_Load(object sender, EventArgs e)
        {
            //LanguageHelper.Translate(this);
            //this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "BÁO CÁO TỒN TIỀN MẶT").ToString();

            //changeFont.Translate(this);

            //translate text
            labelControl1.Text = LanguageHelper.TranslateMsgString(".reportKhoangThoiGian", "Khoảng Thời Gian").ToString();
            labelControl2.Text = LanguageHelper.TranslateMsgString(".reportTuNgay", "Từ Ngày").ToString();
            labelControl3.Text = LanguageHelper.TranslateMsgString(".reportDenNgay", "Đến Ngày").ToString();

            tungay.ReadOnly = true;
            denngay.ReadOnly = true;


            rTime.SetTime2(thoigian);

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
        private void thoigian_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeTime.thoigian_change3(thoigian, tungay, denngay);
        }
        public string laytendv(string f)
        {
            string b = "";
            try
            {
                var lst = (from a in db.donvis select a).Single(t => t.id == f);
                b = lst.tendonvi;
            }
            catch
            { }
            return b;
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(SplashScreen2), true, true, false);

            try
            {
                //reset db
                db.CommandTimeout = 0;

                //db = new KetNoiDBDataContext();

                Biencucbo.sp = "";
                Biencucbo.doituong = "";
                Biencucbo.congviec = "";
                Biencucbo.kho = "";

                var listc = "";
                var list0 = "";

                //fifostock:
                var lstTThu = (from a in db.pthus
                               join b in db.pthucts on a.id equals b.idthu into g
                               from sub in g.DefaultIfEmpty()
                               where a.ngaythu < tungay.DateTime && a.iddv.Length > 2
                               select new
                               {
                                   a.iddv,

                                   tt = sub.thanhtien == null ? 0 : sub.thanhtien
                               }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, ttien = c.Sum(t => t.tt) });

                var lstTChi = (from a in db.pchis
                               join b in db.pchicts on a.id equals b.idchi into g
                               from sub in g.DefaultIfEmpty()
                               where a.ngaychi < tungay.DateTime && a.iddv.Length > 2
                               select new
                               {
                                   a.iddv,

                                   tt = sub.sotien == null ? 0 : sub.sotien
                               }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, ttien = c.Sum(t => t.tt) });


                var tondauki = (from a in db.donvis
                                join b in lstTThu on a.id equals b.iddv into k
                                join c in lstTChi on a.id equals c.iddv into l
                                from thud in k.DefaultIfEmpty()
                                from chid in l.DefaultIfEmpty()
                                where a.id.Length > 2
                                select new
                                {
                                    iddv = a.id,

                                    tt = (thud.ttien == null ? 0 : thud.ttien) - (chid.ttien == null ? 0 : chid.ttien),
                                });

                // thu trong ky
                var thu = (from a in db.donvis
                           join b in db.r_pthus on a.id equals b.iddv into g
                           from thutk in g.DefaultIfEmpty()
                           where thutk.ngaythu >= tungay.DateTime && thutk.ngaythu <= denngay.DateTime && a.id.Length > 2
                           select new
                           {
                               iddv = a.id,

                               gtthu = thutk.thanhtien,

                           }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, gt = c.Sum(t => t.gtthu) });

                // chi trong ky
                var chi = (from a in db.donvis
                           join b in db.r_pchis on a.id equals b.iddv into g
                           from chitk in g.DefaultIfEmpty()
                           where chitk.ngaychi >= tungay.DateTime && chitk.ngaychi <= denngay.DateTime && a.id.Length > 2
                           select new
                           {
                               iddv = a.id,

                               gtchi = chitk.sotien,

                           }).GroupBy(t => t.iddv).Select(c => new { iddv = c.Key, gt = c.Sum(t => t.gtchi) });

                var listc2 = (from a in db.donvis
                              join b in tondauki on a.id equals b.iddv
                              join c in thu on a.id equals c.iddv
                              join d in chi on a.id equals d.iddv
                              where a.id.Length > 2

                              select new
                              {
                                  iddv = a.id,
                                  tendonvi = a.tendonvi,
                                  idcn = laytendv(a.iddv),
                                  tondauki = b.tt,
                                  thutk = c.gt,
                                  chitk = d.gt,
                                  toncuoiki = b.tt + c.gt - d.gt,
                              });//.ToList();//.GroupBy(t => t.idcn);


                // gridView2.Columns["idcn"].GroupIndex = 1;
                //// gridView2.Columns["cotbom"].GroupIndex = 2;
                //// gridView2.Columns["voibom"].GroupIndex = 3;
                //// gridView2.Columns["ngay"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                // //gridView2.Columns["id"].Visible = false;//.OptionsColumn.AllowShowHide;
                //                                         //gridView2.Columns["saiso"].Visible = false; //an cot sai so
                // gridView2.ExpandAllGroups();

                // gridView2.BestFitColumns();
                // //gridControl1.DataSource = (from a in listc2 select a).GroupBy(t => t.idcn);
                // gridControl1.DataSource = listc2;


                // //check


                //r_tontien2 report = new r_tontien2();
                //report.GridControl = gridControl1;

                //ReportPrintTool printTool = new ReportPrintTool(report);
                ////printTool.PrintingSystem.PageMargins.Right = 0;

                //printTool.ShowPreviewDialog();
                //gridView2.ClearGrouping();
                //gridView2.ClearSorting();
                ////gridView2.Columns["id"].Visible = true;


                r_tontien xtra = new r_tontien();
                //xtra.DataSource = listc;
                xtra.DataSource = listc2;
                xtra.ShowPreviewDialog();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            SplashScreenManager.CloseForm(false);
        }
    }
}