using ApexEngine.Input;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.UI
{
    public abstract class AbstractControl : Node
    {
        public enum Layout
        {
            Fixed,
            Centered
        }

        public bool clicked = false;
        protected Layout layout = Layout.Fixed;
        protected int width, height;
        public bool isMouseOver = false;
        public bool hasFocus = false;
        protected string name = "control";
        protected Color4f color = new Color4f(1, 1, 1, 1);
        private bool isVibrating = false;
        private float vibrateTick = 0f, maxVibrateTick = 1f;
        private float oldX;

        public abstract void Render();

        public abstract void UpdateControl();

        public SpriteRenderer SpriteRenderer
        {
            get; set;
        }

        public InputManager InputManager
        {
            get; set;
        }

        public Layout ControlLayout
        {
            get; set;
        }


        public override void Update(RenderManager renderManager)
        {
            base.Update(renderManager);
            if (isVibrating)
            {
                if (vibrateTick < maxVibrateTick)
                {
                    vibrateTick += 0.1f;// GameTime.getDeltaTime()*2f;
                    SetLocalTranslation(
                            new Vector3f(oldX + (float)System.Math.Sin(vibrateTick * 15f) * 5f, GetLocalTranslation().y, 0));
                }
                else
                {
                    SetLocalTranslation(new Vector3f(oldX, GetLocalTranslation().y, 0));
                    isVibrating = false;
                    OnDoneVibrating();
                }
            }
            else if (!isVibrating)
            {

                if (this.layout == Layout.Centered)
                {
                    if (this.GetParent() != null && (GetParent() is AbstractControl)) {
                        float parentCenterX = ((AbstractControl)GetParent()).width / 2f;
                        float parentCenterY = ((AbstractControl)GetParent()).height / 2f;
                        float halfWidth = this.width / 2f;
                        float halfHeight = this.height / 2f;
                        if (GetLocalTranslation().x != (parentCenterX - halfWidth)
                                || GetLocalTranslation().y != (parentCenterY - halfHeight))
                        {
                            this.SetLocalTranslation(
                                    new Vector3f(parentCenterX - halfWidth, parentCenterY - halfHeight, 0));
                        }
                    } else if (this.GetParent() == null || !(GetParent() is AbstractControl)) {
                        float halfWidth = this.width / 2f;
                        float halfHeight = this.height / 2f;
                        float halfScreenWidth = InputManager.SCREEN_WIDTH / 2f;
                        float halfScreenHeight = InputManager.SCREEN_HEIGHT / 2f;
                        if (GetLocalTranslation().x != (halfScreenWidth - halfWidth)
                                || GetLocalTranslation().y != (halfScreenHeight - halfHeight))
                        {
                            this.SetLocalTranslation(
                                    new Vector3f(halfScreenWidth - halfWidth, halfScreenHeight - halfHeight, 0));
                        }
                    }

                }
            }
            UpdateControl();
        }


        public void Vibrate()
        {
            OnStartVibrating();
            vibrateTick = 0f;
            isVibrating = true;
            oldX = GetLocalTranslation().x;
        }

        public void Vibrate(float time)
        {
            maxVibrateTick = time;
            Vibrate();
        }

        public void OnStartVibrating()
        {

        }

        public void OnDoneVibrating()
        {

        }

        public Color4f Color
        {
            get { return color; }
            set { color.Set(value); }
        }

        public void MouseOver()
        {
            isMouseOver = true;
        }

        public void MouseLeave()
        {
            isMouseOver = false;
        }

        public bool IsMouseOver()
        {
            return isMouseOver;
        }

        public AbstractControl(string name)
        {
            Name = name;
        }

        public AbstractControl(string name, int x, int y) : base(name)
        {
            this.SetLocalTranslation(new Vector3f(x, y, 0));
        }

        public int Width
        {
            get { return width; } set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
    }
}
