using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BUS;
using ControlLocalizer;

namespace GUI
{
    public partial class r_sothuchi : DevExpress.XtraReports.UI.XtraReport
    {
        public r_sothuchi()
        {
            InitializeComponent();

            LanguageHelper.Translate(this);

            changeFont.Translate(this);
             
            string a = string.Format("{0:n2}", Biencucbo.tondau);
            txttondau.Text = a + "  KIP"; 
             string b = string.Format("{0:n2}", Biencucbo.toncuoi);
            txttoncuoi.Text = b +"  KIP";

            tran_rp.tran2(txtkho, txttime, ngay2, xrPageInfo2);

            if (Biencucbo.ngonngu.ToString() == "Lao") 
            { 
                XtraReport xtraReport = this;
                var list = xtraReport.AllControls<XRControl>();
                foreach (var c in list)
                {
                    if (c == xrTableCell19 || c == xrTableCell20 || c == xrTableCell21 || c == xrTableCell22 || c == xrTableCell24 || c == xrTableCell25 || c == xrTableCell26 || c == xrTableCell28)
                    {
                        c.Font = new System.Drawing.Font("Times New Roman", c.Font.Size, c.Font.Style);
                    }
                } 
            }
        }
    }
}
