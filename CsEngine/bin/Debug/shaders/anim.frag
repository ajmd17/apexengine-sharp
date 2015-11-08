
varying vec3 v_position;
varying vec4 redCol;
void main()
{
	gl_FragColor = vec4(redCol.xyz, 1.0);
}