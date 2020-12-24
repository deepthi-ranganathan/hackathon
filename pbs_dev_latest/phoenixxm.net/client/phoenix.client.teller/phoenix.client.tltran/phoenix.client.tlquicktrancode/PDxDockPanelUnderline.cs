using Phoenix.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Phoenix.Client.Teller
{
    class PDxDockPanelUnderline: PDxDockPanel
    {
        public bool DrawUnderline { get; set; }
        public Point UnderlineLocation { get; set; }
        public int UnderlineWidth { get; set; }
        public Color UnderlineColor { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var color = System.Drawing.Color.Black;
            if (DrawUnderline && UnderlineColor != null && UnderlineLocation != null && UnderlineWidth > 0)
            {
                e.Graphics.DrawLine(new Pen(UnderlineColor), new Point(UnderlineLocation.X, UnderlineLocation.Y),
                                    new System.Drawing.Point(UnderlineLocation.X+UnderlineWidth, UnderlineLocation.Y));
            }
        }
    }
}
