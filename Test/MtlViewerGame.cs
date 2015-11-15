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
        Quaternion rot = new Quaternion();
        float rotTime = 0f;
        public override void Init()
        {
        }

        public override void Render()
        {
        }

        public override void Update()
        {
            rotTime += 1f;
            rot.SetFromAxis(Vector3f.UNIT_Y, rotTime);
            rootNode.GetChild(0).SetLocalRotation(rot);
        }
    }
}
