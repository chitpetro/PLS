using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BUS
{
    public class Biencucbo
    {
        // hethong
        public static string ten = "acmin";
        public static string idnv = "admin";
        public static string phongban = "IT";
        public static string donvi = "00";
        public static string tendvbc = "";

        public static string dvTen = "";
        public static string ma = "";
        public static string tien = "";
        public static int getID = 0;
        public static string skin = "";
        public static string skin2 = "";
        public static string hostname = "";
        public static string IPaddress = "";
        public static string DbName;
        public static string ServerName;

        // account
        public static int hdaccount = 0;
        public static int hdaccount2 = 2;
        public static int soaccount = 1;
        public static string account = "";

        // donvi
        public static int hddv = 0;

        // doituong 
        public static int hddt = 0;

        // phong ban
        public static int hdpb = 0;

        //nhomdoituong
        public static int hdndt = 0;

        //sanpham
        public static int hdsp = 0;

        // Phiếu nhập
        public static int hdpn = 0;

        // Hoá đơn
        public static int hdhd = 0;

        //Chốt công tơ
        public static int hdct = 0;
        public static string chotcongto = "";

        //Phiếu thu tiền
        public static int hdpt = 0;
        public static string thanhtoan = "";

        // phiếu chi tiền
        public static int hdpc = 0;

        // Khai báo giá
        public static int hdgia = 0;
        public static int hdkk = 0;

        // sơ đồ trụ bơm
        public static int hdsd = 0;

        //báo cáo
        public static string kho = "Tất cả";
        public static string sp = "Tất cả";
        public static string doituong = "Tất cả";
        public static string congviec = "Tất cả";
        public static string time = "";
        public static string loai = "";
        public static string muccp = "";

        public static double tondau = 0;
        public static double tondau2 = 0;
        public static double toncuoi = 0;

        //ngon ngu
        public static object ngonngu;
        // DMTK
        public static int hddmtk;

        //tiền tệ
        public static int hdtt = 0;

        // thuế bán le
        public static float thuebl = 10;

        // phân quyền
        public static PhanQuyen2 QuyenDangChon { get; set; }

        //lay moc time
        public static int gtime = 0;
         
    }
}
