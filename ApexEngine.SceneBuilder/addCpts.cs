using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApexEngine.Scene.Components;
using ApexEngine.Terrain.SimplexTerrain;
using ApexEngine;

namespace ApexEditor
{
    public partial class addCpts : Form
    {
        public GameComponent resComponent = null;
        private Game game;

        public addCpts(Game game)
        {
            InitializeComponent();
            this.game = game;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString() == "Procedural Terrain")
                resComponent = new SimplexTerrainComponent(game.PhysicsWorld);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
