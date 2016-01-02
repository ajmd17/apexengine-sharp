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
using ApexEditor.NormalMapGenerator;
using ApexEngine.Scene.Components;
using ApexEngine.Math;
using ApexEngine.Rendering.Animation;

namespace ApexEditor
{
    public partial class Form1 : Form
    {
        ApexEngineControl apxCtrl;
        private int activeNodeID;
        frmMatEditor matEditor;
        private ApexEngine.Rendering.Shadows.ShadowMappingComponent shadowCpt;
        private SceneEditorGame.CamModes camMode = SceneEditorGame.CamModes.Freelook;
        private Vector3f rotateAxis = new Vector3f(Vector3f.UnitX);



        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("Apex Editor Started.");
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
            ImageList ilist1 = new ImageList();
            ilist1.Images.Add(Properties.Resources.node_16);
            ilist1.Images.Add(Properties.Resources.geometry_16);
            ilist1.Images.Add(Properties.Resources.material);
            treeView1.ImageList = ilist1;

            SceneEditorGame game = new SceneEditorGame();
            //  game.Camera = new ApexEngine.Rendering.Cameras.DefaultCamera(game.InputManager, 75);
            game.Camera.Translation = new ApexEngine.Math.Vector3f(0, 2, 0);
            game.Camera.Enabled = false;
            apxCtrl = new ApexEngineControl(game);
            apxCtrl.Dock = DockStyle.Fill;
            pnlGameView.Controls.Add(apxCtrl);


            contextMenuStrip1.Renderer = new metroToolStripRenderer();
            metroMenuStrip2.Renderer = new metroToolStripRenderer();

            

            PopulateTreeView(game.RootNode);
            /*

            SceneEditorGame orthoTop = new SceneEditorGame();
          //  orthoTop.Camera = new ApexEngine.Rendering.Cameras.OrthoCamera(-5, 5, -5, 5, -5, 5);
            orthoTop.Camera.Translation = new ApexEngine.Math.Vector3f(0, 0, -5);
           // orthoTop.RenderManager.GeometryList = game.RenderManager.GeometryList;
            ApexEngineControl orthoTopCtrl = new ApexEngineControl(orthoTop);
            orthoTopCtrl.Dock = DockStyle.Fill;
            pnlOrthoTop.Controls.Add(orthoTopCtrl);*/
            apxCtrl.MouseWheel += new MouseEventHandler(MouseScroll);

        }

        float rot;

