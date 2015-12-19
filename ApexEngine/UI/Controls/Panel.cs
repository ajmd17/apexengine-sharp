using ApexEngine.Math;
using System;

namespace ApexEngine.UI.Controls
{
    public class Panel : AbstractControl
    {
        private static Color4f grey = new Color4f(0.65f, 0.65f, 0.65f, 1.0f);

        public Panel(String name, int x, int y, int width, int height, Color4f color) : base(name, x, y)
        {
            this.width = width;
            this.height = height;
            this.Color = color;
        }

        public Panel(String name, int x, int y, int width, int height) : this(name, x, y, width, height, grey)
        {
        }

        public override void Render()
        {
            SpriteRenderer.Render(null, GetWorldTranslation().x, GetWorldTranslation().y, width, height,
                   this.Color);
        }

        public override void UpdateControl()
        {
        }
    }
}