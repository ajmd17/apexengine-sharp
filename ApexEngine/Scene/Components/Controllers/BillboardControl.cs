using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Rendering;
using ApexEngine.Math;

namespace ApexEngine.Scene.Components.Controllers
{
    public class BillboardControl : Controller
    {
        private Camera cam;
        private Quaternion rotation = new Quaternion();
        private Vector3f camDir = new Vector3f();

        public BillboardControl(Camera cam)
        {
            this.cam = cam;
        }

        public override void Init()
        {
        }

        public override void Update()
        {
            camDir.Set(cam.Translation);
            camDir.SubtractStore(GameObject.GetWorldTranslation());
            camDir.Y = 0;
            camDir.NormalizeStore();

            rotation.SetToLookAt(camDir, Vector3f.UnitY);
            if (!GameObject.GetLocalRotation().Equals(rotation))
                GameObject.SetLocalRotation(rotation);
        }
    }
}
