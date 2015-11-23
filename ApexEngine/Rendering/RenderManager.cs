using ApexEngine.Rendering.PostProcess;
using ApexEngine.Scene;
using ApexEngine.Scene.Components;
using ApexEngine.Math;
using System.Collections.Generic;
using System;
#if OPENGL
using OpenTK.Graphics.OpenGL;
#else
using OpenTK.Graphics.ES20;
#endif

namespace ApexEngine.Rendering
{
    public class RenderManager
    {
        public enum DepthRenderMode
        {
            Depth,
            Shadow
        };

        protected List<Geometry> geometries = new List<Geometry>();
        protected List<RenderComponent> components = new List<RenderComponent>();
        private static float elapsedTime = 0f;
        private PostProcessor postProcessor;
        private Framebuffer depthFbo;
        private Texture depthTexture;
        private Vector4f backgroundColor = new Vector4f(0.39f, 0.58f, 0.93f, 1.0f);
        private Action userRender;

        public static Renderer renderer;

        public enum Bucket
        {
            Opaque,
            Transparent,
            Sky,
            Particle
        };

        public RenderManager(Renderer renderer, Camera cam, Action userRender)
        {
            RenderManager.renderer = renderer;
            this.userRender = userRender;
            postProcessor = new PostProcessor(this, cam);
            depthFbo = new Framebuffer(cam.Width, cam.Height);
        }

        public Vector4f BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor.Set(value); }
        }

        public PostProcessor PostProcessor
        {
            get { return postProcessor; }
        }

        public Texture DepthTexture
        {
            get { return depthTexture; }
        }

        public List<RenderComponent> RenderComponents
        {
            get { return components; }
        }

        public List<Geometry> GeometryList
        {
            get { return geometries; }
        }

        public void Init()
        {
            postProcessor.Init();
            depthFbo.Init();
        }

        public void AddComponent(RenderComponent cmp)
        {
            components.Add(cmp);
            cmp.renderManager = this;
            cmp.Init();
        }

        public void RemoveComponent(RenderComponent cmp)
        {
            components.Remove(cmp);
            cmp.renderManager = null;
        }

        public static float ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }

        public void AddGeometry(Geometry geom)
        {
            if (!geometries.Contains(geom))
                geometries.Add(geom);
        }

        public void RemoveGeometry(Geometry geom)
        {
            if (geometries.Contains(geom))
                geometries.Remove(geom);
        }

        public void SaveScreenToTexture(Camera cam, Texture toSaveTo)
        {
            toSaveTo.Use();

            renderer.CopyTexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, 0, 0, cam.Width, cam.Height);

            Texture.Clear();
        }

        public void RenderDepthTexture(Environment env, Camera cam)
        {
            if (depthFbo.Width != cam.Width || depthFbo.Height != cam.Height)
            {
                depthFbo.Width = cam.Width;
                depthFbo.Height = cam.Height;
                depthFbo.Init();
            }
            depthFbo.Capture();

            renderer.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderBucketDepth(env, cam, Bucket.Opaque, DepthRenderMode.Depth);
            RenderBucketDepth(env, cam, Bucket.Transparent, DepthRenderMode.Depth);

            depthFbo.Release();
            depthTexture = depthFbo.ColorTexture;
        }

        public void RenderBucket(Environment env, Camera cam, Bucket bucket)
        {
            for (int i = 0; i < geometries.Count; i++)
            {
                if (geometries[i].AttachedToRoot && geometries[i].Bucket == bucket)
                {
                    geometries[i].Render(env, cam);
                }
            }
        }

        public void RenderBucketDepth(Environment env, Camera cam, Bucket bucket, DepthRenderMode renderMode)
        {
            for (int i = 0; i < geometries.Count; i++)
            {
                if (geometries[i].AttachedToRoot && geometries[i].Bucket == bucket)
                {
                    if (geometries[i].DepthShader == null)
                        geometries[i].DepthShader = ShaderManager.GetShader(typeof(Shaders.DepthShader));
                    if (renderMode == DepthRenderMode.Shadow && geometries[i].Material.GetBool(Material.MATERIAL_CASTSHADOWS))
                    {
                        geometries[i].DepthShader.Use();
                        geometries[i].DepthShader.ApplyMaterial(geometries[i].Material);
                        geometries[i].DepthShader.SetTransforms(geometries[i].GetWorldMatrix(), cam.ViewMatrix, cam.ProjectionMatrix);
                        geometries[i].DepthShader.Update(env, cam, geometries[i].Mesh);
                        geometries[i].DepthShader.Render(geometries[i].Mesh);
                        geometries[i].DepthShader.End();
                    }

                    Shader.Clear();
                    Texture.Clear();
                }
            }
        }

        public void Render(Environment env, Camera cam)
        {
            renderer.Viewport(0, 0, cam.Width, cam.Height);
            renderer.ClearColor(backgroundColor.x, backgroundColor.y, backgroundColor.z, backgroundColor.w);
            renderer.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (RenderComponent rc in components)
            {
                rc.Render();
                rc.Update();
            }
            postProcessor.Capture();

            RenderBucket(env, cam, Bucket.Sky);
            RenderBucket(env, cam, Bucket.Opaque);
            RenderBucket(env, cam, Bucket.Transparent);
            RenderBucket(env, cam, Bucket.Particle);
            userRender();

            postProcessor.Release();
        }
    }
}