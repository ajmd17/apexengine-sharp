using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ApexEngine;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using ApexEngine.Rendering;
using ApexEngine.Math;
using ApexEngine.Input;

namespace ApexEditor
{
    public partial class ApexEngineControl : UserControl
    {
        Game game;
        public Game GetGame()
        {
            return game;
        }
        public ApexEngineControl(Game game)
        {
            InitializeComponent();
            this.game = game;
            timer1.Start();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl1.MakeCurrent();
            game.RenderInternal();
            glControl1.SwapBuffers();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            game.InitInternal();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            game.UpdateInternal();
            glControl1.Invalidate();
            RenderManager.WINDOW_X = this.ParentForm.Location.X + this.Location.X;
            RenderManager.WINDOW_Y = this.ParentForm.Location.Y + this.Location.Y;
            
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, this.Width, this.Height);
            RenderManager.SCREEN_HEIGHT = this.Height;
            RenderManager.SCREEN_WIDTH = this.Width;
        }

        private void glControl1_Move(object sender, EventArgs e)
        {
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                Input.MouseButtonDown(OpenTK.Input.MouseButton.Right);
            if (e.Button == MouseButtons.Left)
                Input.MouseButtonDown(OpenTK.Input.MouseButton.Left);
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                Input.MouseButtonUp(OpenTK.Input.MouseButton.Right);
            if (e.Button == MouseButtons.Left)
                Input.MouseButtonUp(OpenTK.Input.MouseButton.Left);
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
