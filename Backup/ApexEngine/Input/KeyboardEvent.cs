using System;

namespace ApexEngine.Input
{
    public class KeyboardEvent
    {
        public Action evt;
        public InputManager.KeyboardKey key;

        public KeyboardEvent(InputManager.KeyboardKey key, Action action)
        {
            this.evt = action;
            this.key = key;
        }
    }
}