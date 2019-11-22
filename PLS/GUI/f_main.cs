using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DAL;
using BUS;
using ControlLocalizer;
using DevExpress.XtraBars.Ribbon;
using System.Reflection;
using System.Diagnostics;
using GUI.Libs;
using GUI.Report.Xuat;
using GUI.Report.Nhap;
using GUI.Report.Ton;

namespace GUI
{
    public partial class f_main : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        t_chucnang t_cn = new t_chucnang();


        void OpenForm<T>()
        {
            var fm = MdiChildren.FirstOrDefault(f => f is T);
            if (fm == null)
            {
                fm = Activator.CreateInstance<T>() as Form;// tao đối tượng T thôi
                fm.MdiParent = this;
                fm.Show();
            }
            else
                fm.Activate();
        }
        public f_main()
        {
            InitializeComponent();
            defaultLookAndFeel1.LookAndFeel.SetSkinStyle(Biencucbo.skin);
        }

        private void f_main_Load(object sender, EventArgs e)
        {
            DangNhap();
            try
            {
                this.Show();
            }
            catch
            {
                Application.Exit();
            }
        }


        private void DangNhap()
        {
            // dang xuat
            

            try
            {
                foreach (Form form in MdiChildren)
                    form.Close();
                db.Dispose();
                db = new KetNoiDBDataContext();

                // dang nhap
                var f = new f_login();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    this.WindowState = FormWindowState.Maximized;
                    //if (Biencucbo.idnv.Trim() != "AD")
                    if (Biencucbo.phongban.Trim() != "Admin")
                    {
                        btnskinht.Visibility = BarItemVisibility.Never;
                    }

                    var lst = (from a in db.skins select a).Single(t => t.trangthai == true);
                    Biencucbo.skin = lst.tenskin;
                    defaultLookAndFeel1.LookAndFeel.SetSkinStyle(Biencucbo.skin);

                    //code moi 
                    LanguageHelper.Language = (ControlLocalizer.LanguageEnum)Biencucbo.ngonngu;

                    changeFont.Translate(this);
                    changeFont.Translate(ribbon);

                    LanguageHelper.Active(LanguageHelper.Language);
                    LanguageHelper.Translate(this);
                    LanguageHelper.Translate(ribbon);

                    this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "PetroLao Co.,Ltd").ToString();

                    var lst2 = (from a in db.donvis where a.id == Biencucbo.donvi select a.tendonvi).FirstOrDefault();
                    Biencucbo.tendvbc = lst2.ToString();

                    btninfo_account.Caption = LanguageHelper.TranslateMsgString("." + Name + "_btn_Wellcome", "Wellcome ") + Biencucbo.ten;
                    btninfo_donvi.Caption = LanguageHelper.TranslateMsgString("." + Name + "_btn_DonVi", "Đơn vị ") + Biencucbo.donvi + "-" + Biencucbo.tendvbc;
                    btninfo_phong.Caption = LanguageHelper.TranslateMsgString("." + Name + "_btn_BoPhan", "Bộ phận ") + Biencucbo.phongban;
                    btnDb.Caption = Biencucbo.DbName;
                    btnVersion.Caption = LanguageHelper.TranslateMsgString("." + Name + "_btn_Version", "Version ") +
                    Assembly.GetExecutingAssembly().GetName().Version.ToString();

                    // duyet ribbon
                    duyetRibbon(ribbon);


                    if (Biencucbo.ngonngu.ToString() == "Lao")
                    {
                        this.Font =
                        btninfo_account.ItemAppearance.Normal.Font =
                        btninfo_account.ItemAppearance.Disabled.Font =
                        btninfo_account.ItemAppearance.Hovered.Font =
                        btninfo_account.ItemAppearance.Pressed.Font =

                        btninfo_donvi.ItemAppearance.Normal.Font =
                        btninfo_donvi.ItemAppearance.Disabled.Font =
                        btninfo_donvi.ItemAppearance.Hovered.Font =
                        btninfo_donvi.ItemAppearance.Pressed.Font =

                        btninfo_phong.ItemAppearance.Normal.Font =
                        btninfo_phong.ItemAppearance.Disabled.Font =
                        btninfo_phong.ItemAppearance.Hovered.Font =
                        btninfo_phong.ItemAppearance.Pressed.Font =

                        btnDb.ItemAppearance.Normal.Font =
                        btnDb.ItemAppearance.Disabled.Font =
                        btnDb.ItemAppearance.Hovered.Font =
                        btnDb.ItemAppearance.Pressed.Font =

                        btnVersion.ItemAppearance.Normal.Font =
                        btnVersion.ItemAppearance.Disabled.Font =
                        btnVersion.ItemAppearance.Hovered.Font =
                        btnVersion.ItemAppearance.Pressed.Font = changeFont.FontLao;
                    }

                }
                else
                    Application.ExitThread();

            }
            catch (Exception ex)
            { Lotus.MsgBox.ShowErrorDialog(ex.ToString()); }
        }


        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_account>();
        }


        public void duyetRibbon(RibbonControl ribbonControl)
        {
            {
                foreach (RibbonPage page in ribbonControl.Pages)
                {
                    t_cn.moi(page.Name, page.Text, string.Empty);
                    foreach (RibbonPageGroup g in page.Groups)
                    {
                        t_cn.moi(g.Name, g.Text, page.Name);

                        foreach (BarItemLink i in g.ItemLinks)
                        {
                            if (i.Item == btndangxuat) continue;

                            t_cn.moi(i.Item.Name, i.Item.Caption, g.Name);

                            // lay quyen
                            //var quyen = db.PhanQuyen2s
                            //    .FirstOrDefault(p => p.TaiKhoan == Biencucbo.idnv && p.ChucNang == i.Item.Name);

                            var quyen = db.PhanQuyen2s
                               .FirstOrDefault(p => p.TaiKhoan == Biencucbo.phongban && p.ChucNang == i.Item.Name);

                            // cheat tài khoản quan tri
                            //if (Biencucbo.idnv == "AD")

                            if (Biencucbo.phongban == "Admin")
                            {
                                if (quyen == null)
                                {
                                    quyen = new PhanQuyen2();
                                    quyen.TaiKhoan = Biencucbo.phongban;
                                    quyen.ChucNang = i.Item.Name;

                                    quyen.Xem = quyen.Them = quyen.Sua = quyen.Xoa = true;

                                    db.PhanQuyen2s.InsertOnSubmit(quyen);
                                    db.SubmitChanges();
                                }
                            }

                            i.Item.Enabled = quyen == null ? false : Convert.ToBoolean(quyen.Xem);
                            // luu vào tag của nút tren ribbon de xu ly sau
                            i.Item.Tag = quyen;


                            if (i.Item is BarSubItem)
                            {
                                var sub = i.Item as BarSubItem;
                                sub.Enabled = true;
                                foreach (BarItemLink y in sub.ItemLinks)
                                {
                                    t_cn.moi(y.Item.Name, y.Item.Caption, i.Item.Name);
                                    // lay quyen
                                    //quyen = db.PhanQuyen2s
                                    //    .FirstOrDefault(p => p.TaiKhoan == Biencucbo.idnv && p.ChucNang == y.Item.Name);
                                    quyen = db.PhanQuyen2s
                                       .FirstOrDefault(p => p.TaiKhoan == Biencucbo.phongban && p.ChucNang == y.Item.Name);

                                    // cheat tài khoản quan tri
                                    //if (Biencucbo.idnv == "AD")
                                    if (Biencucbo.phongban == "Admin")
                                    {
                                        if (quyen == null)
                                        {
                                            quyen = new PhanQuyen2();
                                            //quyen.TaiKhoan = Biencucbo.idnv;
                                            quyen.TaiKhoan = Biencucbo.phongban;
                                            quyen.ChucNang = y.Item.Name;

                                            quyen.Xem = quyen.Them = quyen.Sua = quyen.Xoa = true;

                                            db.PhanQuyen2s.InsertOnSubmit(quyen);
                                            db.SubmitChanges();
                                        }
                                    }

                                    y.Item.Enabled = quyen == null ? false : Convert.ToBoolean(quyen.Xem);
                                    // luu vào tag của nút tren ribbon de xu ly sau
                                    y.Item.Tag = quyen;
                                }
                            }
                        }
                    }
                }
            }
        }


        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            string a = Biencucbo.ngonngu.ToString();
            string dlg = "";
            if (a == "Vietnam") dlg = "Bạn muốn đăng xuất?";
            if (a == "Lao") dlg = "ທ່ານຕ້ອງການລົງຊື່ອອກບໍ່?";

            if (Lotus.MsgBox.ShowYesNoDialog(dlg) == DialogResult.Yes)
            {
                this.Hide();
                DangNhap();
                try
                {
                    this.Show();
                }
                catch
                {
                    Application.Exit();
                }
            }
        }

        private void btndoidv_ItemClick(object sender, ItemClickEventArgs e)
        {
            account ac = (from a in db.accounts select a).Single(t => t.name == Biencucbo.ten);
            Biencucbo.dvTen = ac.madonvi;

            f_doidv frm = new f_doidv();
            frm.ShowDialog();
            var lst2 = (from a in db.donvis select a).Single(t => t.id == Biencucbo.donvi);
            btninfo_donvi.Caption = Biencucbo.donvi + " - " + lst2.tendonvi.ToString();
        }

        private void btndonvi_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_donvi>();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_doituong>();
        }

        private void barButtonItem4_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_sanpham>();
        }

        private void btnNhomDT_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_nhomdoituong>();
        }

        private void btnpnhap_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_pnhap>();
        }

        private void btnskinht_ItemClick(object sender, ItemClickEventArgs e)
        {
            f_Skin frm = new f_Skin();
            frm.ShowDialog();

        }

        private void btnxuat_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_hd>();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_pthu>();
        }

        private void btndoidonvi1_ItemClick(object sender, ItemClickEventArgs e)
        {
            account ac = (from a in db.accounts select a).Single(t => t.name == Biencucbo.ten);
            Biencucbo.dvTen = ac.madonvi;

            f_doidv frm = new f_doidv();
            frm.ShowDialog();
            var lst2 = (from a in db.donvis select a).Single(t => t.id == Biencucbo.donvi);
            btninfo_donvi.Caption = Biencucbo.donvi + " - " + lst2.tendonvi;

            ShowAlert.Alert_Change_Success(this);
        }

        private void btnpchi_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_pchi>();
        }

        private void btngiasp_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            f_giasp form = new f_giasp();
            form.ShowDialog();
        }

        private void btnsochitietnhapkho_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_chitietnhapkho frm = new Report.Nhap.f_chitietnhapkho();
            frm.ShowDialog();
        }

        private void btnnhaptheokho_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_nhaptheokho frm = new Report.Nhap.f_nhaptheokho();
            frm.ShowDialog();
        }

        private void btnsochitietxuatkho_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_chitietxuatkho frm = new Report.Nhap.f_chitietxuatkho();
            frm.ShowDialog();

        }

        private void btnxuattheokho_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_xuattheokho frm = new Report.Nhap.f_xuattheokho();
            frm.ShowDialog();
        }

        private void btnnhapxuatton_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_tonfifo frm = new Report.Nhap.f_tonfifo();
            frm.ShowDialog();
        }

        private void btnbcchi_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_chitietchi frm = new Report.Nhap.f_chitietchi();
            frm.ShowDialog();
        }

        private void btnthu_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_chitietpthu frm = new Report.Nhap.f_chitietpthu();
            frm.ShowDialog();
        }

        private void btnthuchi_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_thuchi frm = new Report.Nhap.f_thuchi();
            frm.ShowDialog();
        }

        private void btnctcnkh_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_ctcnkh frm = new Report.Nhap.f_ctcnkh();
            frm.ShowDialog();
        }

        private void btnctcnncc_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_ctcnncc frm = new Report.Nhap.f_ctcnncc();
            frm.ShowDialog();
        }

        private void btnthcnkh_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_thcnkh frm = new Report.Nhap.f_thcnkh();
            frm.ShowDialog();
        }

        private void btnthcnncc_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_thcnncc frm = new Report.Nhap.f_thcnncc();
            frm.ShowDialog();
        }

        private void btndoanhthu_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_doanhthu frm = new Report.Nhap.f_doanhthu();
            frm.ShowDialog();
        }

        private void btnnhapxuat_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            Report.Nhap.f_chitietnhapxuat frm = new Report.Nhap.f_chitietnhapxuat();
            frm.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_chotcongto>();
        }

        private void btnhis_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            f_History frm = new f_History();

            frm.ShowDialog();
        }

        private void btnngonngu_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            LanguageHelper.ShowTranslateTool();
        }

        private void f_main_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
        }

        private void btnclose_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Lotus.MsgBox.ShowYesNoCancelDialog("Bạn muốn thoát phần mềm?") == DialogResult.Yes)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
                db.Dispose();
                Application.Exit();
            }
        }

        private void btnmize_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btndanhmuctaikhoan_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_dmtk>();
        }

        private void btnphanquyen_ItemClick(object sender, ItemClickEventArgs e)
        {

            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            //OpenForm<FrmPhanQuyen>();
            OpenForm<frmPhanQuyenChucNang>();
        }

        private void btnketquakd_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
        }

        private void barButtonItem2_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_tiente>();
        }

        private void btnkiemke_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_kiemke>();
        }

        private void btnChuyenTien_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_noptienCN>();
        }

        private void btnTeamviewer_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Process.Start("tv.exe");
            }
            catch
            {
                XtraMessageBox.Show("Please setup Teamviewer !");
            }
        }

        private void btnkhoaso_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            f_khoaso frm = new f_khoaso();
            frm.ShowDialog();
        }

        private void btnPhongBan_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_phongban>();
        }

        private void btnthongkeDV_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            f_thongketong frm = new f_thongketong();
            frm.ShowDialog();
        }

        private void btnthongkeCN_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            f_thongke frm = new f_thongke();
            frm.ShowDialog();
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            f_thongketonkho frm = new f_thongketonkho();
            frm.ShowDialog();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            f_tontienmat f = new f_tontienmat();
            f.ShowDialog();
        }

        private void btnttoan_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_pttoan>();
        }

        private void btncantruconno_ItemClick(object sender, ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            OpenForm<f_pcantru>();
        }
    }
}