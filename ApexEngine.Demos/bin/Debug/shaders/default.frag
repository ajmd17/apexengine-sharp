#version 150

#include <lighting>
#include <apex3d>
#include <material>
#include <shadows>

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
	vec4 diffuseTexture = vec4(1.0);
	vec2 texCoord = vec2(v_texCoord0.x, -v_texCoord0.y);
	
	if (Material_HasHeightMap == 1)
	{
		vec4 parTex = texture2D(Material_HeightMap, texCoord);
		texCoord = ParallaxTexCoords(parTex, texCoord, normalize(v_parallaxView));
	}
	
	if (Material_HasDiffuseMap == 1)
	{
		diffuseTexture = texture2D(Material_DiffuseMap, texCoord);
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
		
		if (Material_HasNormalMap == 1)
		{
		    vec3 normalsTexture;
			normalsTexture.xy = (2.0 * (vec2(1.0) - texture2D(Material_NormalMap, texCoord).rg) - 1.0);
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
		
		vec3 ambient = Env_AmbientLight.color.xyz + Material_AmbientColor.xyz;
		vec3 diffuseLight = (ambient + vec3(ndotl)) * Env_DirectionalLight.color.xyz;
		diffuseLight *= shadowColor;
		
		
		vec3 diffuse = diffuseLight;
		vec3 specular = vec3(specularity) * Env_DirectionalLight.color.xyz;
		specular *= shadowness;
		
		
		for (int i = 0; i < Env_NumPointLights; i++)
		{
			PointLight pl = Env_PointLights[i];
			vec3 pl_dir = normalize(pl.position - v_position.xyz);
			float p_ndotl = max(dot(n, pl_dir), 0.0);
			diffuse += (ambient + vec3(p_ndotl)) * pl.color.xyz;
			
			float pl_specularity;
			pl_specularity = BlinnPhongDirectional(n, v_position.xyz, Apex_CameraPosition, pl_dir, Material_Shininess);
			vec3 pl_spec = vec3(pl_specularity) * pl.color.xyz;
			specular += pl_spec;
		}	
		
		diffuse *= Material_DiffuseColor.xyz;
		specular *= Material_SpecularColor.xyz;

		vec4 lightSum = vec4((diffuse*diffuseTexture.xyz) + specular, 1.0);
		lightSum = CalculateFog(lightSum, Env_AmbientLight.color, v_position.xyz, Apex_CameraPosition, Env_FogStart, Env_FogEnd);
		
		gl_FragColor = lightSum;
	}
	else
	{
		vec4 lightSum = v_diffuse * diffuseTexture + v_specular;
		lightSum = CalculateFog(lightSum, Env_AmbientLight.color, v_position.xyz, Apex_CameraPosition, Env_FogStart, Env_FogEnd);
		gl_FragColor = lightSum;
	}
}