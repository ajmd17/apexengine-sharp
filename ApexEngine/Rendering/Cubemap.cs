using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace ApexEngine.Rendering
{
    public class Cubemap : Texture
    {
        private static readonly TextureTarget[] cubemap_names = 
        {
            TextureTarget.TextureCubeMapPositiveX,
            TextureTarget.TextureCubeMapNegativeX,
            TextureTarget.TextureCubeMapPositiveY,
            TextureTarget.TextureCubeMapNegativeY,
            TextureTarget.TextureCubeMapPositiveZ,
            TextureTarget.TextureCubeMapNegativeZ,
        };

        public static Cubemap LoadCubemap(string[] filepaths)
        {
            if (filepaths.Length != 6)
                throw new Exception("A cubemap is made up of exactly six textures. " + filepaths.Length + " textures were supplied.");
            int id = Texture.GenTextureID();
            Cubemap res = new Cubemap(id);
            res.Use();
            for (int i = 0; i < filepaths.Length; i++)
            {
                Bitmap bmp = new Bitmap(filepaths[i]);
                BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                RenderManager.Renderer.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba8, bmp_data.Width, bmp_data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

                bmp.UnlockBits(bmp_data);
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            res.GenerateMipmap();
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);
            return res;
        }

        public Cubemap(int id) : base(id)
        {
        }

        public override void Use()
        {
            GL.BindTexture(TextureTarget.TextureCubeMap, id);
        }

        public override void SetFilter(int min, int mag)
        {
            throw new NotImplementedException();
        }

        public override void GenerateMipmap()
        {
            GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
        }
    }
}
