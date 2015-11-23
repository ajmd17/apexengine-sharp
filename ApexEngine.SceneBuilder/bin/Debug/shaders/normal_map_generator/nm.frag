#version 400

varying vec2 v_texCoord0;
uniform sampler2D u_texture;
uniform int u_imgWidth;
uniform int u_imgHeight;
uniform float u_delta;

float threshold(in float thr1, in float thr2 , in float val) {
 if (val < thr1) {return 0.0;}
 if (val > thr2) {return 1.0;}
 return val;
}

// averaged pixel intensity from 3 color channels
float avg_intensity(in vec4 pix) {
 return (pix.r + pix.g + pix.b)/3.;
}

vec4 get_pixel(in vec2 coords, in float dx, in float dy) {
 return texture2D(u_texture,coords + vec2(dx, dy));
}

// returns pixel color
float IsEdge(in vec2 coords){
  float dxtex = 1.0 / u_imgWidth /*image width*/;
  float dytex = 1.0 / u_imgHeight /*image height*/;
  float pix[9];
  int k = -1;
  float delta;

  // read neighboring pixel intensities
  for (int i=-1; i<2; i++) {
   for(int j=-1; j<2; j++) {
    k++;
    pix[k] = avg_intensity(get_pixel(coords,float(i)*dxtex,
                                          float(j)*dytex));
   }
  }

  // average color differences around neighboring pixels
  delta = (abs(pix[1]-pix[7])+
          abs(pix[5]-pix[3]) +
          abs(pix[0]-pix[8])+
          abs(pix[2]-pix[6])
           )/4.;

  return threshold(0.1,0.9,clamp(u_delta*delta,0.0,1.0));
}

vec4 gen_bump()
{
	vec2 size = vec2(2.0,0.0);
	vec3 off = vec3(-0.0025,0.0,0.0025);

    float s11 = IsEdge(v_texCoord0);
    float s01 = IsEdge(v_texCoord0 + off.xy);
    float s21 = IsEdge(v_texCoord0 + off.zy);
    float s10 = IsEdge(v_texCoord0 + off.yx);
    float s12 = IsEdge(v_texCoord0 + off.yz);
    vec3 va = normalize(vec3(off.z, 0.0, s21-s01));
    vec3 vb = normalize(vec3(0.0, off.z, s12-s10));
    vec4 bump = vec4( cross(va,vb), s11 );
	
	return bump;
}

void main()
{
	//float edge = IsEdge(v_texCoord0);
	vec4 color = vec4(gen_bump().rgb, 1.0) * vec4(0.5) + vec4(0.5);
	gl_FragColor = color;
}
