using ApexEngine.Assets;
using ApexEngine.Math;
using ApexEngine.Rendering;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApexEngine.Plugins.Skydome
{
    public class SkyShader : Shader
    {
        private static Assets.ShaderTextLoader textLoader = Assets.ShaderTextLoader.GetInstance();

        private const float PI = 3.141592654f;

        private readonly Color4f sunColor = new Color4f(0.1f, 0.1f, 0.1f, 1.0f);
        private int nSamples;           // Number of sample rays to use in integral equation
        private float fSamples;         // float version of the above
        private float Kr;               // Rayleigh scattering constant
        private float KrESun, Kr4PI;    // Kr * ESun, Kr * 4 * PI
        private float Km;               // Mie scattering constant
        private float KmESun, Km4PI;    // Km * ESun, Km * 4 * PI
        private float ESun;             // Sun brightness constant
        private float G;                // The Mie phase asymmetry factor
        private float innerRadius = 100f;      // Ground radius (outer radius is always 1.025 * innerRadius)
        private float scale;            // 1 / (outerRadius - innerRadius)
        private float scaleDepth;       // The scale depth (i.e. the altitude at which the atmosphere's average density is found)
        private float scaleOverScaleDepth; // scale / scaleDepth

        private Vector3f wavelength = new Vector3f();
        private Vector3f invWavelength4 = new Vector3f(); // 1 / pow(wavelength, 4) for the red, green, and blue channels
        private Vector3f sunDirNor = new Vector3f();
        private Vector3f skydomeScale = new Vector3f(100f);
        private float exposure;

        private Transform skyTransform = new Transform();

        public SkyShader(ShaderProperties properties)
            : base(properties, (string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\sky.vert"),
                               (string)AssetManager.Load(AppDomain.CurrentDomain.BaseDirectory + "\\shaders\\sky.frag"))
        {
            DefaultValues();
        }

        public override void ApplyMaterial(Material material)
        {
            base.ApplyMaterial(material);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        public override void End()
        {
            base.End();

            GL.Disable(EnableCap.Blend);
        }

        private void DefaultValues()
        {
            nSamples = 3;

            // values that work well
            Kr = 0.0025f;
            Km = 0.0015f;
            ESun = 10f;
            exposure = 2f;
            wavelength.Set(0.731f, 0.612f, 0.455f);

            G = -0.990f;
            invWavelength4.Set(Vector3f.Zero);
            scaleDepth = 0.25f;
            UpdateCalculations();
        }

        public void UpdateCalculations()
        {
            scale = 1.0f / ((innerRadius * 1.025f) - innerRadius);
            scaleOverScaleDepth = scale / scaleDepth;
            KrESun = Kr * ESun;
            KmESun = Km * ESun;
            Kr4PI = Kr * 4.0f * PI;
            Km4PI = Km * 4.0f * PI;

            invWavelength4.x = 1.0f / (float)System.Math.Pow(wavelength.x, 4.0f);
            invWavelength4.y = 1.0f / (float)System.Math.Pow(wavelength.y, 4.0f);
            invWavelength4.z = 1.0f / (float)System.Math.Pow(wavelength.z, 4.0f);

            fSamples = (float)nSamples;
        }

        public override void Update(Rendering.Environment environment, Camera cam, Mesh mesh)
        {
            base.Update(environment, cam, mesh);

            if (environment.DirectionalLight != null)
            {
                sunDirNor.Set(environment.DirectionalLight.Direction);
                sunDirNor.NormalizeStore();
            }

            skyTransform.SetTranslation(cam.Translation);
            skyTransform.SetScale(skydomeScale);
            
            SetUniform("u_world", skyTransform.GetMatrix());
            SetUniform("u_view", cam.ViewMatrix);
            SetUniform("u_proj", cam.ProjectionMatrix);
            SetUniform("v3LightPos", sunDirNor);
            SetUniform("v3InvWavelength", invWavelength4);
            SetUniform("fKrESun", KrESun);
            SetUniform("fKmESun", KmESun);
            SetUniform("fOuterRadius", innerRadius * 1.025f);
            SetUniform("fInnerRadius", innerRadius);
            SetUniform("fOuterRadius2", (innerRadius * 1.025f)* (innerRadius * 1.025f));
            SetUniform("fInnerRadius2", innerRadius * innerRadius);
            SetUniform("fKr4PI", Kr4PI);
            SetUniform("fKm4PI", Km4PI);
            SetUniform("fScale", scale);
            SetUniform("fScaleDepth", scaleDepth);
            SetUniform("fScaleOverScaleDepth", scaleOverScaleDepth);
            SetUniform("fSamples", fSamples);
            SetUniform("nSamples", nSamples);
            SetUniform("fg", G);
            SetUniform("fg2", G * G);
            SetUniform("fExposure", exposure);
            SetUniform("v3CameraPos", cam.Translation);
            SetUniform("fCameraHeight2", cam.Height * cam.Height);
            SetUniform("u_sunColor", this.sunColor);
        }
    }
}
