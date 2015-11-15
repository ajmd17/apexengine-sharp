#include <lighting>
#include <apex3d>
#include <material>
varying vec2 v_texCoord0;
varying vec4 v_normal;
varying vec4 v_position;
varying vec4 v_diffuse;
varying vec4 v_specular;
void main()
{
	vec4 diffuseTexture = vec4(1.0);
	if (Material_HasDiffuseMap == 1)
	{
		diffuseTexture = texture2D(Material_DiffuseMap, vec2(v_texCoord0.x, -v_texCoord0.y));
	}
	if (Material_PerPixelLighting == 1)
	{
		vec3 n = normalize(v_normal.xyz);
		vec3 l = normalize(Env_DirectionalLight.direction.xyz);
		float ndotl = LambertDirectional(n, l);
		float specularity;
		
		if (Material_SpecularTechnique == 0)
		{
			specularity = BlinnPhongDirectional(v_normal.xyz, v_position.xyz, Apex_CameraPosition, Env_DirectionalLight.direction, Material_Shininess);
		}
		else if (Material_SpecularTechnique == 1)
		{
			specularity = SpecularDirectional(v_normal.xyz, v_position.xyz, Apex_CameraPosition, Env_DirectionalLight.direction, Material_Shininess);
		}
		
		vec3 diffuse = vec3(ndotl) * Material_DiffuseColor.xyz * Env_DirectionalLight.color.xyz + Env_AmbientLight.color.xyz + Material_AmbientColor.xyz;
		vec3 specular = vec3(specularity) * Material_SpecularColor.xyz;
		
		vec4 lightSum = vec4(diffuse*diffuseTexture+specular, 1.0);
		
		gl_FragColor = lightSum;
	}
	else
	{
		gl_FragColor = v_diffuse * diffuseTexture + v_specular;
	}
	
}