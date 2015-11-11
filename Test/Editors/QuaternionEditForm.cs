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
    public partial class QuaternionEditForm : Form
    {
        public float x, y, z, w;
        
        public QuaternionEditForm(float _x, float _y, float _z, float _w)
        {
            InitializeComponent();
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.w = _w;
            textBox1.Text = x.ToString();
            textBox2.Text = y.ToString();
            textBox3.Text = z.ToString();
            textBox4.Text = w.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float.TryParse(textBox1.Text, out x);
            float.TryParse(textBox2.Text, out y);
            float.TryParse(textBox3.Text, out z);
            float.TryParse(textBox4.Text, out w);
            this.DialogResult = DialogResult.OK;
        }
    }
}
