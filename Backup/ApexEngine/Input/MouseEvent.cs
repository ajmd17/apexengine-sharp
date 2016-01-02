using System;

namespace ApexEngine.Input
{
    public class MouseEvent
    {
        public Action evt;
        public InputManager.MouseButton btn;
        public bool mouseUpEvt = false;

        public MouseEvent(InputManager.MouseButton btn, bool isMouseUpEvt, Action evt)
        {
            this.btn = btn;
            this.evt = evt;
            this.mouseUpEvt = isMouseUpEvt;
        }
    }
}