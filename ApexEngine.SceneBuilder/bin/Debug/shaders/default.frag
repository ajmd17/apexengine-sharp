#include <lighting>
#include <apex3d>
#include <material>
varying vec2 v_texCoord0;
varying vec4 v_normal;
varying vec4 v_position;
varying vec4 v_diffuse;
varying vec4 v_specular;
varying vec3 v_tangent;
varying vec3 v_bitangent;
varying mat3 v_TBN;
varying vec3 v_lightVec;
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
		vec3 l = normalize(Env_DirectionalLight.direction);
		if (Material_HasNormalMap == 1)
		{
		    vec3 normalsTexture;
			normalsTexture.xy = (2.0 * texture2D(Material_NormalMap, vec2(v_texCoord0.x, -v_texCoord0.y)).rg - 1.0);
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