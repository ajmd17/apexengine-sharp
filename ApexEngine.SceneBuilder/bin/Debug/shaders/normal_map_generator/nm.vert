attribute vec3 a_position;
attribute vec2 a_texcoord0;
varying vec2 v_texCoord0;
void main() {
    v_texCoord0 = vec2(a_texcoord0.x, -a_texcoord0.y);
    gl_Position = vec4(a_position, 1.0);
}