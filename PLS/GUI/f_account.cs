﻿using System;
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
using DevExpress.XtraGrid.Views.Grid;
using ControlLocalizer;
using GUI.Libs;

namespace GUI
{
    public partial class f_account : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        t_account ac = new t_account();

        public f_account()
        {
            InitializeComponent();
            loadData();
        }

        t_tudong td = new t_tudong();
        private void loadData()
        {
            var load = (from a in db.accounts
                        join d in db.donvis on a.madonvi equals d.id
                        where a.madonvi == Biencucbo.donvi || d.iddv == Biencucbo.donvi
                        select new
                        {
                            loadid = a.id,
                            loaduname = a.uname,
                            loadname = a.name,
                            loadpass = a.pass,
                            loadphongban = a.phongban,
                            loadmadonvi = a.madonvi,
                            loaddonvi = d.tendonvi,
                            IsActived = a.IsActived
                        });
            dataaccount.DataSource = load;
        }
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Biencucbo.hdaccount = 0;
            string check = "NV" + Biencucbo.donvi.Trim().ToString();
            var Lst = (from s in db.tudongs where s.maphieu == check select new { so = s.so }).ToList();

            if (Lst.Count() == 0)
            {
                int so;
                Biencucbo.soaccount = 1;
                so = Biencucbo.soaccount + 1;
                td.themtudong(check, so);
            }
            else
            {
                int k;
                txt1.DataBindings.Clear();
                txt1.DataBindings.Add("text", Lst, "so");
                k = Convert.ToInt32(txt1.Text);

                Biencucbo.soaccount = k;
                k = k + 1;

                td.suatudong(check, k);
            }

            f_themaccount fr = new f_themaccount();
            fr.ShowDialog();

            loadData();
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            var q = Biencucbo.QuyenDangChon;
            if (q == null) return;
            dataaccount.Enabled = (bool)q.Xem;

            if ((bool)q.Them == true)
            {
                btnnew.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                btnnew.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            if ((bool)q.Sua == true)
            {
                btnsua.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                btnsua.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            if ((bool)q.Xoa == true)
            {
                btnxoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                btnxoa.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        //btn sua
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var q = Biencucbo.QuyenDangChon;

            //if (Biencucbo.ten == gridView2.GetFocusedRowCellValue("loadname").ToString() || Biencucbo.ten == "Admin")
            if (Biencucbo.ten == "Admin" || (Biencucbo.phongban == "IT" && q.Sua == true) || (Biencucbo.phongban == "Admin" && q.Sua == true))//sua het
            {
                Biencucbo.hdaccount = 1;
                Biencucbo.account = gridView2.GetFocusedRowCellValue("loadid").ToString();

                f_themaccount frm = new f_themaccount();
                frm.ShowDialog();

                loadData();

                ShowAlert.Alert_Edit_Success(this);
            }
            else if (q.Sua == true && Biencucbo.ten == gridView2.GetFocusedRowCellValue("loadname").ToString()) //sua 1
            {
                Biencucbo.hdaccount = 1;
                Biencucbo.account = gridView2.GetFocusedRowCellValue("loadid").ToString();

                f_themaccount frm = new f_themaccount();
                frm.ShowDialog();

                loadData();

                ShowAlert.Alert_Edit_Success(this);
            }
        }
        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            var q = Biencucbo.QuyenDangChon;

            //if (Biencucbo.ten == gridView2.GetFocusedRowCellValue("loadname").ToString() || Biencucbo.ten == "Admin")
            if (Biencucbo.ten == "Admin" || (Biencucbo.phongban == "IT" && q.Sua == true) || (Biencucbo.phongban == "Admin" && q.Sua == true))//sua het
            {
                Biencucbo.hdaccount = 1;
                Biencucbo.account = gridView2.GetFocusedRowCellValue("loadid").ToString();

                f_themaccount frm = new f_themaccount();
                frm.ShowDialog();

                loadData();
            }
            else if (q.Sua == true && Biencucbo.ten == gridView2.GetFocusedRowCellValue("loadname").ToString()) //sua 1
            {
                Biencucbo.hdaccount = 1;
                Biencucbo.account = gridView2.GetFocusedRowCellValue("loadid").ToString();

                f_themaccount frm = new f_themaccount();
                frm.ShowDialog();

                loadData();
            }
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            loadData();
        }

        //btn xoa
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Lotus.MsgBox.ShowYesNoCancelDialog("Bạn có chắc chắn muốn xóa tài khoản này khỏi hệ thống?") == DialogResult.Yes)
            {
                ac.xoa(gridView2.GetFocusedRowCellValue("loadid").ToString());
                loadData();
                ShowAlert.Alert_Del_Success(this);
            }
        }

        private void gridView2_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridView2.IsGroupRow(e.RowHandle))
            {
                if (e.Info.IsRowIndicator)
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1;
                        e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    }
                    SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                    Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                    BeginInvoke(new MethodInvoker(delegate { cal(_Width, gridView2); }));
                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1));
                SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                BeginInvoke(new MethodInvoker(delegate { cal(_Width, gridView2); }));
            }
        }
        bool cal(Int32 _Width, GridView _View)
        {
            _View.IndicatorWidth = _View.IndicatorWidth < _Width ? _Width : _View.IndicatorWidth;
            return true;
        }

        private void f_account_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Tài Khoản Đăng Nhập").ToString();

            changeFont.Translate(this);
            changeFont.Translate(barManager1);
        }
    }
}