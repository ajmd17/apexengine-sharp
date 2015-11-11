#version 150
attribute vec3 a_position;
attribute vec3 a_normal;
attribute vec2 a_texcoord0;
attribute vec4 a_boneweights;
attribute vec4 a_boneindices;
uniform mat4 Bone[$NUM_BONES];
varying vec2 v_texCoord0;
varying vec3 v_normal;
varying vec3 v_position;
varying vec4 v_boneweights;
void main()
{
	mat4 NormalMatrix;
	vec4 index = a_boneindices;
	vec4 weight = a_boneweights;
	mat4 skinning = mat4(0.0);
	int index0 = int(index.x);
	skinning += weight.x * Bone[index0];
	int index1 = int(index.y);
	skinning += weight.y * Bone[index1];
	int index2 = int(index.z);
	skinning += weight.z * Bone[index2];
	int index3 = int(index.w);
	skinning += weight.w * Bone[index3];
	NormalMatrix = transpose(inverse(Apex_WorldMatrix*skinning));
	
	v_boneweights = a_boneweights;
	v_position = (vec4(a_position, 1.0)).xyz;
	v_normal = a_normal;
	v_texCoord0 = a_texcoord0;

	gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * skinning * vec4(v_position, 1.0);
}