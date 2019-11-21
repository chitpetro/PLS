using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ControlLocalizer;
using BUS;

namespace GUI
{
    public partial class r_pchi : DevExpress.XtraReports.UI.XtraReport
    {
        public r_pchi()
        {
            InitializeComponent();
            LanguageHelper.Translate(this);

            changeFont.Translate(this);

            tran_rp.tran1(ngay2, xrPageInfo2);

            if (Biencucbo.ngonngu.ToString() == "Lao")
            {
                XtraReport xtraReport = this;
                var list = xtraReport.AllControls<XRControl>();
                foreach (var c in list)
                {
                    if (c == xrTableCell1 || c == xrTableCell3 || c == xrTableCell4 || c == xrTableCell7 || c == xrTableCell8)
                    {
                        c.Font = new System.Drawing.Font("Times New Roman", c.Font.Size, c.Font.Style);
                    }
                } 
            } 
        }
    }
}
