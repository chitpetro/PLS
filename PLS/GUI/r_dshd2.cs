﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid;
using BUS;
using ControlLocalizer;
using System.Drawing.Printing;

namespace GUI
{
    public partial class r_dshd2 : DevExpress.XtraReports.UI.XtraReport
    {
        public r_dshd2()
        {
            InitializeComponent();

            LanguageHelper.Translate(this);

            changeFont.Translate(this);

            txttime.Text = Biencucbo.time;

            tran_rp.tran1(ngay2, xrPageInfo2);
        } 
    }
}