#include <lighting>
#include <apex3d>
#include <material>
varying vec2 v_texCoord0;
varying vec4 v_normal;
varying vec4 v_position;
void main()
{
	float ndotl = LambertDirectional(v_normal.xyz, Env_DirectionalLight.direction);
	float blinnPhong = BlinnPhongDirectional(v_normal.xyz, v_position.xyz, Apex_CameraPosition, Env_DirectionalLight.direction);
	
	vec3 diffuse = vec3(ndotl) * Env_DirectionalLight.color.xyz + Env_AmbientLight.color.xyz;
	vec3 specular = vec3(blinnPhong);
	
	vec4 lightSum = vec4(diffuse+specular, 1.0);
	
	gl_FragColor = lightSum;
}