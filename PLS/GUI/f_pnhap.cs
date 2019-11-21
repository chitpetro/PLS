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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using System.Data.Linq;
using DevExpress.XtraReports.UI;
using ControlLocalizer;
using GUI.Libs;

namespace GUI
{
    public partial class f_pnhap : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        t_pnhap pnct = new t_pnhap();

        //form
        public f_pnhap()
        {
            InitializeComponent();

            var lst = (from a in db.r_giasps where a.iddv == Biencucbo.donvi select new { id = a.idsp, tensp = a.tensp, dvt = a.dvt });
            if (lst == null) return;
            btnmasp1.DataSource = lst;
            rsearchTenSP.DataSource = btnmasp1.DataSource;
            btndvt.DataSource = btnmasp1.DataSource;

            txttiente.Properties.DataSource = new DAL.KetNoiDBDataContext().tientes;

            txttygia.Properties.Mask.EditMask = "n";

            btnthue1.DataSource = new DAL.KetNoiDBDataContext().thues;
            rsearchthuesuat.DataSource = btnthue1.DataSource;
            btncongviec1.DataSource = new DAL.KetNoiDBDataContext().congviecs;
            txtiddt.Properties.DataSource = new DAL.KetNoiDBDataContext().doituongs;

            txtloainhap.Properties.DataSource = new DAL.KetNoiDBDataContext().dmpnhaps;

            if (Biencucbo.ngonngu.ToString() == "Vietnam")
            {
                coldiengiai.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "diengiai", "Tổng cộng:")});
                gridColumn49.Caption = "Danh mục";
                gridColumn50.Caption = "Danh mục";
                gridColumn35.Caption = "Mã Đối Tượng";
                gridColumn36.Caption = "Tên Đối Tượng ";
                gridColumn37.Caption = "Nhóm Đối Tượng";
                gridColumn38.Caption = "Loại Đối Tượng";
                gridColumn39.Caption = "Địa Chỉ";
                gridColumn46.Caption = "Tiền tệ";
                gridColumn47.Caption = "Tỷ giá";
                gridColumn48.Caption = "Ghi chú";
                gridColumn27.Caption = "Mã Sản Phẩm";
                gridColumn28.Caption = "Tên Sản Phẩm";
                gridColumn29.Caption = "ĐVT";
                gridColumn44.Caption = "Mã công việc";
                gridColumn42.Caption = "Tên công việc";
                gridColumn43.Caption = "Nhóm công việc";
            }
            else //lao
            {
                coldiengiai.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "diengiai", "ລວມທັງໝົດ:")});
                gridColumn49.Caption = "ລາຍການ";
                gridColumn50.Caption = "ລາຍການ";
                gridColumn35.Caption = "ລະຫັດ";
                gridColumn36.Caption = "ຊື່ເປົ້າໝາຍ";
                gridColumn37.Caption = "ກຸ່ມເປົ້າໝາຍ";
                gridColumn38.Caption = "ປະເພດເປົ້າໝາຍ";
                gridColumn39.Caption = "ທີ່ຢູ່";
                gridColumn46.Caption = "ເງິນຕາ";
                gridColumn47.Caption = "ອັດຕາ";
                gridColumn48.Caption = "ໝາຍເຫດ";
                gridColumn27.Caption = "ລະຫັດ";
                gridColumn28.Caption = "ຜະລິດຕະພັນ";
                gridColumn29.Caption = "ຫົວໜ່ວຍຄິດໄລ່";
                gridColumn44.Caption = "ລະຫັດວຽກງານ";
                gridColumn42.Caption = "ຊື່ວຽກງານ";
                gridColumn43.Caption = "ກຸ່ມໜ້າວຽກ";
            }
        }
        //load
        public void load()
        {
            this.label1.BackColor = System.Drawing.Color.Transparent;
            db = new KetNoiDBDataContext();
            Biencucbo.hdpn = 2;
            txt1.Enabled = false;

            btnLuu.Enabled = false;
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            btnin.Enabled = true;
            btnreload.Enabled = false;

            txtdv.ReadOnly = true;
            txtid.ReadOnly = true;
            txtdiachi.ReadOnly = true;
            txtidnv.ReadOnly = true;
            txtphongban.ReadOnly = true;
            txttiente.ReadOnly = true;
            txttygia.ReadOnly = true;

            // Enable
            txtghichu.ReadOnly = true;
            txtngaynhap.ReadOnly = true;
            txtiddt.ReadOnly = true;
            txtloainhap.ReadOnly = true;

            gridView1.OptionsBehavior.Editable = false;

            try
            {
                var lst = (from a in db.pnhaps where a.iddv == Biencucbo.donvi select a.so).Max();
                var lst1 = (from b in db.pnhaps where b.iddv == Biencucbo.donvi select b).FirstOrDefault(t => t.so == lst);
                if (lst1 == null) return;

                gcchitiet.DataSource = lst1.pnhapcts;

                txtid.Text = lst1.id;

                txtidnv.Text = lst1.idnv;
                txtdv.Text = lst1.iddv;
                txtngaynhap.DateTime = DateTime.Parse(lst1.ngaynhap.ToString());
                txtiddt.Text = lst1.iddt;
                txttiente.Text = lst1.tiente;
                txttygia.Text = lst1.tygia.ToString();
                txtghichu.Text = lst1.ghichu;
                txt1.Text = lst1.so.ToString();
                txtloainhap.Text = lst1.loainhap;
            }
            catch
            {
            }
        }
        
        // phân quyền 
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            var q = Biencucbo.QuyenDangChon;
            if (q == null) return;

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
        private void f_pnhap_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);

            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Phiếu Nhập Kho").ToString();

            changeFont.Translate(this);
            changeFont.Translate(barManager1);

            load();
        }
        // Mở
        private void btnmo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            db = new KetNoiDBDataContext();
            f_PN frm = new f_PN();
            frm.ShowDialog();
            if (Biencucbo.getID == 1)
            {
                //load pnhap
                try
                {
                    var lst = (from pn in db.pnhaps select new { a = pn }).FirstOrDefault(x => x.a.id == Biencucbo.ma);

                    if (lst == null) return;

                    txtid.Text = lst.a.id;
                    txtidnv.Text = lst.a.idnv;
                    txtdv.Text = lst.a.iddv;
                    txtngaynhap.DateTime = DateTime.Parse(lst.a.ngaynhap.ToString());
                    txtiddt.Text = lst.a.iddt;
                    txtghichu.Text = lst.a.ghichu;
                    txt1.Text = lst.a.so.ToString();
                    txtloainhap.Text = lst.a.loainhap;
                    gcchitiet.DataSource = lst.a.pnhapcts;
                    txttiente.Text = lst.a.tiente.ToString();
                    txttygia.Text = lst.a.tygia.ToString();
                }
                catch
                {
                }
                //btn
                btnnew.Enabled = true;
                btnsua.Enabled = true;
                btnLuu.Enabled = false;
                btnmo.Enabled = true;
                btnxoa.Enabled = true;
                btnin.Enabled = true;
                btnreload.Enabled = false;
            }
        }
        //Thêm
        private void btnnew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Biencucbo.hdpn = 0;
            txtid.DataBindings.Clear();
            txtid.Text = "YYYYY";

            gcchitiet.DataSource = new DAL.KetNoiDBDataContext().View_pnhapcts;
            for (int i = 0; i <= gridView1.RowCount - 1; i++)
            {
                gridView1.DeleteRow(i);
            }
            gridView1.AddNewRow();

            txtdv.Text = Biencucbo.donvi;
            txtngaynhap.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            txtphongban.Text = Biencucbo.phongban;
            txtidnv.Text = Biencucbo.idnv.Trim();
            lbltennv.Text = Biencucbo.ten;
            txtngaynhap.Focus();
            txtiddt.Text = "";
            lbltendt.Text = "";
            txtloainhap.Text = "";
            txtghichu.Text = "";
            txttiente.Text = "KIP";
            txttygia.Text = "1";

            //btn
            btnnew.Enabled = false;
            btnmo.Enabled = false;
            btnLuu.Enabled = true;
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnin.Enabled = false;
            btnreload.Enabled = false;

            //enabled
            txtghichu.ReadOnly = false;
            txtngaynhap.ReadOnly = false;
            txtiddt.ReadOnly = false;
            txtloainhap.ReadOnly = false;
            txttiente.ReadOnly = false;
            txttygia.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = true;
        }

        //Lưu
        public void luu()
        {
            t_history hs = new t_history();
            t_tudong td = new t_tudong();

            gridView1.UpdateCurrentRow();
            int check1 = 0;
            if (txtngaynhap.Text == "" || txtiddt.Text == "" || txtloainhap.Text == "" || txttiente.Text == "" || txttygia.Text == "")
            {
                Lotus.MsgBox.ShowWarningDialog("Thông tin chưa đầy đủ - Vui lòng kiểm tra lại!");
                return;
            }
            if (Convert.ToDateTime(txtngaynhap.DateTime.ToShortDateString()) > Convert.ToDateTime(DateTime.Now.ToShortDateString()))
            {
                Lotus.MsgBox.ShowWarningDialog("Ngày nhập không thể lớn hơn Ngày hiện tại! Vui lòng kiểm tra lại!");
                return;
            }
            else
            {
                try
                {
                    for (int i = 0; i <= gridView1.RowCount - 1; i++)
                    {

                        if (gridView1.GetRowCellDisplayText(i, "soluong").ToString() == "" || gridView1.GetRowCellDisplayText(i, "dongia").ToString() == "" || gridView1.GetRowCellDisplayText(i, "chietkhau").ToString() == "")
                        {
                            check1 = 1;
                        }
                        else if (gridView1.GetRowCellDisplayText(i, "idsanpham").ToString() == "")
                        {
                            check1 = 2;
                        }
                    }
                }
                catch (Exception)
                {
                }

                if (check1 == 1)
                {
                    Lotus.MsgBox.ShowWarningDialog("Thông tin chi tiết sản phẩm chưa đầy đủ - Vui Lòng Kiểm Tra Lại");
                    return;
                }
                else if (check1 == 2)
                {
                    Lotus.MsgBox.ShowWarningDialog("Mã sản phẩm không được để trống - Vui Lòng Kiểm Tra Lại");
                    return;
                }
                else
                {
                    if (Biencucbo.hdpn == 0)
                    {
                        db = new KetNoiDBDataContext();
                        try
                        {
                            string check = "PN" + Biencucbo.donvi.Trim().ToString();
                            var lst1 = (from s in db.tudongs where s.maphieu == check select new { so = s.so }).ToList();

                            if (lst1.Count == 0)
                            {
                                int so;

                                so = 2;
                                td.themtudong(check, so);
                                txtid.Text = check + "_000001";
                                txt1.Text = "1";
                            }
                            else
                            {

                                int k;
                                txt1.DataBindings.Clear();
                                txt1.DataBindings.Add("text", lst1, "so");
                                k = 0;
                                k = Convert.ToInt32(txt1.Text);
                                string so0 = "";
                                if (k < 10)
                                {
                                    so0 = "00000";
                                }
                                else if (k >= 10 & k < 100)
                                {
                                    so0 = "0000";
                                }
                                else if (k >= 100 & k < 1000)
                                {
                                    so0 = "000";
                                }
                                else if (k >= 1000 & k < 10000)
                                {
                                    so0 = "00";
                                }
                                else if (k >= 10000 & k < 100000)
                                {
                                    so0 = "0";
                                }
                                else if (k >= 100000)
                                {
                                    so0 = "";
                                }
                                txtid.Text = check + "_" + so0 + k;

                                k = k + 1;

                                td.suatudong(check, k);

                            }
                            pnct.moipn(txtid.Text, txtngaynhap.DateTime, txtiddt.Text, txtdv.Text, txtidnv.Text, txtghichu.Text, Convert.ToInt32(txt1.Text), txtloainhap.Text, txttiente.Text, float.Parse(txttygia.Text));
                            // History 
                            //hs.add(txtid.Text, "Thêm mới chứng từ - ເພີ່ມເອກະສານໃໝ່ - ERROR");

                            for (int i = 0; i <= gridView1.RowCount - 1; i++)
                            { 
                                gridView1.SetRowCellValue(i, "idpnhap", txtid.Text);
                                gridView1.SetRowCellValue(i, "id", txtid.Text + i);

                                pnct.moict(gridView1.GetRowCellValue(i, "idsanpham").ToString(), gridView1.GetRowCellValue(i, "diengiai").ToString(), float.Parse(gridView1.GetRowCellValue(i, "soluong").ToString()), float.Parse(gridView1.GetRowCellValue(i, "dongia").ToString()), gridView1.GetRowCellValue(i, "idcv").ToString(), gridView1.GetRowCellValue(i, "loaithue").ToString(), float.Parse(gridView1.GetRowCellValue(i, "thue").ToString()), float.Parse(gridView1.GetRowCellValue(i, "chietkhau").ToString()), float.Parse(gridView1.GetRowCellValue(i, "thanhtien").ToString()), gridView1.GetRowCellValue(i, "idpnhap").ToString(), gridView1.GetRowCellValue(i, "id").ToString(), gridView1.GetRowCellValue(i, "tiente").ToString(), float.Parse(gridView1.GetRowCellValue(i, "tygia").ToString()), float.Parse(gridView1.GetRowCellValue(i, "nguyente").ToString()));
                            }
                            // History 
                            hs.add(txtid.Text, "Thêm mới chứng từ - ເພີ່ມເອກະສານໃໝ່");

                            //btn
                            btnmo.Enabled = true;
                            btnnew.Enabled = true;
                            btnLuu.Enabled = false;
                            btnsua.Enabled = true;
                            btnxoa.Enabled = true;
                            btnin.Enabled = true;
                            btnreload.Enabled = false;

                            //enabled
                            txtghichu.ReadOnly = true;
                            txtngaynhap.ReadOnly = true;
                            txtiddt.ReadOnly = true;
                            txtloainhap.ReadOnly = true;
                            txttiente.ReadOnly = true;
                            txttygia.ReadOnly = true;
                            gridView1.OptionsBehavior.Editable = false;
                            Biencucbo.hdpn = 2;

                           
                            ShowAlert.Alert_Add_Success(this);
                        }
                        catch (Exception ex)
                        {
                            Lotus.MsgBox.ShowErrorDialog(ex.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            pnct.suapn(txtid.Text, DateTime.Parse(txtngaynhap.Text), txtiddt.Text, txtghichu.Text, int.Parse(txt1.Text), txtloainhap.Text, txttiente.Text, float.Parse(txttygia.Text));
                            //hs.add(txtid.Text, "Sửa chứng từ - ດັດແກ້ເອກະສານ - ERROR");

                            //sua ct
                            LuuPhieu();
                            hs.add(txtid.Text, "Sửa chứng từ - ດັດແກ້ເອກະສານ");
                        }
                        catch (Exception ex)
                        {
                            Lotus.MsgBox.ShowErrorDialog(ex.ToString());
                        }

                        //btn
                        btnmo.Enabled = true;
                        btnnew.Enabled = true;
                        btnLuu.Enabled = false;
                        btnsua.Enabled = true;
                        btnxoa.Enabled = true;
                        btnin.Enabled = true;
                        btnreload.Enabled = false;

                        //enabled
                        txtghichu.ReadOnly = true;
                        txtngaynhap.ReadOnly = true;
                        txtiddt.ReadOnly = true;
                        txtloainhap.ReadOnly = true;
                        txttiente.ReadOnly = true;
                        txttygia.ReadOnly = true;
                        gridView1.OptionsBehavior.Editable = false;
                        Biencucbo.hdpn = 2;

                        
                        ShowAlert.Alert_Edit_Success(this);
                    }
                }
            }
        }
        private void btnLuu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //check khoa so
            if (checkKhoaSo.checkkhoaso(txtdv, txtngaynhap) == false) return;
            try
            {
                gridView1.PostEditor();
                luu();
            }
            catch
            {
            }
        }
        bool LuuPhieu()
        {
            // kiem tra truoc khi luu
            layoutControl1.Validate();
            gridView1.CloseEditor();
            gridView1.UpdateCurrentRow();

            // if(kiem tra rang buoc)
            //  return false;

            try
            {
                var c1 = db.pnhaps.Context.GetChangeSet();
                db.pnhapcts.Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                Lotus.MsgBox.ShowErrorDialog(ex.Message);
                return false;
            }

            return true;
        }
        //Sửa
        private void btnsua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //check khoa so
            if (checkKhoaSo.checkkhoaso(txtdv, txtngaynhap) == false) return;

            if (txtid.Text == "")
            {
                return;
            }

            var check = (from a in db.pnhaps
                         join b in db.pchis on a.id equals b.link
                         where a.id == txtid.Text
                         select b);
            if (check.Count() == 0)
            {
                try
                {
                    var lst = (from pn in db.pnhaps select pn).FirstOrDefault(x => x.id == txtid.Text);
                    if (lst == null) return;
                    gcchitiet.DataSource = lst.pnhapcts;
                    //enabled
                    txtghichu.ReadOnly = false;
                    txtngaynhap.ReadOnly = false;
                    txtiddt.ReadOnly = false;
                    txtloainhap.ReadOnly = false;
                    txttiente.ReadOnly = false;
                    txttygia.ReadOnly = true;

                    gridView1.OptionsBehavior.Editable = true;
                    // btn
                    btnsua.Enabled = false;
                    btnLuu.Enabled = true;
                    btnmo.Enabled = false;
                    btnnew.Enabled = false;
                    btnxoa.Enabled = false;
                    btnin.Enabled = false;
                    btnreload.Enabled = true;
                    Biencucbo.hdpn = 1;
                }
                catch
                {
                }
            }
            else
            {
                XtraMessageBox.Show("Không thể thao tác vì có sự liên kết - kiểm tra phiếu " + check.FirstOrDefault().id);
            }
        }

        //Xóa
        private void btnxoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //check khoa so
            if (checkKhoaSo.checkkhoaso(txtdv, txtngaynhap) == false) return;

            if (txtid.Text == "")
            {
                return;
            }

            var check = (from a in db.pnhaps
                         join b in db.pchis on a.id equals b.link
                         where a.id == txtid.Text
                         select b);
            if (check.Count() == 0)
            {
                if (XtraMessageBox.Show("Bạn có chắc chắn muốn xóa Phiếu " + txtid.Text + " không?", "THÔNG BÁO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {

                    try
                    {
                        t_history hs = new t_history();
                        hs.add(txtid.Text, "Xóa chứng từ - ລົບເອກະສານ");

                        for (int i = gridView1.DataRowCount - 1; i >= 0; i--)
                        {
                            pnct.xoact(gridView1.GetRowCellValue(i, "id").ToString());
                            gridView1.DeleteRow(i);
                        }
                        pnct.xoaPN(txtid.Text);

                        //btn
                        btnmo.Enabled = true;
                        btnnew.Enabled = true;
                        btnLuu.Enabled = false;
                        btnsua.Enabled = true;
                        btnxoa.Enabled = true;
                        btnin.Enabled = true;
                        btnreload.Enabled = false;

                        //enabled
                        txtghichu.ReadOnly = true;
                        txtngaynhap.ReadOnly = true;
                        txtiddt.ReadOnly = true;
                        txtloainhap.ReadOnly = true;
                        txttiente.ReadOnly = true;
                        txttygia.ReadOnly = true;
                        gridView1.OptionsBehavior.Editable = false;

                        //gcchitiet.DataSource = new DAL.KetNoiDBDataContext().View_pnhapcts;
                        txtdv.Text = "";
                        txtid.Text = "";
                        txtidnv.Text = "";
                        txtdv.Text = "";
                        txtngaynhap.Text = "";
                        txtiddt.Text = "";
                        txtghichu.Text = "";
                        txt1.Text = "";
                        txtloainhap.Text = "";
                        lbltendt.Text = "";
                        lbltennv.Text = "";
                        txttiente.Text = "";
                        txttygia.Text = "";

                        ShowAlert.Alert_Del_Success(this);
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("Không thể thao tác vì có sự liên kết - kiểm tra phiếu " + check.FirstOrDefault().id);
            }
        }

        //reload
        private void btnload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Biencucbo.hdpn == 1)
            {
                db = new KetNoiDBDataContext();
                try
                {
                    var lst = (from pn in db.pnhaps select pn).FirstOrDefault(x => x.id == txtid.Text);

                    if (lst == null) return;

                    db.Refresh(RefreshMode.OverwriteCurrentValues, db.pnhapcts);

                    gcchitiet.DataSource = lst.pnhapcts;

                    txtidnv.Text = lst.idnv;
                    txtdv.Text = lst.iddv;
                    txtngaynhap.DateTime = DateTime.Parse(lst.ngaynhap.ToString());
                    txtiddt.Text = lst.iddt;
                    txtghichu.Text = lst.ghichu;
                    txt1.Text = lst.so.ToString();
                    txtloainhap.Text = lst.loainhap;
                    txttiente.Text = lst.tiente;
                    txttygia.Text = lst.tygia.ToString();
                    gcchitiet.DataSource = lst.pnhapcts;

                    txtghichu.ReadOnly = true;
                    txtngaynhap.ReadOnly = true;
                    txtiddt.ReadOnly = true;
                    txtloainhap.ReadOnly = true;
                    txttiente.ReadOnly = true;
                    txttygia.ReadOnly = true;
                    gridView1.OptionsBehavior.Editable = false;
                }
                catch
                {
                }

                //btn
                btnnew.Enabled = true;
                btnsua.Enabled = true;
                btnLuu.Enabled = false;
                btnmo.Enabled = true;
                btnxoa.Enabled = true;
                btnin.Enabled = true;
                btnreload.Enabled = false;
                gridView1.OptionsBehavior.Editable = false;
            }

            else if (Biencucbo.hdpn == 0)
            {
                load();
                //btn
                btnnew.Enabled = true;
                btnsua.Enabled = true;
                btnLuu.Enabled = false;
                btnmo.Enabled = true;
                btnxoa.Enabled = true;
                btnin.Enabled = true;
                btnreload.Enabled = false;
                gridView1.OptionsBehavior.Editable = false;
            }
            Biencucbo.hdpn = 2;
        }

        // thay đổi
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            gridView1.PostEditor();
            if (e.Column.FieldName == "soluong" || e.Column.FieldName == "dongia" || e.Column.FieldName == "chietkhau" || e.Column.FieldName == "loaithue" || e.Column.FieldName == "tygia")
            {

                try
                {
                    try
                    {
                        gridView1.SetFocusedRowCellValue("thue", (((float.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (float.Parse(gridView1.GetFocusedRowCellValue("dongia").ToString()))) - ((float.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (float.Parse(gridView1.GetFocusedRowCellValue("chietkhau").ToString())))) * float.Parse(gridView1.GetFocusedRowCellDisplayText("loaithue").ToString()) / 100);
                    }
                    catch
                    {

                        gridView1.SetFocusedRowCellValue("nguyente", (((float.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (float.Parse(gridView1.GetFocusedRowCellValue("dongia").ToString()))) - ((float.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (float.Parse(gridView1.GetFocusedRowCellValue("chietkhau").ToString())))) + float.Parse(gridView1.GetFocusedRowCellValue("thue").ToString()));
                    }
                    finally
                    {
                        gridView1.SetFocusedRowCellValue("thue", (((float.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (float.Parse(gridView1.GetFocusedRowCellValue("dongia").ToString()))) - ((float.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (float.Parse(gridView1.GetFocusedRowCellValue("chietkhau").ToString())))) * float.Parse(gridView1.GetFocusedRowCellDisplayText("loaithue").ToString()) / 100);
                        gridView1.SetFocusedRowCellValue("nguyente", (((float.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (float.Parse(gridView1.GetFocusedRowCellValue("dongia").ToString()))) - ((float.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (float.Parse(gridView1.GetFocusedRowCellValue("chietkhau").ToString())))) + float.Parse(gridView1.GetFocusedRowCellValue("thue").ToString()));
                    }
                }
                catch (Exception)
                {
                }
            }
            else if (e.Column.FieldName == "nguyente")
            {
                try
                {
                    gridView1.SetFocusedRowCellValue("thanhtien", (float.Parse(gridView1.GetFocusedRowCellValue("nguyente").ToString())) * (float.Parse(txttygia.Text)));
                }
                catch (Exception)
                {
                }
            }
        }

        //Phím Tắt
        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Biencucbo.hdpn == 2)
                return;

            if (e.KeyCode == Keys.Insert)
            {
                gridView1.AddNewRow();

            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (Biencucbo.hdpn == 1)
                {
                    try
                    {
                        pnhapct ct = (from c in db.pnhapcts select c).Single(x => x.id == gridView1.GetFocusedRowCellValue("id").ToString());
                        db.pnhapcts.DeleteOnSubmit(ct);
                    }
                    catch
                    {
                    }
                }
                gridView1.DeleteRow(gridView1.FocusedRowHandle);
            }
        }
        //đối tượng
        private void txtiddt_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var lst = (from a in db.doituongs select a).Single(t => t.id == txtiddt.Text);
                lbltendt.Text = lst.ten.ToString();
                txtdiachi.Text = lst.diachi.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void btnthue1_EditValueChanged(object sender, EventArgs e)
        {
            gridView1.PostEditor();
        }
        //Dòng mới
        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            if (Biencucbo.hdpn == 1)
            {
                var ct = gridView1.GetFocusedRow() as pnhapct;
                if (ct == null) return;

                int i = 0, k = 0;
                string a;

                k = gridView1.DataRowCount;
                a = txtid.Text + k;

                for (i = 0; i <= gridView1.DataRowCount - 1;)
                {
                    if (a == gridView1.GetRowCellValue(i, "id").ToString())
                    {
                        k = k + 1;
                        a = txtid.Text + k;
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }

                ct.idpnhap = txtid.Text;
                ct.soluong = 0;
                ct.dongia = 0;
                ct.chietkhau = 0;
                ct.diengiai = "";
                ct.loaithue = "";
                ct.idcv = "";
                ct.thue = 0;
                ct.thanhtien = 0;
                ct.id = a;
                ct.tiente = "KIP";
                ct.tygia = 1;
                ct.nguyente = 0;
            }
            else
            {
                gridView1.SetFocusedRowCellValue("diengiai", "");
                gridView1.SetFocusedRowCellValue("soluong", 0);
                gridView1.SetFocusedRowCellValue("dongia", 0);
                gridView1.SetFocusedRowCellValue("chietkhau", 0);
                gridView1.SetFocusedRowCellValue("thue", 0);
                gridView1.SetFocusedRowCellValue("tygia", 1);
                gridView1.SetFocusedRowCellValue("loaithue", "");
                gridView1.SetFocusedRowCellValue("tiente", "KIP");
                gridView1.SetFocusedRowCellValue("idcv", "");
            }
        }

        private void btnin_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var lst = (from a in db.r_pnhaps where a.id == txtid.Text select a);
                r_pnhap xtra = new r_pnhap();
                xtra.DataSource = lst;
                xtra.ShowPreviewDialog();
            }
            catch
            {
            }
        }
        private void f_pnhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Biencucbo.hdpn != 2)
            {
                var a = Lotus.MsgBox.ShowYesNoCancelDialog("Phiếu này chưa được lưu - Bạn có muốn lưu Phiếu này trước khi thoát không?");
                if (a == DialogResult.Yes)
                {
                    luu();
                }
                else if (a == DialogResult.Cancel) e.Cancel = true;
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!gridView1.IsGroupRow(e.RowHandle)) //Nếu không phải là Group
            {
                if (e.Info.IsRowIndicator) //Nếu là dòng Indicator
                {
                    if (e.RowHandle < 0)
                    {
                        e.Info.ImageIndex = 0;
                        e.Info.DisplayText = string.Empty;
                    }
                    else
                    {
                        e.Info.ImageIndex = -1; //Không hiển thị hình
                        e.Info.DisplayText = (e.RowHandle + 1).ToString(); //Số thứ tự tăng dần
                    }
                    SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font); //Lấy kích thước của vùng hiển thị Text
                    Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                    BeginInvoke(new MethodInvoker(delegate { cal(_Width, gridView1); })); //Tăng kích thước nếu Text vượt quá
                }
            }
            else
            {
                e.Info.ImageIndex = -1;
                e.Info.DisplayText = string.Format("[{0}]", (e.RowHandle * -1)); //Nhân -1 để đánh lại số thứ tự tăng dần
                SizeF _Size = e.Graphics.MeasureString(e.Info.DisplayText, e.Appearance.Font);
                Int32 _Width = Convert.ToInt32(_Size.Width) + 20;
                BeginInvoke(new MethodInvoker(delegate { cal(_Width, gridView1); }));
            }
        }
        bool cal(Int32 _Width, GridView _View)
        {
            _View.IndicatorWidth = _View.IndicatorWidth < _Width ? _Width : _View.IndicatorWidth;
            return true;
        }

        private void btnthue1_EditValueChanged_1(object sender, EventArgs e)
        {
            gridView1.PostEditor();
        }

        private void rsearchtiente1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridView1.PostEditor();
                var lst = (from a in db.tientes select a).Single(t => t.tiente1 == gridView1.GetFocusedRowCellValue("tiente").ToString());
                if (lst == null) return;
                gridView1.SetFocusedRowCellValue("tygia", lst.tygia);
            }
            catch
            {
            }
        }

        private void btnmasp1_EditValueChanged(object sender, EventArgs e)
        {
            gridView1.PostEditor();
        }

        private void txttiente_EditValueChanged(object sender, EventArgs e)
        {
            if (txttiente.Text != "")
            {
                var lst = (from a in db.tientes select a).FirstOrDefault(t => t.tiente1 == txttiente.Text);
                txttygia.Text = lst.tygia.ToString();

                for (int i = 0; i <= gridView1.RowCount - 1; i++)
                {
                    try
                    {
                        gridView1.SetRowCellValue(i, "thanhtien", float.Parse(gridView1.GetRowCellValue(i, "nguyente").ToString()) * float.Parse(txttygia.Text));
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}