using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DAL;

namespace GUI
{
    public partial class frmSuaSo : Form
    {
        t_chotcongto ct = new t_chotcongto();
        public frmSuaSo()
        {
            InitializeComponent();
        }
        KetNoiDBDataContext db = new KetNoiDBDataContext();

        private void frmSuaSo_Load(object sender, EventArgs e)
        {
            txtid.Text = f_chotcongto.id;
            txtSo.Text = f_chotcongto.sott;
        }

        //sua so tt trong bảng chotcongto & chotcongtoct
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtSoMoi.Text == "0")
            {
                MessageBox.Show("Failed");
                this.Close();
            }
            else
            {
                try
                {
                    ct.suaSOchotcongtoct(txtid.Text.Trim(), int.Parse(txtSoMoi.Text));
                    ct.suaSOchotcongto(txtid.Text, int.Parse(txtSoMoi.Text));

                    MessageBox.Show("OK");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}