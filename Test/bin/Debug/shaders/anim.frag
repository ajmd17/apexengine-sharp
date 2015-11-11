varying vec2 v_texCoord0;
varying vec3 v_normal;
uniform sampler2D u_texture;
varying vec4 v_boneweights;
void main()
{
	float ndotl = dot(v_normal, vec3(1.0));
	gl_FragColor = vec4(v_boneweights.xyz, 1.0);
}