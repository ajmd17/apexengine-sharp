#version 150
#ifdef SKINNING
#include <skinning>
#endif
#include <lighting>
#include <apex3d>
#include <material>
attribute vec3 a_position;
attribute vec3 a_normal;
attribute vec2 a_texcoord0;
varying vec2 v_texCoord0;
varying vec4 v_position;
varying vec4 v_normal;
varying vec4 v_lighting;
void main()
{
	v_texCoord0 = a_texcoord0;
	
	#ifndef SKINNING
		v_position = (Apex_WorldMatrix * vec4(a_position, 1.0));	
		v_normal = transpose(inverse(Apex_WorldMatrix)) * vec4(a_normal, 0.0);
		gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * vec4(a_position, 1.0);
	#endif
	
	#ifdef SKINNING
		mat4 skinningMatrix = CreateSkinningMatrix();
		v_position = Apex_WorldMatrix * skinningMatrix * vec4(a_position, 1.0);	
		v_normal = transpose(inverse(Apex_WorldMatrix * skinningMatrix)) * vec4(a_normal, 0.0);
		gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * skinningMatrix * vec4(a_position, 1.0);
	#endif
	if (Material_PerPixelLighting == 0)
	{
		vec3 diffuse;
		vec3 specular;
		
		float specularity;
		
		vec3 n = normalize(v_normal.xyz);
		vec3 l = normalize(Env_DirectionalLight.direction);
		
		float ndotl = LambertDirectional(n, l);
		diffuse = vec3(ndotl) * Material_DiffuseColor.xyz * Env_DirectionalLight.color.xyz + Env_AmbientLight.color.xyz + Material_AmbientColor.xyz;
		
		if (Material_SpecularTechnique == 0)
		{
			specularity = BlinnPhongDirectional(v_normal.xyz, v_position.xyz, Apex_CameraPosition, Env_DirectionalLight.direction, Material_Shininess);
		}
		else if (Material_SpecularTechnique == 1)
		{
			specularity = SpecularDirectional(v_normal.xyz, v_position.xyz, Apex_CameraPosition, Env_DirectionalLight.direction, Material_Shininess);
		}
		
		specular = vec3(specularity);
		specular *= Material_SpecularColor.xyz;
		
		v_lighting = vec4(diffuse + specular, 1.0);
	}
}