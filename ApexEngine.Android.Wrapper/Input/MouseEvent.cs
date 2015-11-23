using System;

namespace ApexEngine.Input
{
    public class MouseEvent
    {
        public Action evt;
        public OpenTK.Input.MouseButton btn;
        public bool mouseUpEvt = false;

        public MouseEvent(OpenTK.Input.MouseButton btn, bool isMouseUpEvt, Action evt)
        {
            this.btn = btn;
            this.evt = evt;
            this.mouseUpEvt = isMouseUpEvt;
        }
    }
}