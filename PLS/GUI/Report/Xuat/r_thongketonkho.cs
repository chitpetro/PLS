using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ControlLocalizer;
using DAL;

namespace GUI.Report.Xuat
{
    public partial class r_thongketonkho : DevExpress.XtraReports.UI.XtraReport
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        public r_thongketonkho(object source)
        //public r_thongketonkho()
        {
            InitializeComponent();

            LanguageHelper.Translate(this);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Biểu đồ Tồn kho").ToString();

            changeFont.Translate(this);

            txtdonvi.Text = f_thongketonkho.tendv;
            txttime.Text = f_thongketonkho.n1.ToShortDateString() + " - " + f_thongketonkho.n2.ToShortDateString();

            //chartControl1.Series["Series 1"].DataSource = source;
            chartControl1.DataSource = source;
            //this.chartControl1.SeriesDataMember = "tensp";
            //this.chartControl1.SeriesTemplate.ArgumentDataMember = "iddv";

            //////this.chartControl1.SeriesTemplate.ArgumentDataMember = "{" + "iddv" + ":" + "tendonvi" + "}";

            //this.chartControl1.SeriesTemplate.ValueDataMembersSerializable = "thanhtien";

            //chartControl1.Series["Series 1"].LegendTextPattern = "{A} : {V:n3}";
            //chartControl1.Series["Series 1"].CrosshairLabelPattern = "{V:n3}";
            ////series1.LegendTextPattern = "{A:dd/MM/yyyy}: {V:n3}";
            //chartControl1.Series["Series 1"].LegendTextPattern = "{V:n3}";

        }
    }
}
