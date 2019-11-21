using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DAL;
using BUS;
using System.Data.Sql;
using ControlLocalizer;
using Lotus;

namespace GUI
{
    public partial class f_connectDB2 : XtraForm
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        public f_connectDB2()
        {
            InitializeComponent();
        }

        public bool KiemTraKetNoi()
        {
            string s = Properties.Settings.Default.thatluangplazaConnectionString;
            if (s == string.Empty) return false;

            // giải mã
            string conn = DecryptString(s, "petrolao");
            System.Data.SqlClient.SqlConnectionStringBuilder b = new SqlConnectionStringBuilder();
            b.ConnectionString = conn;
            Biencucbo.DbName = b.InitialCatalog;
            Biencucbo.ServerName = b.DataSource;
            SqlConnection sqlCon = new SqlConnection(conn);
             
            // gán cho DAL tren bo nhớ 
            DAL.Settings.Default.ConnectionString = conn;
            try
            {
                sqlCon.Open();
                db = new KetNoiDBDataContext(sqlCon);
                return true;
            }
            catch 
            { 
            }
            return false;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtDbName.Text == "")
            {
                XtraMessageBox.Show("Database name is not be empty", "Warning");
                return;
            }
            string thatluangplazaConnectionString_new = "";

            thatluangplazaConnectionString_new = "Data Source = " + txtServer.Text + "; Initial Catalog = " + txtDbName.Text + "; Persist Security Info = True; User ID = " + txtTen.Text + "; Password = " + txtPass.Text + "";

            SqlConnection sqlCon = new SqlConnection(thatluangplazaConnectionString_new);
            try
            {
                sqlCon.Open();

                db = new KetNoiDBDataContext(sqlCon);

                Lotus.MsgBox.ShowSuccessfulDialog("Connection succeeded");
                Biencucbo.DbName = txtDbName.Text;
                Biencucbo.ServerName = txtServer.Text;
                thoat_luon = true;
                // luu connstring mã hóa vào setting
                Properties.Settings.Default.thatluangplazaConnectionString = EncryptString(thatluangplazaConnectionString_new, "petrolao");
                DAL.Settings.Default.ConnectionString = thatluangplazaConnectionString_new;
              
                Properties.Settings.Default.Save();
                 
                //MsgBox.ShowWarningDialog("Reset to active");
                Application.Restart(); 

                DialogResult = DialogResult.OK;
            }
            catch
            {
                Lotus.MsgBox.ShowUnsuccessfulDialog("Connection failed, please check again or contact Admin");
                sqlCon.Close();
            }
        }

        private void f_connectDB_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "CẬP NHẬT KẾT NỐI CƠ SỞ DỮ LIỆU").ToString();

            changeFont.Translate(this); 

            txtDbName.Text = Biencucbo.DbName;
            if (Biencucbo.ServerName == "192.168.2.10")
            {
                rlan.Checked = true; 
            }
            else
            {
                rnet.Checked = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        bool thoat_luon = false;
        
        private void rlan_CheckedChanged(object sender, EventArgs e)
        {
            txtTen.ReadOnly = true;
            txtServer.Enabled = false; 
            txtPass.ReadOnly = true;

            if (rlan.Checked == true)
            {
                rnet.Checked = false;
                txtServer.Text = "192.168.2.10"; 
                txtTen.Text = "sa";
                txtPass.Text = "2267562676a@#$%";
            }
        }

        private void rnet_CheckedChanged(object sender, EventArgs e)
        {
            txtTen.ReadOnly = true;
            txtServer.Enabled = false; 
            txtPass.ReadOnly = true;
            if (rnet.Checked == true)
            {
                rlan.Checked = false;
                txtServer.Text = "183.182.109.4,1433"; 
                txtTen.Text = "sa";
                txtPass.Text = "2267562676a@#$%";
            }
        }

        //hàm mã hoá
        public static string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            System.Security.Cryptography.MD5CryptoServiceProvider HashProvider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            System.Security.Cryptography.TripleDESCryptoServiceProvider TDESAlgorithm = new System.Security.Cryptography.TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = System.Security.Cryptography.CipherMode.ECB;
            TDESAlgorithm.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            try
            {
                System.Security.Cryptography.ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return Convert.ToBase64String(Results);
        }
        //hàm giải mã
        public static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();


            System.Security.Cryptography.MD5CryptoServiceProvider HashProvider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            System.Security.Cryptography.TripleDESCryptoServiceProvider TDESAlgorithm = new System.Security.Cryptography.TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = System.Security.Cryptography.CipherMode.ECB;
            TDESAlgorithm.Padding = System.Security.Cryptography.PaddingMode.PKCS7;


            try
            {
                byte[] DataToDecrypt = Convert.FromBase64String(Message);
                System.Security.Cryptography.ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return UTF8.GetString(Results);
        }
    }
}