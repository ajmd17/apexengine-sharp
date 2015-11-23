#version 150
varying vec2 v_texCoord0;
uniform sampler2D tex;
uniform vec2 u_scale;
uniform int u_texWidth;
uniform int u_texHeight;
void main()
{

	vec4 firstPass = texture2D(tex, v_texCoord0);
	float blurSize = 30.0;
	float bloom = 1.0;
	int width = u_texWidth;
	int height = u_texHeight;

	float v;
	float pi = 3.141592653589793;
	float e_step = 1.0 / float(width);
	float radius = blurSize;
	if ( radius < 0.0 ) radius = 0.0;
	int steps = int(min(radius * 0.7, sqrt(radius) * pi));
	float r = radius / float(steps);
	float t = bloom / (float(steps) * 2.0 + 1.0);
	float x = v_texCoord0.x;
	float y = v_texCoord0.y;
	vec4 sum = texture2D(tex, vec2(x, y)) * t;
	
	for(int i = 1; i <= steps; i++) {
		v = (cos(i / float(steps + 1) / pi) + 1.0) * 0.5;
		sum += texture2D(tex, vec2(x, y + float(i) * e_step * r)) * v * t;
		sum += texture2D(tex, vec2(x, y - float(i) * e_step * r)) * v * t;
	}
	
 
  gl_FragColor = sqrt((firstPass * firstPass) + (sum * sum));
}