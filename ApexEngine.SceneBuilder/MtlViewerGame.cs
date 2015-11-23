using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine;
using ApexEngine.Math;
namespace ApexEditor
{
    public class MtlViewerGame : Game
    {
        bool rotate = true;
        Quaternion rot = new Quaternion();
        float rotTime = 0f;
        
        public bool Rotate
        {
            get { return rotate; }
            set { rotate = value; }
        }

        public override void Init()
        {
            this.Environment.DirectionalLight.Direction.Set(-1, 1, -1);
        }

        public override void Render()
        {
        }

        public override void Update()
        {
            if (rotate)
            {
                rotTime += 1f;
                rot.SetFromAxis(Vector3f.UNIT_Y, rotTime);
                rootNode.GetChild(0).SetLocalRotation(rot);
            }
        }
    }
}
