/*vec4 pack_depth(const in float depth)
{
	vec4 bit_shift =
		vec4(256.0*256.0*256.0, 256.0*256.0, 256.0, 1.0);
    vec4 bit_mask =
        vec4(0.0, 1.0/256.0, 1.0/256.0, 1.0/256.0);
        vec4 res = fract(depth * bit_shift);
    res -= res.xxyz * bit_mask;
    return res;
}*/
void main()
{
	gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);//pack_depth(gl_FragCoord.z);
}