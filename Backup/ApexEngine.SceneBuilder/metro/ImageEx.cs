using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace ModernUISample.metro
{
    /// <summary>
    /// Extension methods for the Image class
    /// </summary>
    public static class ImageEx
    {
        /// <summary>
        /// Adjusts the red, green and blue parts of the image
        /// 
        /// Examples:
        /// Original image should be dark/black (main color RGB(51,51,51) and less)
        ///
        ///    blue (metro): 0,1,3,0.55
        ///    light grey: 1,1,1,0.35
        ///    white: 1,1,1,0.01
        ///    green: 0,2,0,0.75
        ///    red: 2,0,0,0.75
        /// </summary>
        /// <param name="img">Image</param>
        /// <param name="red">red part adjustment (>= 0.00, 1.0 = no changes)</param>
        /// <param name="green">red part adjustment (>= 0.00, 1.0 = no changes)</param>
        /// <param name="blue">blue part adjustment (>= 0.00, 1.0 = no changes)</param>
        /// <param name="gamma">Gamma adjustment (> 0.00, 1.0 = no changes)</param>
        /// <returns>the modified Image</returns>
        public static Image AdjustRGBGamma(this Image img, float red, float green, float blue, float gamma)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            ImageAttributes imgAttributes = new ImageAttributes();

            ColorMatrix matrix = new ColorMatrix(
                new float[][] {
                new float[] { red, 0, 0, 0, 0 },  
                new float[] { 0, green, 0, 0, 0 }, 
                new float[] { 0, 0, blue, 0, 0 }, 
                new float[] { 0, 0, 0, 1.0f, 0 }, 
                new float[] { 0, 0, 0, 0, 1 }, 
                });

            imgAttributes.ClearColorMatrix();
            imgAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imgAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height),
                    0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttributes);
            }

            return (Image)bmp;
        }
    }
}
