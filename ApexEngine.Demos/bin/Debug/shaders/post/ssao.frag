
#version 150
precision highp float;

#define KERNEL_SIZE 32

// This constant removes artifacts caused by neighbour fragments with minimal depth difference.
#define CAP_MIN_DISTANCE 0.0001

// This constant avoids the influence of fragments, which are too far away.
#define CAP_MAX_DISTANCE 0.05

uniform sampler2D u_texture;
uniform sampler2D u_normalTexture;
uniform sampler2D u_depthTexture;

uniform vec3 u_kernel[32];

uniform sampler2D u_rotationNoiseTexture; 

uniform vec2 u_rotationNoiseScale;
uniform int u_enabled;
uniform mat4 u_view;
uniform mat4 u_inverseProjectionMatrix;
uniform mat4 u_invViewProj;
uniform mat4 u_projectionMatrix;

uniform float u_radius;

varying vec2 v_texCoord0;

out vec4 fragColor;

float unpack_depth(const in vec4 rgba_depth) {
    const vec4 bit_shift =
    vec4(1.0/(256.0*256.0*256.0)
    , 1.0/(256.0*256.0)
    , 1.0/256.0
    , 1.0);
    float depth = dot(rgba_depth, bit_shift);
    return rgba_depth.r;
    return depth;
}

vec3 readNormal(in vec2 coord)  
{  
         return normalize(texture2D(u_normalTexture, coord).xyz*2.0  - 1.0);  
}

float linearDepth(float depth) {
	/*const float near = 0.1;
	const float far = 100.0;
    float z = depth; // back to NDC 
    return (2.0 * near * far) / (far + near - z * (far - near));   */ 
    return depth;
}

vec3 getPosition(vec2 uv, float depth) {
     vec4 pos = vec4(uv.x * 2.0 - 1.0, (uv.y * 2.0 - 1.0), depth * 2.0 - 1.0, 1.0);
     pos = inverse(u_projectionMatrix * u_view) * pos;
     pos = pos/pos.w;
     return pos.xyz;
}

float doAmbientOcclusion(vec2 tcoord,vec2 uv, vec3 p, vec3 cnorm)
{
	vec3 diff = getPosition(tcoord + uv, texture2D(u_depthTexture, tcoord + uv).r) - p;
	vec3 v = normalize(diff);
	float d = length(diff)*0.2;
	return max(0.0,dot(cnorm,v)-0.1)*(1.0/(1.0+d))*2.5;
}

#ifndef NO_GI


vec3 doGlobalIllumination(vec2 tcoord,vec2 uv, vec3 p, vec3 cnorm)
{
	vec3 ddiff = getPosition(tcoord + uv, texture2D(u_depthTexture, tcoord + uv).r) - p;
	vec3 vv = normalize(ddiff);
    float rd = length(ddiff);
    return texture2D(u_texture, tcoord + uv).rgb * 1.0*clamp(dot(readNormal(tcoord+uv),-vv),0.0,1.0)*
                                         clamp(dot( cnorm,vv ),0.0,1.0)/
                                         (rd*rd+1.0);  
	
	
}

#endif

vec4 getViewPos(vec2 texCoord)
{
	// Calculate out of the fragment in screen space the view space position.

	float x = texCoord.s * 2.0 - 1.0;
	float y = texCoord.t * 2.0 - 1.0;
	
	// Assume we have a normal depth range between 0.0 and 1.0
	float z = linearDepth(texture(u_depthTexture, texCoord).r);
	
	vec4 posProj = vec4(x, y, z, 1.0);
	
	vec4 posView = inverse(u_projectionMatrix) * posProj;
	
	posView /= posView.w;
	
	return posView;
}

vec3 getTriPlanarBlend(vec3 _wNorm){
    // in wNorm is the world-space normal of the fragment
    vec3 blending = abs( _wNorm );
    blending = normalize(max(blending, 0.00001)); // Force weights to sum to 1.0
    float b = (blending.x + blending.y + blending.z);
    blending /= vec3(b, b, b);
    return blending;
}

