using ApexEngine.Math;

namespace ApexEngine.Rendering
{
    public class Shader
    {
        public const string A_POSITION = "a_position";
        public const string A_TEXCOORD0 = "a_texcoord0";
        public const string A_TEXCOORD1 = "a_texcoord1";
        public const string A_NORMAL = "a_normal";
        public const string A_TANGENT = "a_tangent";
        public const string A_BITANGENT = "a_bitangent";
        public const string A_BONEWEIGHT = "a_boneweights";
        public const string A_BONEINDEX = "a_boneindices";

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

        protected Material currentMaterial = null;
        protected ShaderProperties properties;
        protected Matrix4f worldMatrix, viewMatrix, projectionMatrix;
        protected int id = 0;

        public enum ShaderTypes
        {
            Vertex,
            Fragment,
            Geometry,
            TessEval,
            TessControl
        }

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
            id = RenderManager.Renderer.GenerateShaderProgram();
        }

        public void Use()
        {
            RenderManager.Renderer.BindShaderProgram(id);
        }

        public virtual void End()
        {
            currentMaterial = null;

            RenderManager.Renderer.SetDepthTest(true);
            RenderManager.Renderer.SetDepthMask(true);
        }

        public static void Clear()
        {
            RenderManager.Renderer.BindShaderProgram(0);
        }

        public void CompileShader()
        {
            Use();
            RenderManager.Renderer.CompileShaderProgram(id);
        }

        public virtual void ApplyMaterial(Material material)
        {
            currentMaterial = material;

            RenderManager.Renderer.SetDepthTest(currentMaterial.GetBool(Material.MATERIAL_DEPTHTEST));
            RenderManager.Renderer.SetDepthMask(currentMaterial.GetBool(Material.MATERIAL_DEPTHMASK));


            if (currentMaterial.GetBool(Material.MATERIAL_CULLENABLED))
            {
                RenderManager.Renderer.SetCullFace(true);
                int i = currentMaterial.GetInt(Material.MATERIAL_FACETOCULL);
                if (i == 0)
                    RenderManager.Renderer.SetFaceToCull(Renderer.Face.Back);
                else if (i == 1)
                    RenderManager.Renderer.SetFaceToCull(Renderer.Face.Front);
            }
            else
                RenderManager.Renderer.SetCullFace(false);

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
            RenderManager.Renderer.SetDepthClamp(true);
            RenderManager.Renderer.SetFaceDirection(Renderer.FaceDirection.Ccw);
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
            AddProgram(vs, ShaderTypes.Vertex);
        }

        public void AddFragmentProgram(string fs)
        {
            AddProgram(fs, ShaderTypes.Fragment);
        }

        public void AddGeometryProgram(string gs)
        {
            AddProgram(gs, ShaderTypes.Geometry);
        }

        public void AddProgram(string code, ShaderTypes type)
        {
            RenderManager.Renderer.AddShader(id, code, type);
        }

        public void SetUniform(string name, int i)
        {
            RenderManager.Renderer.SetShaderUniform(id, name, i);
        }

        public void SetUniform(string name, float f)
        {
            RenderManager.Renderer.SetShaderUniform(id, name, f);
        }

        public void SetUniform(string name, float x, float y)
        {
            RenderManager.Renderer.SetShaderUniform(id, name, x, y);
        }

        public void SetUniform(string name, float x, float y, float z)
        {
            RenderManager.Renderer.SetShaderUniform(id, name, x, y, z);
        }

        public void SetUniform(string name, float x, float y, float z, float w)
        {
            RenderManager.Renderer.SetShaderUniform(id, name, x, y, z, w);
        }

        public void SetUniform(string name, Vector2f vec)
        {
            SetUniform(name, vec.X, vec.Y);
        }

        public void SetUniform(string name, Vector2f[] vec)
        {
            for (int i = 0; i < vec.Length; i++)
                SetUniform(name + "[" + i.ToString() + "]", vec[i].X, vec[i].Y);
        }

        public void SetUniform(string name, Vector3f vec)
        {
            SetUniform(name, vec.X, vec.Y, vec.Z);
        }

        public void SetUniform(string name, Vector3f[] vec)
        {
            for (int i = 0; i < vec.Length; i++)
                SetUniform(name + "[" + i.ToString() + "]", vec[i].X, vec[i].Y, vec[i].Z);
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
            RenderManager.Renderer.SetShaderUniform(id, name, mat.values);
        }
    }
}