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
    public partial class r_ds_donvi : DevExpress.XtraReports.UI.XtraReport
    {
        public r_ds_donvi()
        {
            InitializeComponent();

            LanguageHelper.Translate(this);

            changeFont.Translate(this);

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