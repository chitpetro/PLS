using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid;
using BUS;
using ControlLocalizer;
using System.Drawing.Printing;

namespace GUI
{
    public partial class r_dscct : DevExpress.XtraReports.UI.XtraReport
    {
        public r_dscct()
        {
            InitializeComponent();

            LanguageHelper.Translate(this);

            changeFont.Translate(this);

            txttime.Text = Biencucbo.time;

            tran_rp.tran1(ngay2, xrPageInfo2);
        }

        private GridControl control;
        public GridControl GridControl
        {
            get
            {
                return control;
            }
            set
            {
                control = value;
                pccReport.PrintableComponent = control;
            }
        }
    }
}
