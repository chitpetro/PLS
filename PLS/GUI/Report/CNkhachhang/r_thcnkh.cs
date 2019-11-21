using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BUS;
using ControlLocalizer;

namespace GUI
{
    public partial class r_thcnkh : DevExpress.XtraReports.UI.XtraReport
    {
        public r_thcnkh()
        {
            InitializeComponent();
            LanguageHelper.Translate(this);

            changeFont.Translate(this);

            tran_rp.tran3(txtkho, txtdoituong, txttime, ngay2, xrPageInfo2);
        }
        int index = 0;
        int index2 = 0;
        string str1 = string.Empty;
        string str2 = string.Empty;


        private void sst_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (tid.Text != str1)
            {
                str1 = tid.Text;
                index++;
                index2 = 0;
            }
            sst.Text = index.ToString();
        }

        private void sst2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
             if (tiddv.Text != str2)
            {

                index2++;
            }

        
            sst2.Text = index.ToString() + "." + index2.ToString();
        }
    }
}
