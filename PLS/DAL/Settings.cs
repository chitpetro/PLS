using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public sealed class Settings
    {
        private static Settings defaultInstance = new Settings();
        public static Settings Default
        {
            get { return defaultInstance; }
        }

        public string ConnectionString
        {
            get
            {
                return Properties.Settings.Default.PLS_2017ConnectionString1;
            }
            set
            {
                Properties.Settings.Default["PLS_2017ConnectionString1"] = value;
            }
        }
    }
}
