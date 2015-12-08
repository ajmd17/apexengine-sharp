#version 330

#ifdef SKINNING
#include <skinning>
#endif

#include <lighting>
#include <apex3d>
#include <material>

attribute vec3 a_position;
attribute vec3 a_normal;
attribute vec2 a_texcoord0;
attribute vec3 a_tangent;
attribute vec3 a_bitangent;

uniform float u_time;

varying vec2 v_texCoord0;
varying vec4 v_position;
varying vec4 v_normal;
varying vec3 v_tangent;
varying vec3 v_bitangent;
varying mat3 v_TBN;
varying vec3 v_parallaxView;

varying vec3 refVec;

varying vec4 v_ambient;
varying vec4 v_diffuse;
varying vec4 v_specular;

void main()
{
	v_texCoord0 = vec2(a_texcoord0.x + sin(u_time*0.001), a_texcoord0.y + cos(u_time*0.001));
	
	
	vec3 c1 = cross(a_normal, vec3(0.0, 0.0, 1.0));
	vec3 c2 = cross(a_normal, vec3(0.0, 1.0, 0.0));
	if (length(c1)>length(c2))
		v_tangent = c1;
	else
		v_tangent = c2;
	v_tangent = normalize(normalMatrix * v_tangent);
	v_bitangent = cross(a_normal, v_tangent);
	v_bitangent = normalize(normalMatrix * v_bitangent);
	
	
	if (Material_HasHeightMap == 1)
	{
		mat3 normalMatrix = mat3(Apex_ViewMatrix * Apex_WorldMatrix);
		v_TBN = mat3(normalMatrix * v_tangent, normalMatrix * v_bitangent, normalMatrix * a_normal);
		v_parallaxView = normalize(-vec3(Apex_ViewMatrix * Apex_WorldMatrix * vec4(a_position, 1.0))) * v_TBN;
	}
	
	#ifndef SKINNING
		v_position = (Apex_WorldMatrix * vec4(a_position, 1.0));	
		mat3 normalMatrix = mat3(transpose(inverse(Apex_WorldMatrix)));
		v_normal = vec4(normalMatrix * a_normal, 0.0);
		v_bitangent = normalMatrix * v_bitangent;
		v_tangent = normalMatrix * v_tangent;
		gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * vec4(a_position, 1.0);
	#endif
	
	#ifdef SKINNING
		mat4 skinningMatrix = CreateSkinningMatrix();
		v_position = Apex_WorldMatrix * skinningMatrix * vec4(a_position, 1.0);	
		v_normal = transpose(inverse(Apex_WorldMatrix * skinningMatrix)) * vec4(a_normal, 0.0);
		gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * skinningMatrix * vec4(a_position, 1.0);
	#endif
	
	vec3 n = normalize(v_normal.xyz);
	if (Material_PerPixelLighting == 0)
	{
		
		float specularity;
		
		vec3 l = normalize(Env_DirectionalLight.direction);
		
		float ndotl = LambertDirectional(n, l);
		
		if (Material_SpecularTechnique == 0)
		{
			specularity = BlinnPhongDirectional(v_normal.xyz, v_position.xyz, Apex_CameraPosition, Env_DirectionalLight.direction, Material_SpecularExponent);
		}
		else if (Material_SpecularTechnique == 1)
		{
			specularity = SpecularDirectional(v_normal.xyz, v_position.xyz, Apex_CameraPosition, Env_DirectionalLight.direction, Material_SpecularExponent, Material_Roughness);
		}
		
		float shineAmt = Material_Shininess;
		
		vec3 surfaceColor = Material_DiffuseColor.xyz;
		
		vec3 diffuseLight = vec3(ndotl) * Env_DirectionalLight.color.xyz;
		vec3 diffuse = diffuseLight * surfaceColor;
		
		vec3 ambient = surfaceColor * (Material_AmbientColor.xyz + Env_AmbientLight.color.xyz);
		
		vec3 specular = ndotl * vec3(specularity) * Env_DirectionalLight.color.xyz;
		
		for (int i = 0; i < Env_NumPointLights; i++)
		{
			PointLight pl = Env_PointLights[i];
			vec3 pl_dir = normalize(pl.position - v_position.xyz);
			float p_ndotl = max(dot(n, pl_dir), 0.0);
			diffuse += vec3(p_ndotl) * surfaceColor * pl.color.xyz;
			
			float pl_specularity = BlinnPhongDirectional(n, v_position.xyz, Apex_CameraPosition, pl_dir, Material_SpecularExponent);
			vec3 pl_spec = vec3(pl_specularity) * pl.color.xyz;
			specular += pl_spec;
		}	
		
		specular *= Material_SpecularColor.xyz;
		
		diffuse = clamp(diffuse, vec3(0.0), vec3(1.0));
		specular = clamp(specular, vec3(0.0), vec3(1.0));
		
		v_diffuse = vec4(diffuse, 1.0);
		v_specular = vec4(specular, 1.0);
		v_ambient = vec4(ambient, 1.0);
	}
	
	if (Material_HasEnvironmentMap == 1)
	{
		refVec = ReflectionVector(n, v_position.xyz, Apex_CameraPosition.xyz);
	}
}