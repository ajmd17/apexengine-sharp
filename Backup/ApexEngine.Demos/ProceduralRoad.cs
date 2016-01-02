using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Rendering;
using ApexEngine.Rendering.Util;
using ApexEngine.Scene;
using System;
using System.Collections.Generic;

namespace ApexEngine.Demos
{
    public class Line
    {
        private Vector3f to, from;

        public Line(Vector3f from, Vector3f to)
        {
            this.from = from;
            this.to = to;
        }

        public Vector3f To
        {
            get { return to; }
            set { to.Set(value); }
        }

        public Vector3f From
        {
            get { return from; }
            set { from.Set(value); }
        }
    }

    public class ProceduralRoad : Game
    {
        private Node roadNode = new Node(), roadCornerNode = new Node(), curbNode = new Node();


        private List<Mesh> roads = new List<Mesh>();
        private GameObject curbModel, curbCorner;
        private Texture road, roadCorner;
        private Line lastLine = new Line(new Vector3f(0, 0, 0), new Vector3f(0, 0, 0));
        private Vector3f tmpVec = new Vector3f();
        Random r = new Random(333);

        public ProceduralRoad(Renderer renderer) : base(renderer)
        {
        }

        public Mesh QuadFromLine(Line line)
        {
            float width = 2f;

            float scale = 2f;

            Vector3f quadSize = line.To.Subtract(line.From);
            float texCoordScale = ((float)System.Math.Abs(quadSize.x) + (float)System.Math.Abs(quadSize.z)) / 4;
            quadSize.NormalizeStore();


            Node rNode = new Node();

              Mesh mesh = new Mesh();
              List<Vertex> vertices = new List<Vertex>();

              vertices.Add(new Vertex(new Vector3f(line.From.x - quadSize.z, line.From.y, line.From.z + quadSize.x), new Vector2f(0, 0), new Vector3f(0, 1, 0)));
              vertices.Add(new Vertex(new Vector3f(line.From.x + quadSize.z, line.From.y, line.From.z - quadSize.x), new Vector2f(0, 1), new Vector3f(0, 1, 0)));
              vertices.Add(new Vertex(new Vector3f(line.To.x + quadSize.z, line.To.y, line.To.z - quadSize.x), new Vector2f(texCoordScale, 1), new Vector3f(0, 1, 0)));

              vertices.Add(new Vertex(new Vector3f(line.To.x + quadSize.z, line.To.y, line.To.z - quadSize.x), new Vector2f(texCoordScale, 1), new Vector3f(0, 1, 0)));
              vertices.Add(new Vertex(new Vector3f(line.To.x - quadSize.z, line.To.y, line.To.z + quadSize.x), new Vector2f(texCoordScale, 0), new Vector3f(0, 1, 0)));
              vertices.Add(new Vertex(new Vector3f(line.From.x - quadSize.z, line.From.y, line.From.z + quadSize.x), new Vector2f(0, 0), new Vector3f(0, 1, 0)));

              mesh.SetVertices(vertices);

            rNode.AddChild(new Geometry(mesh));


            return mesh;
        }

        

        public override void Init()
        {
            road = AssetManager.LoadTexture(AssetManager.GetAppPath() + "\\textures\\road.jpg");
            roadCorner = AssetManager.LoadTexture(AssetManager.GetAppPath() + "\\textures\\road_corner.jpg");
            curbModel = AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\curb.obj");
            curbCorner = AssetManager.LoadModel(AssetManager.GetAppPath() + "\\models\\curb_corner.obj");

            Random rand = new Random();

            int xAmt = rand.Next(2, 7), zAmt = rand.Next(2, 7), scaleX = rand.Next(10, 30), scaleZ = rand.Next(10, 30);

            int offset = 3;

            AddRoadCorner(new Line(new Vector3f(0, 0, -2), new Vector3f(0, 0, 0)));

            for (int x = 0; x <= xAmt; x++)
            {
                for (int i = 0; i < zAmt; i++)
                {
                    AddRoad(new Line(new Vector3f(x * scaleX, 0, i * scaleZ), new Vector3f(x * scaleX, 0, (i + 1) * scaleZ - 2)));
                    AddRoadCorner(new Line(new Vector3f(x * scaleX, 0, (i + 1) * scaleZ - 2), new Vector3f(x * scaleX, 0, (i + 1) * scaleZ)));

                    if (x == 0)
                    {
                        // long bar
                        Node curb2 = (Node)curbModel.Clone();
                        tmpVec.Set(x * scaleX - 1, 0, (i + 1) * scaleZ - 1);
                        tmpVec.y = HeightRoad(tmpVec);
                        curb2.SetLocalTranslation(tmpVec);
                        curb2.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, 180));
                        curb2.SetLocalScale(new Vector3f(1, 1, scaleZ));
                        curbNode.AddChild(curb2);

                    }
                    else if (x == (xAmt - 1))
                    {
                        Node curb2 = (Node)curbModel.Clone();
                        tmpVec.Set((x + 1) * scaleX + 1, 0, (i) * scaleZ - 1);
                        tmpVec.y = HeightRoad(tmpVec);
                        curb2.SetLocalTranslation(tmpVec);
                        curb2.SetLocalScale(new Vector3f(1, 1, scaleZ));
                        curbNode.AddChild(curb2);
                    }

                    Node curb0 = (Node)curbModel.Clone();
                    tmpVec.Set(x * scaleX + 1, 0, i * scaleZ + 1);
                    tmpVec.y = HeightRoad(tmpVec);
                    curb0.SetLocalTranslation(tmpVec);
                    curb0.SetLocalScale(new Vector3f(1, 1, scaleZ - offset - 1));
                    curbNode.AddChild(curb0);

                    Node curb1 = (Node)curbModel.Clone();
                    tmpVec.Set(x * scaleX - 1, 0, (i + 1) * scaleZ - offset);
                    tmpVec.y = HeightRoad(tmpVec);
                    curb1.SetLocalTranslation(tmpVec);
                    curb1.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, 180));
                    curb1.SetLocalScale(new Vector3f(1, 1, scaleZ - offset - 1));
                    curbNode.AddChild(curb1);

