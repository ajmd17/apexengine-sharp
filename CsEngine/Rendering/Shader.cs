using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace CsEngine.Rendering
{
    public class Shader
    {
        const string A_POSITION = "a_position";
        const string A_TEXCOORD0 = "a_texcoord0";
        const string A_TEXCOORD1 = "a_texcoord1";
        const string A_NORMAL = "a_normal";
        const string A_TANGENT = "a_tangent";
        const string A_BITANGENT = "a_bitangent";
        const string A_BONEWEIGHT = "a_boneweight";
        const string A_BONEINDEX = "a_boneindex";
        protected int id = 0;
        public Shader(string vs_code, string fs_code)
        {
            Create();
            AddVertexProgram(vs_code);
            AddFragmentProgram(fs_code);
            CompileShader();
        }
        public void Create()
        {
            id = GL.CreateProgram();
            if (id == 0)
            {
                throw new Exception("An error occurred while creating the shader!");
            }
        }
        public void Use()
        {
            GL.UseProgram(id);
        }
        public void End()
        {

        }
        public static void Clear()
        {
            GL.UseProgram(0);
        }
        public void CompileShader()
        {
            Use();
            GL.BindAttribLocation(id, 0, A_POSITION);
            GL.BindAttribLocation(id, 1, A_TEXCOORD0);
            GL.BindAttribLocation(id, 2, A_TEXCOORD1);
            GL.BindAttribLocation(id, 3, A_NORMAL);
            GL.BindAttribLocation(id, 4, A_TANGENT);
            GL.BindAttribLocation(id, 5, A_BITANGENT);
            GL.BindAttribLocation(id, 6, A_BONEINDEX);
            GL.BindAttribLocation(id, 7, A_BONEWEIGHT);
            GL.LinkProgram(id);
            GL.ValidateProgram(id);
        }
        public void Render(Mesh mesh)
        {
            SetDefaultValues();
            //SetUniform("u_world", worldMatrix);
            //SetUniform("u_view", viewMatrix);
            //SetUniform("u_proj", projectionMatrix);
            mesh.Render();
        }
        static void SetDefaultValues()
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
        }
        void AddVertexProgram(string vs)
        {
            AddProgram(vs, ShaderType.VertexShader);
        }
        void AddFragmentProgram(string fs)
        {
            AddProgram(fs, ShaderType.FragmentShader);
        }
        void AddGeometryProgram(string gs)
        {
            AddProgram(gs, ShaderType.GeometryShader);
        }
        void AddProgram(string code, ShaderType type)
        {
            int shader = GL.CreateShader(type);
            if (shader == 0)
            {
                throw new Exception("Error creating shader.\n\tShader type: " + type + "\n\tCode: " + code);
            }
            GL.ShaderSource(shader, code);
            GL.CompileShader(shader);
            GL.AttachShader(id, shader);
        }
    }
}
