using ApexEngine.Rendering.Shaders.Post;
using System;

namespace ApexEngine.Rendering.PostProcess
{
    public abstract class PostFilter
    {
        protected PostShader shader;
        protected Camera cam;
        private bool saveColorTexture = true;
        protected Texture colorTexture, depthTexture;

        public PostFilter(string fs_code) : this(new PostShader(fs_code))
        {
        }

        public PostFilter(Shader shader)
        {
            if (shader is PostShader)
                this.shader = (PostShader)shader;
            else
                throw new Exception("Must be of type PostShader!");
        }

        public PostShader Shader
        {
            get { return shader; }
            set { shader = value; }
        }

        public Camera Cam
        {
            get { return cam; }
            set { cam = value; }
        }

        public bool SaveColorTexture
        {
            get { return saveColorTexture; }
            set { saveColorTexture = value; }
        }

        public Texture ColorTexture
        {
            get { return colorTexture; }
            set { colorTexture = value; }
        }

        public Texture DepthTexture
        {
            get { return depthTexture; }
            set { depthTexture = value; }
        }

        public abstract void Update();

        public abstract void End();
    }
}