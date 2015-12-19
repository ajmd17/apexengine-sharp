// Conversion output is limited to 2048 chars
// Share Varycode on Facebook and tweet on Twitter
// to double the limits.

using ApexEngine.Input;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using ApexEngine.UI.Controls;
using System.Collections.Generic;

namespace ApexEngine.UI
{
    public class GUIManager
    {
        private InputManager inputManager;
        private SpriteRenderer spriteRenderer;
        private RenderManager renderManager;
        private List<AbstractControl> controls = new List<AbstractControl>();
        private List<Stage> stages = new List<Stage>();
        private Stage currentStage = null;
        private Node guiNode = new Node("gui");

        public GUIManager(Game game)
        {
            this.inputManager = game.InputManager;
            this.renderManager = game.RenderManager;
            this.spriteRenderer = game.RenderManager.SpriteRenderer;
            game.RootNode.AddChild(guiNode);

            inputManager.AddMouseEvent(new MouseEvent(InputManager.MouseButton.Left, false, () =>
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i] is Clickable)
                    {
                        if (Collides(controls[i], inputManager.GetMouseX() + inputManager.SCREEN_WIDTH / 2, inputManager.GetMouseY() + inputManager.SCREEN_HEIGHT / 2))
                        {
                            controls[i].hasFocus = true;
                            ((Clickable)controls[i]).LeftClicked();
                            controls[i].clicked = true;
                        }
                        else
                        {
                            controls[i].hasFocus = false;
                        }
                    }
                }
            }));

            inputManager.AddMouseEvent(new MouseEvent(InputManager.MouseButton.Left, true, () =>
            {
                for (int i = controls.Count - 1; i > -1; i--)
                {
                    if (i < controls.Count)
                    {
                        if (controls[i].clicked == true)
                        {
                            ((Clickable)controls[i]).LetGo();
                            controls[i].clicked = false;
                        }
                    }
                }
            }));
        }

        public Node GUINode
        {
            get { return guiNode; }
        }

        public void AddStage(Stage stage)
        {
            stages.Add(stage);
        }

        public void RemoveStage(Stage stage)
        {
            stages.Remove(stage);
        }

        public Stage GetStage(string name)
        {
            foreach (Stage stg in stages)
            {
                if (stg.Name.Equals(name))
                    return stg;
            }
            return null;
        }

        public Stage GetStage(int index)
        {
            return stages[index];
        }

        public void ShowStage(Stage stage)
        {
            currentStage = stage;
        }

        public void ShowStage(int index)
        {
            ShowStage(GetStage(index));
        }

        public Stage CurrentStage
        {
            get { return currentStage; }
        }

        public void HideStage()
        {
            currentStage = null;
        }

        public static bool Collides(AbstractControl control, int x, int y)
        {
            if (x > control.GetWorldTranslation().x && x < control.GetWorldTranslation().x + control.Width)
            {
                if (y > control.GetWorldTranslation().y && y < control.GetWorldTranslation().y + control.Height)
                {
                    return true;
                }
            }
            return false;
        }

        public void Update()
        {
            guiNode.Update(renderManager);

            if (currentStage != null)
            {
                if (currentStage.Width != inputManager.SCREEN_WIDTH
                        || currentStage.Height != inputManager.SCREEN_HEIGHT)
                {
                    currentStage.Width = (inputManager.SCREEN_WIDTH);
                    currentStage.Height = (inputManager.SCREEN_HEIGHT);
                }
            }

            MouseOver();
        }

        public void Render()
        {
            if (currentStage != null)
            {
                currentStage.InputManager = inputManager;
                currentStage.SpriteRenderer = spriteRenderer;
                currentStage.Render();
            }

            for (int i = 0; i < guiNode.Children.Count; i++)
            {
                if (guiNode.GetChild(i) is AbstractControl)
                {
                    RenderControl((AbstractControl)guiNode.GetChild(i));
                }
            }
        }

        private void RenderControl(AbstractControl ctrl)
        {
            ctrl.Render();
            for (int i = 0; i < ctrl.Children.Count; i++)
            {
                if (ctrl.GetChild(i) is AbstractControl)
                    RenderControl((AbstractControl)ctrl.GetChild(i));
            }
        }

        private void MouseOver()
        {
            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i] is Clickable)
                {
                    if (Collides(controls[i], inputManager.GetMouseX() + inputManager.SCREEN_WIDTH / 2,
                            inputManager.GetMouseY() + inputManager.SCREEN_HEIGHT / 2))
                    {
                        if (!controls[i].IsMouseOver())
                        {
                            controls[i].MouseOver();
                            controls[i].isMouseOver = true;
                        }
                    }
                    else
                    {
                        if (controls[i].IsMouseOver())
                        {
                            controls[i].MouseLeave();
                            controls[i].isMouseOver = false;
                            if (controls[i].clicked == true)
                            {
                                controls[i].clicked = false;
                            }
                        }
                    }
                }
            }
        }

        public void AddControl(AbstractControl ctrl)
        {
            controls.Add(ctrl);
            guiNode.AddChild(ctrl);
            ctrl.InputManager = inputManager;
            ctrl.SpriteRenderer = spriteRenderer;
        }

        public void RemoveControl(AbstractControl ctrl)
        {
            controls.Remove(ctrl);
            guiNode.RemoveChild(ctrl);
        }
    }
}