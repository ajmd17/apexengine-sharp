using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
namespace ApexEngine.Rendering
{
    public class Texture
    {
        protected int id;
        public static Texture LoadTexture(string path)
        {
            Bitmap bmp = new Bitmap(path);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            int texID = GL.GenTexture();
            Texture tex = new Texture(texID);
            tex.Use();
            tex.SetFilter((int)TextureMinFilter.LinearMipmapLinear, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, bmp_data.Scan0);
            tex.GenerateMipmap();

            bmp.UnlockBits(bmp_data);
            Texture.Clear();
            return tex;
        }
        public Texture(int id)
        {
            this.id = id;
        }
        public int GetID()
        {
            return this.id;
        }
        public void SetID(int id)
        {
            this.id = id;
        }
        public static void Clear()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, id);
        }
        public static void ActiveTextureSlot(int slot)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + slot);
        }
        public void SetWrap(int s, int t)
        {
            Use();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, Convert.ToInt32(TextureWrapMode.Repeat));
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, Convert.ToInt32(TextureWrapMode.Repeat));
        }
        public void SetFilter(int min, int mag)
        {
            Use();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, min);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, mag);
        }
        public void GenerateMipmap()
        {
            Use();
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
    }
}
