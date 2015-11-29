#version 150

#ifdef SKINNING
#include <skinning>
#endif

#include <apex3d>

attribute vec3 a_position;

varying vec4 v_position;

void main()
{
	v_position = Apex_WorldMatrix * vec4(a_position, 1.0);
	gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * vec4(a_position, 1.0);
}