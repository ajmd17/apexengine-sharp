using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApexEngine.Rendering.PostProcess;
using ApexEngine.Rendering;
using ApexEngine.Math;

namespace ApexEngine.Rendering.Shadows
{
    public class ShadowPostFilter : PostFilter
    {
        const string SHADER_CODE = "#version 150\n" +
              "uniform sampler2D u_shadowMap0;\n" +
              "uniform sampler2D u_shadowMap1;\n" +
              "uniform sampler2D u_shadowMap2;\n" +
              "uniform sampler2D u_shadowMap3;\n" +
              "uniform mat4 u_shadowMapViewProjTrans0;\n" +
              "uniform mat4 u_shadowMapViewProjTrans1;\n" +
              "uniform mat4 u_shadowMapViewProjTrans2;\n" +
              "uniform mat4 u_shadowMapViewProjTrans3;\n" +
              "uniform sampler2D u_depthTexture;\n" +
              "uniform sampler2D u_sceneTexture;\n"
              + "uniform sampler2D u_noiseTexture;\n" +
              "uniform float splits[4];\n" +
              "uniform mat4 u_invViewProj;\n" +
              "uniform mat4 u_camMat;\n" +
              "varying vec2 v_texCoord0;\n" +
              "uniform mat4 u_proj;\n" +
              "uniform mat4 u_view;\n" +
              "uniform vec3 u_cameraPosition;\n"
              + "#ifdef AMBIENT_LIGHT \n"
              + "struct AmbientLight {\n" +
              "		vec4 color;\n" +
              "	};\n" +
              "	uniform AmbientLight ambientLight;\n"
              + "#endif \n"
              + "uniform int B_debugMode;\n"
              + "const vec2 poisson16[] = vec2[](    // These are the Poisson Disk Samples\n"
              + "                                vec2( -0.94201624,  -0.39906216 ),\n"
              + "                                vec2(  0.94558609,  -0.76890725 ),\n"
              + "                                vec2( -0.094184101, -0.92938870 ),\n"
              + "                                vec2(  0.34495938,   0.29387760 ),\n"
              + "                                vec2( -0.91588581,   0.45771432 ),\n"
              + "                                vec2( -0.81544232,  -0.87912464 ),\n"
              + "                                vec2( -0.38277543,   0.27676845 ),\n"
              + "                                vec2(  0.97484398,   0.75648379 ),\n"
              + "                                vec2(  0.44323325,  -0.97511554 ),\n"
              + "                                vec2(  0.53742981,  -0.47373420 ),\n"
              + "                                vec2( -0.26496911,  -0.41893023 ),\n"
              + "                                vec2(  0.79197514,   0.19090188 ),\n"
              + "                                vec2( -0.24188840,   0.99706507 ),\n"
              + "                                vec2( -0.81409955,   0.91437590 ),\n"
              + "                                vec2(  0.19984126,   0.78641367 ),\n"
              + "                                vec2(  0.14383161,  -0.14100790 )\n"
              + "                               );\n"
              + "float unpack_depth(const in vec4 rgba_depth){\n" +
              "    const vec4 bit_shift =\n" +
              "        vec4(1.0/(256.0*256.0*256.0)\n" +
              "           , 1.0/(256.0*256.0)\n" +
              "            , 1.0/256.0\n" +
              "            , 1.0);\n" +
              "    float depth = dot(rgba_depth, bit_shift);\n" +
              "    return depth;\n" +
              "}\n"
              + "vec3 getShadowCoord(int index, vec3 worldPos) {\n"
              + "     vec4 shadowPos = vec4(0., 0., 0., 0.);\n"
              + "     if (index == 0) {\n"
              + "         shadowPos = u_shadowMapViewProjTrans0 * vec4(worldPos, 1.0);\n"
              + "     } else if (index == 1) {\n"
              + "         shadowPos = u_shadowMapViewProjTrans1 * vec4(worldPos, 1.0);\n"
              + "     } else if (index == 2) {\n"
              + "         shadowPos = u_shadowMapViewProjTrans2 * vec4(worldPos, 1.0);\n"
              + "     } else {\n"
              + "         shadowPos = u_shadowMapViewProjTrans3 * vec4(worldPos, 1.0);\n"
              + "     }\n"
              + "     shadowPos *= 0.5;\n"
              + "     shadowPos += 0.5;\n"
              + "     return shadowPos.xyz;\n"
              + "}\n"
              + "vec3 getPosition(vec2 uv, float depth) {\n"
              + "     vec4 pos = vec4(uv.x * 2.0 - 1.0, (uv.y * 2.0 - 1.0), depth * 2.0 - 1.0, 1.0);\n"
              + "     pos = inverse(u_proj * u_view) * pos;\n"
              + "     pos = pos/pos.w;\n"
              + "     return pos.xyz;\n"
              + "}\n"
              + "float getShadowness(int idx, vec3 coord)\n"
              + "{\n"
              + "	float result = 1.0;\n"
              + "	float depth;\n"
              + "	if (idx == 0) {\n"
              + "		depth = texture2D(u_shadowMap0, coord.xy).r;\n"
              + "	} else if (idx == 1) {\n"
              + "		depth = texture2D(u_shadowMap1, coord.xy).r;\n"
              + "	} else if (idx == 2) {\n"
              + "		depth = texture2D(u_shadowMap2, coord.xy).r;\n"
              + "	} else {\n"
              + "		depth = texture2D(u_shadowMap3, coord.xy).r;\n"
              + "	}\n"
              + "   result = max(step(coord.z+0.00001, depth), 0.0);\n"
              + "	return result;\n"
              + "}\n"
              + "bool inRadius(vec3 cam, vec3 world, float radius) {\n" +
              "		if (cam.x >= (world.x - (radius)) && cam.x <= (world.x + (radius))) {\n" +
              "			if (cam.y >= (world.y - (radius)) && cam.y <= (world.y + (radius))) {\n" +
              "				if (cam.z >= (world.z - (radius)) && cam.z <= (world.z + (radius))) {\n" +
              "					return true;\n" +
              "             }\n	" +
              "     	}\n	" +
              "		}\n" +
              "" +
              "		return false;\n" +
              "	 }\n"
              + "float random(vec4 seed4) {\n"
              + ""
              + "	float dot_product = dot(seed4, vec4(12.9898,78.233,45.164,94.673));\n" +
              "    return fract(sin(dot_product) * 43758.5453);"
              + "}\n" +

