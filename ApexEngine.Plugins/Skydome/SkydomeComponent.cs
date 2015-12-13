using ApexEngine.Assets;
using ApexEngine.Rendering;
using ApexEngine.Scene;
using ApexEngine.Scene.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Plugins.Skydome
{
    public class SkydomeComponent : GameComponent
    {
        Geometry model;
        public override void Init()
        {
            model = ((Node)AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\dome.obj")).GetChildGeom(0);
            model.Material.SetValue(Material.MATERIAL_FACETOCULL, 1);
            model.Material.SetValue(Material.MATERIAL_DEPTHMASK, false);
            model.Material.SetValue(Material.MATERIAL_DEPTHTEST, false);
            model.Material.Bucket = RenderManager.Bucket.Sky;
            model.SetShader(typeof(SkyShader));
            model.SetLocalScale(new Math.Vector3f(100f));
            rootNode.AddChild(model);
        }

        public override void Update()
        {
        }
    }
}
