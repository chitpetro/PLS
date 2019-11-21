using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using BUS;
using System.Data.Linq;
using System.Net;
using System.Globalization;
using System.Resources;
using DevExpress.Xpo;
using ControlLocalizer;
using DevExpress.XtraEditors;
using System.Threading;
using DevExpress.XtraSplashScreen;

namespace GUI
{
    public partial class f_login : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        t_account ac = new t_account();
        t_history hs = new t_history();
        public f_login()
        {
            InitializeComponent();

            var lst = (from a in db.skins select a).Single(t => t.trangthai == true);
            Biencucbo.skin = lst.tenskin;
            defaultLookAndFeel1.LookAndFeel.SetSkinStyle(Biencucbo.skin);
           
            //txtname.ForeColor = Color.LightGray;
            //txtname.Text = "Please Enter Name";

            //txtpass1.ForeColor = Color.LightGray;
            //txtpass1.Text = "*********";

            imageComboBoxEdit1.Properties.Items.AddEnum(typeof(LanguageEnum));
            LanguageHelper.Language = LanguageEnum.Vietnam;
            imageComboBoxEdit1.EditValue = LanguageHelper.Language;
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string dlg = "";
            if (Biencucbo.ngonngu.ToString() == "Vietnam") dlg = "Bạn muốn thoát phần mềm?";
            else dlg = "ທ່ານຕ້ອງການອອກຈາກລະບົບບໍ່?";

            if (Lotus.MsgBox.ShowYesNoDialog(dlg) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void txtpass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnlogin_Click(sender, e);
                
            }
        }
        private void txtuser_Leave(object sender, EventArgs e)
        {
            //if (txtname.Text == "")
            //{
            //    txtname.Text = "Please Enter Name";
            //    txtname.ForeColor = Color.Gray;
            //}
        }
        private void txtuser_Enter(object sender, EventArgs e)
        {
            //if (txtname.Text == "Please Enter Name")
            //{
            //    txtname.Text = "";
            //    txtname.ForeColor = Color.Black;
            //}
        }
        private void txtpass_Leave(object sender, EventArgs e)
        {
            //    if (txtpass1.Text == "")
            //    {
            //        txtpass1.Text = "*********";
            //        txtpass1.ForeColor = Color.Gray;
            //    }
        }
        private void txtpass_Enter(object sender, EventArgs e)
        {
            //if (txtpass1.Text == "*********")
            //{
            //    txtpass1.Text = "";
            //    txtpass1.ForeColor = Color.Black;
            //}
        }

        //language
        private void btnLangVI_Click(object sender, EventArgs e)
        {
            btnLangVI.Enabled = false;
            btnLangLA.Enabled = true;

            imageComboBoxEdit1_EditValueChanged(sender, e);
            LanguageHelper.Active((LanguageEnum)LanguageEnum.Vietnam);
            Biencucbo.ngonngu = LanguageEnum.Vietnam;
        }

        private void btnLangLA_Click(object sender, EventArgs e)
        {
            btnLangVI.Enabled = true;
            btnLangLA.Enabled = false;

            imageComboBoxEdit1_EditValueChanged(sender, e);
            LanguageHelper.Active((LanguageEnum)LanguageEnum.Lao);
            Biencucbo.ngonngu = LanguageEnum.Lao;
        }

        private void imageComboBoxEdit1_EditValueChanged(object sender, EventArgs e)
        {
            LanguageHelper.Active((LanguageEnum)imageComboBoxEdit1.EditValue);
        }

        private void f_login_Load(object sender, EventArgs e)
        {
            this.TransparencyKey = Color.Peru;
            this.BackColor = Color.Peru;

            btnLangVI_Click(sender, e);
            lbldb.Text = "Data: " + Biencucbo.DbName;
            txtuser1.Focus();
        
        }
        private void btnconnect_Click(object sender, EventArgs e)
        {
            f_connectDB frm = new f_connectDB();
            frm.ShowDialog();
            lbldb.Text = "Data: " + Biencucbo.DbName;
        }

      

        private void txtidnv_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnlgin_Click(object sender, EventArgs e)
        {
            //Kiểm tra Tên và Pass có tồn tại hay không 
            if (ac.dangnhap(txtuser1.Text, txtpass1.Text).Count == 0)
            {
                Lotus.MsgBox.ShowErrorDialog("Tài khoản hoặc mật khẩu không đúng! Vui lòng kiểm tra lại!");
                txtpass1.Text = "";
            }
            else
            {
                //kiểm tra Tài khoản có đang hoạt động không 
                if (ac.dangnhap2(txtuser1.Text, txtpass1.Text).Count == 0)
                {
                    Lotus.MsgBox.ShowWarningDialog("Tài khoản của bạn đang bị khoá! Vui lòng liên hệ Admin!");
                }
                else
                {
                    var Lst = (from s in db.accounts where s.uname == txtuser1.Text && s.pass == txtpass1.Text select s).Single();

                    dataGridView1.DataSource = Lst;

                    txtname.DataBindings.Clear();
                    txtphongban.DataBindings.Clear();
                    txtmadonvi.DataBindings.Clear();
                    txtidnv.DataBindings.Clear();

                    txtname.DataBindings.Add("text", Lst, "name");
                    txtphongban.DataBindings.Add("text", Lst, "phongban");
                    txtmadonvi.DataBindings.Add("text", Lst, "madonvi");
                    txtidnv.DataBindings.Add("text", Lst, "id");

                    // lấy thông tin máy
                    string hostname = "";
                    System.Net.IPHostEntry ip = new IPHostEntry();
                    hostname = System.Net.Dns.GetHostName();
                    ip = System.Net.Dns.GetHostByName(hostname);
                    Biencucbo.hostname = ip.HostName;

                    foreach (System.Net.IPAddress listip in ip.AddressList)
                    {
                        Biencucbo.IPaddress = listip.ToString();
                    }

                    Biencucbo.ten = Lst.name;
                    Biencucbo.dvTen = Lst.madonvi;
                    Biencucbo.phongban = Lst.phongban;
                    Biencucbo.donvi = Lst.madonvi;
                    Biencucbo.idnv = Lst.id;
                    hs.add("Login", "Đăng nhập - ລົງຊື່ເຂົ້າ");

                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void txtpass1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnlgin_Click(sender, e);

            }
        }
    }
}
