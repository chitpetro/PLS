namespace GUI
{
    partial class f_connectDB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f_connectDB));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnsave = new DevExpress.XtraEditors.SimpleButton();
            this.btntestconnect = new DevExpress.XtraEditors.SimpleButton();
            this.txtpass = new DevExpress.XtraEditors.TextEdit();
            this.txtuname = new DevExpress.XtraEditors.TextEdit();
            this.txtdata = new DevExpress.XtraEditors.TextEdit();
            this.txtserver = new DevExpress.XtraEditors.TextEdit();
            this.rdgmode = new DevExpress.XtraEditors.RadioGroup();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem5 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtpass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtuname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtdata.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtserver.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdgmode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnsave);
            this.layoutControl1.Controls.Add(this.btntestconnect);
            this.layoutControl1.Controls.Add(this.txtpass);
            this.layoutControl1.Controls.Add(this.txtuname);
            this.layoutControl1.Controls.Add(this.txtdata);
            this.layoutControl1.Controls.Add(this.txtserver);
            this.layoutControl1.Controls.Add(this.rdgmode);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(404, 206);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnsave
            // 
           
            this.btnsave.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            this.btnsave.Location = new System.Drawing.Point(202, 164);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(106, 30);
            this.btnsave.StyleController = this.layoutControl1;
            this.btnsave.TabIndex = 10;
            this.btnsave.Text = "Save && Exit";
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // btntestconnect
            // 
       
            this.btntestconnect.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            this.btntestconnect.Location = new System.Drawing.Point(98, 164);
            this.btntestconnect.Name = "btntestconnect";
            this.btntestconnect.Size = new System.Drawing.Size(100, 30);
            this.btntestconnect.StyleController = this.layoutControl1;
            this.btntestconnect.TabIndex = 9;
            this.btntestconnect.Text = "Test Connect";
            this.btntestconnect.Click += new System.EventHandler(this.btntestconnect_Click);
            // 
            // txtpass
            // 
            this.txtpass.Location = new System.Drawing.Point(63, 130);
            this.txtpass.Name = "txtpass";
          
            this.txtpass.Properties.UseSystemPasswordChar = true;
            this.txtpass.Size = new System.Drawing.Size(329, 20);
            this.txtpass.StyleController = this.layoutControl1;
            this.txtpass.TabIndex = 8;
            // 
            // txtuname
            // 
            this.txtuname.Location = new System.Drawing.Point(63, 106);
            this.txtuname.Name = "txtuname";
            this.txtuname.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("txtuname.Properties.ContextImageOptions.Image")));
            this.txtuname.Size = new System.Drawing.Size(329, 20);
            this.txtuname.StyleController = this.layoutControl1;
            this.txtuname.TabIndex = 7;
            // 
            // txtdata
            // 
            this.txtdata.Location = new System.Drawing.Point(63, 82);
            this.txtdata.Name = "txtdata";
            this.txtdata.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("txtdata.Properties.ContextImageOptions.Image")));
            this.txtdata.Size = new System.Drawing.Size(329, 20);
            this.txtdata.StyleController = this.layoutControl1;
            this.txtdata.TabIndex = 6;
            // 
            // txtserver
            // 
            this.txtserver.Location = new System.Drawing.Point(63, 58);
            this.txtserver.Name = "txtserver";
            this.txtserver.Properties.ContextImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("txtserver.Properties.ContextImageOptions.Image")));
            this.txtserver.Size = new System.Drawing.Size(329, 20);
            this.txtserver.StyleController = this.layoutControl1;
            this.txtserver.TabIndex = 5;
            // 
            // rdgmode
            // 
            this.rdgmode.Location = new System.Drawing.Point(134, 32);
            this.rdgmode.Name = "rdgmode";
            this.rdgmode.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "LAN"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "NETWORK")});
            this.rdgmode.Size = new System.Drawing.Size(148, 22);
            this.rdgmode.StyleController = this.layoutControl1;
            this.rdgmode.TabIndex = 4;
            this.rdgmode.SelectedIndexChanged += new System.EventHandler(this.rdgmode_SelectedIndexChanged);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.emptySpaceItem3,
            this.emptySpaceItem4,
            this.emptySpaceItem5});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(404, 206);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.rdgmode;
            this.layoutControlItem1.Location = new System.Drawing.Point(122, 20);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(54, 14);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(152, 26);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtserver;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 46);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(151, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(384, 24);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Text = "Server";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(48, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txtdata;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 70);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(151, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(384, 24);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.Text = "Database";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(48, 13);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txtuname;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 94);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(151, 24);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(384, 24);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.Text = "Username";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(48, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.txtpass;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 118);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(151, 24);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(384, 34);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Text = "Password";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(48, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btntestconnect;
            this.layoutControlItem6.Location = new System.Drawing.Point(86, 152);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(104, 34);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.btnsave;
            this.layoutControlItem7.Location = new System.Drawing.Point(190, 152);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(110, 34);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(384, 20);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 20);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(122, 26);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(274, 20);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(110, 26);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.Location = new System.Drawing.Point(0, 152);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(86, 34);
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem5
            // 
            this.emptySpaceItem5.AllowHotTrack = false;
            this.emptySpaceItem5.Location = new System.Drawing.Point(300, 152);
            this.emptySpaceItem5.Name = "emptySpaceItem5";
            this.emptySpaceItem5.Size = new System.Drawing.Size(84, 34);
            this.emptySpaceItem5.TextSize = new System.Drawing.Size(0, 0);
            // 
            // f_connectDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 206);
            this.Controls.Add(this.layoutControl1);
            this.Name = "f_connectDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CONNECT DATABASE";
            this.Load += new System.EventHandler(this.f_connectDB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtpass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtuname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtdata.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtserver.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdgmode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton btnsave;
        private DevExpress.XtraEditors.SimpleButton btntestconnect;
        private DevExpress.XtraEditors.TextEdit txtpass;
        private DevExpress.XtraEditors.TextEdit txtuname;
        private DevExpress.XtraEditors.TextEdit txtdata;
        private DevExpress.XtraEditors.TextEdit txtserver;
        private DevExpress.XtraEditors.RadioGroup rdgmode;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem5;
    }
}