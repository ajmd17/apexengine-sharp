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

namespace ApexEditor
{
    public partial class Form1 : Form
    {
        ApexEngineControl apxCtrl;
        private int activeNodeID;
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Apex Edtior Started.");
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
            ApexEngine.TestGame game = new ApexEngine.TestGame();
            apxCtrl = new ApexEngineControl(game);
            apxCtrl.Dock = DockStyle.Fill;
            pnlGameView.Controls.Add(apxCtrl);
            PopulateTreeView(game.RootNode);
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
                newNode.Tag = (Geometry)obj;
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
            if (e.Node.Name == "root")
                propertyGrid1.Enabled = false;
            else
                propertyGrid1.Enabled = true;
            if (e.Node.Tag != null)
            {
                propertyGrid1.SelectedObject = e.Node.Tag;
            }
        }

        private void addToSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                ApexEngine.Scene.GameObject loadedModel = ApexEngine.Assets.AssetManager.LoadModel(openFileDialog1.FileName);
                loadedModel.SetLocalTranslation(new ApexEngine.Math.Vector3f(0, 0, 5));
                apxCtrl.GetGame().RootNode.AddChild(loadedModel);
                activeNodeID = apxCtrl.GetGame().RootNode.Children.Count - 1;
                // PopulateTreeView(apxCtrl.GetGame().RootNode);
                AddTreeViewItem(treeView1.Nodes[0], loadedModel);
            }
        }

        private void openAsSeperateSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                for (int i = apxCtrl.GetGame().RootNode.Children.Count - 1; i > -1; i--)
                {
                    apxCtrl.GetGame().RootNode.RemoveChild(apxCtrl.GetGame().RootNode.GetChild(i));
                }
                treeView1.Nodes.Clear();
                ApexEngine.Scene.GameObject loadedModel = ApexEngine.Assets.AssetManager.LoadModel(openFileDialog1.FileName);
                loadedModel.SetLocalTranslation(new ApexEngine.Math.Vector3f(0, 0, 5));
                apxCtrl.GetGame().RootNode.AddChild(loadedModel);
                activeNodeID = apxCtrl.GetGame().RootNode.Children.Count - 1;
                PopulateTreeView(apxCtrl.GetGame().RootNode);
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            treeView1.Nodes.Clear();
            PopulateTreeView(apxCtrl.GetGame().RootNode);

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                GameObject[] objs = new GameObject[apxCtrl.GetGame().RootNode.Children.Count];
                for (int i = 0; i < objs.Length; i++)
                {
                    objs[i] = apxCtrl.GetGame().RootNode.GetChild(i);
                }

                ApexEngine.Assets.Apx.ApxExporter.ExportModel(saveFileDialog1.FileName, objs);
            }
        }
    }
}
