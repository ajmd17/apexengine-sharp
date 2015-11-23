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
        private int framerate = 10;
        Game game;

        public int Framerate
        {
            get { return framerate; }
            set { framerate = value; timer1.Interval = framerate; }
        }

        public Game Game
        {
            get { return game; }
            set { game = value; }
        }

        public ApexEngineControl(Game game)
        {
            InitializeComponent();
            this.game = game;
            timer1.Interval = framerate;
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
            Point p = this.PointToScreen(Location);
            game.InputManager.WINDOW_X = p.X;
            game.InputManager.WINDOW_Y = p.Y;
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            glControl1.MakeCurrent();
            GL.Viewport(0, 0, this.Width, this.Height);
            game.Camera.Width = this.Width;
            game.Camera.Height = this.Height;
            game.InputManager.SCREEN_HEIGHT = this.Height;
            game.InputManager.SCREEN_WIDTH = this.Width;
        }

        private void glControl1_Move(object sender, EventArgs e)
        {
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                game.InputManager.MouseButtonDown(OpenTK.Input.MouseButton.Right);
            if (e.Button == MouseButtons.Left)
                game.InputManager.MouseButtonDown(OpenTK.Input.MouseButton.Left);
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                game.InputManager.MouseButtonUp(OpenTK.Input.MouseButton.Right);
            if (e.Button == MouseButtons.Left)
                game.InputManager.MouseButtonUp(OpenTK.Input.MouseButton.Left);
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ApexEngineControl_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
