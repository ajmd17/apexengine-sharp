using System;

namespace ApexEngine.UI.Controls
{
    public class Stage : Panel
    {
        public Stage(String name) : base(name, 0, 0, 100, 100, new Math.Color4f(1, 1, 1, 1))
        {
        }

        public override void UpdateControl()
        {
            this.Width = InputManager.SCREEN_WIDTH;
            this.Height = InputManager.SCREEN_HEIGHT;
        }
    }
}