using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using BUS;
using DAL;
using DevExpress.XtraEditors;

namespace GUI
{
    public partial class f_connectDB : DevExpress.XtraEditors.XtraForm
    {
        KetNoiDBDataContext dbData = new KetNoiDBDataContext();
        public f_connectDB()
        {
            InitializeComponent();
        }
        public bool KiemTraKetNoi()
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("appconn.xml");//mở file.xml lên
            var s = xmlDoc.DocumentElement["conn"].InnerText;
            if (s == string.Empty) return false;

            // giải mã
            var conn = custom.Decrypt(s);
            var b = new SqlConnectionStringBuilder();
            b.ConnectionString = conn;
            Biencucbo.DbName = b.InitialCatalog;
            Biencucbo.ServerName = b.DataSource;
            var sqlCon = new SqlConnection(conn);

            // gán cho DAL tren bo nhớ 
            DAL.Settings.Default.ConnectionString = conn;
            
            try
            {
                sqlCon.Open();
                dbData = new KetNoiDBDataContext(sqlCon);
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        private bool thoat_luon;

        private void btntestconnect_Click(object sender, EventArgs e)
        {
            if (txtdata.Text == "")
            {
                XtraMessageBox.Show("Database name is not be empty", "Warning");
                txtdata.ErrorText = "Database name is not be empty";
                return;
            }
            var ConnectionString_new = "";

            ConnectionString_new = "Data Source = " + txtserver.Text + "; Initial Catalog = " +
                                                 txtdata.Text + "; Persist Security Info = True; User ID = " +
                                                 txtuname.Text + "; Password = " + txtpass.Text + "";

            var sqlCon = new SqlConnection(ConnectionString_new);
            try
            {
                sqlCon.Open();
                //db = new KetNoiDBDataContext(sqlCon);

                XtraMessageBox.Show("Connection succeeded");
                //Settings.Default.Save();
            }
            catch
            {
                XtraMessageBox.Show("Connection failed, please check again or contact Admin");
                sqlCon.Close();
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (txtdata.Text == "")
            {
                XtraMessageBox.Show("Database name is not be empty", "Warning");
                txtdata.ErrorText = "Database name is not be empty";
                return;
            }
            var ConnectionString_new = "";

            ConnectionString_new = "Data Source = " + txtserver.Text + "; Initial Catalog = " +
                                                 txtdata.Text + "; Persist Security Info = True; User ID = " +
                                                 txtuname.Text + "; Password = " + txtpass.Text + "";

            var sqlCon = new SqlConnection(ConnectionString_new);
            try
            {
                sqlCon.Open();

                dbData = new KetNoiDBDataContext(sqlCon);

                XtraMessageBox.Show("Connection succeeded");
                Biencucbo.DbName = txtdata.Text;
                Biencucbo.ServerName = txtserver.Text;
                thoat_luon = true;
                // luu connstring mã hóa vào setting
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("appconn.xml");//mở file.xml lên
                xmlDoc.DocumentElement["conn"].InnerText = custom.Encrypt(ConnectionString_new);
                xmlDoc.Save("appconn.xml");
                DAL.Settings.Default.ConnectionString = ConnectionString_new;

                //Settings.Default.Save();

                DialogResult = DialogResult.OK;
            }
            catch
            {
                XtraMessageBox.Show("Connection failed, please check again or contact Admin");
                sqlCon.Close();
            }
        }

        private void f_connectDB_Load(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("appconn.xml");//mở file.xml lên
            var s = xmlDoc.DocumentElement["conn"].InnerText;
            if (s == string.Empty)
            {
                txtdata.Text = "PLS_2017";

                txtserver.Text = "192.168.2.10";

                txtuname.Text = "sa";

                txtpass.Text = "2267562676a@#$%";
                rdgmode.SelectedIndex = 0;
                return ;
            }


            // giải mã
            var conn = custom.Decrypt(s);
            var b = new SqlConnectionStringBuilder();
            b.ConnectionString = conn;
            txtdata.Text = b.InitialCatalog;
            txtserver.Text = b.DataSource;
            txtuname.Text = b.UserID;
            txtpass.Text = b.Password;
          
                txtdata.Text = "PLS_2017";
          
                txtserver.Text = "192.168.2.10";
         
                txtuname.Text = "sa";
            
                txtpass.Text = "2267562676a@#$%";

            if (txtserver.Text == "192.168.2.10")
            {
                rdgmode.SelectedIndex = 0;
            }
            else
            {
                rdgmode.SelectedIndex = 1;
            }
            
        }

        private void rdgmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (rdgmode.SelectedIndex)
            {
                case 0:
                    txtserver.Text = "192.168.2.10";
                    break;
                case 1:
                    txtserver.Text = "183.182.109.4";
                    break;
            }

        }
    }
}