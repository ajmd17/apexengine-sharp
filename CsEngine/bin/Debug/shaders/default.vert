#version 150
attribute vec3 a_position;
attribute vec2 a_texcoord0;
varying vec2 v_texCoord0;
varying vec3 v_position;
void main()
{
	
	v_position = (vec4(a_position, 1.0)).xyz;
	v_texCoord0 = a_texcoord0;

	gl_Position = Apex_ProjectionMatrix * Apex_ViewMatrix * Apex_WorldMatrix * vec4(v_position, 1.0);
}