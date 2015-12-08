using ApexEngine.Math;
using ApexEngine.Scene.Components;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
namespace ApexEngine.Rendering
{
    public class Shader
    {
        private const string A_POSITION = "a_position";
        private const string A_TEXCOORD0 = "a_texcoord0";
        private const string A_TEXCOORD1 = "a_texcoord1";
        private const string A_NORMAL = "a_normal";
        private const string A_TANGENT = "a_tangent";
        private const string A_BITANGENT = "a_bitangent";
        private const string A_BONEWEIGHT = "a_boneweights";
        private const string A_BONEINDEX = "a_boneindices";

        public const string APEX_WORLDMATRIX = "Apex_WorldMatrix";
        public const string APEX_VIEWMATRIX = "Apex_ViewMatrix";
        public const string APEX_PROJECTIONMATRIX = "Apex_ProjectionMatrix";
        public const string APEX_CAMERAPOSITION = "Apex_CameraPosition";
        public const string APEX_CAMERADIRECTION = "Apex_CameraDirection";
        public const string APEX_ELAPSEDTIME = "Apex_ElapsedTime";

        public const string MATERIAL_ALPHADISCARD = "Material_AlphaDiscard";
        public const string MATERIAL_SHININESS = "Material_Shininess";
        public const string MATERIAL_ROUGHNESS = "Material_Roughness";
        public const string MATERIAL_METALNESS = "Material_Metalness";
        public const string MATERIAL_AMBIENTCOLOR = "Material_AmbientColor";
        public const string MATERIAL_DIFFUSECOLOR = "Material_DiffuseColor";
        public const string MATERIAL_SPECULARCOLOR = "Material_SpecularColor";
        public const string MATERIAL_SPECULARTECHNIQUE = "Material_SpecularTechnique";
        public const string MATERIAL_SPECULAREXPONENT = "Material_SpecularExponent";
        public const string MATERIAL_PERPIXELLIGHTING = "Material_PerPixelLighting";

        public const string ENV_FOGCOLOR = "Env_FogColor";
        public const string ENV_FOGSTART = "Env_FogStart";
        public const string ENV_FOGEND = "Env_FogEnd";
        public const string ENV_NUMPOINTLIGHTS = "Env_NumPointLights";


        public static FrontFaceDirection FrontFace = FrontFaceDirection.Ccw;
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
            AddVertexProgram(Util.ShaderUtil.FormatShaderIncludes(Assets.AssetManager.GetAppPath(), Util.ShaderUtil.FormatShaderVersion(Util.ShaderUtil.FormatShaderProperties(vs_code, properties))));
            AddFragmentProgram(Util.ShaderUtil.FormatShaderIncludes(Assets.AssetManager.GetAppPath(), Util.ShaderUtil.FormatShaderVersion(Util.ShaderUtil.FormatShaderProperties(fs_code, properties))));
            CompileShader();
        }

        public Shader(ShaderProperties properties, string vs_code, string fs_code, string gs_code)
        {
            this.properties = properties;
            Create();
            AddVertexProgram(Util.ShaderUtil.FormatShaderIncludes(Assets.AssetManager.GetAppPath(), Util.ShaderUtil.FormatShaderVersion(GetApexVertexHeader() + Util.ShaderUtil.FormatShaderProperties(vs_code, properties))));
            AddFragmentProgram(Util.ShaderUtil.FormatShaderIncludes(Assets.AssetManager.GetAppPath(), Util.ShaderUtil.FormatShaderVersion(Util.ShaderUtil.FormatShaderProperties(fs_code, properties))));
            AddGeometryProgram(Util.ShaderUtil.FormatShaderIncludes(Assets.AssetManager.GetAppPath(), Util.ShaderUtil.FormatShaderVersion(Util.ShaderUtil.FormatShaderProperties(gs_code, properties))));
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

        public virtual void End()
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

        public virtual void ApplyMaterial(Material material)
        {
            currentMaterial = material;
            if (currentMaterial.GetBool(Material.MATERIAL_DEPTHTEST))
                GL.Enable(EnableCap.DepthTest);
            else
                GL.Disable(EnableCap.DepthTest);
            if (currentMaterial.GetBool(Material.MATERIAL_DEPTHMASK))
                GL.DepthMask(true);
            else
                GL.DepthMask(false);
            if (currentMaterial.GetBool(Material.MATERIAL_CULLENABLED))
            {
                GL.Enable(EnableCap.CullFace);
                int i = currentMaterial.GetInt(Material.MATERIAL_FACETOCULL);
                if (i == 0)
                    GL.CullFace(CullFaceMode.Back);
                else if (i == 1)
                    GL.CullFace(CullFaceMode.Front);
            }
            else
                GL.Disable(EnableCap.CullFace);
            SetUniform(MATERIAL_ALPHADISCARD, currentMaterial.GetFloat(Material.MATERIAL_ALPHADISCARD));
        }

        public void Render(Mesh mesh)
        {
            mesh.Render();
        }

        public virtual void Update(Environment environment, Camera cam, Mesh mesh)
        {
            SetDefaultValues();
            SetUniform(APEX_WORLDMATRIX, worldMatrix);
            SetUniform(APEX_VIEWMATRIX, viewMatrix);
            SetUniform(APEX_PROJECTIONMATRIX, projectionMatrix);
            SetUniform(APEX_CAMERAPOSITION, cam.Translation);
            SetUniform(APEX_CAMERADIRECTION, cam.Direction);
        }

        private static void SetDefaultValues()
        {
            GL.Enable(EnableCap.DepthClamp);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.FrontFace(FrontFace);
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
            int status = -1;
            string info = "";
            GL.GetShaderInfoLog(shader, out info);
            GL.GetShader(shader, ShaderParameter.CompileStatus, out status);
            if (status != 1)
            {
                Console.WriteLine("Shader compiler error!\nType: " + type.ToString() + "\nName: " + this.GetType().ToString() + "\n\n"/* + "Source code:\n" + code + "\n\n"*/ + 
                           info + "\n" + "Status Code: " + status.ToString());
            }
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

        public void SetUniform(string name, Vector2f[] vec)
        {
            for (int i = 0; i < vec.Length; i++)
                SetUniform(name + "[" + i.ToString() + "]", vec[i].x, vec[i].y);
        }

        public void SetUniform(string name, Vector3f vec)
        {
            SetUniform(name, vec.x, vec.y, vec.z);
        }

        public void SetUniform(string name, Vector3f[] vec)
        {
            for (int i = 0; i < vec.Length; i++)
                SetUniform(name + "[" + i.ToString() + "]", vec[i].x, vec[i].y, vec[i].z);
        }

        public void SetUniform(string name, Vector4f vec)
        {
            SetUniform(name, vec.x, vec.y, vec.z, vec.w);
        }

        public void SetUniform(string name, Vector4f[] vec)
        {
            for (int i = 0; i < vec.Length; i++)
                SetUniform(name + "[" + i.ToString() + "]", vec[i].x, vec[i].y, vec[i].z, vec[i].w);
        }

        public void SetUniform(string name, Matrix4f mat)
        {
            int loc = GL.GetUniformLocation(id, name);
            GL.UniformMatrix4(loc, 1, true, mat.values);
        }
    }
}