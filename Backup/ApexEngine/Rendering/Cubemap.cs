using System;

namespace ApexEngine.Rendering
{
    public class Cubemap : Texture
    {
        public static Cubemap LoadCubemap(string[] filepaths)
        {
            return RenderManager.Renderer.LoadCubemap(filepaths);
        }

        public Cubemap(int id) : base(id)
        {
        }

        public override void Use()
        {
            RenderManager.Renderer.BindTextureCubemap(id);
        }

        public override void SetFilter(int min, int mag)
        {
            throw new NotImplementedException();
        }

        public override void GenerateMipmap()
        {
            RenderManager.Renderer.GenerateMipmapCubemap();
        }
    }
}