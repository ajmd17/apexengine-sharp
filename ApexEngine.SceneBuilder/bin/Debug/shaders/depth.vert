#version 150

#ifdef SKINNING
#include <skinning>
#endif
#include <apex3d>
#include <material>

attribute vec3 a_position;
attribute vec3 a_normal;
attribute vec2 a_texcoord0;

varying vec2 v_texCoord0;
varying vec4 v_position;

void main()
{
	v_texCoord0 = a_texcoord0;
	
	gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * vec4(a_position, 1.0);
}