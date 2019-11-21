using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BUS;
using ControlLocalizer;

namespace GUI
{
    public partial class r_chitietxuatkho2 : DevExpress.XtraReports.UI.XtraReport
    {
        public r_chitietxuatkho2()
        {
            InitializeComponent();

            LanguageHelper.Translate(this);

            changeFont.Translate(this);

            tran_rp.tran9(txtsp, txtkho, txtdoituong, txtcongviec, txtloaixuat, txttime, ngay2, xrPageInfo2);

            if (Biencucbo.ngonngu.ToString() == "Lao")
            {
                XtraReport xtraReport = this;
                var list = xtraReport.AllControls<XRControl>();
                foreach (var c in list)
                {
                    if (c == xrTableCell1 || c == xrTableCell17 || c == xrTableCell39)
                    {
                        c.Font = new System.Drawing.Font("Times New Roman", c.Font.Size, c.Font.Style);
                    }
                }
            }

            if (Biencucbo.donvi == "7520")
            {
                xrTableCell20.Visible = xrTableCell15.Visible = xrTableCell27.Visible = false;
                giamdoc.Visible = ketoan.Visible = nguoilap.Visible = true;
            }

        }
    }
}
