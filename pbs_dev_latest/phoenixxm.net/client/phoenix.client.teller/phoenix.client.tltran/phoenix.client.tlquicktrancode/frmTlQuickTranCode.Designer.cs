using System;
using DevExpress.XtraBars.Docking;
using Phoenix.Windows.Forms;

namespace Phoenix.Client.Teller
{
    partial class frmTlQuickTranCode
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
			this.components = new System.ComponentModel.Container();
			this.dockMgr = new Phoenix.Windows.Forms.PDxDockManager(this.components);
			this.barAndDockingController1 = new Phoenix.Windows.Forms.PDxBarAndDockingController(this.components);
			this.dockPanelCustomer = new Phoenix.Client.Teller.PDxDockPanelUnderline();
			this.dockPanel3_Container = new Phoenix.Windows.Forms.PDxControlContainer();
			this.ucCustomer1 = new Phoenix.Client.Teller.ucCustomer();
			this.dockPanelCashIn = new Phoenix.Client.Teller.PDxDockPanelUnderline();
			this.controlContainer1 = new Phoenix.Windows.Forms.PDxControlContainer();
			this.ucCashIn1 = new Phoenix.Client.Teller.ucCashIn();
			this.dockPanelAccounts = new Phoenix.Client.Teller.PDxDockPanelUnderline();
			this.dockPanel2_Container = new Phoenix.Windows.Forms.PDxControlContainer();
			this.ucAccounts1 = new Phoenix.Client.Teller.ucAccounts();
			this.panelTransactions = new Phoenix.Client.Teller.PDxGroupControlUnderline();
			this.ucTransactions1 = new Phoenix.Client.Teller.ucTransactions();
			this.documentMgr = new Phoenix.Windows.Forms.PDxDocumentManager(this.components);
			this.noDocumentsView1 = new DevExpress.XtraBars.Docking2010.Views.NoDocuments.NoDocumentsView(this.components);
			((System.ComponentModel.ISupportInitialize)(this.dockMgr)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
			this.dockPanelCustomer.SuspendLayout();
			this.dockPanel3_Container.SuspendLayout();
			this.dockPanelCashIn.SuspendLayout();
			this.controlContainer1.SuspendLayout();
			this.dockPanelAccounts.SuspendLayout();
			this.dockPanel2_Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.panelTransactions)).BeginInit();
			this.panelTransactions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.documentMgr)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.noDocumentsView1)).BeginInit();
			this.SuspendLayout();
			// 
			// dockMgr
			// 
			this.dockMgr.Controller = this.barAndDockingController1;
			this.dockMgr.DockingEnabled = true;
			this.dockMgr.Form = this;
			this.dockMgr.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanelCustomer,
			this.dockPanelCashIn,
			this.dockPanelAccounts});
			this.dockMgr.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl"});
			// 
			// barAndDockingController1
			// 
			this.barAndDockingController1.PaintStyleName = "mScheme";
			// 
			// dockPanelCustomer
			// 
			this.ucTransactions1.SetBoundPropertyName(this.dockPanelCustomer, "");
			this.ucAccounts1.SetBoundPropertyName(this.dockPanelCustomer, "");
			this.dockPanelCustomer.Controls.Add(this.dockPanel3_Container);
			this.dockPanelCustomer.Dock = DevExpress.XtraBars.Docking.DockingStyle.Top;
			this.dockPanelCustomer.DrawUnderline = true;
			this.dockPanelCustomer.FloatSize = new System.Drawing.Size(200, 53);
			this.dockPanelCustomer.FloatVertical = true;
			this.dockPanelCustomer.ID = new System.Guid("e84c4c1f-0c91-4261-bfc5-10ae602ae509");
			this.dockPanelCustomer.IsResizable = false;
			this.dockPanelCustomer.Location = new System.Drawing.Point(0, 0);
			this.dockPanelCustomer.Name = "dockPanelCustomer";
			this.dockPanelCustomer.Options.AllowFloating = false;
			this.dockPanelCustomer.Options.FloatOnDblClick = false;
			this.dockPanelCustomer.Options.ShowCloseButton = false;
			this.dockPanelCustomer.OriginalSize = new System.Drawing.Size(200, 63);
			this.dockPanelCustomer.PanelCollection = null;
			this.dockPanelCustomer.Size = new System.Drawing.Size(830, 63);
			this.dockPanelCustomer.Text = "Customer";
			this.dockPanelCustomer.UnderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(165)))), ((int)(((byte)(183)))));
			this.dockPanelCustomer.UnderlineLocation = new System.Drawing.Point(60, 20);
			this.dockPanelCustomer.UnderlineWidth = 5;
			// 
			// dockPanel3_Container
			// 
			this.ucAccounts1.SetBoundPropertyName(this.dockPanel3_Container, "");
			this.ucTransactions1.SetBoundPropertyName(this.dockPanel3_Container, "");
			this.dockPanel3_Container.Controls.Add(this.ucCustomer1);
			this.dockPanel3_Container.IsResizable = false;
			this.dockPanel3_Container.Location = new System.Drawing.Point(4, 16);
			this.dockPanel3_Container.Name = "dockPanel3_Container";
			this.dockPanel3_Container.Size = new System.Drawing.Size(822, 42);
			this.dockPanel3_Container.TabIndex = 0;
			// 
			// ucCustomer1
			// 
			this.ucAccounts1.SetBoundPropertyName(this.ucCustomer1, "");
			this.ucTransactions1.SetBoundPropertyName(this.ucCustomer1, "");
			this.ucCustomer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucCustomer1.ForeColor = System.Drawing.SystemColors.WindowText;
			this.ucCustomer1.Location = new System.Drawing.Point(0, 0);
			this.ucCustomer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.ucCustomer1.Name = "ucCustomer1";
			this.ucCustomer1.Size = new System.Drawing.Size(822, 42);
			this.ucCustomer1.TabIndex = 0;
			// 
			// dockPanelCashIn
			// 
			this.ucTransactions1.SetBoundPropertyName(this.dockPanelCashIn, "");
			this.ucAccounts1.SetBoundPropertyName(this.dockPanelCashIn, "");
			this.dockPanelCashIn.Controls.Add(this.controlContainer1);
			this.dockPanelCashIn.Dock = DevExpress.XtraBars.Docking.DockingStyle.Top;
			this.dockPanelCashIn.DrawUnderline = true;
			this.dockPanelCashIn.FloatVertical = true;
			this.dockPanelCashIn.ID = new System.Guid("68890719-9c72-4fc8-8cbc-8b2b18da5308");
			this.dockPanelCashIn.IsResizable = false;
			this.dockPanelCashIn.Location = new System.Drawing.Point(0, 258);
			this.dockPanelCashIn.Name = "dockPanelCashIn";
			this.dockPanelCashIn.Options.AllowFloating = false;
			this.dockPanelCashIn.Options.ShowAutoHideButton = false;
			this.dockPanelCashIn.Options.ShowCloseButton = false;
			this.dockPanelCashIn.OriginalSize = new System.Drawing.Size(200, 63);
			this.dockPanelCashIn.PanelCollection = null;
			this.dockPanelCashIn.Size = new System.Drawing.Size(830, 63);
			this.dockPanelCashIn.Text = "Cash In and Check Details";
            this.dockPanelCashIn.UnderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(165)))), ((int)(((byte)(183)))));
			this.dockPanelCashIn.UnderlineLocation = new System.Drawing.Point(38, 20);
			this.dockPanelCashIn.UnderlineWidth = 5;
			// 
			// controlContainer1
			// 
			this.ucAccounts1.SetBoundPropertyName(this.controlContainer1, "");
			this.ucTransactions1.SetBoundPropertyName(this.controlContainer1, "");
			this.controlContainer1.Controls.Add(this.ucCashIn1);
			this.controlContainer1.IsResizable = false;
			this.controlContainer1.Location = new System.Drawing.Point(4, 16);
			this.controlContainer1.Name = "controlContainer1";
			this.controlContainer1.Size = new System.Drawing.Size(822, 42);
			this.controlContainer1.TabIndex = 0;
			// 
			// ucCashIn1
			// 
			this.ucTransactions1.SetBoundPropertyName(this.ucCashIn1, "");
			this.ucAccounts1.SetBoundPropertyName(this.ucCashIn1, "");
			this.ucCashIn1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucCashIn1.Location = new System.Drawing.Point(0, 0);
			this.ucCashIn1.Margin = new System.Windows.Forms.Padding(2);
			this.ucCashIn1.Name = "ucCashIn1";
			this.ucCashIn1.Size = new System.Drawing.Size(822, 42);
			this.ucCashIn1.TabIndex = 0;
			// 
			// dockPanelAccounts
			// 
			this.ucTransactions1.SetBoundPropertyName(this.dockPanelAccounts, "");
			this.ucAccounts1.SetBoundPropertyName(this.dockPanelAccounts, "");
			this.dockPanelAccounts.Controls.Add(this.dockPanel2_Container);
			this.dockPanelAccounts.Dock = DevExpress.XtraBars.Docking.DockingStyle.Top;
			this.dockPanelAccounts.DrawUnderline = true;
			this.dockPanelAccounts.FloatVertical = true;
			this.dockPanelAccounts.ID = new System.Guid("4c3e655b-1d07-43ac-854d-07e9cd35f134");
			this.dockPanelAccounts.IsResizable = false;
			this.dockPanelAccounts.Location = new System.Drawing.Point(0, 63);
			this.dockPanelAccounts.Name = "dockPanelAccounts";
			this.dockPanelAccounts.Options.AllowFloating = false;
			this.dockPanelAccounts.Options.FloatOnDblClick = false;
			this.dockPanelAccounts.Options.ShowCloseButton = false;
			this.dockPanelAccounts.OriginalSize = new System.Drawing.Size(200, 195);
			this.dockPanelAccounts.PanelCollection = null;
			this.dockPanelAccounts.Size = new System.Drawing.Size(830, 195);
			this.dockPanelAccounts.Text = "Accounts";
			this.dockPanelAccounts.UnderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(165)))), ((int)(((byte)(183)))));
			this.dockPanelAccounts.UnderlineLocation = new System.Drawing.Point(28, 20);
			this.dockPanelAccounts.UnderlineWidth = 5;
			// 
			// dockPanel2_Container
			// 
			this.ucAccounts1.SetBoundPropertyName(this.dockPanel2_Container, "");
			this.ucTransactions1.SetBoundPropertyName(this.dockPanel2_Container, "");
			this.dockPanel2_Container.Controls.Add(this.ucAccounts1);
			this.dockPanel2_Container.IsResizable = false;
			this.dockPanel2_Container.Location = new System.Drawing.Point(4, 16);
			this.dockPanel2_Container.Name = "dockPanel2_Container";
			this.dockPanel2_Container.Size = new System.Drawing.Size(822, 174);
			this.dockPanel2_Container.TabIndex = 0;
			// 
			// ucAccounts1
			// 
			this.ucTransactions1.SetBoundPropertyName(this.ucAccounts1, "");
			this.ucAccounts1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucAccounts1.Location = new System.Drawing.Point(0, 0);
			this.ucAccounts1.Margin = new System.Windows.Forms.Padding(2);
			this.ucAccounts1.Name = "ucAccounts1";
			this.ucAccounts1.Size = new System.Drawing.Size(822, 174);
			this.ucAccounts1.TabIndex = 0;
			// 
			// panelTransactions
			// 
			this.panelTransactions.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.panelTransactions.AppearanceCaption.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.panelTransactions.AppearanceCaption.Options.UseFont = true;
			this.panelTransactions.AppearanceCaption.Options.UseForeColor = true;
			this.ucTransactions1.SetBoundPropertyName(this.panelTransactions, "");
			this.ucAccounts1.SetBoundPropertyName(this.panelTransactions, "");
			this.panelTransactions.Controls.Add(this.ucTransactions1);
			this.panelTransactions.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
			this.panelTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelTransactions.DrawUnderline = true;
			this.panelTransactions.Location = new System.Drawing.Point(0, 321);
			this.panelTransactions.Name = "panelTransactions";
			this.panelTransactions.Size = new System.Drawing.Size(830, 194);
			this.panelTransactions.TabIndex = 6;
			this.panelTransactions.Text = "Transactions";
			this.panelTransactions.UnderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(165)))), ((int)(((byte)(183)))));
			this.panelTransactions.UnderlineLocation = new System.Drawing.Point(32, 19);
			this.panelTransactions.UnderlineWidth = 5;
			// 
			// ucTransactions1
			// 
			this.ucAccounts1.SetBoundPropertyName(this.ucTransactions1, "");
			this.ucTransactions1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucTransactions1.Location = new System.Drawing.Point(2, 20);
			this.ucTransactions1.Margin = new System.Windows.Forms.Padding(2);
			this.ucTransactions1.Name = "ucTransactions1";
			this.ucTransactions1.Size = new System.Drawing.Size(826, 172);
			this.ucTransactions1.TabIndex = 0;
			// 
			// documentMgr
			// 
			this.documentMgr.ClientControl = this.panelTransactions;
			this.documentMgr.View = this.noDocumentsView1;
			this.documentMgr.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.noDocumentsView1});
			// 
			// frmTlQuickTranCode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ucTransactions1.SetBoundPropertyName(this, "");
			this.ucAccounts1.SetBoundPropertyName(this, "");
			this.ClientSize = new System.Drawing.Size(830, 515);
			this.Controls.Add(this.panelTransactions);
			this.Controls.Add(this.dockPanelCashIn);
			this.Controls.Add(this.dockPanelAccounts);
			this.Controls.Add(this.dockPanelCustomer);
			this.EditRecordTitle = "Multi Transactions";
			this.Name = "frmTlQuickTranCode";
			this.NewRecordTitle = "Multi Transactions";
			this.Text = "Multi Transactions";
			((System.ComponentModel.ISupportInitialize)(this.dockMgr)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
			this.dockPanelCustomer.ResumeLayout(false);
			this.dockPanel3_Container.ResumeLayout(false);
			this.dockPanelCashIn.ResumeLayout(false);
			this.controlContainer1.ResumeLayout(false);
			this.dockPanelAccounts.ResumeLayout(false);
			this.dockPanel2_Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.panelTransactions)).EndInit();
			this.panelTransactions.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.documentMgr)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.noDocumentsView1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private PDxDockPanelUnderline dockPanelAccounts;
        private PDxControlContainer dockPanel2_Container;
        private PDxDockPanelUnderline dockPanelCustomer;
        private PDxControlContainer dockPanel3_Container;
        //private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private PDxDockManager dockMgr;
        private ucCustomer ucCustomer1;
        private ucAccounts ucAccounts1;
        private PDxDockPanelUnderline dockPanelCashIn;
        private PDxControlContainer controlContainer1;
        private ucCashIn ucCashIn1;
        //private DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup documentGroup1;
        //private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document documentTransaction;
        private PDxGroupControlUnderline panelTransactions;
        private PDxDocumentManager documentMgr;
        //private DevExpress.XtraBars.Docking2010.Views.NoDocuments.NoDocumentsView noDocumentsView1;
        //private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView2;
        private ucTransactions ucTransactions1;
        private DevExpress.XtraBars.Docking2010.Views.NoDocuments.NoDocumentsView noDocumentsView1;
		private PDxBarAndDockingController barAndDockingController1;
	}
}