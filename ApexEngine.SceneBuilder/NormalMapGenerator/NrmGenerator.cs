using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApexEngine.Scene;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Util;


namespace ApexEditor.NormalMapGenerator
{
    public partial class NrmGenerator : Form
    {
        Geometry quadGeom;

        public NrmGenerator()
        {
            InitializeComponent();
        }

        private void NrmGenerator_Load(object sender, EventArgs e)
        {
            Mesh quadMesh = MeshFactory.CreateQuad();
            quadGeom = new Geometry(quadMesh);
            quadGeom.SetShader(ShaderManager.GetShader(typeof(NormalMapShader)));
            MtlViewerGame mtlPreview = new MtlViewerGame();
            mtlPreview.Rotate = false;
            mtlPreview.Camera.Enabled = false;
            mtlPreview.Camera.Width = 256;
            mtlPreview.Camera.Height = 256;

          //  mtlPreview.Camera = new ApexEngine.Rendering.Cameras.OrthoCamera(-2, 2, -2, 2, -2, 2);
            mtlPreview.Camera.Translation = new ApexEngine.Math.Vector3f(0, 0, -3);
            mtlPreview.RootNode.AddChild(quadGeom);
            ApexEngineControl mtlViewer = new ApexEngineControl(mtlPreview);
            mtlViewer.Framerate = 50;
            mtlViewer.Dock = DockStyle.Fill;
            pnlObj.Controls.Add(mtlViewer);

            quadGeom.Material.SetValue("delta_value", 5.0f);
            //mtlPreview.RenderManager.PostProcessor.PostFilters.Add(new BlurPostFilter(true));
            //mtlPreview.RenderManager.PostProcessor.PostFilters.Add(new BlurPostFilter(false));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Texture tex = Texture.LoadTexture(openFileDialog1.FileName);
                quadGeom.Material.SetValue(Material.TEXTURE_DIFFUSE, tex);
            }
        }

        private void deltaLevel_Scroll(object sender, EventArgs e)
        {
            quadGeom.Material.SetValue("delta_value", (float)deltaLevel.Value / 10.0f);
        }
    }
}