              "void main() {\n" +
              "		vec4 color = texture2D(u_sceneTexture, v_texCoord0);\n" +
              "		float depth =  (texture2D(u_depthTexture, v_texCoord0)).r;\n" +
              "		vec3 worldPosition = getPosition(v_texCoord0, depth);\n" +

              "		vec3 camera = u_cameraPosition;//vec2(u_cameraPosition.x, u_cameraPosition.z);\n" +
              "		vec3 world = worldPosition;//vec2(worldPosition.x, worldPosition.z);\n" +

              "		" +
              "		float dist = distance(world, camera);\n" +
              "	 	vec3 splitColor;\n" +
              "		vec4 shadow;\n"

              + "  int index = 0;"
              + "  if (inRadius(camera, world, splits[0])) {\n"
              + "	index = 0;\n"
              + "   splitColor = vec3(0.0, 1.0, 0.0);\n"
              + "  } else if (inRadius(camera, world, splits[1])){\n"
              + "	index = 1;\n"
              + "   splitColor = vec3(1.0, 0.0, 0.0);\n"
              + "  } else if (inRadius(camera, world, splits[2])){\n"
              + "	index = 2;\n"
              + "   splitColor = vec3(0.0, 0.0, 1.0);\n"
              + "  } else if (inRadius(camera, world, splits[3])) {\n"
              + "	index = 3;\n"
              + "   splitColor = vec3(1.0, 1.0, 0.0);\n"
              + "  }\n" +
              "     " +
              "		float radius = 0.035;\n" +
              "		for (int x = 0; x < 4; x++) {\n" +
              "			for (int y = 0; y < 4; y++) {\n"
              + "			int idk = int(16.0*random(vec4(worldPosition, x*4+y)))%16;\n" +
              "				vec2 offset = (poisson16[x*4+y]*radius);\n" +
              "				vec3 shadowCoord = getShadowCoord(index, worldPosition+vec3(offset, (offset.x+offset.y)/2.));\n"
              + "			vec4 cshad = vec4(getShadowness(index, shadowCoord));\n"
              + "			#ifdef AMBIENT_LIGHT\n"
              + "				if (cshad.r < 1.0) {\n"
              + "					//cshad *= texture2D(u_noiseTexture, v_texCoord0).r;\n"
              + "					cshad += mix(ambientLight.color, vec4(0.5), 0.5);\n"
              + "				}\n"
              + "			#endif\n" +
              "				shadow += cshad;\n" +
              "			}\n" +
              "		}\n" +
              "		shadow /= 16.0;\n" +
              "  	float fogEnd = splits[3];\n" +
              "  	float fogStart = splits[2];\n" +
              "  	float fogFactor = (fogEnd - dist)/(fogEnd - fogStart);\n" +
              "  	fogFactor = clamp(fogFactor, 0.0, 1.0);\n" +
              "  	shadow = mix(vec4(1.0), shadow, fogFactor);\n" +
              "		shadow = max(vec4(0.5), shadow);\n" +
              "		if (depth == 1.) { shadow = vec4(1.0); splitColor = vec3(1.0, 1.0, 0.0); }\n" +
              "		if (B_debugMode == 0) {\n" +
              "			gl_FragColor = color * shadow;\n" +
              "		} else if (B_debugMode > 0) {\n" +
              "			gl_FragColor = vec4(splitColor, 1.0) * shadow;\n" +
              "		}\n" +
              "" +
              " //gl_FragColor = texture2D(u_shadowMap0, v_texCoord0);\n" +
              "}\n" +
              "" +
              "" +
              "";

