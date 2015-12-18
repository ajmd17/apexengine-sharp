using ApexEngine.Math;
using ApexEngine.Rendering;

namespace ApexEngine.Scene
{
    public class Sprite : Geometry
    {
        private Texture2D texture;
        private static Color4f white = new Color4f(1, 1, 1, 1);

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
        }

        public override void Render(Rendering.Environment environment, Camera cam)
        {
            SpriteRenderer spriteRenderer = renderManager.SpriteRenderer;
            spriteRenderer.Render(texture, WorldTransform, WorldTransform.GetTranslation().x,
                   WorldTransform.GetTranslation().y,
                    texture.Width * WorldTransform.GetScale().x, texture.Height * WorldTransform.GetScale().y, white);
        }
    }
}