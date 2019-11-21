using DAL;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DevExpress.UserSkins;

namespace GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CapNhatOnline();

            SplashScreenManager.ShowForm(typeof(SplashScreen1));
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load("appconn.xml");
            }
            catch
            {
                try
                {

                    string filepath = "appconn.xml";
                    WebClient webClient = new WebClient();
                    
                    webClient.DownloadFileAsync(new Uri("http://www.petrolao.com.la/config/CCS/appconn.xml"), filepath);
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }

            // kiem tra kêt nối nối
            var fketnoi = new f_connectDB();
            var isconnected = fketnoi.KiemTraKetNoi();
            if (isconnected == false)
            {
                XtraMessageBox.Show("Connection failed!");
                SplashScreenManager.CloseForm();
                if (fketnoi.ShowDialog() == DialogResult.Cancel)
                    return;
            }
            else
            {
                SplashScreenManager.CloseForm();
            }
            try
            {
                var tmpPath = Application.StartupPath + "\\tmp";
                Directory.Delete(tmpPath, true);
            }
            catch (Exception ex)
            {
            }

            BonusSkins.Register();

            // ok
            // hien form main
            Application.Run(new f_main());
        }

        private static void Completed(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download completed!");
        }

        public static void CapNhatOnline()
        {
            SplashScreenManager.ShowForm(typeof(SplashScreen1));

            //var app = String.Format("{0}\\{1}", Application.StartupPath, "Lotus.AutoUpdate.exe");
            var app = String.Format("{0}\\{1}", Application.StartupPath, "Lotus.AutoUpdate_eng.exe");
            if (!File.Exists(app)) return;

           // string host = "http://www.petrolao.com.la/config/info.xml";

            //update may Chi Huong Petro
            string host = "http://www.petrolao.com.la/config/PLS_dev18/info.xml";

            var info = new ProcessStartInfo();
            info.FileName = app;
            info.Arguments = string.Format("{0} {1} {2}",
                Assembly.GetExecutingAssembly().GetName().Name,
                Assembly.GetExecutingAssembly().GetName().Version,
                host);

            var process = Process.Start(info);
            if (process != null) process.WaitForExit();
            SplashScreenManager.CloseForm();
        }
    }
}