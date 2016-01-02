using ApexEngine.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApexEditor
{
    public partial class frmSimplify : Form
    {
        private Mesh mesh;
        private Mesh newMesh;

        public Mesh NewMesh
        {
            get { return newMesh; }
        }


        public frmSimplify(Mesh mesh)
        {
            InitializeComponent();
            this.mesh = mesh;
        }

        private void SimplifyMesh()
        {
            newMesh = new Mesh();
            List<Vertex> newVerts = new List<Vertex>();
            for (int i = 0; i < mesh.indices.Count; i+=6)
            {
                // triangle
                newVerts.Add(mesh.vertices[mesh.indices[i]]);
                newVerts.Add(mesh.vertices[mesh.indices[i+1]]);
                newVerts.Add(mesh.vertices[mesh.indices[i+2]]);
            }
            newMesh.SetVertices(newVerts);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SimplifyMesh();
            this.DialogResult = DialogResult.OK;
        }

        private void frmSimplify_Load(object sender, EventArgs e)
        {

        }
    }
}
