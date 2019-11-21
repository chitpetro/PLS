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
using DevExpress.XtraEditors;
using System.Data.Linq;
using DevExpress.XtraReports.UI;
using ControlLocalizer;
using GUI.Libs;

namespace GUI
{
    public partial class f_hd : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        t_hoadon hd = new t_hoadon();
        t_cttk tk = new t_cttk();
        public double thue = 0;
        public double doanhthu = 0;
        public int key = 0;

        public f_hd()
        {
            InitializeComponent();
            // tinh so luong ton san pham
            try
            {
                var lst = (from a in db.r_giasps where a.iddv == Biencucbo.donvi select new { id = a.idsp, tensp = a.tensp, dvt = a.dvt });
                if (lst == null) return;
                btnmasp.DataSource = lst;
                rsearchTenSP.DataSource = btnmasp.DataSource;
                btndvt.DataSource = btnmasp.DataSource;

            }
            catch
            {
            }

            rsearchtiente.DataSource = new DAL.KetNoiDBDataContext().tientes;

            txttiente.Properties.DataSource = new DAL.KetNoiDBDataContext().tientes;
            btnthue.DataSource = new DAL.KetNoiDBDataContext().thues;
            rsearchthuesuat.DataSource = btnthue.DataSource;

            btncongviec.DataSource = new DAL.KetNoiDBDataContext().congviecs;
            txtiddt.Properties.DataSource = new DAL.KetNoiDBDataContext().doituongs;

            txtloaihd.Properties.DataSource = new DAL.KetNoiDBDataContext().dmpxuats;

            //translate 
            if (Biencucbo.ngonngu.ToString() == "Vietnam")
            {
                coldiengiai.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "diengiai", "Tổng Cộng:")});

                gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "diengiai", "Tổng cộng:")});
                gridColumn56.Caption = "Danh mục";
                gridColumn57.Caption = "Danh mục";
                gridColumn48.Caption = "Mã Đối Tượng";
                gridColumn49.Caption = "Tên Đối Tượng ";
                gridColumn50.Caption = "Nhóm Đối Tượng";
                gridColumn51.Caption = "Loại Đối Tượng";
                gridColumn52.Caption = "Địa Chỉ";
                gridColumn53.Caption = "Tiền tệ";
                gridColumn54.Caption = "Tỷ giá";
                gridColumn55.Caption = "Ghi chú";
                gridColumn27.Caption = "Mã SP";
                gridColumn28.Caption = "Sản Phẩm";
                gridColumn32.Caption = "ĐVT";
                gridColumn33.Caption = "Loại";
                gridColumn41.Caption = "Mã Công Việc";
                gridColumn42.Caption = "Tên Công Việc";
                gridColumn43.Caption = "Nhóm Công Việc";
            }
            else
            {
                coldiengiai.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "diengiai", "ລວມທັງໝົດ:")});
                gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "diengiai", "ລວມທັງໝົດ:")});
                gridColumn56.Caption = "ລາຍການ";
                gridColumn57.Caption = "ລາຍການ";
                gridColumn48.Caption = "ລະຫັດ";
                gridColumn49.Caption = "ຊື່ເປົ້າໝາຍ";
                gridColumn50.Caption = "ລະຫັດກຸ່ມເປົ້າໝາຍ";
                gridColumn51.Caption = "ປະເພດເປົ້າໝາຍ";
                gridColumn52.Caption = "ທີ່ຢູ່";
                gridColumn53.Caption = "ເງິນຕາ";
                gridColumn54.Caption = "ອັດຕາ";
                gridColumn55.Caption = "ໝາຍເຫດ";
                gridColumn27.Caption = "ລະຫັດ";
                gridColumn28.Caption = "ຊື່ຜະລິດຕະພັນ";
                gridColumn32.Caption = "ຫົວໜ່ວຍຄິດໄລ່";
                gridColumn33.Caption = "ປະເພດ";
                gridColumn41.Caption = "ລະຫັດວຽກງານ";
                gridColumn42.Caption = "ຊື່ວຽກງານ";
                gridColumn43.Caption = "ກຸ່ມໜ້າວຽກ";
            }

            //lay quyen
            var quyen1 = db.PhanQuyen2s.FirstOrDefault(p => p.TaiKhoan == Biencucbo.phongban && p.ChucNang == "HoaDon_ThanhToan");
            btnthanhtoan.Enabled = (bool)quyen1.Xem;
        }

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

        //load
        public void load()
        {

            //db = new KetNoiDBDataContext();
            db.CommandTimeout = 0;
            Biencucbo.hdhd = 2;
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

            // Enable
            txtghichu.ReadOnly = true;
            txtngaylap.ReadOnly = true;
            txtvat.ReadOnly = true;
            txttiente.ReadOnly = true;
            txttygia.ReadOnly = true;
            txtiddt.ReadOnly = true;
            txtloaihd.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;

            try
            {
                var lst = (from a in db.hoadons where a.iddv == Biencucbo.donvi select a.so).Max();
                var lst1 = (from b in db.hoadons where b.iddv == Biencucbo.donvi select b).FirstOrDefault(t => t.so == lst);
                if (lst1 == null) return;

                gcchitiet.DataSource = lst1.hoadoncts;

                txtid.Text = lst1.id;

                txtidnv.Text = lst1.idnv;
                txtdv.Text = lst1.iddv;
                txtngaylap.DateTime = DateTime.Parse(lst1.ngayhd.ToString());
                txtiddt.Text = lst1.iddt;
                txtghichu.Text = lst1.ghichu;
                txtvat.Text = lst1.soVAT;
                txt1.Text = lst1.so.ToString();
                txtloaihd.Text = lst1.loaixuat;
                txttiente.Text = lst1.tiente;
                txttygia.Text = lst1.tygia.ToString();
            }
            catch
            {
            }
        }

        //Mở
        private void btnmo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //db = new KetNoiDBDataContext();
            db.CommandTimeout = 0;

            f_dshd frm = new f_dshd();
            frm.ShowDialog();
            if (Biencucbo.getID == 1)
            {
                //load hoa don
                try
                {
                    var lst = (from hd in db.hoadons select new { a = hd }).FirstOrDefault(x => x.a.id == Biencucbo.ma);

                    if (lst == null) return;

                    txtid.Text = lst.a.id;
                    txtidnv.Text = lst.a.idnv;
                    txtdv.Text = lst.a.iddv;
                    txtngaylap.DateTime = DateTime.Parse(lst.a.ngayhd.ToString());
                    txtiddt.Text = lst.a.iddt;
                    txtghichu.Text = lst.a.ghichu;
                    txtvat.Text = lst.a.soVAT;
                    txttiente.Text = lst.a.tiente;
                    txttygia.Text = lst.a.tygia.ToString();
                    txt1.Text = lst.a.so.ToString();
                    txtloaihd.Text = lst.a.loaixuat;
                    gcchitiet.DataSource = lst.a.hoadoncts;

                    //btn
                    btnnew.Enabled = true;
                    btnsua.Enabled = true;
                    btnLuu.Enabled = false;
                    btnmo.Enabled = true;
                    btnxoa.Enabled = true;
                    btnin.Enabled = true;
                    btnreload.Enabled = false;
                }
                catch
                {
                }
            }
        }

        //Add new
        private void btnnew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Biencucbo.hdhd = 0;
            txtid.DataBindings.Clear();
            txtid.Text = "YYYYY";

            gcchitiet.DataSource = new DAL.KetNoiDBDataContext().View_hoadoncts;
            for (int i = 0; i <= gridView1.RowCount - 1; i++)
            {
                gridView1.DeleteRow(i);
            }
            gridView1.AddNewRow();

            txtdv.Text = Biencucbo.donvi;
            txtngaylap.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            txtphongban.Text = Biencucbo.phongban;
            txtidnv.Text = Biencucbo.idnv.Trim();
            lbltennv.Text = Biencucbo.ten;
            //txtngaylap.Focus();
            txtloaihd.Focus();
            txtiddt.Text = "";
            lbltendt.Text = "";
            txtloaihd.Text = "";
            txtghichu.Text = "";
            txtvat.Text = "";
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
            txtngaylap.ReadOnly = false;
            txtiddt.ReadOnly = false;
            txttiente.ReadOnly = false;
            txttygia.ReadOnly = true;
            txtvat.ReadOnly = false;
            txtloaihd.ReadOnly = false;
            gridView1.OptionsBehavior.Editable = true;
        }

        //Lưu
        public void luu()
        {

            t_history hs = new t_history();
            t_tudong td = new t_tudong();

            gridView1.UpdateCurrentRow();

            int check1 = 0;
            if (txtngaylap.Text == "" || txtiddt.Text == "" || txtloaihd.Text == "" || txttiente.Text == "" || txttygia.Text == "")
            {
                Lotus.MsgBox.ShowWarningDialog("Thông tin chưa đầy đủ - Vui lòng kiểm tra lại!");
                return;
            }
           if (Convert.ToDateTime(txtngaylap.DateTime.ToShortDateString()) > Convert.ToDateTime(DateTime.Now.ToShortDateString()))
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
                        //re-check
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
                    if (Biencucbo.hdhd == 0)
                    {
                        db = new KetNoiDBDataContext();
                        try
                        {
                            string check = "HD" + Biencucbo.donvi.Trim().ToString();
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
                            hd.moihd(txtid.Text, txtngaylap.DateTime, txtiddt.Text, txtidnv.Text, txtdv.Text, txtghichu.Text, Convert.ToInt32(txt1.Text), txtloaihd.Text, txttiente.Text, double.Parse(txttygia.Text), txtvat.Text);
                            // History 
                            //hs.add(txtid.Text, "Thêm mới hoá đơn - ເພີ່ມໃບບິນໃໝ່ -- ERROR");
                            for (int i = 0; i <= gridView1.RowCount - 1; i++)
                            {
                                gridView1.SetRowCellValue(i, "idhoadon", txtid.Text);
                                gridView1.SetRowCellValue(i, "id", txtid.Text + i);

                                hd.moihdct(gridView1.GetRowCellValue(i, "idhoadon").ToString(), gridView1.GetRowCellValue(i, "idsanpham").ToString(), gridView1.GetRowCellValue(i, "diengiai").ToString(), double.Parse(gridView1.GetRowCellValue(i, "soluong").ToString()), double.Parse(gridView1.GetRowCellValue(i, "dongia").ToString()), gridView1.GetRowCellValue(i, "idcv").ToString(), gridView1.GetRowCellValue(i, "loaithue").ToString(), double.Parse(gridView1.GetRowCellValue(i, "thue").ToString()), double.Parse(gridView1.GetRowCellValue(i, "chietkhau").ToString()), double.Parse(gridView1.GetRowCellValue(i, "thanhtien").ToString()), gridView1.GetRowCellValue(i, "id").ToString(), gridView1.GetRowCellValue(i, "tiente").ToString(), double.Parse(gridView1.GetRowCellValue(i, "tygia").ToString()), double.Parse(gridView1.GetRowCellValue(i, "nguyente").ToString()));
                            }
                            // History 
                            //hs.edit(txtid.Text, "Thêm mới hoá đơn - ເພີ່ມໃບບິນໃໝ່");
                            hs.add(txtid.Text, "Thêm mới hoá đơn - ເພີ່ມໃບບິນໃໝ່");

                            for (int i = 0; i < gridView1.DataRowCount; i++)
                            {
                                thue = double.Parse(gridView1.GetRowCellValue(i, "thue").ToString());
                                doanhthu = double.Parse(gridView1.GetRowCellValue(i, "nguyente").ToString()) - thue;
                                tk.moi(txtid.Text + i + "1", txtdv.Text, "HD", txtid.Text, txtngaylap.DateTime, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), Convert.ToInt32(txt1.Text), txtiddt.Text, gridView1.GetRowCellValue(i, "diengiai").ToString(), "131", "511", doanhthu * double.Parse(txttygia.Text), txttiente.Text, double.Parse(txttygia.Text), doanhthu, gridView1.GetRowCellValue(i, "idsanpham").ToString(), "", txtidnv.Text, txtloaihd.Text, gridView1.GetRowCellValue(i, "idcv").ToString(), "");
                                tk.moi(txtid.Text + i + "2", txtdv.Text, "HD", txtid.Text, txtngaylap.DateTime, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), Convert.ToInt32(txt1.Text), txtiddt.Text, gridView1.GetRowCellValue(i, "diengiai").ToString(), "131", "333", thue * double.Parse(txttygia.Text), txttiente.Text, double.Parse(txttygia.Text), thue, gridView1.GetRowCellValue(i, "idsanpham").ToString(), "", txtidnv.Text, txtloaihd.Text, gridView1.GetRowCellValue(i, "idcv").ToString(), "");
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
                            txtngaylap.ReadOnly = true;
                            txtvat.ReadOnly = true;
                            txtiddt.ReadOnly = true;
                            txtloaihd.ReadOnly = true;
                            txttiente.ReadOnly = true;
                            txttygia.ReadOnly = true;
                            gridView1.OptionsBehavior.Editable = false;
                            Biencucbo.hdhd = 2; 

                            ShowAlert.Alert_Add_Success(this);
                        }
                        catch (Exception ex)
                        {
                            Lotus.MsgBox.ShowErrorDialog(ex.ToString());
                        }
                    }
                    else//SUA
                    {
                        try
                        {
                            hd.suahd(txtid.Text, DateTime.Parse(txtngaylap.Text), txtiddt.Text, txtghichu.Text, int.Parse(txt1.Text), txtloaihd.Text, txttiente.Text, double.Parse(txttygia.Text), txtvat.Text);
                            
                            //sua ct
                            LuuPhieu();
                            //hs.add(txtid.Text, "Sửa hoá đơn - ດັດແກ້ໃບບິນ - ERROR");
                            tk.xoa(txtid.Text);
                            for (int i = 0; i < gridView1.DataRowCount; i++)
                            {

                                thue = double.Parse(gridView1.GetRowCellValue(i, "thue").ToString());
                                doanhthu = double.Parse(gridView1.GetRowCellValue(i, "nguyente").ToString()) - thue;
                                tk.moi(txtid.Text + i + "1", txtdv.Text, "HD", txtid.Text, txtngaylap.DateTime, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), Convert.ToInt32(txt1.Text), txtiddt.Text, gridView1.GetRowCellValue(i, "diengiai").ToString(), "131", "511", doanhthu * double.Parse(txttygia.Text), txttiente.Text, double.Parse(txttygia.Text), doanhthu, gridView1.GetRowCellValue(i, "idsanpham").ToString(), "", txtidnv.Text, txtloaihd.Text, gridView1.GetRowCellValue(i, "idcv").ToString(), "");

                                tk.moi(txtid.Text + i + "2", txtdv.Text, "HD", txtid.Text, txtngaylap.DateTime, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), Convert.ToInt32(txt1.Text), txtiddt.Text, gridView1.GetRowCellValue(i, "diengiai").ToString(), "131", "333", thue * double.Parse(txttygia.Text), txttiente.Text, double.Parse(txttygia.Text), thue, gridView1.GetRowCellValue(i, "idsanpham").ToString(), "", txtidnv.Text, txtloaihd.Text, gridView1.GetRowCellValue(i, "idcv").ToString(), "");
                            }
                            //hs.edit(txtid.Text, "Sửa hoá đơn - ດັດແກ້ໃບບິນ");
                            hs.add(txtid.Text, "Sửa hoá đơn - ດັດແກ້ໃບບິນ");
                            
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
                            txtngaylap.ReadOnly = true;
                            txtvat.ReadOnly = true;
                            txtiddt.ReadOnly = true;
                            txtloaihd.ReadOnly = true;
                            txttiente.ReadOnly = true;
                            txttygia.ReadOnly = true;
                            gridView1.OptionsBehavior.Editable = false;
                            Biencucbo.hdhd = 2;
                             
                            ShowAlert.Alert_Edit_Success(this);
                        }
                        catch (Exception ex)
                        {
                            Lotus.MsgBox.ShowErrorDialog(ex.ToString());
                        }

                    }
                }
            }
        }

        private void btnLuu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //check khoa so
            if (checkKhoaSo.checkkhoaso(txtdv, txtngaylap) == false) return;

            gridView1.PostEditor();
            luu();
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
                db.hoadoncts.Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                Lotus.MsgBox.ShowErrorDialog(ex.Message);
                return false;
            }

            return true;
        }

        private void btnsua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //check khoa so
            if (checkKhoaSo.checkkhoaso(txtdv, txtngaylap) == false) return;

            if (txtid.Text == "") return;

            var check = (from a in db.hoadons
                         join b in db.pthus on a.id equals b.link
                         where a.id == txtid.Text
                         select b);
            if (check.Count() == 0)
            {
                try
                {
                    var lst = (from pn in db.hoadons select pn).FirstOrDefault(x => x.id == txtid.Text);
                    if (lst == null) return;
                    gcchitiet.DataSource = lst.hoadoncts;
                    //enabled
                    txtghichu.ReadOnly = false;
                    txtngaylap.ReadOnly = false;
                    txtvat.ReadOnly = false;
                    txtiddt.ReadOnly = false;
                    txtloaihd.ReadOnly = false;
                    txttiente.ReadOnly = false;
                    txttygia.ReadOnly = true;
                    gridView1.OptionsBehavior.Editable = true;
                    Biencucbo.hdhd = 1;

                    txtghichu.Focus();

                    // btn
                    btnsua.Enabled = false;
                    btnLuu.Enabled = true;
                    btnmo.Enabled = false;
                    btnnew.Enabled = false;
                    btnxoa.Enabled = false;
                    btnin.Enabled = false;
                    btnreload.Enabled = true;
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

        private void btnxoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //check khoa so
            if (checkKhoaSo.checkkhoaso(txtdv, txtngaylap) == false) return;

            tk.xoa(txtid.Text);
            if (txtid.Text == "") return;
            var check = (from a in db.hoadons
                         join b in db.pthus on a.id equals b.link
                         where a.id == txtid.Text
                         select b);
            if (check.Count() == 0)
            {
                if (XtraMessageBox.Show("Bạn có chắc chắn muốn xóa Phiếu " + txtid.Text + " không?", "THÔNG BÁO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    t_history hs = new t_history();

                    try
                    {
                        hs.add(txtid.Text, "Xóa hoá đơn - ລົບໃບບິນ");

                        for (int i = gridView1.DataRowCount - 1; i >= 0; i--)
                        {
                            hd.xoact(gridView1.GetRowCellValue(i, "id").ToString());
                            gridView1.DeleteRow(i);
                        }

                        hd.xoahd(txtid.Text);

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
                        txtngaylap.ReadOnly = true;
                        txtvat.ReadOnly = true;
                        txtiddt.ReadOnly = true;
                        txtloaihd.ReadOnly = true;
                        txttiente.ReadOnly = true;
                        txttygia.ReadOnly = true;
                        gridView1.OptionsBehavior.Editable = false;

                        txtdv.Text = "";
                        txtid.Text = "";
                        txtidnv.Text = "";
                        txtdv.Text = "";
                        txtngaylap.Text = "";
                        txtiddt.Text = "";
                        txtghichu.Text = "";
                        txtvat.Text = "";
                        txt1.Text = "";
                        txtloaihd.Text = "";
                        lbltendt.Text = "";
                        lbltennv.Text = "";
                        txttiente.Text = "";
                        txttygia.Text = "";

                        ShowAlert.Alert_Del_Success(this);
                    }
                    catch { }
                }
                else
                {
                    XtraMessageBox.Show("Không thể thao tác vì có sự liên kết - kiểm tra phiếu " + check.FirstOrDefault().id);
                }
            }
        }

        private void btnin_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var lst = (from a in db.r_pxuats where a.id == txtid.Text select a);
            r_pxuat xtra = new r_pxuat();

            xtra.DataSource = lst;
            xtra.ShowPreviewDialog();
        }

        private void btnload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Biencucbo.hdhd == 1)
            {
                db = new KetNoiDBDataContext();

                var lst = (from pn in db.hoadons select pn).FirstOrDefault(x => x.id == txtid.Text);

                if (lst == null) return;

                //db.Refresh(RefreshMode.OverwriteCurrentValues, db.hoadoncts);

                gcchitiet.DataSource = lst.hoadoncts;

                txtidnv.Text = lst.idnv;
                txtdv.Text = lst.iddv;
                txtngaylap.DateTime = DateTime.Parse(lst.ngayhd.ToString());
                txtiddt.Text = lst.iddt;
                txtghichu.Text = lst.ghichu;
                txtvat.Text = lst.soVAT;
                txt1.Text = lst.so.ToString();
                txtloaihd.Text = lst.loaixuat;
                txttiente.Text = lst.tiente;
                txttygia.Text = lst.tygia.ToString();
                gcchitiet.DataSource = lst.hoadoncts;

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

            else if (Biencucbo.hdhd == 0)
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
            Biencucbo.hdhd = 2;
        }

        private void btnmasp_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridView1.PostEditor();
                var lst = new DAL.KetNoiDBDataContext().r_giasps;
                var lst2 = (from a in lst where a.iddv == Biencucbo.donvi select a).Single(t => t.idsp == gridView1.GetFocusedRowCellValue("idsanpham").ToString());


                gridView1.SetFocusedRowCellValue("dongia", double.Parse(lst2.giaban.ToString()));
            }
            catch (Exception ex)
            {
                Lotus.MsgBox.ShowErrorDialog(ex.ToString());
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            gridView1.PostEditor();
            if (e.Column.FieldName == "soluong" || e.Column.FieldName == "dongia" || e.Column.FieldName == "chietkhau" || e.Column.FieldName == "loaithue" || e.Column.FieldName == "tygia")
            {
                try
                {
                    try
                    {
                        gridView1.SetFocusedRowCellValue("thue", (((double.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (double.Parse(gridView1.GetFocusedRowCellValue("dongia").ToString()))) - ((double.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (double.Parse(gridView1.GetFocusedRowCellValue("chietkhau").ToString())))) * double.Parse(gridView1.GetFocusedRowCellDisplayText("loaithue").ToString()) / 100);
                    }
                    catch
                    {

                        gridView1.SetFocusedRowCellValue("nguyente", (((double.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (double.Parse(gridView1.GetFocusedRowCellValue("dongia").ToString()))) - ((double.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (double.Parse(gridView1.GetFocusedRowCellValue("chietkhau").ToString())))) + double.Parse(gridView1.GetFocusedRowCellValue("thue").ToString()));
                    }
                    finally
                    {
                        gridView1.SetFocusedRowCellValue("thue", (((double.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (double.Parse(gridView1.GetFocusedRowCellValue("dongia").ToString()))) - ((double.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (double.Parse(gridView1.GetFocusedRowCellValue("chietkhau").ToString())))) * double.Parse(gridView1.GetFocusedRowCellDisplayText("loaithue").ToString()) / 100);
                        gridView1.SetFocusedRowCellValue("nguyente", (((double.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (double.Parse(gridView1.GetFocusedRowCellValue("dongia").ToString()))) - ((double.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString())) * (double.Parse(gridView1.GetFocusedRowCellValue("chietkhau").ToString())))) + double.Parse(gridView1.GetFocusedRowCellValue("thue").ToString()));
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
                    gridView1.SetFocusedRowCellValue("thanhtien", (double.Parse(gridView1.GetFocusedRowCellValue("nguyente").ToString())) * (double.Parse(txttygia.Text)));
                }
                catch (Exception)
                {
                }
            }
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Biencucbo.hdhd == 2)
                return;
            if (e.KeyCode == Keys.Insert)
            {
                gridView1.AddNewRow();

            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (Biencucbo.hdhd == 1)
                {
                    try
                    {

                        hoadonct ct = (from c in db.hoadoncts select c).Single(x => x.id == gridView1.GetFocusedRowCellValue("id").ToString());
                        db.hoadoncts.DeleteOnSubmit(ct);
                    }
                    catch
                    {
                    }
                }
                gridView1.DeleteRow(gridView1.FocusedRowHandle);
            }
        }

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

        private void btnthue_EditValueChanged(object sender, EventArgs e)
        {
            gridView1.PostEditor();
        }

        //add new row
        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            if (Biencucbo.hdhd == 1)
            {
                var ct = gridView1.GetFocusedRow() as hoadonct;
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

                ct.idhoadon = txtid.Text;
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

        private void f_hd_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Biencucbo.hdhd != 2)
            {
                var a = Lotus.MsgBox.ShowYesNoCancelDialog("Hoá đơn này chưa được lưu - Bạn có muốn lưu Hoá đơn này trước khi thoát không?");
                if (a == DialogResult.Yes)
                {
                    luu();
                }
                else if (a == DialogResult.Cancel) e.Cancel = true;
            }
        }

        private void rsearchtiente_EditValueChanged(object sender, EventArgs e)
        {
            gridView1.PostEditor();
            try
            {
                var lst = (from a in db.tientes select a).Single(t => t.tiente1 == gridView1.GetFocusedRowCellValue("tiente").ToString());
                if (lst == null) return;
                gridView1.SetFocusedRowCellValue("tygia", lst.tygia);
            }
            catch
            {
            }
        }

        private void btnIdSp_EditValueChanged(object sender, EventArgs e)
        {
            gridView1.PostEditor();
        }

        private void f_hd_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Hoá Đơn Bán Hàng Kiêm Xuất Kho").ToString();

            changeFont.Translate(this);
            changeFont.Translate(barManager1);

            load();
        }

        private void btnthanhtoan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Biencucbo.hdhd == 0 || Biencucbo.hdhd == 1)
                return;
            Biencucbo.thanhtoan = txtid.Text;
            f_pthu_tt frm = new f_pthu_tt();
            frm.ShowDialog();
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
                        gridView1.SetRowCellValue(i, "thanhtien", double.Parse(gridView1.GetRowCellValue(i, "nguyente").ToString()) * double.Parse(txttygia.Text));
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void gia_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            f_giasp form = new f_giasp();
            form.ShowDialog();

            var lst = new DAL.KetNoiDBDataContext().r_giasps;
            try
            {
                var lst2 = (from a in lst where a.iddv == Biencucbo.donvi select new { id = a.idsp, tensp = a.tensp, dvt = a.dvt });
                if (lst2 == null) return;
                btnmasp.DataSource = lst2;
                rsearchTenSP.DataSource = btnmasp.DataSource;
                btndvt.DataSource = btnmasp.DataSource;
            }
            catch
            {
            }
        }

        private void btnPrev_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtngaylap.Text == "") return;
            else
            {
                try
                {
                    var lst = (from pn in db.hoadons select new { a = pn }).FirstOrDefault(x => x.a.iddv == Biencucbo.donvi && x.a.ngayhd == (txtngaylap.DateTime.AddDays(-1)));
                    if (lst == null)
                    {
                        lst = (from pn in db.hoadons select new { a = pn }).OrderByDescending(x => x.a.ngayhd).FirstOrDefault(x => x.a.iddv == Biencucbo.donvi && x.a.ngayhd < txtngaylap.DateTime);
                    }
                    txtid.Text = lst.a.id;
                    txtidnv.Text = lst.a.idnv;
                    txtdv.Text = lst.a.iddv;
                    txtngaylap.DateTime = DateTime.Parse(lst.a.ngayhd.ToString());
                    txtiddt.Text = lst.a.iddt;
                    txtghichu.Text = lst.a.ghichu;
                    txtvat.Text = lst.a.soVAT;
                    txttiente.Text = lst.a.tiente;
                    txttygia.Text = lst.a.tygia.ToString();
                    txt1.Text = lst.a.so.ToString();
                    txtloaihd.Text = lst.a.loaixuat;
                    gcchitiet.DataSource = lst.a.hoadoncts;
                }
                catch
                {
                }
            }
        }

        private void btnNext_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtngaylap.Text == "") return;
            else
            {
                try
                {
                    var lst = (from pn in db.hoadons select new { a = pn }).FirstOrDefault(x => x.a.iddv == Biencucbo.donvi && x.a.ngayhd == (txtngaylap.DateTime.AddDays(1)));
                    if (lst == null)
                    {
                        lst = (from pn in db.hoadons select new { a = pn }).OrderBy(x => x.a.ngayhd).FirstOrDefault(x => x.a.iddv == Biencucbo.donvi && x.a.ngayhd > txtngaylap.DateTime);
                    }
                    txtid.Text = lst.a.id;
                    txtidnv.Text = lst.a.idnv;
                    txtdv.Text = lst.a.iddv;
                    txtngaylap.DateTime = DateTime.Parse(lst.a.ngayhd.ToString());
                    txtiddt.Text = lst.a.iddt;
                    txtghichu.Text = lst.a.ghichu;
                    txtvat.Text = lst.a.soVAT;
                    txttiente.Text = lst.a.tiente;
                    txttygia.Text = lst.a.tygia.ToString();
                    txt1.Text = lst.a.so.ToString();
                    txtloaihd.Text = lst.a.loaixuat;
                    gcchitiet.DataSource = lst.a.hoadoncts;
                }
                catch
                {
                }
            }
        }
    }
}