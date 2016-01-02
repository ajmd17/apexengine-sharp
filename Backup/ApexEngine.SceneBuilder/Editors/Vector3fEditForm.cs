using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApexEditor.Editors
{
    public partial class Vector3fEditForm : Form
    {
        public float x, y, z;

        private void Vector3fEditForm_Load(object sender, EventArgs e)
        {

        }

        public Vector3fEditForm(float _x, float _y, float _z)
        {
            InitializeComponent();
            this.x = _x;
            this.y = _y;
            this.z = _z;
            textBox1.Text = x.ToString();
            textBox2.Text = y.ToString();
            textBox3.Text = z.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float.TryParse(textBox1.Text, out x);
            float.TryParse(textBox2.Text, out y);
            float.TryParse(textBox3.Text, out z);
            this.DialogResult = DialogResult.OK;
        }
    }
}