        private void MouseScroll(object sender, MouseEventArgs e)
        {
            float diff = ((float)e.Delta)*0.1f;
            SceneEditorGame seg = (SceneEditorGame)apxCtrl.Game;

            /* if (camMode == SceneEditorGame.CamModes.Grab)
             {
                 if (seg.objectHolding != null)
                 {
                  //   Vector3f ctrans = new Vector3f(seg.objectHolding.GetWorldTranslation());

                  //   ctrans.AddStore(seg.Camera.Direction.Multiply(diff));
                     seg.objectHolding.SetLocalTranslation(seg.Camera.Translation.Add(seg.Camera.Direction.Multiply(diff)));
                 }
             }*/
            if (camMode == SceneEditorGame.CamModes.Rotate)
            {
                if (seg.objectHolding != null)
                {
                    seg.objectHolding.SetLocalRotation(seg.objectHolding.GetLocalRotation().Multiply(new Quaternion().SetFromAxis(rotateAxis, diff)));
                    Console.WriteLine(seg.objectHolding.GetLocalRotation());
                }
            }
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
                TreeNode matNode = new TreeNode(geom.Material.GetName() + " (Material)");
                matNode.Tag = geom.Material;
                matNode.ImageIndex = 2;
                matNode.SelectedImageIndex = 2;
                newNode.Nodes.Add(matNode);
                newNode.ImageIndex = 1;
                newNode.SelectedImageIndex = 1;

                if (geom.Mesh != null)
                {
                    if (geom.Mesh.GetSkeleton() != null)
                    {
                        TreeNode skeletonNode = new TreeNode(obj.Name + " (Skeleton)");
                        skeletonNode.Tag = geom.Mesh.GetSkeleton();
                        for (int i = 0; i < geom.Mesh.GetSkeleton().GetNumBones(); i++)
                        {
                            TreeNode boneNode = new TreeNode(geom.Mesh.GetSkeleton().GetBone(i).Name + " (Bone)");
                            boneNode.Tag = geom.Mesh.GetSkeleton().GetBone(i);
                            skeletonNode.Nodes.Add(boneNode);
                        }
                        newNode.Nodes.Add(skeletonNode);
                    }
                }

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
                newNode.ImageIndex = 0;
                newNode.SelectedImageIndex = 0;
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

        private void ReloadComponents()
        {
            listBox1.Items.Clear();
            foreach (GameComponent gc in apxCtrl.Game.GameComponents)
            {
                listBox1.Items.Add(gc);
            }
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
               // List<Geometry> geoms = ApexEngine.Rendering.Util.MeshUtil.GatherGeometry(loadedModel);
               // foreach (Geometry g in geoms)
                    apxCtrl.Game.PhysicsWorld.AddObject(loadedModel, 0f, ApexEngine.Scene.Physics.PhysicsWorld.PhysicsShape.Box);
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
                List<GameObject> objs = ApexEngine.Rendering.Util.RenderUtil.GatherObjects(apxCtrl.Game.RootNode);
                foreach (GameObject g in objs)
                    apxCtrl.Game.PhysicsWorld.RemoveObject(g);

                for (int i = apxCtrl.Game.RootNode.Children.Count - 1; i > -1; i--)
                    apxCtrl.Game.RootNode.RemoveChild(apxCtrl.Game.RootNode.GetChild(i));
                treeView1.Nodes.Clear();
                ApexEngine.Scene.GameObject loadedModel = ApexEngine.Assets.AssetManager.LoadModel(openFileDialog1.FileName);
                apxCtrl.Game.RootNode.AddChild(loadedModel);
                apxCtrl.Game.PhysicsWorld.AddObject(loadedModel, 0f, ApexEngine.Scene.Physics.PhysicsWorld.PhysicsShape.Box);
                activeNodeID = apxCtrl.Game.RootNode.Children.Count - 1;
                PopulateTreeView(apxCtrl.Game.RootNode);
            }
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
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
                propertyGrid1.SelectedObject = e.Node.Tag;
                if (e.Node.Tag is GameObject)
                {
                    SceneEditorGame seg = (SceneEditorGame)apxCtrl.Game;
                    seg.objectHolding = (GameObject)e.Node.Tag;
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
                if (treeView1.SelectedNode.Tag is GameObject && !(treeView1.SelectedNode.Tag is Bone))
                {
                    if (treeView1.SelectedNode.Tag != apxCtrl.Game.RootNode)
                    {
                        GameObject selectedObj = (GameObject)treeView1.SelectedNode.Tag;
                        List<GameObject> objsAttached = ApexEngine.Rendering.Util.RenderUtil.GatherObjects(selectedObj);
                        for (int i = 0; i < objsAttached.Count; i++)
                        {
                            apxCtrl.Game.PhysicsWorld.RemoveObject(objsAttached[i]);
                            if (objsAttached[i] is Geometry)
                            {
                                Geometry geom = (Geometry)objsAttached[i];
                                geom.Mesh.vertices.Clear();
                                geom.Mesh.indices.Clear();
                                geom.Mesh = null;
                            }
                            objsAttached[i] = null; 
                        }
                        apxCtrl.Game.PhysicsWorld.RemoveObject(selectedObj);
                        selectedObj.GetParent().RemoveChild(selectedObj);
                        selectedObj = null;
                        System.GC.Collect();
                    }
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                    treeView1.SelectedNode.Tag = null;
                }
                else if (treeView1.SelectedNode.Tag is Bone)
                {
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
            if (treeView1.SelectedNode != null)
            {
                

                if (treeView1.SelectedNode.Tag == apxCtrl.Game.RootNode ||  (treeView1.SelectedNode.Tag is Bone))
                {
                    contextMenuStrip1.Items[0].Enabled = false;
                }
                else
                {
                    contextMenuStrip1.Items[0].Enabled = true;

                }
                if (treeView1.SelectedNode.Tag is Geometry)
                {
                    setToOriginToolStripMenuItem.Enabled = true;
                    modifyToolStripMenuItem.Enabled = true;
                }
                else
                {
                    setToOriginToolStripMenuItem.Enabled = false;
                    modifyToolStripMenuItem.Enabled = false;
                }

                if (treeView1.SelectedNode.Tag is Node && !(treeView1.SelectedNode.Tag is Bone))
                {
                    lockToolStripMenuItem.Enabled = true;
                    if (((Node)treeView1.SelectedNode.Tag).HasController(typeof(ApexEngine.Scene.Physics.RigidBodyControl)))
                        lockToolStripMenuItem.Checked = true;
                    else
                        lockToolStripMenuItem.Checked = false;
                }
                else
                {
                    lockToolStripMenuItem.Enabled = false;
                    lockToolStripMenuItem.Checked = false;
                }
            }
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag != apxCtrl.Game.RootNode && treeView1.SelectedNode.Tag is GameObject)
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
                matEdit.Show();
                if (matEdit.DialogResult == DialogResult.OK)
                {
                    treeView1.SelectedNode.Tag = matEdit.Material;
                }
            }
        }

