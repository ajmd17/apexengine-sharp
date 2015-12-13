
namespace ApexEngine.Rendering
{
    public abstract class Renderer
    {
        public enum Face
        {
            Back,
            Front
        }

        public enum FaceDirection
        {
            Cw,
            Ccw
        }

        public enum BlendMode
        {
            AlphaBlend
        }

        public enum BufferBit : int
        {
            Depth = 0x00000100,
            Accum = 0x00000200,
            Stencil = 0x00000400,
            Color = 0x00004000,
            Coverage = 0x00008000
        }

        /// <summary>
        /// Creates a render context for a game. Allows the game to actually render.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public abstract void CreateContext(Game game, int width, int height);

        #region Mesh stuff

        /// <summary>
        /// Generate required buffers for the mesh
        /// </summary>
        /// <param name="mesh"></param>
        public abstract void CreateMesh(Mesh mesh);

        /// <summary>
        /// Upload mesh data to the GPU
        /// </summary>
        /// <param name="mesh"></param>
        public abstract void UploadMesh(Mesh mesh);

        /// <summary>
        /// Render the mesh
        /// </summary>
        /// <param name="mesh"></param>
        public abstract void RenderMesh(Mesh mesh);

        #endregion Mesh stuff

        #region Texture stuff

        /// <summary>
        /// Load a 2D texture
        /// </summary>
        /// <param name="n"></param>
        /// <param name="textures"></param>
        public abstract Texture2D LoadTexture2D(string path);

        /// <summary>
        /// Load a cubemap texture from 6 texture paths
        /// </summary>
        /// <param name="filepaths"></param>
        /// <returns></returns>
        public abstract Cubemap LoadCubemap(string[] filepaths);

        public abstract int GenTexture();

        public abstract void GenTextures(int n, out int textures);

        public abstract void BindTexture2D(int i);

        public abstract void BindTexture3D(int i);

        public abstract void BindTextureCubemap(int i);

        public abstract void TextureWrap2D(Texture.WrapMode s, Texture.WrapMode t);

        public abstract void TextureWrapCube(Texture.WrapMode r, Texture.WrapMode s, Texture.WrapMode t);

        public abstract void TextureFilter2D(int min, int mag);

        public abstract void GenerateMipmap2D();

        public abstract void GenerateMipmapCubemap();

        public abstract void ActiveTextureSlot(int slot);

        #endregion Texture stuff

        #region Shader stuff

        public abstract int GenerateShaderProgram();

        public abstract void BindShaderProgram(int id);

        public abstract void CompileShaderProgram(int id);

        public abstract void AddShader(int id, string code, Shader.ShaderTypes type);

        public abstract void SetShaderUniform(int id, string name, int i);

        public abstract void SetShaderUniform(int id, string name, float i);

        public abstract void SetShaderUniform(int id, string name, float x, float y);

        public abstract void SetShaderUniform(int id, string name, float x, float y, float z);

        public abstract void SetShaderUniform(int id, string name, float x, float y, float z, float w);

        public abstract void SetShaderUniform(int id, string name, float[] matrix);

        #endregion Shader stuff

        #region Framebuffer stuff

        public abstract void GenFramebuffers(int n, out int framebuffers);

        public abstract void SetupFramebuffer(int framebufferID, int colorTextureID, int depthTextureID, int width, int height);

        public abstract void BindFramebuffer(int id);

        #endregion Framebuffer stuff

        #region Enable/disable stuff

        public abstract void SetDepthTest(bool depthTest);

        public abstract void SetDepthMask(bool depthMask);

        public abstract void SetDepthClamp(bool depthClamp);

        public abstract void SetBlend(bool blend);

        public abstract void SetBlendMode(BlendMode blendMode);

        public abstract void SetCullFace(bool cullFace);

        public abstract void SetFaceToCull(Face face);

        public abstract void SetFaceDirection(FaceDirection faceDirection);

        #endregion Enable/disable stuff

        #region Rendering stuff

        public abstract void Viewport(int x, int y, int width, int height);

        public abstract void Clear(bool clearColor, bool clearDepth, bool clearStencil);

        public abstract void DrawVertex(float x, float y);

        public abstract void DrawVertex(float x, float y, float z);

        #endregion Rendering stuff

        public abstract void ClearColor(float r, float g, float b, float a);

        public abstract void CopyScreenToTexture2D(int width, int height);
    }
}