#version 150
attribute vec3 a_position;
attribute vec4 a_boneweights;
attribute vec4 a_boneindices;
uniform mat4 u_world;
uniform mat4 u_view;
uniform mat4 u_proj;
varying vec4 redCol;
uniform mat4 Bone[53];
uniform mat4 testMat;
varying vec3 v_position;
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
	NormalMatrix = transpose(inverse(u_world*skinning));
	v_position = (vec4(a_position, 1.0)).xyz;

	redCol = vec4(v_position, 1.0);
	gl_Position = u_proj * u_view * u_world * skinning * vec4(v_position, 1.0);
}