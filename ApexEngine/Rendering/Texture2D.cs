using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexEngine.Rendering
{
    public class Texture2D : Texture
    {
        protected string texturePath = "";
        protected int width, height;
       
        public Texture2D(int id) : base(id)
        {
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

        public static void Clear()
        {
            RenderManager.Renderer.BindTexture(TextureTarget.Texture2D, 0);
        }

        public override void Use()
        {
            RenderManager.Renderer.BindTexture(TextureTarget.Texture2D, id);
        }

        public void SetWrap(int s, int t)
        {
            Use();
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, Convert.ToInt32(TextureWrapMode.Repeat));
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, Convert.ToInt32(TextureWrapMode.Repeat));
        }

        public override void SetFilter(int min, int mag)
        {
            Use();
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, min);
            RenderManager.Renderer.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, mag);
        }

        public override void GenerateMipmap()
        {
            Use();
            RenderManager.Renderer.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public override string ToString()
        {
            return texturePath;
        }
    }
}
