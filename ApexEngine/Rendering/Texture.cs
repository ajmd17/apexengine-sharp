using System;
using System.Drawing;
using System.Drawing.Imaging;
#if OPENGL
using OpenTK.Graphics.OpenGL;
#else
using OpenTK.Graphics.ES20;
#endif
namespace ApexEngine.Rendering
{
    public abstract class Texture
    {
        protected int id;

        public static Texture LoadTexture(string path)
        {
            Bitmap bmp = null;
            if (path.EndsWith(".tga"))
            {
                bmp = ApexEngine.Assets.Util.TargaImage.LoadTargaImage(path);
            }
            else
                bmp = new Bitmap(path);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            Texture2D tex = new Texture2D(GenTextureID());
            tex.TexturePath = path;
            tex.Use();
            tex.SetWrap(Convert.ToInt32(TextureWrapMode.Repeat), Convert.ToInt32(TextureWrapMode.Repeat));
            tex.SetFilter((int)TextureMinFilter.LinearMipmapLinear, (int)TextureMagFilter.Linear);
            RenderManager.Renderer.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
            tex.GenerateMipmap();
            tex.Width = bmp.Width;
            tex.Height = bmp.Height;

            bmp.UnlockBits(bmp_data);
            Texture2D.Clear();
            return tex;
        }

        public static int GenTextureID()
        {
            int texID = GL.GenTexture();
            return texID;
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

        public static void ActiveTextureSlot(int slot)
        {
            RenderManager.Renderer.ActiveTexture(TextureUnit.Texture0 + slot);
        }

        public abstract void Use();

        public abstract void SetFilter(int min, int mag);

        public abstract void GenerateMipmap();
    }
}