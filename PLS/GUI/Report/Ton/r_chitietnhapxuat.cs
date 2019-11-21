using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BUS;
using ControlLocalizer;

namespace GUI
{
    public partial class r_chitietnhapxuat : DevExpress.XtraReports.UI.XtraReport
    {
        public r_chitietnhapxuat()
        {
            InitializeComponent();
            LanguageHelper.Translate(this);

            changeFont.Translate(this);

            string a = string.Format("{0:n3}", Biencucbo.tondau);
            txttondau.Text = a + "  Lit";
            string b = string.Format("{0:n3}", Biencucbo.toncuoi);
            txttoncuoi.Text = b + "  Lit";

            tran_rp.tran4(txtkho, txtsp, txttime, ngay2, xrPageInfo2);

            if (Biencucbo.ngonngu.ToString() == "Lao") 
            { 
                XtraReport xtraReport = this;
                var list = xtraReport.AllControls<XRControl>();
                foreach (var c in list)
                {
                    if (c == xrTableCell27 || c == xrTableCell28 || c == xrTableCell29 || c == xrTableCell30 || c == xrTableCell32 || c == xrTableCell33 || c == xrTableCell34 || c == xrTableCell35 || c == xrTableCell36 || c == xrTableCell39 || c == xrTableCell40 || c == xrTableCell42 || c == txttondau || c == txttoncuoi)
                    {
                        c.Font = new System.Drawing.Font("Times New Roman", c.Font.Size, c.Font.Style);
                    }
                } 
            }
        }
    }
}
