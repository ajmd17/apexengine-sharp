using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Input
{
    public class KeyboardEvent
    {
        public Action evt;
        public OpenTK.Input.Key key;
        public KeyboardEvent(OpenTK.Input.Key key, Action action)
        {
            this.evt = action;
            this.key = key;
        }

    }
}
