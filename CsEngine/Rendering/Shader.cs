using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using ApexEngine.Math;
namespace ApexEngine.Rendering
{
    public class Shader
    {
        const string A_POSITION = "a_position";
        const string A_TEXCOORD0 = "a_texcoord0";
        const string A_TEXCOORD1 = "a_texcoord1";
        const string A_NORMAL = "a_normal";
        const string A_TANGENT = "a_tangent";
        const string A_BITANGENT = "a_bitangent";
        const string A_BONEWEIGHT = "a_boneweights";
        const string A_BONEINDEX = "a_boneindices";
        protected Material currentMaterial = null;
        protected ShaderProperties properties;
        protected Matrix4f worldMatrix, viewMatrix, projectionMatrix;
        protected int id = 0;
        public static string GetApexVertexHeader()
        {
            string res = "";
            res += "uniform mat4 Apex_WorldMatrix;\nuniform mat4 Apex_ViewMatrix;\nuniform mat4 Apex_ProjectionMatrix;\n";
            res += "mat4 FinalTransform() {\n" +
                   "     return Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix;\n" +
                   "}\n";
            return res;
        }
        public Shader(ShaderProperties properties, string vs_code, string fs_code)
        {
            this.properties = properties;
            Create();
            AddVertexProgram(Util.ShaderUtil.FormatShaderVersion(GetApexVertexHeader() + Util.ShaderUtil.FormatShaderProperties(vs_code, properties)));
            AddFragmentProgram(Util.ShaderUtil.FormatShaderVersion(Util.ShaderUtil.FormatShaderProperties(fs_code, properties)));
            CompileShader();
        }
        public ShaderProperties GetProperties()
        {
            return properties;
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
            currentMaterial = null;
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
            GL.BindAttribLocation(id, 6, A_BONEWEIGHT);
            GL.BindAttribLocation(id, 7, A_BONEINDEX);
            GL.LinkProgram(id);
            GL.ValidateProgram(id);
        }
        public void ApplyMaterial(Material material)
        {
            currentMaterial = material;
            this.SetUniform("Material_Shininess", 25f); // material.Shininess
        }
        public void Render(Mesh mesh)
        {
            mesh.Render();
        }
        public virtual void Update(Camera cam, Mesh mesh)
        {
            SetDefaultValues();
            SetUniform("Apex_WorldMatrix", worldMatrix);
            SetUniform("Apex_ViewMatrix", viewMatrix);
            SetUniform("Apex_ProjectionMatrix", projectionMatrix);
            SetUniform("Apex_CameraPosition", cam.Translation);
            SetUniform("Apex_CameraDirection", cam.Direction);
            SetUniform("Apex_ElapsedTime", RenderManager.ElapsedTime);
        }
        static void SetDefaultValues()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
        }
        public void SetTransforms(Matrix4f world, Matrix4f view, Matrix4f proj)
        {
            worldMatrix = world;
            viewMatrix = view;
            projectionMatrix = proj;
        }
        public Matrix4f GetWorldMatrix()
        {
            return worldMatrix;
        }
        public Matrix4f GetViewMatrix()
        {
            return viewMatrix;
        }
        public Matrix4f GetProjectionMatrix()
        {
            return projectionMatrix;
        }
        public void AddVertexProgram(string vs)
        {
            AddProgram(vs, ShaderType.VertexShader);
        }
        public void AddFragmentProgram(string fs)
        {
            AddProgram(fs, ShaderType.FragmentShader);
        }
        public void AddGeometryProgram(string gs)
        {
            AddProgram(gs, ShaderType.GeometryShader);
        }
        public void AddProgram(string code, ShaderType type)
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
        public void SetUniform(string name, int i)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.Uniform1(loc, i);
        }
        public void SetUniform(string name, float f)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.Uniform1(loc, f);
        }
        public void SetUniform(string name, float x, float y)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.Uniform2(loc, x, y);
        }
        public void SetUniform(string name, float x, float y, float z)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.Uniform3(loc, x, y, z);
        }
        public void SetUniform(string name, float x, float y, float z, float w)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.Uniform4(loc, x, y, z, w);
        }
        public void SetUniform(string name, Vector2f vec)
        {
            SetUniform(name, vec.x, vec.y);
        }
        public void SetUniform(string name, Vector3f vec)
        {
            SetUniform(name, vec.x, vec.y, vec.z);
        }
        public void SetUniform(string name, Vector4f vec)
        {
            SetUniform(name, vec.x, vec.y, vec.z, vec.w);
        }
        public void SetUniform(string name, Matrix4f mat)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.UniformMatrix4(loc, 1, true, mat.values);
        }
    }
}
