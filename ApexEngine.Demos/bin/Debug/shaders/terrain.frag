#version 330

#ifdef DEFAULT

#include <lighting>
#include <apex3d>
#include <material>
#include <shadows>

uniform sampler2D terrainTexture0;
uniform sampler2D terrainTexture0Normal;
uniform int terrainTexture0HasNormal;
uniform float terrainTexture0Scale;

uniform sampler2D splatTexture;
uniform int hasSplatMap;

uniform sampler2D slopeTexture;
uniform sampler2D slopeTextureNormal;
uniform int slopeTextureHasNormal;
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
	
	float tex0Strength = 0.0;
	vec4 texColor0 = pow(texture2D(terrainTexture0, texCoord * terrainTexture0Scale), vec4(2.2, 2.2, 2.2, 1.0));
	vec4 texColor1 = vec4(0.9); // TODO: snow
	vec4 texColor2;
	vec4 texColor3;
	
	vec4 slopeColor = pow(texture2D(slopeTexture, texCoord * slopeScale), vec4(2.2, 2.2, 2.2, 1.0));
	
	
	vec3 up = vec3(0.0, 1.0, 0.0);
	float ang = max(abs(v_normal.x), 0.0);
	
	tex0Strength = 1.0-clamp(ang/(1.0-0.65), 0.0, 1.0);
	
	vec4 flatColor;
	flatColor = texColor0;
	
	if (hasSplatMap == 1)
	{
		// texColor1 = red
		// texColor2 = green
		// texColor3 = blue;
		
		vec4 splatColor = texture2D(splatTexture, texCoord);
		
		
	//	flatColor = mix(flatColor, texColor2, splatColor.g);
	//	flatColor = mix(flatColor, texColor3, splatColor.b);
	}
	
	flatColor = mix(flatColor, texColor1, clamp( v_position.y / 16.0, 0.0, 1.0));
	
	diffuseTexture = mix(slopeColor, flatColor, tex0Strength);
	
	
	
	if (ang >= (1.0-0.65))
	{
		diffuseTexture = slopeColor;
	}
	
	if (Material_PerPixelLighting == 1)
	{
		vec3 shadowColor = vec3(1.0);
		float shadowness = 1.0;
		
		vec3 n = normalize(v_normal.xyz);
		vec3 l = normalize(Env_DirectionalLight.direction);
		
		if (terrainTexture0HasNormal == 1)
		{
		    vec3 normalsTexture;
			normalsTexture.xy = (2.0 * (vec2(1.0) - texture2D(terrainTexture0Normal, texCoord * terrainTexture0Scale).rg) - 1.0);
			normalsTexture.z = sqrt(1.0 - dot(normalsTexture.xy, normalsTexture.xy));
			n = mix(n, normalize((v_tangent * normalsTexture.x) + (v_bitangent * normalsTexture.y) + (n * normalsTexture.z)), tex0Strength);
		}
		
		if (slopeTextureHasNormal == 1)
		{
		    vec3 normalsTexture;
			normalsTexture.xy = (2.0 * (vec2(1.0) - texture2D(slopeTextureNormal, texCoord * slopeScale).rg) - 1.0);
			normalsTexture.z = sqrt(1.0 - dot(normalsTexture.xy, normalsTexture.xy));
			n = mix(n, normalize((v_tangent * normalsTexture.x) + (v_bitangent * normalsTexture.y) + (n * normalsTexture.z)), 1.0-tex0Strength);
		}
		
		float ndotl = LambertDirectional(n, l);
		float specularity;
		
		if (Env_ShadowsEnabled == 1)
		{
			const float radius = 0.05;
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
		//	shadowColor = mix(vec3(0.5), shadowColor, shadowness);
			
			shadowColor = CalculateFogLinear(shadowColor, vec3(1.0), v_position.xyz, Apex_CameraPosition, Env_ShadowMapSplits[2], Env_ShadowMapSplits[3]);
		}
		
		if (Material_SpecularTechnique == 0)
		{
			specularity = BlinnPhongDirectional(n, v_position.xyz, Apex_CameraPosition, l, Material_Shininess);
		}
		else if (Material_SpecularTechnique == 1)
		{
			specularity = SpecularDirectional(n, v_position.xyz, Apex_CameraPosition, l, Material_Shininess, Material_Roughness);
		}
		
		float shineAmt = Material_Shininess;
		
		vec3 surfaceColor = Material_DiffuseColor.xyz * diffuseTexture.xyz;
		
		vec3 diffuseLight = vec3(ndotl) * Env_DirectionalLight.color.xyz * shadowColor;
		vec3 diffuse = diffuseLight * surfaceColor;
		
		vec3 ambient = surfaceColor * (Material_AmbientColor.xyz + Env_AmbientLight.color.xyz);
		
		vec3 specular = vec3(specularity);
		
		specular *= shadowness;
		
		vec3 reflection;
		float fresnel = Fresnel(n, v_position.xyz, Apex_CameraPosition, l, Material_Roughness);
		fresnel = pow(fresnel, 5.0);
		reflection = vec3(fresnel);
		specular += reflection;
		specular *= Env_DirectionalLight.color.xyz;
		
		for (int i = 0; i < Env_NumPointLights; i++)
		{
			PointLight pl = Env_PointLights[i];
			vec3 pl_dir = normalize(pl.position - v_position.xyz);
			float p_ndotl = max(dot(n, pl_dir), 0.0);
			diffuse += (vec3(p_ndotl)) * pl.color.xyz;
			
			float pl_specularity = BlinnPhongDirectional(n, v_position.xyz, Apex_CameraPosition, pl_dir, Material_SpecularExponent);
			vec3 pl_spec = vec3(pl_specularity) * pl.color.xyz;
			specular += pl_spec;
		}	
		
		
		diffuse = clamp(diffuse, vec3(0.0), vec3(1.0));
		specular = clamp(specular, vec3(0.0), vec3(1.0));

		diffuse = mix(diffuse, vec3(0.0), Material_Shininess);
		specular *= Material_Shininess;

		vec4 lightSum = vec4(ambient + diffuse + specular, 1.0);
		
		lightSum = CalculateFog(lightSum, Env_FogColor, v_position.xyz, Apex_CameraPosition, Env_FogStart, Env_FogEnd);
		gl_FragColor = lightSum;
	}
	else
	{
		gl_FragColor = diffuseTexture;
	}
	gl_FragColor.rgb = pow(gl_FragColor.rgb, vec3(1.0/2.2));
	gl_FragColor.a = 1.0;
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
		if (diffuseTexture.a < Material_AlphaDiscard)
		{
			discard;
		}
	}

	gl_FragColor = vec4(v_normal.xyz*vec3(0.5)+vec3(0.5), 1.0);
}

#endif