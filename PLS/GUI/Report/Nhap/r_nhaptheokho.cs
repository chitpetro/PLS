using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BUS;
using ControlLocalizer;

namespace GUI
{
    public partial class r_nhaptheokho : DevExpress.XtraReports.UI.XtraReport
    {
        public r_nhaptheokho()
        {
            InitializeComponent();
            LanguageHelper.Translate(this);

            changeFont.Translate(this);

            tran_rp.tran8(txtsp, txtkho, txtdoituong, txtcongviec, txtloainhap, txttime, ngay2, xrPageInfo2);

            if (Biencucbo.ngonngu.ToString() == "Lao") 
            {
                //change font
                XtraReport xtraReport = this;
                var list = xtraReport.AllControls<XRControl>();
                foreach (var c in list)
                {
                    if (c == xrTableCell27 || c == xrTableCell28 || c == xrTableCell30 || c == xrTableCell22 || c == xrTableCell24 || c == xrTableCell19 || c == xrTableCell18 || c == xrTableCell17 || c == xrTableCell4 || c == xrTableCell3 || c == xrTableCell2 || c == xrTableCell1 || c == xrTableCell35 || c == xrTableCell39)
                    {
                        c.Font = new System.Drawing.Font("Times New Roman", c.Font.Size, c.Font.Style);
                    }
                } 
            } 
        }
    }
}