                    if (x != 0)
                    {
                        Node curbCorner1 = (Node)curbCorner.Clone();
                        tmpVec.Set((x) * scaleX - 2, 0, (i + 1) * scaleZ - offset + 1);
                        tmpVec.y = HeightRoad(tmpVec);
                        curbCorner1.SetLocalTranslation(tmpVec);
                        curbCorner1.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, 180 + 90));
                        curbNode.AddChild(curbCorner1);
                    }

                    if (x != xAmt)
                    {
                        Node curbCorner0 = (Node)curbCorner.Clone();
                        tmpVec.Set(x * scaleX + 2, 0, i * scaleZ);
                        tmpVec.y = HeightRoad(tmpVec);
                        curbCorner0.SetLocalTranslation(tmpVec);
                        curbCorner0.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, 90));
                        curbNode.AddChild(curbCorner0);
                    }
                }
            }

            for (int z = 0; z <= zAmt; z++)
            {
                for (int i = 0; i < xAmt; i++)
                {
                    AddRoad(new Line(new Vector3f(i * scaleX + 1, 0, z * scaleZ - 1), new Vector3f((i + 1) * scaleX - 1, 0, z * scaleZ - 1)));
                    AddRoadCorner(new Line(new Vector3f((i + 1) * scaleX - 1, 0, z * scaleZ - 1), new Vector3f((i + 1) * scaleX + 1, 0, z * scaleZ - 1)));

                    // long pieces
                    if (z == 0)
                    {
                        // long bar
                        Node curb2 = (Node)curbModel.Clone();
                        tmpVec.Set(i * scaleX, 0, z * scaleZ - 2);
                        tmpVec.y = HeightRoad(tmpVec);
                        curb2.SetLocalTranslation(tmpVec);
                        curb2.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, 180 + 90));
                        curb2.SetLocalScale(new Vector3f(scaleX, 1, 1));
                        curbNode.AddChild(curb2);
                    }
                    else if (z == (zAmt - 1))
                    {
                        Node curb2 = (Node)curbModel.Clone();
                        tmpVec.Set((i + 1) * scaleX, 0, (z + 1) * scaleZ);
                        tmpVec.y = HeightRoad(tmpVec);
                        curb2.SetLocalTranslation(tmpVec);
                        curb2.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, 180 + 90 + 180));
                        curb2.SetLocalScale(new Vector3f(scaleX, 1, 1));
                        curbNode.AddChild(curb2);
                    }

                    Node curb0 = (Node)curbModel.Clone();
                    tmpVec.Set((i + 1) * scaleX - 2, 0, z * scaleZ);
                    tmpVec.y = HeightRoad(tmpVec);
                    curb0.SetLocalTranslation(tmpVec);
                    curb0.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, 90));
                    curb0.SetLocalScale(new Vector3f(scaleX - offset - 1, 1, 1));
                    curbNode.AddChild(curb0);

                    Node curb1 = (Node)curbModel.Clone();
                    tmpVec.Set((i) * scaleX + offset - 1, 0, z * scaleZ - 2);
                    tmpVec.y = HeightRoad(tmpVec);
                    curb1.SetLocalTranslation(tmpVec);
                    curb1.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, 180 + 90));
                    curb1.SetLocalScale(new Vector3f(scaleX - offset - 1, 1, 1));
                    curbNode.AddChild(curb1);

                    if (z != zAmt)
                    {
                        Node curbCorner0 = (Node)curbCorner.Clone();
                        Vector3f curbVec1 = new Vector3f((i + 1) * scaleX - 1, 0, z * scaleZ + 1);
                        curbVec1.y = HeightRoad(curbVec1);
                        curbCorner0.SetLocalTranslation(curbVec1);
                        curbCorner0.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, 180));
                        curbNode.AddChild(curbCorner0);
                    }
                    if (z != 0)
                    {
                        Node curbCorner1 = (Node)curbCorner.Clone();
                        Vector3f curbVec1 = new Vector3f((i) * scaleX + offset - 2, 0, z * scaleZ - 3);
                        curbVec1.y = HeightRoad(curbVec1);
                        curbCorner1.SetLocalTranslation(curbVec1);
                        curbCorner1.SetLocalRotation(new Quaternion().SetFromAxis(Vector3f.UnitY, 360));
                        curbNode.AddChild(curbCorner1);
                    }
                }
            }

            Geometry roadMerged = new Geometry(MeshUtil.MergeMeshes(roadNode));
            Geometry curbMerged = new Geometry(MeshUtil.MergeMeshes(curbNode));

            rootNode.AddChild(curbMerged);

            roadMerged.Material.SetValue(Material.MATERIAL_CULLENABLED, false);
            roadMerged.Material.SetValue(Material.TEXTURE_DIFFUSE, road);
            roadMerged.Material.SetValue(Material.SHININESS, 0.0f);

            Geometry roadCornerMerged = new Geometry(MeshUtil.MergeMeshes(roadCornerNode));

            roadCornerMerged.Material.SetValue(Material.MATERIAL_CULLENABLED, false);
            roadCornerMerged.Material.SetValue(Material.TEXTURE_DIFFUSE, roadCorner);
            roadCornerMerged.Material.SetValue(Material.SHININESS, 0.0f);

            rootNode.AddChild(roadCornerMerged);
            rootNode.AddChild(roadMerged);

            /*

                        addroad(new line(new vector3f(0, 0, 0), new vector3f(0, 0, 10)));
                        AddRoad(new Line(new Vector3f(0, 0, 10), new Vector3f(1, 0, 12)));
                        AddRoad(new Line(new Vector3f(1, 0, 12), new Vector3f(5, 0, 12)));*/

            /*

            for (int x = 0; x < 16; x ++)
            {
                    AddRoad(new Line(new Vector3f((float)System.Math.Sin(x)*13, 0, (float)System.Math.Cos(x) * 13), new Vector3f((float)System.Math.Sin(x+1) * 13, 0, (float)System.Math.Cos(x+1) * 13)));
            }
            */
            /*     AddRoad(new Line(new Vector3f(0, 0, 10), new Vector3f(-3, 0, 11)));
                 AddRoad(new Line(new Vector3f(-3, 0, 11), new Vector3f(-4, 0, 12)));
                 AddRoad(new Line(new Vector3f(-4, 0, 12), new Vector3f(-5, 0, 13)));
                 AddRoad(new Line(new Vector3f(-5, 0, 13), new Vector3f(-6, 0, 14)));*/
            //    AddRoad(new Line(new Vector3f(0, 0, 10), new Vector3f(10, 0, 10)));

            /*  for (int i = 0; i < 4; i++)
              {
                  Line myline = new Line(lastLine.To, lastLine.To.Add(new Vector3f(rand.Next(-15, 15),0, rand.Next(-15, 15))));
                  Geometry quad = new Geometry(QuadFromLine(myline));
                  quad.Material.SetValue(Material.MATERIAL_CULLENABLED, false);
                  quad.Material.SetValue(Material.TEXTURE_DIFFUSE, road);
                  quad.Material.SetValue(Material.SHININESS, 0.0f);
                  lastLine = myline;

                  rootNode.AddChild(quad);
              }
              */
        }
        private float HeightRoad(Vector3f vec)
        {
            return 0;
        }

        public void AddRoad(Line line)
        {
            Line ln = new Line(new Vector3f(line.From.x, HeightRoad(line.From), line.From.z), new Vector3f(line.To.x, HeightRoad(line.To), line.To.z));

            roadNode.AddChild(new Geometry(QuadFromLine(ln)));




            Vector3f quadSize = line.To.Subtract(line.From);
            quadSize.NormalizeStore();



            // side 1

           // Mesh side1 = MeshFactory.CreateCube(new Vector3f(line.From.x - quadSize.z - 1, line.From.y - 0.1f, line.From.z + 1), new Vector3f(line.To.x + quadSize.z + 1, line.To.y + 0.1f, line.To.z - 1));
          //  curbNode.AddChild(new Geometry(side1));

        }

        public void AddRoadCorner(Line line)
        {
            Line ln = new Line(new Vector3f(line.From.x, HeightRoad(line.From), line.From.z), new Vector3f(line.To.x, HeightRoad(line.To), line.To.z));

            roadCornerNode.AddChild(new Geometry(QuadFromLine(ln)));





        }

        public override void Render()
        {
        }

        public override void Update()
        {
        }
    }
}