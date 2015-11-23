#version 150

#include <lighting>
#include <apex3d>
#include <material>
#include <shadows>

uniform sampler2D terrainTexture0;
uniform sampler2D terrainTexture0Normal;
uniform int terrainTexture0HasNormal;
uniform float terrainTexture0Scale;

uniform sampler2D slopeTexture;
uniform float slopeScale;

varying vec2 v_texCoord0;
varying vec4 v_normal;
varying vec4 v_position;
varying vec4 v_diffuse;
varying vec4 v_specular;
varying vec3 v_tangent;
varying vec3 v_bitangent;
varying mat3 v_TBN;
varying vec3 v_lightVec;
varying vec3 v_parallaxView;

void main()
{
	vec4 diffuseTexture = vec4(1.0, 0.0, 0.0, 1.0);
	vec2 texCoord = vec2(-v_texCoord0.x, -v_texCoord0.y);
	
	vec4 texColor0 = texture2D(terrainTexture0, texCoord * terrainTexture0Scale);
	vec4 slopeColor = texture2D(slopeTexture, texCoord * slopeScale);
	
	vec3 up = vec3(0.0, 1.0, 0.0);
	float ang = max(abs(v_normal.x), 0.0);
	
	diffuseTexture = mix(texColor0, slopeColor, clamp(ang/(1.0-0.5), 0.0, 1.0));
	
	if (ang > (1.0-0.5))
	{
		diffuseTexture = slopeColor;
	}
	
	if (Material_PerPixelLighting == 1)
	{
		vec3 shadowColor = vec3(1.0);
		float shadowness = 1.0;
		
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
		
		vec3 n = normalize(v_normal.xyz);
		vec3 l = normalize(Env_DirectionalLight.direction);
		
		if (terrainTexture0HasNormal == 1)
		{
		    vec3 normalsTexture;
			normalsTexture.xy = (2.0 * (vec2(1.0) - texture2D(terrainTexture0Normal, texCoord * terrainTexture0Scale).rg) - 1.0);
			normalsTexture.z = sqrt(1.0 - dot(normalsTexture.xy, normalsTexture.xy));
			n = normalize((v_tangent * normalsTexture.x) + (v_bitangent * normalsTexture.y) + (n * normalsTexture.z));
		}
		
		float ndotl = LambertDirectional(n, l);
		float specularity;
		
		if (Material_SpecularTechnique == 0)
		{
			specularity = BlinnPhongDirectional(n, v_position.xyz, Apex_CameraPosition, l, Material_Shininess);
		}
		else if (Material_SpecularTechnique == 1)
		{
			specularity = SpecularDirectional(n, v_position.xyz, Apex_CameraPosition, l, Material_Shininess);
		}
		vec3 diffuseLight = (vec3(ndotl) + Env_AmbientLight.color.xyz + Material_AmbientColor.xyz);
		diffuseLight *= shadowColor;
		vec3 diffuse = diffuseLight * Material_DiffuseColor.xyz * Env_DirectionalLight.color.xyz;
		vec3 specular = vec3(specularity) * Material_SpecularColor.xyz * Env_DirectionalLight.color.xyz;
		specular *= shadowness;
		
		
		for (int i = 0; i < Env_NumPointLights; i++)
		{
			PointLight pl = Env_PointLights[i];
			vec3 pl_dir = normalize(pl.position - v_position.xyz);
			float p_ndotl = dot(n, pl_dir);
			diffuse += vec3(p_ndotl) * pl.color.xyz;
		}
		
		vec4 lightSum = vec4(diffuse * diffuseTexture.rgb, 1.0);
		lightSum = CalculateFog(lightSum, Env_AmbientLight.color, v_position.xyz, Apex_CameraPosition, Env_FogStart, Env_FogEnd);
		gl_FragColor = lightSum;
	}
	else
	{
		gl_FragColor = diffuseTexture;
	}
}