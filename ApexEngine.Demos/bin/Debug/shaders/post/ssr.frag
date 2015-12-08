#version 150

uniform sampler2D u_texture;
uniform sampler2D u_depthTexture;
uniform sampler2D u_normalTexture;

uniform vec3 u_cameraPosition;

uniform mat4 u_viewMatrix;
uniform mat4 u_projectionMatrix;

uniform float u_near;
uniform float u_far;

varying vec2 v_texCoord0;

const float zFar = 100.0;
const float zNear = 1.0;
const float stepSize = 0.002;
const float fade = 10.0;
const float maxDelta = 0.0025;
 

vec3 getPosition(vec2 uv, float depth) 
{
     vec4 pos = vec4(uv.x * 2.0 - 1.0, (uv.y * 2.0 - 1.0), depth * 2.0 - 1.0, 1.0);
     pos = inverse(u_projectionMatrix * u_viewMatrix) * pos;
     pos = pos/pos.w;
     return pos.xyz;
}

float reprojectDepth(vec3 worldPos)
{
	vec4 pos = u_projectionMatrix  * vec4(worldPos, 1.0);
	vec2 uv;
	uv.x = pos.x;
	uv.y = pos.y;
	float depth = pos.z * 0.5 + 0.5;
	
	vec4 tex = texture2D(u_depthTexture, uv);
	return tex.r;
}

float linearizeDepth(float depth)
{
    float near = u_near;
    float far = u_near;
    float linearDepth = (2.0 * near) / (far + near - depth * (far - near));

    return linearDepth;
}


vec4 Raycast(vec3 direction, vec2 startPosition, float depth)
{
	vec3 reflectionVector = direction;
    float size = length(reflectionVector.xy);
    reflectionVector = normalize(reflectionVector)/size;
    reflectionVector = reflectionVector * stepSize;
	
	vec4 color = vec4(0.0);
	
	float currentDepth = depth;
	
	vec2 samplePosition = startPosition;
	for (int i = 1; i < 40; i++)
	{
		samplePosition += reflectionVector.xy;
		currentDepth = depth + reflectionVector.z * depth;
		float sampleDepth = (texture2D(u_depthTexture, samplePosition).r);
		if(currentDepth > sampleDepth)
        {
           // float delta = (currentDepth - sampleDepth);
          //  if ( delta < maxDelta )
          //  {
                color = texture2D(u_texture, samplePosition);
                color.a *= fade / i;
              //  break;
           // }
        }
	}
	return color;
}

 

void main()
{
	vec3 viewNormal = (vec4(texture2D(u_normalTexture, v_texCoord0).xyz , 1.0)).xyz;
	float depth = texture2D(u_depthTexture, v_texCoord0).r;
    vec3 viewPos = (vec4(getPosition(v_texCoord0, depth), 1.0)).xyz;
 
	if (depth < 1.0)
	{
		vec4 V = vec4(viewPos - u_cameraPosition, 0.0);
    // Reflection vector
    vec4 reflected = reflect(V, vec4(viewNormal, 0.0));
	
 
    vec4 color = Raycast(reflected.xyz, v_texCoord0, depth);
 
 
 
 
    // Get color
   // gl_FragColor = vec4(vec3(1.0),
   //     pow(specular, reflectionSpecularFalloffExponent) *
   //     screenEdgefactor * clamp(-reflected.z, 0.0, 1.0) *
    //    clamp((searchDist - length(viewPos - hitPos)) * searchDistInv, 0.0, 1.0) * coords.w);
		
		gl_FragColor = vec4(color.rgb, 1.0);
		
		//vec3 world = getPosition(v_texCoord0, depth);
		//gl_FragColor = vec4(reprojectDepth(world));
	//gl_FragColor = vec4(reflected.xyz, 1.0);
		
	//gl_FragColor = vec4(coords.rgb, 1.0);
	} 
	else
	{
		gl_FragColor = vec4(0.0);
	}
}