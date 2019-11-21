using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using ControlLocalizer;
using BUS;

namespace GUI
{
    public partial class r_pnhap : DevExpress.XtraReports.UI.XtraReport
    {
        public r_pnhap()
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
                    if (c == xrTableCell12 || c == xrTableCell1 || c == xrTableCell2 || c == xrTableCell6 || c == xrTableCell8 || c == xrTableCell9 || c == xrTableCell7 || c == xrTableCell10 || c == xrTableCell3 || c == xrTableCell5 || c == xrTableCell4 || c == xrTableCell17 || c == xrTableCell18 || c == xrTableCell19 || c == xrTableCell14)
                    {
                        c.Font = new System.Drawing.Font("Times New Roman", c.Font.Size, c.Font.Style);
                    }
                }


                foreach (var c in list)
                {
                    if (c == xrLabel11 || c == xrLabel7 || c == xrLabel5 || c == xrLabel21)
                    {
                        c.Font = new System.Drawing.Font(c.Font.Name, c.Font.Size, c.Font.Style);
                    }
                }
            }
        }
    }
}