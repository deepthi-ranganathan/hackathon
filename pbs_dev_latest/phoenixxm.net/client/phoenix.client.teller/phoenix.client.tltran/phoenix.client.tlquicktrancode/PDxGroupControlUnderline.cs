using Phoenix.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phoenix.Client.Teller
{
    class PDxGroupControlUnderline : PDxGroupControl
    {
        public bool DrawUnderline { get; set; }
        public Point UnderlineLocation { get; set; }
        public int UnderlineWidth { get; set; }
        public Color UnderlineColor { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);
                var color = System.Drawing.Color.Black;
                if (DrawUnderline && UnderlineColor != null && UnderlineLocation != null && UnderlineWidth > 0)
                {
                    e.Graphics.DrawLine(new Pen(UnderlineColor), new Point(UnderlineLocation.X, UnderlineLocation.Y),
                                        new System.Drawing.Point(UnderlineLocation.X + UnderlineWidth, UnderlineLocation.Y));
                }
            }
            //Begin Bug #93253
            catch (Exception ex)
            {
                
            }
            //End Bug #93253
        }
    }
}
