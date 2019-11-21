using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BUS;
using ControlLocalizer;
using System.Data;
using System.Windows.Forms;
using DAL;
namespace GUI
{
    public partial class r_xuattheokho : DevExpress.XtraReports.UI.XtraReport
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        public r_xuattheokho()
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
                    if (c == xrTableCell1 || c == xrTableCell2 || c == xrTableCell3 || c == xrTableCell4 || c == xrTableCell5 || c == xrTableCell17 || c == xrTableCell18 || c == xrTableCell19 || c == xrTableCell24 || c == xrTableCell26 || c == xrTableCell23 || c == xrTableCell39 || c == xrTableCell14 || c == xrTableCell35 || c == xrTableCell27 || c == xrTableCell28 || c == xrTableCell15 || c == xrTableCell30)
                    {
                        c.Font = new System.Drawing.Font("Times New Roman", c.Font.Size, c.Font.Style);
                    }
                } 
            }

            if (Biencucbo.donvi=="7520")
            {
                xrTableCell31.Visible = xrTableCell21.Visible = xrTableCell20.Visible = false;
                giamdoc.Visible = ketoan.Visible = nguoilap.Visible = true;
            }
        }
        
        private void xrTableCell2_PreviewDoubleClick(object sender, PreviewMouseEventArgs e)
        { 
            //MessageBox.Show(e.Brick.Text);

            //string a = e.Brick.Text;
             
            //a = a.Substring(0, 3);
             
            //MessageBox.Show(a);

            string soHD = e.Brick.Text.ToLower();
            string res = string.Empty;

            if (soHD.Contains("hd"))
                res = "HD";
            else if (soHD.Contains("pxk"))
                res = "PXK";


            MessageBox.Show(res);
        } 
    }
}
