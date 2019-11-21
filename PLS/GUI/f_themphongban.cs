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
using DevExpress.Utils.Win;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using GUI.Properties;
using ControlLocalizer;
using GUI.Libs;

namespace GUI
{

    public partial class f_themphongban : Form
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        t_phongban dt = new t_phongban();
        public f_themphongban()
        {
            InitializeComponent(); 
        }

        private void f_themdoituong_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Thêm Phòng Ban").ToString();

            changeFont.Translate(this);
            changeFont.Translate(barManager1);

            if (Biencucbo.hdpb == 1)
            {
                txtid.Enabled = false;
                //var Lst = (from dt in db.phongbans where dt.id == Biencucbo.ma select dt).ToList();

                var lst = db.phongbans.FirstOrDefault(t => t.id == Biencucbo.ma);

                txtid.DataBindings.Clear();
                txtten.DataBindings.Clear();
                
                txtid.Text.Trim();
                txtid.Text = lst.id;
                txtten.Text = lst.ten; 
            }
        }
          

        private void btnhuy_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void luu_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (txtid.Text == "" || txtten.Text == "")
            {
                Lotus.MsgBox.ShowWarningDialog("Thông tin chưa đầy đủ - Vui lòng kiểm tra lại!");
            }
            else
            {
                if (Biencucbo.hdpb == 0)
                {
                    //khong cho trung ID va Ten
                    var Lst = (from dt in db.phongbans where dt.id == txtid.Text || dt.ten == txtten.Text select dt).ToList();

                    if (Lst.Count == 1)
                    {
                        Lotus.MsgBox.ShowErrorDialog("Phòng Ban này đã tồn tại, Vui Lòng Kiểm tra Lại");
                    }
                    else
                    {
                        dt.moi(txtid.Text.Trim(), txtten.Text);
                        this.Close();

                        ShowAlert.Alert_Add_Success(this);
                    }
                }
                else
                {
                    var Lst = (from l in db.phongbans where l.ten == txtten.Text && l.id != txtid.Text select l).ToList();

                    if (Lst.Count == 1)
                    {
                        Lotus.MsgBox.ShowErrorDialog("Phòng Ban này đã tồn tại, Vui Lòng Kiểm tra Lại");
                    }
                    else
                    {
                        //sua
                        dt.moi(txtid.Text.Trim(), txtten.Text);
                        this.Close();

                        ShowAlert.Alert_Edit_Success(this);
                    }
                }
            }
        }

        private void txtid_Leave(object sender, EventArgs e)
        {
            txtten.Text = txtid.Text;
        }
    }
}
