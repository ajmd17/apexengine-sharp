#version 150

#ifdef SKINNING
#include <skinning>
#endif
#include <apex3d>
#include <material>

attribute vec3 a_position;
attribute vec3 a_normal;
attribute vec2 a_texcoord0;

varying vec3 v_normal;

void main()
{
	v_normal = a_normal;
	gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * vec4(a_position, 1.0);
}