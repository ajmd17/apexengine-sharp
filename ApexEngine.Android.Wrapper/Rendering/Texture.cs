using System;
using System.Drawing;
#if OPENGL
using OpenTK.Graphics.OpenGL;
#else
using OpenTK.Graphics.ES20;
#endif
namespace ApexEngine.Rendering
{
    public class Texture
    {
        protected int id;
        private string texturePath = "";
        private int width, height;

        public static Texture LoadTexture(string path)
        {
            
            int texID = GL.GenTexture();
            Texture tex = new Texture(texID);
            tex.TexturePath = path;

            return tex;
        }

        public Texture(int id)
        {
            this.id = id;
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public string TexturePath
        {
            get { return texturePath; }
            set { texturePath = value; }
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
            RenderManager.renderer.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Use()
        {
            RenderManager.renderer.BindTexture(TextureTarget.Texture2D, id);
        }

        public static void ActiveTextureSlot(int slot)
        {
            RenderManager.renderer.ActiveTexture(TextureUnit.Texture0 + slot);
        }

        public void SetWrap(int s, int t)
        {
            Use();
            RenderManager.renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, Convert.ToInt32(TextureWrapMode.Repeat));
            RenderManager.renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, Convert.ToInt32(TextureWrapMode.Repeat));
        }

        public void SetFilter(int min, int mag)
        {
            Use();
            RenderManager.renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, min);
            RenderManager.renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, mag);
        }

        public void GenerateMipmap()
        {
            Use();
            #if OPENGL
            RenderManager.renderer.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            #endif
        }

        public override string ToString()
        {
            return texturePath;
        }
    }
}