using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phoenix.Client.Teller
{
    public partial class ucImages : UserControl
    {

        private static ucImages _instance = new ucImages();
        public ucImages()
        {
            InitializeComponent();

            
        }


        //public static DevExpress.Utils.SharedImageCollection Images
        //{
        //    get{
        //        return _instance.sharedImageCollection1;
        //    }

        //    }

        public static DevExpress.Utils.Images Images
        {
            get
            {
                return _instance.sharedImageCollection1.ImageSource.Images;
            }

        }


        public class ImageNames
        {
            public const string Employee16 = "employee_16x16.png";
            public const string Ide16 = "ide_16x16.png";
            public const string BoDetails16 = "bodetails_16x16.png";
            public const string FindCustomer16 = "findcustomers_16x16.png";
            public const string DeleteList16 = "deletelist_16x16.png";
            public const string InsertList16 = "insert_16x16.png";
            public const string Error16 = "error_16x16.png";
            public const string Warning16 = "error_16x16.png";
        }
    }
}
