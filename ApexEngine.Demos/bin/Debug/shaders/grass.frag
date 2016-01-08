#version 330

#ifdef DEFAULT

#include <lighting>
#include <apex3d>
#include <material>
#include <shadows>

varying vec2 v_texCoord0;
varying vec4 v_normal;
varying vec4 v_position;
varying vec3 v_tangent;
varying vec3 v_bitangent;
varying mat3 v_TBN;
varying vec3 v_lightVec;
varying vec3 v_parallaxView;

uniform float u_fadeStart;
uniform float u_fadeEnd;

varying vec3 refVec;

varying vec4 v_ambient;
varying vec4 v_diffuse;
varying vec4 v_specular;

void main()
{
	vec4 diffuseTexture = vec4(1.0);
	vec2 texCoord = vec2(v_texCoord0.x, -v_texCoord0.y);
	
	if (Material_HasHeightMap == 1)
	{
		vec4 parTex = texture2D(Material_HeightMap, texCoord);
		texCoord = ParallaxTexCoords(parTex, texCoord, normalize(v_parallaxView));
	}
	
	if (Material_HasDiffuseMap == 1)
	{
		diffuseTexture = pow(texture2D(Material_DiffuseMap, texCoord), vec4(2.2, 2.2, 2.2, 1.0));
		if (diffuseTexture.a < Material_AlphaDiscard)
		{
			discard;
		}
	}
	
	if (Material_PerPixelLighting == 1)
	{
		vec3 shadowColor = vec3(1.0);
		float shadowness = 1.0;
		
		vec3 n = normalize(v_normal.xyz);
		vec3 v = normalize(Apex_CameraPosition - v_position.xyz);
		vec3 l = normalize(Env_DirectionalLight.direction);
		
		if (Material_HasNormalMap == 1)
		{
		    vec3 normalsTexture;
			normalsTexture.xy = 2.0 * (vec2(1.0) - texture2D(Material_NormalMap, texCoord).rg) - 1.0;
			normalsTexture.z = sqrt(1.0 - dot(normalsTexture.xy, normalsTexture.xy));
			n = normalize((v_tangent * normalsTexture.x) + (v_bitangent * normalsTexture.y) + (n * normalsTexture.z));
		}
		float ndotl = clamp(LambertDirectional(n, l), 0.0, 1.0);
		float specularity;
		
		if (Env_ShadowsEnabled == 1)
		{
			const float radius = 0.035;
			int index = GetShadowMapSplit(Apex_CameraPosition, v_position.xyz);
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					vec2 offset = poisson16[x*4+y]*radius;
					vec3 shadowCoord = GetShadowCoord(index, v_position.xyz+vec3(offset, (offset.y+offset.x)*0.50));
					shadowness += GetShadowness(index, shadowCoord);
				}
			}
			shadowness /= 16.0;
			shadowColor = vec3(shadowness);
			shadowColor = mix(vec3(0.5), shadowColor, shadowness);
			
			shadowColor = CalculateFog(shadowColor, vec3(1.0), v_position.xyz, Apex_CameraPosition, Env_ShadowMapSplits[2], Env_ShadowMapSplits[3]);
		}
		
		if (Material_SpecularTechnique == 0)
		{
			specularity = BlinnPhongDirectional(n, v_position.xyz, Apex_CameraPosition, l, Material_SpecularExponent);
		}
		else if (Material_SpecularTechnique == 1)
		{
			specularity = SpecularDirectional(n, v_position.xyz, Apex_CameraPosition, l, Material_SpecularExponent, Material_Roughness);
		}
		
		float shineAmt = Material_Shininess;
		
		vec3 surfaceColor = Material_DiffuseColor.xyz * diffuseTexture.xyz;
		
		vec3 diffuseLight = Env_DirectionalLight.color.xyz * shadowColor;
		vec3 diffuse = diffuseLight * surfaceColor;
		
		vec3 ambient = surfaceColor * (Material_AmbientColor.xyz + Env_AmbientLight.color.xyz);
		
		
		diffuse = clamp(diffuse, vec3(0.0), vec3(1.0));
		

		vec4 lightSum = vec4(ambient + diffuse, diffuseTexture.a);
		lightSum = CalculateFog(lightSum, Env_FogColor, v_position.xyz, Apex_CameraPosition, Env_FogStart, Env_FogEnd);
		gl_FragColor = lightSum;
		
		
	}
	else
	{
		vec4 lightSum = v_ambient + (v_diffuse * diffuseTexture) + v_specular;
		lightSum = CalculateFog(lightSum, Env_FogColor, v_position.xyz, Apex_CameraPosition, Env_FogStart, Env_FogEnd);
		gl_FragColor = lightSum;
	}
	gl_FragColor.rgb = pow(gl_FragColor.rgb, vec3(1.0/2.2));
	gl_FragColor.a *= 1.0 - CalculateFog(v_position.xyz, Apex_CameraPosition, u_fadeStart, u_fadeEnd);
}

#endif

#ifdef NORMALS

#include <material>

varying vec4 v_normal;
varying vec2 v_texCoord0;

void main()
{
	vec2 texCoord = vec2(v_texCoord0.x, -v_texCoord0.y);
	if (Material_HasDiffuseMap == 1)
	{
		vec4 diffuseTexture;
		diffuseTexture = pow(texture2D(Material_DiffuseMap, texCoord), vec4(2.2, 2.2, 2.2, 1.0));
		if (diffuseTexture.a < 0.4)
		{
			discard;
		}
	}

	gl_FragColor = vec4(v_normal.xyz*vec3(0.5)+vec3(0.5), 1.0);
}

#endif


#ifdef DEPTH

vec4 pack_depth(const in float depth)
{
	vec4 bit_shift =
		vec4(256.0*256.0*256.0, 256.0*256.0, 256.0, 1.0);
    vec4 bit_mask =
        vec4(0.0, 1.0/256.0, 1.0/256.0, 1.0/256.0);
        vec4 res = fract(depth * bit_shift);
    res -= res.xxyz * bit_mask;
    return res;
}
void main()
{
	gl_FragColor = pack_depth(gl_FragCoord.z);
}


#endif