#version 150

#include <material>
#include <apex3d>
#include <shadows>

varying vec4 v_position;

void main()
{
	vec3 shadowColor = vec3(1.0);
	float shadowness;
	
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
	vec4 diffuseColor = Material_DiffuseColor * vec4(shadowColor, 1.0);
	//diffuseColor = CalculateFog(diffuseColor, Env_AmbientLight.color, v_position.xyz, Apex_CameraPosition, Env_FogStart, Env_FogEnd);
	gl_FragColor = diffuseColor;
}