#version 150

#ifdef NORMALS

#include <material>

varying vec3 v_normal;
varying vec2 v_texCoord0;

void main()
{
	vec2 texCoord = vec2(v_texCoord0.x, -v_texCoord0.y);
	if (Material_HasDiffuseMap == 1)
	{
		vec4 diffuseTexture;
		diffuseTexture = texture2D(Material_DiffuseMap, texCoord);
		if (diffuseTexture.a < Material_AlphaDiscard)
		{
			discard;
		}
	}

	gl_FragColor = vec4(v_normal*vec3(0.5)+vec3(0.5), 1.0);
}

#endif