using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittlePet
{
    /// <summary>
    /// Contains static methods for common drawing utilities.
    /// </summary>
    public static class GraphicsHelper
    {
        /// <summary>
        /// Measures the size of a string in the provided font.
        /// </summary>
        /// <param name="s">The string to measure.</param>
        /// <param name="font">The font to render with.</param>
        /// <returns>The size of the string.</returns>
        public static SizeF MeasureString(string s, Font font)
        {
            SizeF result;
            using (var image = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(image))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                result = g.MeasureString(s, font);
            }

            return result;
        }
    }
}
