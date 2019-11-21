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
using DevExpress.XtraEditors;
using System.Data.Linq;
using DevExpress.XtraReports.UI;
using ControlLocalizer;
using DevExpress.XtraBars;
using GUI.Libs;

namespace GUI
{
    public partial class f_chotcongto : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        KetNoiDBDataContext db = new KetNoiDBDataContext();
        t_chotcongto hd = new t_chotcongto();

        public f_chotcongto()
        {
            InitializeComponent();

            var q = Biencucbo.QuyenDangChon;
            if (q == null) return;

            if (Biencucbo.idnv == "AD" || (Biencucbo.phongban == "IT" && q.Sua == true) || (Biencucbo.phongban == "Admin" && q.Sua == true))
            {
                btnsetting.Visibility = BarItemVisibility.Always;
                btnSuaSoTT.Visibility = BarItemVisibility.Always;
                btnXoaNhanh.Visibility = BarItemVisibility.Always;
            }
            else
            {
                btnsetting.Visibility = BarItemVisibility.Never;
                btnSuaSoTT.Visibility = BarItemVisibility.Never;
                btnXoaNhanh.Visibility = BarItemVisibility.Never;
            }

            gridView1.ClearGrouping();
            var lst = (from a in db.donvis select a).Single(t => t.id == Biencucbo.dvTen);

            if (lst.dvql == "0")
            {
                gridView1.Columns["loaisp"].GroupIndex = 1;
                btnct.Enabled = true;
                btnmsp.Enabled = false;

            }
            else if (lst.dvql == "1")
            {
                gridView1.Columns["cotbom"].GroupIndex = 1;

                btnct.Enabled = false;
                btnmsp.Enabled = true;
            }

            if (Biencucbo.ngonngu.ToString() == "Vietnam")
            {
                gridColumn45.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "thu", "Tổng Cộng:")});
            }
            else
            {
                gridColumn45.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "thu", "ລວມທັງໝົດ:")});
            }

            //lay quyen
            var quyen1 = db.PhanQuyen2s.FirstOrDefault(p => p.TaiKhoan == Biencucbo.phongban && p.ChucNang == "ThanhToanKhachVangLai");
            btnthanhtoan.Enabled = (bool)quyen1.Xem;
            var quyen2 = db.PhanQuyen2s.FirstOrDefault(p => p.TaiKhoan == Biencucbo.phongban && p.ChucNang == "KhachNo");
            btnkhachno.Enabled = (bool)quyen2.Xem;
            var quyen3 = db.PhanQuyen2s.FirstOrDefault(p => p.TaiKhoan == Biencucbo.phongban && p.ChucNang == "ChotCongTo_LichSuThaoTac");
            try
            {
                ChotCongTo_LichSuThaoTac.Enabled = (bool)quyen3.Xem;
            }
            catch { }
        }

        //load
        public void load()
        {
            db = new KetNoiDBDataContext();
            Biencucbo.hdct = 2;
            txt1.Enabled = false;
            btnLuu.Enabled = false;
            btnsua.Enabled = true;
            btnxoa.Enabled = true;
            btnin.Enabled = true;
            btnreload.Enabled = false;
            txtdv.ReadOnly = true;
            txtid.ReadOnly = true;
            txtidnv.ReadOnly = true;
            txtphongban.ReadOnly = true;

            // Enable

            txtngaylap.ReadOnly = true;
            gridView1.OptionsBehavior.Editable = false;

            try
            {
                var lst = (from a in db.chotcongtos where a.iddv == Biencucbo.donvi select a.so).Max();
                var lst1 = (from b in db.chotcongtos where b.iddv == Biencucbo.donvi select b).FirstOrDefault(t => t.so == lst);
                if (lst1 == null) return;
                gcchitiet.DataSource = lst1.chotcongtocts;
                txtid.Text = lst1.id;
                txtidnv.Text = lst1.idnv;
                txtdv.Text = lst1.iddv;
                txtngaylap.DateTime = DateTime.Parse(lst1.ngay.ToString());
                txt1.Text = lst1.so.ToString();
                gridView1.ExpandAllGroups();

            }
            catch
            {
            }
        }


        //Mở
        private void btnmo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            db = new KetNoiDBDataContext();
            f_dscct frm = new f_dscct();
            frm.ShowDialog();
            if (Biencucbo.getID == 1)
            {
                //load hoa don
                try
                {
                    var lst = (from hd in db.chotcongtos select new { a = hd }).FirstOrDefault(x => x.a.id == Biencucbo.ma);

                    if (lst == null) return;

                    txtid.Text = lst.a.id;
                    txtidnv.Text = lst.a.idnv;
                    txtdv.Text = lst.a.iddv;
                    txtngaylap.DateTime = DateTime.Parse(lst.a.ngay.ToString());

                    txt1.Text = lst.a.so.ToString();

                    gcchitiet.DataSource = lst.a.chotcongtocts;
                    gridView1.ExpandAllGroups();

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
            Biencucbo.hdct = 0;
            txtid.DataBindings.Clear();
            txtid.Text = "YYYYY";
            gcchitiet.DataSource = new DAL.KetNoiDBDataContext().tam_chotcongtocts;


            var lst = from a in db.giasps
                      where a.iddv == Biencucbo.donvi
                      select a;
            var lst2 = (from a in db.chotcongtocts where a.iddv == Biencucbo.donvi select a).Max(t => t.so);
            var lst3 = (from a in db.chotcongtocts where a.iddv == Biencucbo.donvi && a.so == lst2 select a);
            var lst4 = from a in db.sodocotboms
                       where a.iddv == Biencucbo.donvi
                       join b in lst on a.idsp equals b.idsp
                       select new
                       {
                           cotbom = a.cotbom,
                           voibom = a.voibom,
                           loaisp = a.idsp,
                           chotdau = 0.000,
                           dongia = b.giaban,
                           stt = a.stt,
                       };
            var lst1 = lst4;

            if (lst2 != null)
            {
                //LEFT JOIN
                lst1 = from a in lst4
                       join c in lst3
                       on a.stt equals c.stt into joined
                       from j in joined.DefaultIfEmpty()
                       select new
                       {
                           cotbom = a.cotbom,
                           voibom = a.voibom,
                           loaisp = a.loaisp,
                           chotdau = j.chotcuoi == null ? 0 : double.Parse(j.chotcuoi.ToString()),
                           dongia = a.dongia,
                           stt = a.stt,
                       };
            }

            gridControl3.DataSource = lst1;
            for (int i = 0; i < gridView16.RowCount; i++)
            {
                gridView1.AddNewRow();
                gridView1.SetFocusedRowCellValue("cotbom", gridView16.GetRowCellValue(i, "cotbom"));
                gridView1.SetFocusedRowCellValue("voibom", gridView16.GetRowCellValue(i, "voibom"));
                gridView1.SetFocusedRowCellValue("loaisp", gridView16.GetRowCellValue(i, "loaisp"));
                gridView1.SetFocusedRowCellValue("chotdau", gridView16.GetRowCellValue(i, "chotdau"));
                gridView1.SetFocusedRowCellValue("chotcuoi", 0);
                gridView1.SetFocusedRowCellValue("thu", 0);
                gridView1.SetFocusedRowCellValue("iddv", Biencucbo.donvi);
                //gridView1.SetFocusedRowCellValue("soluong", 0);
                gridView1.SetFocusedRowCellValue("dongia", gridView16.GetRowCellValue(i, "dongia"));
                //gridView1.SetFocusedRowCellValue("thanhtien", 0);
                gridView1.SetFocusedRowCellValue("idct", "");
                gridView1.SetFocusedRowCellValue("id", "");
                gridView1.SetFocusedRowCellValue("stt", gridView16.GetRowCellValue(i, "stt"));
                gridView1.SetFocusedRowCellValue("so", 0);
            }
            gridView1.UpdateCurrentRow();
            gridView1.ExpandAllGroups();
            txtdv.Text = Biencucbo.donvi;
            txtngaylap.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            txtphongban.Text = Biencucbo.phongban;
            txtidnv.Text = Biencucbo.idnv.Trim();
            lbltennv.Text = Biencucbo.ten;

            btnnew.Enabled = false;
            btnmo.Enabled = false;
            btnLuu.Enabled = true;
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
            btnin.Enabled = false;
            btnreload.Enabled = false;
            //enabled

            txtngaylap.ReadOnly = false;
            txtngaylap.ReadOnly = false;
            gridView1.OptionsBehavior.Editable = true;
        }


        //Lưu
        public void luu()
        {
            t_history hs = new t_history();
            t_tudong td = new t_tudong();

            gridView1.UpdateCurrentRow();

            int check1 = 0;
            if (txtngaylap.Text == "")
            {
                Lotus.MsgBox.ShowWarningDialog("Thông tin chưa đầy đủ - Vui lòng kiểm tra lại!");
            }
            if (Convert.ToDateTime(txtngaylap.DateTime.ToShortDateString()) > Convert.ToDateTime(DateTime.Now.ToShortDateString()))
            {
                Lotus.MsgBox.ShowWarningDialog("Ngày nhập không thể lớn hơn Ngày hiện tại! Vui lòng kiểm tra lại!");
            }
            else
            {
                try
                {
                    for (int i = 0; i < gridView1.DataRowCount; i++)
                    {
                        //re-check
                        if (gridView1.GetRowCellDisplayText(i, "soluong").ToString() == "" || gridView1.GetRowCellDisplayText(i, "dongia").ToString() == "" || gridView1.GetRowCellDisplayText(i, "chotdau").ToString() == "" || gridView1.GetRowCellDisplayText(i, "chotcuoi").ToString() == "" || gridView1.GetRowCellDisplayText(i, "thu").ToString() == "")
                        {
                            check1 = 1;
                        }
                        else if (gridView1.GetRowCellDisplayText(i, "loaisp").ToString() == "")
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
                    Lotus.MsgBox.ShowWarningDialog("Thông tin chi tiết chưa đầy đủ - Vui Lòng Kiểm Tra Lại");
                }
                else if (check1 == 2)
                {
                    Lotus.MsgBox.ShowWarningDialog("Mã sản phẩm không được để trống - Vui Lòng Kiểm Tra Lại");
                }
                else
                {
                    if (Biencucbo.hdct == 0)
                    {
                        db = new KetNoiDBDataContext();
                        try
                        {
                            string check = "CT" + Biencucbo.donvi.Trim().ToString();
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
                            hd.moicongto(txtid.Text, txtngaylap.DateTime, txtidnv.Text, txtdv.Text, Convert.ToInt32(txt1.Text));
                            hs.add(txtid.Text, "Thêm mới Chốt công tơ - ຕື່ມການບັນທຶກເລກກົງເຕີໃໝ່ - (ERROR)");
                            for (int i = 0; i <= gridView1.DataRowCount - 1; i++)
                            {
                                gridView1.SetRowCellValue(i, "idct", txtid.Text);
                                gridView1.SetRowCellValue(i, "id", txtid.Text + i);
                                hd.moicongtoct(gridView1.GetRowCellValue(i, "cotbom").ToString(), gridView1.GetRowCellValue(i, "voibom").ToString(), gridView1.GetRowCellValue(i, "idct").ToString(), gridView1.GetRowCellValue(i, "loaisp").ToString(), double.Parse(gridView1.GetRowCellValue(i, "chotdau").ToString()), double.Parse(gridView1.GetRowCellValue(i, "chotcuoi").ToString()), double.Parse(gridView1.GetRowCellValue(i, "soluong").ToString()), double.Parse(gridView1.GetRowCellValue(i, "thu").ToString()), double.Parse(gridView1.GetRowCellValue(i, "dongia").ToString()), double.Parse(gridView1.GetRowCellValue(i, "thanhtien").ToString()), gridView1.GetRowCellValue(i, "id").ToString(), Convert.ToInt32(txt1.Text), Biencucbo.donvi, int.Parse(gridView1.GetRowCellValue(i, "stt").ToString()));
                            }
                            hs.edit(txtid.Text, "Thêm mới Chốt công tơ - ຕື່ມການບັນທຶກເລກກົງເຕີໃໝ່");
                            //btn
                            btnmo.Enabled = true;
                            btnnew.Enabled = true;
                            btnLuu.Enabled = false;
                            btnsua.Enabled = true;
                            btnxoa.Enabled = true;
                            btnin.Enabled = true;
                            btnreload.Enabled = false;

                            txtngaylap.ReadOnly = true;

                            gridView1.OptionsBehavior.Editable = false;
                            Biencucbo.hdct = 2;

                            // History


                            ShowAlert.Alert_Add_Success(this);
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message);
                        }
                    }
                    else //sua
                    {
                        try
                        {
                            hd.suachotcongto(txtid.Text, DateTime.Parse(txtngaylap.Text));

                            //sua ct
                            LuuPhieu();
                            //btn
                            btnmo.Enabled = true;
                            btnnew.Enabled = true;
                            btnLuu.Enabled = false;
                            btnsua.Enabled = true;
                            btnxoa.Enabled = true;
                            btnin.Enabled = true;
                            btnreload.Enabled = false;

                            //enabled

                            txtngaylap.ReadOnly = true;

                            gridView1.OptionsBehavior.Editable = false;
                            Biencucbo.hdct = 2;


                            hs.add(txtid.Text, "Sửa Chốt Công Tơ - ດັດແກ້ການບັນທຶກກົງເຕີ");
                            ShowAlert.Alert_Edit_Success(this);
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message);
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

            try
            {
                db.chotcongtocts.Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private void btnsua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //check khoa so
            if (checkKhoaSo.checkkhoaso(txtdv, txtngaylap) == false) return;

            if (txtid.Text == "") return;
            var check = (from a in db.chotcongtos
                         join b in db.hoadons on a.id equals b.link
                         where a.id == txtid.Text
                         select b);
            if (check.Count() == 0)
            {
                try
                {
                    var lst = (from pn in db.chotcongtos select pn).FirstOrDefault(x => x.id == txtid.Text);
                    if (lst == null) return;
                    gcchitiet.DataSource = lst.chotcongtocts;
                    //enabled

                    txtngaylap.ReadOnly = false;

                    gridView1.OptionsBehavior.Editable = true;
                    Biencucbo.hdct = 1;
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

            if (txtid.Text == "") return;

            var check = (from a in db.chotcongtos
                         join b in db.hoadons on a.id equals b.link
                         where a.id == txtid.Text
                         select b);
            if (check.Count() == 0)
            {
                if (XtraMessageBox.Show("Bạn có chắc chắn muốn xóa Phiếu " + txtid.Text + " không?", "THÔNG BÁO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    t_history hs = new t_history();

                    try
                    {
                        hs.add(txtid.Text, "Xóa chốt công tơ - ລົບການບັກທຶກກົງເຕີ");

                        for (int i = gridView1.DataRowCount - 1; i >= 0; i--)
                        {
                            hd.xoact(gridView1.GetRowCellValue(i, "id").ToString());
                            gridView1.DeleteRow(i);
                        }

                        hd.xoachotcongto(txtid.Text);

                        //btn
                        btnmo.Enabled = true;
                        btnnew.Enabled = true;
                        btnLuu.Enabled = false;
                        btnsua.Enabled = true;
                        btnxoa.Enabled = true;
                        btnin.Enabled = true;
                        btnreload.Enabled = false;

                        //enabled

                        txtngaylap.ReadOnly = true;

                        gridView1.OptionsBehavior.Editable = false;

                        txtdv.Text = "";
                        txtid.Text = "";
                        txtidnv.Text = "";
                        txtdv.Text = "";
                        txtngaylap.Text = "";

                        txt1.Text = "";

                        lbltennv.Text = "";

                        ShowAlert.Alert_Del_Success(this);
                    }
                    catch { }
                }
            }
            else
            {
                XtraMessageBox.Show("Không thể thao tác vì có sự liên kết - kiểm tra phiếu " + check.FirstOrDefault().id);
            }
        }
        private void btnload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Biencucbo.hdct == 1)
            {
                db = new KetNoiDBDataContext();

                var lst = (from pn in db.chotcongtos select pn).FirstOrDefault(x => x.id == txtid.Text);

                if (lst == null) return;

                gcchitiet.DataSource = lst.chotcongtocts;
                gridView1.ExpandAllGroups();

                txtidnv.Text = lst.idnv;
                txtdv.Text = lst.iddv;
                txtngaylap.DateTime = DateTime.Parse(lst.ngay.ToString());


                txt1.Text = lst.so.ToString();

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

            else if (Biencucbo.hdct == 0)
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
            Biencucbo.hdct = 2;
        }
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            gridView1.PostEditor();
            if (e.Column.FieldName == "dongia" || e.Column.FieldName == "thu" || e.Column.FieldName == "chotdau" || e.Column.FieldName == "chotcuoi")
            {

                try
                {
                    try
                    {
                        gridView1.SetFocusedRowCellValue("soluong", double.Parse(gridView1.GetFocusedRowCellValue("chotcuoi").ToString()) - double.Parse(gridView1.GetFocusedRowCellValue("chotdau").ToString()) - double.Parse(gridView1.GetFocusedRowCellValue("thu").ToString()));
                    }
                    catch
                    {

                        gridView1.SetFocusedRowCellValue("thanhtien", double.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString()) * double.Parse(gridView1.GetFocusedRowCellValue("dongia").ToString()));
                    }
                    finally
                    {
                        gridView1.SetFocusedRowCellValue("soluong", double.Parse(gridView1.GetFocusedRowCellValue("chotcuoi").ToString()) - double.Parse(gridView1.GetFocusedRowCellValue("chotdau").ToString()) - double.Parse(gridView1.GetFocusedRowCellValue("thu").ToString()));
                        gridView1.SetFocusedRowCellValue("thanhtien", double.Parse(gridView1.GetFocusedRowCellValue("soluong").ToString()) * double.Parse(gridView1.GetFocusedRowCellValue("dongia").ToString()));
                    }


                }
                catch (Exception)
                {
                }
            }
        }

        private void f_hd_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Biencucbo.hdct != 2)
            {
                var a = Lotus.MsgBox.ShowYesNoCancelDialog("Hoá đơn này chưa được lưu - Bạn có muốn lưu Hoá đơn này trước khi thoát không?");
                if (a == DialogResult.Yes)
                {
                    luu();
                }
                else if (a == DialogResult.Cancel) e.Cancel = true;
            }
        }
        private void f_hd_Load(object sender, EventArgs e)
        {
            LanguageHelper.Translate(this);
            LanguageHelper.Translate(barManager1);
            this.Text = LanguageHelper.TranslateMsgString("." + Name + "_title", "Chốt Công Tơ").ToString();

            changeFont.Translate(this);
            changeFont.Translate(barManager1);

            load();

        }

        private void btnthanhtoan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Biencucbo.hdct == 0 || Biencucbo.hdct == 1)
                return;
            Biencucbo.chotcongto = txtid.Text;
            f_hd_khach frm = new f_hd_khach();
            frm.ShowDialog();
        }

        private void gia_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

        private void btnct_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            donvi dv = (from d in db.donvis select d).Single(t => t.id == Biencucbo.dvTen);
            dv.dvql = "1";
            db.SubmitChanges();
            gridView1.ClearGrouping();
            gridView1.Columns["cotbom"].GroupIndex = 1;
            btnct.Enabled = false;
            btnmsp.Enabled = true;
            gridView1.ExpandAllGroups();
        }

        private void btnmsp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            donvi dv = (from d in db.donvis select d).Single(t => t.id == Biencucbo.dvTen);
            dv.dvql = "0";
            db.SubmitChanges();
            gridView1.ClearGrouping();
            gridView1.Columns["loaisp"].GroupIndex = 1;
            btnct.Enabled = true;
            btnmsp.Enabled = false;
            gridView1.ExpandAllGroups();
        }

        private void btnkhachno_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Biencucbo.hdct == 0 || Biencucbo.hdct == 1)
                return;
            Biencucbo.chotcongto = txtid.Text;
            f_hd_khachno frm = new f_hd_khachno();
            frm.ShowDialog();
        }

        private void btnsodo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Biencucbo.QuyenDangChon = e.Item.Tag as PhanQuyen2;
            if (Biencucbo.hdct == 0 || Biencucbo.hdct == 1)
                return;

            f_sodo frm = new f_sodo();
            frm.ShowDialog();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            f_kt_banhang kt = new f_kt_banhang();
            kt.ShowDialog();
        }

        private void btnsetting_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridColumn39.OptionsColumn.ReadOnly = false;
            gridColumn39.OptionsColumn.AllowFocus = true;

            gridColumn54.OptionsColumn.ReadOnly = false;
            gridColumn54.OptionsColumn.AllowFocus = true;
        }

        private void btnPre_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (txtngaylap.Text == "") return;
            else
            {
                //load hoa don
                try
                {
                    var lst = (from hd in db.chotcongtos select new { a = hd }).FirstOrDefault(x => x.a.iddv == Biencucbo.donvi && x.a.ngay == (txtngaylap.DateTime.AddDays(-1)));

                    if (lst == null) return;

                    txtid.Text = lst.a.id;
                    txtidnv.Text = lst.a.idnv;
                    txtdv.Text = lst.a.iddv;
                    txtngaylap.DateTime = DateTime.Parse(lst.a.ngay.ToString());

                    txt1.Text = lst.a.so.ToString();

                    gcchitiet.DataSource = lst.a.chotcongtocts;
                    gridView1.ExpandAllGroups();

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

        private void btnNext_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (txtngaylap.Text == "") return;
            else
            {
                //load hoa don
                try
                {
                    var lst = (from hd in db.chotcongtos select new { a = hd }).FirstOrDefault(x => x.a.iddv == Biencucbo.donvi && x.a.ngay == (txtngaylap.DateTime.AddDays(1)));

                    if (lst == null) return;

                    txtid.Text = lst.a.id;
                    txtidnv.Text = lst.a.idnv;
                    txtdv.Text = lst.a.iddv;
                    txtngaylap.DateTime = DateTime.Parse(lst.a.ngay.ToString());

                    txt1.Text = lst.a.so.ToString();

                    gcchitiet.DataSource = lst.a.chotcongtocts;
                    gridView1.ExpandAllGroups();

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

        public static string id = "";
        public static string sott = "";
        private void btnSuaSoTT_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Biencucbo.hdct == 2)
            {
                //load hoa don
                try
                {
                    //var lst = (from hd in db.chotcongtos select new { a = hd }).FirstOrDefault(x => x.a.iddv == Biencucbo.donvi && x.a.id == txtid.Text);
                    var lst = (from hd in db.chotcongtos select new { a = hd }).FirstOrDefault(x => x.a.id == txtid.Text);

                    if (lst == null) return;

                    id = lst.a.id;
                    sott = lst.a.so.ToString();

                    frmSuaSo f = new frmSuaSo();
                    f.ShowDialog();
                }
                catch
                {
                }
            }
        }

        t_pthu pt = new t_pthu();
        t_hoadon hdon = new t_hoadon();
        private void btnXoaNhanh_ItemClick(object sender, ItemClickEventArgs e)
        {
            //check khoa so
            //if (checkKhoaSo.checkkhoaso(txtdv, txtngaylap) == false) return;

            if (txtid.Text == "") return;

            if (XtraMessageBox.Show("Bạn có chắc chắn muốn xóa Phiếu " + txtid.Text + " không?", "THÔNG BÁO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                t_history hs = new t_history();

                try
                {
                    //hs.add(txtid.Text, "Xóa chốt công tơ - ລົບການບັກທຶກກົງເຕີ");

                    var list = (from a in db.View_XoaNhanh_ChotCT_HoaDon_PThus select a).FirstOrDefault(x => x.idCongTo == txtid.Text.Trim());
                    if (list != null)
                    {
                        //xoa
                        //count
                        var pthu_count = (from a in db.pthus where a.id == list.idPThu select a).Count();
                        var pthuct_count = (from a in db.pthucts where a.idthu == list.idPThuCT select a).Count();
                        var hoadon_count = (from a in db.hoadons where a.id == list.idHoaDon select a).Count();
                        var hoadonct_count = (from a in db.hoadoncts where a.idhoadon == list.idHoaDonCT select a).Count();
                        var chotCTct_count = (from a in db.chotcongtocts where a.idct == list.icCongToCT select a).Count();
                        var chotCT_count = (from a in db.chotcongtos where a.id == list.idCongTo select a).Count();
                        //xoa  pthu

                        for (int i = 0; i < pthuct_count; i++)
                        {
                            //pt.xoact(list.idPThuCT);

                            pthuct ct = (from c in db.pthucts select c).FirstOrDefault(x => x.idthu == list.idPThuCT);
                            db.pthucts.DeleteOnSubmit(ct);
                            db.SubmitChanges();
                        }
                        for (int i = 0; i < pthu_count; i++)
                        {
                            //pt.xoapthu(list.idPThu);
                            pthu pt = (from c in db.pthus select c).FirstOrDefault(x => x.id == list.idPThu);
                            db.pthus.DeleteOnSubmit(pt);
                            db.SubmitChanges();
                        }
                        for (int i = 0; i < hoadonct_count; i++)
                        {
                            hoadonct ct = (from c in db.hoadoncts select c).FirstOrDefault(x => x.idhoadon == list.idHoaDonCT);
                            db.hoadoncts.DeleteOnSubmit(ct);
                            db.SubmitChanges();
                        }
                        for (int i = 0; i < hoadon_count; i++)
                        {
                            hoadon hd = (from c in db.hoadons select c).Single(x => x.id == list.idHoaDon);
                            db.hoadons.DeleteOnSubmit(hd);
                            db.SubmitChanges();
                        }
                        for (int i = 0; i < chotCTct_count; i++)
                        {
                            chotcongtoct ct = (from c in db.chotcongtocts select c).FirstOrDefault(x => x.idct == list.icCongToCT);
                            db.chotcongtocts.DeleteOnSubmit(ct);
                            db.SubmitChanges();
                        }
                        for (int i = 0; i < chotCT_count; i++)
                        {
                            chotcongto hd = (from c in db.chotcongtos select c).Single(x => x.id == list.idCongTo);
                            db.chotcongtos.DeleteOnSubmit(hd);
                            db.SubmitChanges();
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

                        txtngaylap.ReadOnly = true;

                        gridView1.OptionsBehavior.Editable = false;

                        txtdv.Text = "";
                        txtid.Text = "";
                        txtidnv.Text = "";
                        txtdv.Text = "";
                        txtngaylap.Text = "";

                        txt1.Text = "";

                        lbltennv.Text = "";

                        ShowAlert.Alert_Del_Success(this);

                    }
                    else
                    {
                        MessageBox.Show("Failed");
                    }

                }
                catch (Exception ex)
                { MessageBox.Show("Failed"); }
            }
        }

        private void ChotCongTo_LichSuThaoTac_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Biencucbo.donvi == "00")
            {
                Lotus.MsgBox.ShowWarningDialog("Vui lòng Chuyển Đơn Vị về Chi Nhánh hoặc Cửa Hàng!");
            }
            else
            {
                f_History_ChotCT f = new f_History_ChotCT();
                f.ShowDialog();
            }
        }
    }
}