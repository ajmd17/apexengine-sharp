using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModernUISample.metro;
using ApexEngine.Scene;
using ApexEngine.Assets;
using ApexEngine.Rendering;

namespace ApexEditor
{
    public partial class Form1 : Form
    {
        ApexEngineControl apxCtrl;
        private int activeNodeID;
        frmMatEditor matEditor;
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Apex Edtior Started.");
            matEditor = new frmMatEditor();
        }
        void Style_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DarkStyle")
            {
                BackColor = MetroUI.Style.BackColor;
                Refresh();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (MetroUI.DesignMode == false)
            {
                MetroUI.Style.PropertyChanged += Style_PropertyChanged;
                MetroUI.Style.DarkStyle = true;
            }
            SceneEditorGame game = new SceneEditorGame();
            game.Camera = new ApexEngine.Rendering.Cameras.DefaultCamera(game.InputManager, 75);
            game.Camera.Translation = new ApexEngine.Math.Vector3f(0, 0, -5);
            apxCtrl = new ApexEngineControl(game);
            apxCtrl.Dock = DockStyle.Fill;
            pnlGameView.Controls.Add(apxCtrl);


            contextMenuStrip1.Renderer = new metroToolStripRenderer();

            

            PopulateTreeView(game.RootNode);
            /*

            SceneEditorGame orthoTop = new SceneEditorGame();
          //  orthoTop.Camera = new ApexEngine.Rendering.Cameras.OrthoCamera(-5, 5, -5, 5, -5, 5);
            orthoTop.Camera.Translation = new ApexEngine.Math.Vector3f(0, 0, -5);
           // orthoTop.RenderManager.GeometryList = game.RenderManager.GeometryList;
            ApexEngineControl orthoTopCtrl = new ApexEngineControl(orthoTop);
            orthoTopCtrl.Dock = DockStyle.Fill;
            pnlOrthoTop.Controls.Add(orthoTopCtrl);*/

        }
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
        private void AddTreeViewItem(TreeNode parent, GameObject obj)
        {
            if (obj is Geometry)
            {
                TreeNode newNode = new TreeNode(obj.Name + " (Geometry)");
                Geometry geom = (Geometry)obj;
                newNode.Tag = geom;
                TreeNode matNode = new TreeNode("Material");
                matNode.Tag = geom.Material;
                newNode.Nodes.Add(matNode);
                if (parent == null)
                    treeView1.Nodes.Add(newNode);
                else
                    parent.Nodes.Add(newNode);
            }
            else if (obj is Node)
            {
                TreeNode newNode = new TreeNode(obj.Name + " (Node)");
                Node n = (Node)obj;
                newNode.Tag = n;
                if (parent == null)
                    treeView1.Nodes.Add(newNode);
                else
                    parent.Nodes.Add(newNode);
                foreach (GameObject g in n.Children)
                {
                    AddTreeViewItem(newNode, g);
                }
            }

        }
        private void PopulateTreeView(GameObject rootObject)
        {
            AddTreeViewItem(null, rootObject);
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        private void addToSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                ApexEngine.Scene.GameObject loadedModel = ApexEngine.Assets.AssetManager.LoadModel(openFileDialog1.FileName);
                apxCtrl.Game.RootNode.AddChild(loadedModel);
                activeNodeID = apxCtrl.Game.RootNode.Children.Count - 1;
                AddTreeViewItem(treeView1.Nodes[0], loadedModel);
            }
        }

        private void openAsSeperateSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                for (int i = apxCtrl.Game.RootNode.Children.Count - 1; i > -1; i--)
                {
                    apxCtrl.Game.RootNode.RemoveChild(apxCtrl.Game.RootNode.GetChild(i));
                }
                treeView1.Nodes.Clear();
                ApexEngine.Scene.GameObject loadedModel = ApexEngine.Assets.AssetManager.LoadModel(openFileDialog1.FileName);
                apxCtrl.Game.RootNode.AddChild(loadedModel);
                activeNodeID = apxCtrl.Game.RootNode.Children.Count - 1;
                PopulateTreeView(apxCtrl.Game.RootNode);
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // treeView1.Nodes.Clear();
           //  PopulateTreeView(apxCtrl.Game.RootNode);
           
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                GameObject[] objs = new GameObject[apxCtrl.Game.RootNode.Children.Count];
                for (int i = 0; i < objs.Length; i++)
                {
                    objs[i] = apxCtrl.Game.RootNode.GetChild(i);
                }
                Console.WriteLine(objs.Length);
                ApexEngine.Assets.Apx.ApxExporter.ExportModel(saveFileDialog1.FileName, objs);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                if (e.Node.Tag is GameObject)
                {
                    propertyGrid1.SelectedObject = e.Node.Tag;
                }
            }
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
           
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is GameObject)
                {
                    if (treeView1.SelectedNode.Tag != apxCtrl.Game.RootNode)
                    {
                        GameObject selectedObj = (GameObject)treeView1.SelectedNode.Tag;
                        selectedObj.GetParent().RemoveChild(selectedObj);
                        treeView1.Nodes.Remove(treeView1.SelectedNode);
                    }
                }
                else if (treeView1.SelectedNode.Tag is Material)
                {
                    Geometry selectedObj = (Geometry)(treeView1.SelectedNode.Parent.Tag);
                    selectedObj.Material = new Material();
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag == apxCtrl.Game.RootNode)
                {
                    contextMenuStrip1.Items[0].Enabled = false;
                }
                else
                {
                    contextMenuStrip1.Items[0].Enabled = true;

                }
            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag != apxCtrl.Game.RootNode)
                {
                    GameObject selectedObj = (GameObject)treeView1.SelectedNode.Tag;
                    saveFileDialog1.FileName = selectedObj.Name;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        ApexEngine.Assets.Apx.ApxExporter.ExportModel(saveFileDialog1.FileName, selectedObj);
                    }
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                GameObject[] objs = new GameObject[apxCtrl.Game.RootNode.Children.Count];
                for (int i = 0; i < objs.Length; i++)
                {
                    objs[i] = apxCtrl.Game.RootNode.GetChild(i);
                }
                Console.WriteLine(objs.Length);
                ApexEngine.Assets.Apx.ApxExporter.ExportModel(saveFileDialog1.FileName, objs);
            }
        }

        private void pnlMtl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlMtl_DoubleClick(object sender, EventArgs e)
        {
            matEditor.ShowDialog();
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Material)
            {
                frmMatEditor matEdit = new frmMatEditor();
                matEdit.Init();
                matEdit.Material = (Material)treeView1.SelectedNode.Tag;
                matEdit.ShowDialog();
                if (matEdit.DialogResult == DialogResult.OK)
                {
                    treeView1.SelectedNode.Tag = matEdit.Material;
                }
            }
        }
    }
}
