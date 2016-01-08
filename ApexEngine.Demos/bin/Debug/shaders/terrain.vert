#version 330

#ifdef DEFAULT

#include <lighting>
#include <apex3d>
#include <material>

attribute vec3 a_position;
attribute vec3 a_normal;
attribute vec2 a_texcoord0;
attribute vec3 a_tangent;
attribute vec3 a_bitangent;

varying vec2 v_texCoord0;
varying vec4 v_position;
varying vec4 v_normal;
varying vec4 v_diffuse;
varying vec4 v_specular;
varying vec3 v_tangent;
varying vec3 v_bitangent;
varying mat3 v_TBN;
varying vec3 v_parallaxView;

void main()
{
	v_texCoord0 = a_texcoord0;
	
	
	vec3 c1 = cross(a_normal, vec3(0.0, 0.0, 1.0));
	vec3 c2 = cross(a_normal, vec3(0.0, 1.0, 0.0));
	if (length(c1)>length(c2))
		v_tangent = c1;
	else
		v_tangent = c2;
	v_tangent = normalize(v_tangent);
	v_bitangent = cross(a_normal, v_tangent);
	v_bitangent = normalize(v_bitangent);
	
	v_position = (Apex_WorldMatrix * vec4(a_position, 1.0));	
	v_normal = transpose(inverse(Apex_WorldMatrix)) * vec4(a_normal, 0.0);
	gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * vec4(a_position, 1.0);
	

	
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
			specularity = BlinnPhongDirectional(v_normal.xyz, v_position.xyz, Apex_CameraPosition, Env_DirectionalLight.direction, Material_SpecularExponent);
		}
		else if (Material_SpecularTechnique == 1)
		{
			specularity = SpecularDirectional(v_normal.xyz, v_position.xyz, Apex_CameraPosition, Env_DirectionalLight.direction, Material_SpecularExponent, Material_Roughness);
		}
		
		specular = vec3(specularity);
		specular *= Material_SpecularColor.xyz;
		
		v_diffuse = vec4(diffuse, 1.0);
		v_specular = vec4(specular, 1.0);
	}
}

#endif

#ifdef NORMALS

#include <apex3d>
#include <material>

attribute vec3 a_position;
attribute vec3 a_normal;
attribute vec2 a_texcoord0;

varying vec4 v_normal;
varying vec2 v_texCoord0;

void main()
{
	v_texCoord0 = a_texcoord0;
	v_normal = transpose(inverse(Apex_WorldMatrix)) * vec4(a_normal, 0.0);
	gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * vec4(a_position, 1.0);
}

#endif

#ifdef DEPTH

#ifdef SKINNING
#include <skinning>
#endif
#include <apex3d>
#include <material>

attribute vec3 a_position;
attribute vec3 a_normal;
attribute vec2 a_texcoord0;

varying vec2 v_texCoord0;
varying vec4 v_position;

void main()
{
	v_texCoord0 = a_texcoord0;
	
	gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * vec4(a_position, 1.0);
}

#endif