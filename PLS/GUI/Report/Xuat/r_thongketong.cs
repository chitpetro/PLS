using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ControlLocalizer;

namespace GUI.Report.Xuat
{
    public partial class r_thongketong : DevExpress.XtraReports.UI.XtraReport
    {
        public r_thongketong(object source)
        {
            InitializeComponent();

            LanguageHelper.Translate(this);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Biểu đồ cột").ToString();

            changeFont.Translate(this);

            chartControl1.Series["Series 1"].DataSource = source;

            txtdonvi.Text = f_thongketong.tendv;
            txttime.Text = f_thongketong.n1.ToShortDateString() + " - " + f_thongketong.n2.ToShortDateString();
        } 
    }
}
