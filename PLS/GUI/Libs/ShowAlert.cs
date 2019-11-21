using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraBars.Alerter;
using GUI.Properties;
using System.Windows.Forms;
using ControlLocalizer;

namespace GUI.Libs
{
    public class ShowAlert
    {
        public static LanguageEnum Language
        {
            get { return LanguageHelper.Language; }
        }

        public static void Alert_Add_Success(Form f)
        {
            AlertControl alertControl1 = new AlertControl();
            alertControl1.AutoFormDelay = 3000;
            alertControl1.ShowCloseButton = false;
            alertControl1.ShowPinButton = false;

            AlertInfo alert = new AlertInfo(Language == LanguageEnum.Vietnam ? "Thông báo" : "ແຈ້ງການ",
                Language == LanguageEnum.Vietnam ? "Thêm thành công" : "ການເພີ່ມສຳເລັດໍ",
                ((System.Drawing.Image)(Resources.apply_32x32)));
            alertControl1.Show(f, alert);
        }

        public static void Alert_Edit_Success(Form f)
        {
            AlertControl alertControl1 = new AlertControl();
            alertControl1.AutoFormDelay = 3000;
            alertControl1.ShowCloseButton = false;
            alertControl1.ShowPinButton = false;

            AlertInfo alert = new AlertInfo(Language == LanguageEnum.Vietnam ? "Thông báo" : "ແຈ້ງການ",
                Language == LanguageEnum.Vietnam ? "Sửa thành công" : "ດັດແກ້ສຳເລັດ",
                ((System.Drawing.Image)(Resources.apply_32x32)));
            alertControl1.Show(f, alert);
        }

        public static void Alert_Del_Success(Form f)
        {
            AlertControl alertControl1 = new AlertControl();
            alertControl1.AutoFormDelay = 3000;
            alertControl1.ShowCloseButton = false;
            alertControl1.ShowPinButton = false;

            AlertInfo alert = new AlertInfo(Language == LanguageEnum.Vietnam ? "Thông báo" : "ແຈ້ງການ",
                Language == LanguageEnum.Vietnam ? "Xoá thành công" : "ລົບສຳເລັດ",
                ((System.Drawing.Image)(Resources.apply_32x32)));
            alertControl1.Show(f, alert);
        }

        public static void Alert_Change_Success(Form f)
        {
            AlertControl alertControl1 = new AlertControl();
            alertControl1.AutoFormDelay = 3000;
            alertControl1.ShowCloseButton = false;
            alertControl1.ShowPinButton = false;

            AlertInfo alert = new AlertInfo(Language == LanguageEnum.Vietnam ? "Thông báo" : "ແຈ້ງການ",
                Language == LanguageEnum.Vietnam ? "Thay đổi thành công" : "ປ່ຽນແປງສຳເລັດ",
                ((System.Drawing.Image)(Resources.apply_32x32)));
            alertControl1.Show(f, alert);
        }
    }
}