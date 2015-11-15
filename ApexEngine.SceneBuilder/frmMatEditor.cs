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
using ApexEngine.Assets;
using ApexEngine.Rendering;
using ApexEngine.Math;
namespace ApexEditor
{
    public partial class frmMatEditor : Form
    {
        MtlViewerGame mtlPreview;
        public Material Material
        {
            get { return ((Geometry)mtlPreview.RootNode.GetChild(0)).Material; }
            set { ((Geometry)mtlPreview.RootNode.GetChild(0)).Material = value; UpdateMaterial();  }
        }
        public frmMatEditor()
        {
            InitializeComponent();
        }
        private void UpdateMaterial()
        {
            listBox1.Items.Clear();
            for (int i = 0; i < Material.Values.Count; i++)
            {
                listBox1.Items.Add(Material.Values.ElementAt(i).Key);
            }
        }
        public void Init()
        {

            Node monkey = (Node)(AssetManager.Load("C:\\Users\\User\\Desktop\\monkey.obj"));
            monkey.GetChild(0).SetLocalRotation(new ApexEngine.Math.Quaternion().SetFromAxis(ApexEngine.Math.Vector3f.UNIT_Y, 180));
            mtlPreview = new MtlViewerGame();
            mtlPreview.Camera.Enabled = false;
            mtlPreview.Camera.Width = 256;
            mtlPreview.Camera.Height = 256;

            // mtlPreview.Camera = new ApexEngine.Rendering.Cameras.OrthoCamera(-2, 2, -2, 2, -2, 2);
            mtlPreview.Camera.Translation = new ApexEngine.Math.Vector3f(0, 0, -3);
            mtlPreview.RootNode.AddChild(monkey.GetChild(0));
            ApexEngineControl mtlViewer = new ApexEngineControl(mtlPreview);
            mtlViewer.Framerate = 50;
            mtlViewer.Dock = DockStyle.Fill;
            pnlMtl.Controls.Add(mtlViewer);
            UpdateMaterial();

        }

        private void frmMatEditor_Load(object sender, EventArgs e)
        {
            cbPPL.Checked = (Material.GetInt(Material.TECHNIQUE_PER_PIXEL_LIGHTING) == 1);
            castsShadows.Checked = (Material.GetInt(Material.MATERIAL_CASTSHADOWS) == 1);
            this.shininessLevel.Value = (int)Material.GetFloat(Material.SHININESS);
            if (Material.GetInt(Material.TECHNIQUE_SPECULAR) == 0)
                specTechnique.SelectedIndex = 1;
            else if (Material.GetInt(Material.TECHNIQUE_SPECULAR) == 1)
                specTechnique.SelectedIndex = 0;
            if (Material.GetInt(Material.MATERIAL_BLENDMODE) == 0)
                blendModeBox.SelectedIndex = 0;
            else if (Material.GetInt(Material.MATERIAL_BLENDMODE) == 1)
                blendModeBox.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void shininessLevel_ValueChanged(object sender, EventArgs e)
        {
        }

        private void shininessLevel_Scroll(object sender, EventArgs e)
        {
            Material.SetValue(Material.SHININESS, shininessLevel.Value * 1.0f);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vector4f vecColor = Material.GetVector4f(Material.COLOR_DIFFUSE);
            colorDialog1.Color = Color.FromArgb((int)(vecColor.x * 255), (int)(vecColor.y * 255), (int)(vecColor.z * 255), (int)(vecColor.w * 255));
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Material.SetValue(Material.COLOR_DIFFUSE, new Vector4f(colorDialog1.Color.R/255f, colorDialog1.Color.G / 255f, colorDialog1.Color.B / 255f, colorDialog1.Color.A / 255f));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Vector4f vecColor = Material.GetVector4f(Material.COLOR_SPECULAR);
            colorDialog2.Color = Color.FromArgb((int)(vecColor.x * 255), (int)(vecColor.y * 255), (int)(vecColor.z * 255), (int)(vecColor.w * 255));
            if (colorDialog2.ShowDialog() == DialogResult.OK)
            {
                Material.SetValue(Material.COLOR_SPECULAR, new Vector4f(colorDialog2.Color.R / 255f, colorDialog2.Color.G / 255f, colorDialog2.Color.B / 255f, colorDialog2.Color.A / 255f));
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Vector4f vecColor = Material.GetVector4f(Material.COLOR_AMBIENT);
            colorDialog3.Color = Color.FromArgb((int)(vecColor.x * 255), (int)(vecColor.y * 255), (int)(vecColor.z * 255), (int)(vecColor.w * 255));
            if (colorDialog3.ShowDialog() == DialogResult.OK)
            {
                Material.SetValue(Material.COLOR_AMBIENT, new Vector4f(colorDialog3.Color.R / 255f, colorDialog3.Color.G / 255f, colorDialog3.Color.B / 255f, colorDialog3.Color.A / 255f));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (specTechnique.Text == "Simple")
            {
                Material.SetValue(Material.TECHNIQUE_SPECULAR, 0);
            }
            else if(specTechnique.Text == "Default")
            {
                Material.SetValue(Material.TECHNIQUE_SPECULAR, 1);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Material.SetValue(Material.TECHNIQUE_PER_PIXEL_LIGHTING, (cbPPL.Checked ? 1 : 0));
        }

        private void blendModeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Material.SetValue(Material.MATERIAL_BLENDMODE, blendModeBox.SelectedIndex);
        }

        private void castsShadows_CheckedChanged(object sender, EventArgs e)
        {
            Material.SetValue(Material.MATERIAL_CASTSHADOWS, (castsShadows.Checked ? 1 : 0));
        }
    }
}
