#include <lighting>
#include <apex3d>
#include <material>
varying vec3 v_normal;
varying vec2 v_texCoord0;
varying vec3 v_position;
void main()
{
	float ndotl = LambertDirectional(v_normal, Env_DirectionalLight.direction);
	float specular = BlinnPhongDirectional(v_normal, v_position, Apex_CameraPosition, Env_DirectionalLight.direction);
	
	vec3 diffuse = vec3(ndotl) * Env_DirectionalLight.color.xyz + Env_AmbientLight.color.xyz;
	
	vec4 lightSum = vec4(diffuse+vec3(specular, specular, specular), 1.0);
	
	gl_FragColor = lightSum;
}