        public const string SHADOWMAP_0 = "u_shadowMap0";
	    public const string SHADOWMAP_1 = "u_shadowMap1";
	    public const string SHADOWMAP_2 = "u_shadowMap2";
	    public const string SHADOWMAP_3 = "u_shadowMap3";

	    private Texture noiseMap;
        private Matrix4f camMat = new Matrix4f();
        public Texture[] shadowMaps = new Texture[4];
        public float[] splits = new float[4];
        public Matrix4f shadowMapViewProjTrans0, shadowMapViewProjTrans1, shadowMapViewProjTrans2, shadowMapViewProjTrans3;
        public Texture shadowMap0, shadowMap1, shadowMap2, shadowMap3;
        public bool debugMode = false;
        private ShadowMappingComponent parent;

        public ShadowPostFilter(ShadowMappingComponent parent) : base(SHADER_CODE)
        {
            this.parent = parent;
            noiseMap = Texture.LoadTexture(Assets.AssetManager.GetAppPath() + "\\textures\\noise.png");
        }

        public override void End()
        {
        }

        public override void Update()
        {
            this.shadowMap0 = parent.ShadowCams[0].ShadowMap;
            this.shadowMap1 = parent.ShadowCams[1].ShadowMap;
            this.shadowMap2 = parent.ShadowCams[2].ShadowMap;
            this.shadowMap3 = parent.ShadowCams[3].ShadowMap;
            this.shadowMapViewProjTrans0 = parent.ShadowCams[0].ViewProjectionMatrix;
            this.shadowMapViewProjTrans1 = parent.ShadowCams[1].ViewProjectionMatrix;
            this.shadowMapViewProjTrans2 = parent.ShadowCams[2].ViewProjectionMatrix;
            this.shadowMapViewProjTrans3 = parent.ShadowCams[3].ViewProjectionMatrix;
            this.splits = parent.Splits;

            camMat.SetToLookAt(cam.Translation, cam.Translation.Add(new Vector3f(0, 0, -1)), cam.Up);
            camMat.MultiplyStore(cam.ProjectionMatrix);
            shader.SetUniform("u_camMat", camMat);
            Texture.ActiveTextureSlot(0);
            shadowMap0.Use();
            Texture.ActiveTextureSlot(1);
            shadowMap1.Use();
            Texture.ActiveTextureSlot(2);
            shadowMap2.Use();
            Texture.ActiveTextureSlot(3);
            shadowMap3.Use();
            Texture.ActiveTextureSlot(4);
            depthTexture.Use();
            Texture.ActiveTextureSlot(5);
            colorTexture.Use();
            Texture.ActiveTextureSlot(6);
            noiseMap.Use();

            shader.SetUniform("u_shadowMap0", 0);
            shader.SetUniform("u_shadowMap1", 1);
            shader.SetUniform("u_shadowMap2", 2);
            shader.SetUniform("u_shadowMap3", 3);
            shader.SetUniform("u_depthTexture", 4);
            shader.SetUniform("u_sceneTexture", 5);
            shader.SetUniform("u_noiseTexture", 6);
            shader.SetUniform("u_shadowMapViewProjTrans0", shadowMapViewProjTrans0);
            shader.SetUniform("u_shadowMapViewProjTrans1", shadowMapViewProjTrans1);
            shader.SetUniform("u_shadowMapViewProjTrans2", shadowMapViewProjTrans2);
            shader.SetUniform("u_shadowMapViewProjTrans3", shadowMapViewProjTrans3);
            shader.SetUniform("u_cameraPosition", cam.Translation);
            shader.SetUniform("u_invViewProj", cam.InverseViewProjectionMatrix);
            shader.SetUniform("u_view", cam.ViewMatrix);
            shader.SetUniform("u_proj", cam.ProjectionMatrix);

            if (debugMode)
            {
                shader.SetUniform("B_debugMode", 1);
            }
            else
            {
                shader.SetUniform("B_debugMode", 0);
            }
            for (int i = 0; i < splits.Length; i++)
            {
                shader.SetUniform("splits[" + i + "]", splits[i]);
            }
        }
    }
}