void main(void)
{
	
		vec4 sceneColor = texture2D(u_texture, v_texCoord0);
		// Normal gathering.
		vec4 nTex = texture(u_normalTexture, v_texCoord0) * 2.0 - 1.0;
		
		vec3 normalView = normalize(nTex).xyz;
		
		
	
		// Go through the kernel samples and create occlusion factor.	
		float occlusion = 0.0;
		#ifndef NO_GI
			vec3 gi = vec3(0.0);
		#endif
		
		float depth = texture(u_depthTexture, v_texCoord0).r;
		
		
		vec3 p = getPosition(v_texCoord0, depth).xyz;
		vec3 n = normalView;
	
	
		vec3 blending = getTriPlanarBlend(n);
	    vec3 xaxis = vec3(texture2D( u_rotationNoiseTexture, p.yz*u_rotationNoiseScale).r);
	    vec3 yaxis = vec3(texture2D( u_rotationNoiseTexture, p.xz*u_rotationNoiseScale).r);
	    vec3 zaxis = vec3(texture2D( u_rotationNoiseTexture, p.xy*u_rotationNoiseScale).r);

		vec2 rand = (xaxis * blending.x + yaxis * blending.y + zaxis * blending.z).xy;
		float rad = u_radius;
		
		vec2 vec[4];
		vec[0] = vec2(1.0, 0.0);
		vec[1] = vec2(-1.0, 0.0);
		vec[2] = vec2(0.0, 1.0);
		vec[3] = vec2(0.0, -1.0);
		
		int iterations = 4;
		for (int j = 0; j < iterations; ++j)
		{
		  vec2 coord1 = reflect(vec[j].xy,rand)*rad;
		  vec2 coord2 = vec2(coord1.x*0.707 - coord1.y*0.707,
					  coord1.x*0.707 + coord1.y*0.707);
		  
		  occlusion += doAmbientOcclusion(v_texCoord0,coord1*(1.0-depth)*0.25, p, n);
		  occlusion += doAmbientOcclusion(v_texCoord0,coord2*(1.0-depth)*0.5, p, n);
		  occlusion += doAmbientOcclusion(v_texCoord0,coord1*(1.0-depth)*0.75, p, n);
		  occlusion += doAmbientOcclusion(v_texCoord0,coord2*(1.0-depth), p, n);
		}
		
		occlusion /= 16.0;
		occlusion *= 0.5;
		occlusion = 1.0-occlusion;
		
		#ifndef NO_GI
		
		for (int j = 0; j < iterations; ++j)
		{
			float rad_gi = rad * 3.0;
		
		  vec2 coord1 = reflect(vec[j].xy,rand)*rad_gi;
		  vec2 coord2 = vec2(coord1.x*0.707 - coord1.y*0.707,
					  coord1.x*0.707 + coord1.y*0.707);
		  
		  gi += doGlobalIllumination(v_texCoord0,coord1*(1.0-depth)*0.25, p, n);
		  gi += doGlobalIllumination(v_texCoord0,coord2*(1.0-depth)*0.5, p, n);
		  gi += doGlobalIllumination(v_texCoord0,coord1*(1.0-depth)*0.75, p, n);
		  gi += doGlobalIllumination(v_texCoord0,coord2*(1.0-depth), p, n);
		}
		
		gi /= 16.0;
		
		#endif
		
		
		float fogFactor = (0.995 - depth)/(0.995 - 0.997);
		fogFactor = clamp(fogFactor, 0.0, 1.0);
		
		occlusion = mix(occlusion, 1.0, fogFactor);
		
		gl_FragColor = sceneColor*vec4(occlusion, occlusion, occlusion, 1.0);
		
		#ifndef NO_GI
			gi = mix(gi, vec3(0.0), fogFactor);
			gl_FragColor += vec4(gi, 1.0);
		#endif
}