using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittlePet
{
    /// <summary>
    /// A simple picture box implementation that lets you control the image
    /// interpolation. If we didn't do this and just used a regular picture
    /// box where we couldn't use NearestNeighbour interpolation, the pixels
    /// would be all blended together.
    /// 
    /// Yes, the default PictureBox class in windows forms is terrible for
    /// pixel art.
    /// </summary>
    public class InterpolationPictureBox : PictureBox
    {
        public InterpolationMode InterpolationMode { get; set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = InterpolationMode;
            base.OnPaint(pe);
        }
    }
}
