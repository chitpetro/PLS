using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BUS;
using ControlLocalizer;

namespace GUI
{
    public partial class r_ctcnncc : DevExpress.XtraReports.UI.XtraReport
    {
        public r_ctcnncc()
        {
            InitializeComponent();
            LanguageHelper.Translate(this);

            changeFont.Translate(this);
             
            if (Biencucbo.tondau > 0)
            {
                string a = string.Format("{0:n2}", Biencucbo.tondau);
                txtnodau.Text = a;
                txtcodau.Text = "0";

            }
            else
            {
                string a = string.Format("{0:n2}", Biencucbo.tondau * (-1));
                txtnodau.Text = "0";
                txtcodau.NullValueText = a;/*(double.Parse((Biencucbo.tondau * (-1)).ToString())).ToString();*/
            }
             

            if (Biencucbo.toncuoi > 0)
            {
                string a = string.Format("{0:n2}", Biencucbo.toncuoi);
                txtnocuoi.Text = a;
                txtcocuoi.Text = "0";

            }
            else
            {
                string a = string.Format("{0:n2}", Biencucbo.toncuoi * (-1));
                txtnocuoi.Text = "0";
                txtcocuoi.NullValueText = a;
            }

            tran_rp.tran3(txtkho, txtdoituong, txttime, ngay2, xrPageInfo2); 
        } 
    }
}
