namespace GUI
{
    partial class f_Skin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f_Skin));
            this.Skinht = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.searchLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cancel = new DevExpress.XtraEditors.SimpleButton();
            this.ok = new DevExpress.XtraEditors.SimpleButton();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Skinht.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // Skinht
            // 
            this.Skinht.Location = new System.Drawing.Point(12, 12);
            this.Skinht.Name = "Skinht";
            this.Skinht.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Skinht.Properties.DisplayMember = "tenskin";
            this.Skinht.Properties.NullText = "";
            this.Skinht.Properties.ValueMember = "tenskin";
            this.Skinht.Properties.View = this.searchLookUpEdit1View;
            this.Skinht.Size = new System.Drawing.Size(213, 20);
            this.Skinht.TabIndex = 2;
            this.Skinht.EditValueChanged += new System.EventHandler(this.Skinht_EditValueChanged);
            // 
            // searchLookUpEdit1View
            // 
            this.searchLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.searchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.searchLookUpEdit1View.Name = "searchLookUpEdit1View";
            this.searchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View.OptionsView.ShowAutoFilterRow = true;
            this.searchLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // cancel
            // 
            this.cancel.Image = ((System.Drawing.Image)(resources.GetObject("cancel.Image")));
            this.cancel.Location = new System.Drawing.Point(268, 9);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(30, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ok
            // 
            this.ok.Image = ((System.Drawing.Image)(resources.GetObject("ok.Image")));
            this.ok.Location = new System.Drawing.Point(232, 10);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(30, 23);
            this.ok.TabIndex = 3;
            this.ok.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Skin";
            this.gridColumn1.FieldName = "tenskin";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // f_Skin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 49);
            this.ControlBox = false;
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.Skinht);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "f_Skin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Skin Hệ Thống";
            this.Load += new System.EventHandler(this.f_Skin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Skinht.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SearchLookUpEdit Skinht;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View;
        private DevExpress.XtraEditors.SimpleButton ok;
        private DevExpress.XtraEditors.SimpleButton cancel;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}