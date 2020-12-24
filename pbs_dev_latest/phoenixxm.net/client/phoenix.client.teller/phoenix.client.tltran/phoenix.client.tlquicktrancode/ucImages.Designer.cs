namespace Phoenix.Client.Teller
{
    partial class ucImages
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucImages));
            this.sharedImageCollection1 = new DevExpress.Utils.SharedImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sharedImageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sharedImageCollection1.ImageSource)).BeginInit();
            this.SuspendLayout();
            // 
            // sharedImageCollection1
            // 
            // 
            // 
            // 
            this.sharedImageCollection1.ImageSource.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("sharedImageCollection1.ImageSource.ImageStream")));
            this.sharedImageCollection1.ImageSource.Images.SetKeyName(0, "employee_16x16.png");
            this.sharedImageCollection1.ImageSource.Images.SetKeyName(1, "ide_16x16.png");
            this.sharedImageCollection1.ImageSource.Images.SetKeyName(2, "bodetails_16x16.png");
            this.sharedImageCollection1.ImageSource.Images.SetKeyName(3, "findcustomers_16x16.png");
            this.sharedImageCollection1.ImageSource.Images.SetKeyName(4, "deletelist_16x16.png");
            this.sharedImageCollection1.ImageSource.Images.SetKeyName(5, "insert_16x16.png");
            this.sharedImageCollection1.ImageSource.Images.SetKeyName(6, "warning_16x16.png");
            this.sharedImageCollection1.ImageSource.Images.SetKeyName(7, "error_16x16.png");
            this.sharedImageCollection1.ParentControl = this;
            // 
            // ucImages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucImages";
            this.Size = new System.Drawing.Size(112, 122);
            ((System.ComponentModel.ISupportInitialize)(this.sharedImageCollection1.ImageSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sharedImageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.SharedImageCollection sharedImageCollection1;
    }
}