        private void generateCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CodeGen().Show();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameTest test = new GameTest();
            test.RootNode = this.apxCtrl.Game.RootNode;
            test.Run();
        }

        private void normalMapGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new NrmGenerator().Show();
        }

        private void metroMenuStrip5_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            addCpts cptDlg = new addCpts(apxCtrl.Game);
            if (cptDlg.ShowDialog() == DialogResult.OK)
            {
                apxCtrl.Game.AddComponent(cptDlg.resComponent);
                ReloadComponents();
                treeView1.Nodes.Clear();
                PopulateTreeView(apxCtrl.Game.RootNode);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReloadComponents();
            treeView1.Nodes.Clear();
            PopulateTreeView(apxCtrl.Game.RootNode);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                apxCtrl.Game.RemoveComponent((GameComponent)listBox1.SelectedItem);
                ReloadComponents();
                treeView1.Nodes.Clear();
                PopulateTreeView(apxCtrl.Game.RootNode);
            }
            catch (Exception ex) { }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            PopulateTreeView(apxCtrl.Game.RootNode);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            PopulateTreeView(apxCtrl.Game.RootNode);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void renderWireframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((SceneEditorGame)apxCtrl.Game).RenderDebug = renderWireframeToolStripMenuItem.Checked;
        }

        private void shadowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*if (shadowsToolStripMenuItem.Checked)
            {
                apxCtrl.Game.RenderManager.AddComponent((shadowCpt == null ? shadowCpt = new ApexEngine.Rendering.Shadows.ShadowMappingComponent(apxCtrl.Game.Camera, apxCtrl.Game.Environment) : shadowCpt));
            }
            else
            {
                apxCtrl.Game.RenderManager.RemoveComponent(shadowCpt);
            }*/
        }

        private void setToOriginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Geometry geom = (Geometry)treeView1.SelectedNode.Tag;
            ApexEngine.Rendering.Util.MeshUtil.SetToOrigin(geom);
        }

        private void lockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameObject go = (GameObject)treeView1.SelectedNode.Tag;
            if (go is Node)
            {
                if (!lockToolStripMenuItem.Checked)
                {
                    List<GameObject> absChildren = ApexEngine.Rendering.Util.RenderUtil.GatherObjects(go);
                    foreach (GameObject child in absChildren)
                    {
                        if (child.HasController(typeof(ApexEngine.Scene.Physics.RigidBodyControl)))
                        {
                            apxCtrl.Game.PhysicsWorld.RemoveObject(child);
                        }
                    }
                    if (!go.HasController(typeof(ApexEngine.Scene.Physics.RigidBodyControl)))
                    {
                        apxCtrl.Game.PhysicsWorld.AddObject(go, 0.0f, ApexEngine.Scene.Physics.PhysicsWorld.PhysicsShape.Box);
                    }
                }
                else
                {
                    if (go.HasController(typeof(ApexEngine.Scene.Physics.RigidBodyControl)))
                    {
                        apxCtrl.Game.PhysicsWorld.RemoveObject(go);
                    }
                    foreach (GameObject child in ((Node)go).Children)
                    {
                        if (!child.HasController(typeof(ApexEngine.Scene.Physics.RigidBodyControl)))
                        {
                            apxCtrl.Game.PhysicsWorld.AddObject(child, 0.0f, ApexEngine.Scene.Physics.PhysicsWorld.PhysicsShape.Box);
                        }
                    }
                }
            }
        }

        private void SetCamMode(SceneEditorGame.CamModes camMode)
        {
            this.camMode = camMode;
            checkBox1.Checked = (camMode == SceneEditorGame.CamModes.Freelook);
            checkBox2.Checked = (camMode == SceneEditorGame.CamModes.Grab);
            checkBox3.Checked = (camMode == SceneEditorGame.CamModes.Rotate);
            ((SceneEditorGame)apxCtrl.Game).CamMode = camMode;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            SetCamMode(SceneEditorGame.CamModes.Freelook);
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            SetCamMode(SceneEditorGame.CamModes.Grab);
        }

        private void checkBox3_Click(object sender, EventArgs e)
        {
            SetCamMode(SceneEditorGame.CamModes.Rotate);
        }

        private void simplifyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void simplifyMeshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag is Geometry)
            {
                Geometry geom = (Geometry)treeView1.SelectedNode.Tag;
                frmSimplify f_simp = new frmSimplify(geom.Mesh);
                if (f_simp.ShowDialog() == DialogResult.OK)
                {
                    geom.Mesh = f_simp.NewMesh;
                }
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            SceneEditorGame seg = (SceneEditorGame)apxCtrl.Game;
            if (e.KeyCode == Keys.C)
            {
                seg.Centered = !seg.Centered;
                Console.WriteLine("Centered: " + seg.Centered);
                checkBox4.Checked = seg.Centered;
                if (seg.Centered)
                {
                 //   if (seg.objectHolding != null)
                //    {
                 //       Vector3f diff = seg.objectHolding.GetLocalTranslation().Subtract(objectHolding.)
                   // }


                    if (seg.objectHolding != null)
                        seg.objectHolding.SetLocalTranslation(seg.objectHolding.GetLocalTranslation().Subtract(seg.objectHolding.GetLocalBoundingBox().Center.Subtract(new Vector3f(0f, seg.objectHolding.GetLocalBoundingBox().Center.Y, 0f))));
                }
                else
                {
                    if (seg.objectHolding != null)
                        seg.objectHolding.SetLocalTranslation(seg.objectHolding.GetLocalTranslation().Add(seg.objectHolding.GetLocalBoundingBox().Center.Subtract(new Vector3f(0f, seg.objectHolding.GetLocalBoundingBox().Center.Y, 0f))));
                }
            }
            else if (e.KeyCode == Keys.F)
            {
                this.SetCamMode(SceneEditorGame.CamModes.Freelook);
            }
            else if (e.KeyCode == Keys.G)
            {
                this.SetCamMode(SceneEditorGame.CamModes.Grab);
            }
            else if (e.KeyCode == Keys.R)
            {
                this.SetCamMode(SceneEditorGame.CamModes.Rotate);
            }
            else if (e.KeyCode == Keys.X)
            {
                if (camMode == SceneEditorGame.CamModes.Grab)
                {
                    seg.MovingX = !seg.MovingX;
                    seg.MovingY = false;
                    seg.MovingZ = false;
                    if (!seg.MovingX)
                    {
                        seg.lastMouseX = seg.InputManager.GetMouseX();
                        seg.lastMouseY = seg.InputManager.GetMouseY();
                    }
                    if (seg.objectHolding != null)
                    {
                        seg.offsetLoc.Set(seg.objectHolding.GetWorldTranslation());
                      //  Vector2f proj = apxCtrl.Game.Camera.Project(seg.objectHolding.GetWorldTranslation());
                    }
                }
                else if (camMode == SceneEditorGame.CamModes.Rotate)
                {
                    rotateAxis.Set(Vector3f.UnitX);
                }
            }
            else if (e.KeyCode == Keys.Y)
            {
                if (camMode == SceneEditorGame.CamModes.Grab)
                {
                    seg.MovingY = !seg.MovingY;
                    seg.MovingX = false;
                    seg.MovingZ = false;
                    if (!seg.MovingY)
                    {
                        seg.lastMouseX = seg.InputManager.GetMouseX();
                        seg.lastMouseY = seg.InputManager.GetMouseY();
                    }
                    if (seg.objectHolding != null)
                    {
                        seg.offsetLoc.Set(seg.objectHolding.GetWorldTranslation());
                      //  Vector2f proj = apxCtrl.Game.Camera.Project(seg.objectHolding.GetWorldTranslation());
                    }
                }
                else if (camMode == SceneEditorGame.CamModes.Rotate)
                {
                    rotateAxis.Set(Vector3f.UnitY);
                }
            }
            else if (e.KeyCode == Keys.Z)
            {
                if (camMode == SceneEditorGame.CamModes.Grab)
                {
                    seg.MovingZ = !seg.MovingZ;
                    seg.MovingY = false;
                    seg.MovingX = false;
                    if (!seg.MovingZ)
                    {
                        seg.lastMouseX = seg.InputManager.GetMouseX();
                        seg.lastMouseY = seg.InputManager.GetMouseY();
                    }
                    if (seg.objectHolding != null)
                    {
                      //  Vector2f proj = apxCtrl.Game.Camera.Project(seg.objectHolding.GetWorldTranslation());
                    }
                }
                else if (camMode == SceneEditorGame.CamModes.Rotate)
                {
                    rotateAxis.Set(Vector3f.UnitZ);
                }
            }

        }

        private void controlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmCtrls().ShowDialog();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            SceneEditorGame seg = (SceneEditorGame)apxCtrl.Game;
            seg.Centered = checkBox4.Checked ;

        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void pnlGameView_Scroll(object sender, ScrollEventArgs e)
        {
        }

        private void renderboundingBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SceneEditorGame seg = (SceneEditorGame)apxCtrl.Game;
            seg.BoundingBoxes = renderboundingBoxesToolStripMenuItem.Checked;
        }
    }
}
