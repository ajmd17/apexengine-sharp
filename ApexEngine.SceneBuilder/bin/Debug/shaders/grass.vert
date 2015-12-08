#version 330

#ifdef SKINNING
#include <skinning>
#endif
#version 330

#include <lighting>
#include <apex3d>
#include <material>

attribute vec3 a_position;
attribute vec3 a_normal;
attribute vec2 a_texcoord0;
attribute vec3 a_tangent;
attribute vec3 a_bitangent;

uniform float u_grassHeight;

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
	

	vec3 modifiedPos = a_position;
	modifiedPos.x += sin(Apex_ElapsedTime * 0.1) * 0.3 * (a_texcoord0.y);
	modifiedPos.z += sin(Apex_ElapsedTime * 0.08) * 0.3 * (a_texcoord0.y);
	
	
	v_position = (Apex_WorldMatrix * vec4(modifiedPos, 1.0));	
	mat3 normalMatrix = mat3(transpose(inverse(Apex_WorldMatrix)));
	v_normal = vec4(normalMatrix * a_normal, 0.0);
	v_bitangent = normalMatrix * v_bitangent;
	v_tangent = normalMatrix * v_tangent;
	gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * vec4(modifiedPos, 1.0);

		
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
}