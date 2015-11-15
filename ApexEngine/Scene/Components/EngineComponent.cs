using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Scene.Components
{
    public interface EngineComponent
    {
        void Update();
        void Init();
    }
}
