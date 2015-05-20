// Compiled shader for Web Player, uncompressed size: 2.1KB

// Skipping shader variants that would not be included into build of current scene.

Shader "Custom/TestKernel" {
SubShader { 


 // Stats for Vertex shader:
 //       d3d11 : 4 math
 //        d3d9 : 4 math
 //      opengl : 1 math
 // Stats for Fragment shader:
 //        d3d9 : 1 math
 Pass {
  GpuProgramID 16839
Program "vp" {
SubProgram "opengl " {
// Stats: 1 math
"!!GLSL
#ifdef VERTEX

void main ()
{
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
}


#endif
#ifdef FRAGMENT
void main ()
{
  gl_FragData[0] = vec4(1.0, 0.0, 0.0, 1.0);
}


#endif
"
}
SubProgram "d3d9 " {
// Stats: 4 math
Bind "vertex" Vertex
Matrix 0 [glstate_matrix_mvp]
"vs_2_0
dcl_position v0
dp4 oPos.x, c0, v0
dp4 oPos.y, c1, v0
dp4 oPos.z, c2, v0
dp4 oPos.w, c3, v0

"
}
SubProgram "d3d11 " {
// Stats: 4 math
Bind "vertex" Vertex
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "UnityPerDraw" 0
"vs_4_0
eefiecedijhpljdppnfhjnjaadaickkmhicpkjbcabaaaaaaheabaaaaadaaaaaa
cmaaaaaagaaaaaaajeaaaaaaejfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafaepfdejfeejepeoaaklklkl
epfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaafdfgfpfaepfdejfeejepeoaafdeieefcniaaaaaaeaaaabaa
dgaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaaaaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaaaaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaaaaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaaaaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadoaaaaab"
}
}
Program "fp" {
SubProgram "opengl " {
"!!GLSL"
}
SubProgram "d3d9 " {
// Stats: 1 math
"ps_2_0
def c0, 1, 0, 0, 1
mov_pp oC0, c0

"
}
SubProgram "d3d11 " {
"ps_4_0
eefiecedaliiphpcakfldjhkaonaodfhlgnchhklabaaaaaalaaaaaaaadaaaaaa
cmaaaaaadmaaaaaahaaaaaaaejfdeheoaiaaaaaaaaaaaaaaaiaaaaaaepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcdiaaaaaaeaaaaaaaaoaaaaaa
gfaaaaadpccabaaaaaaaaaaadgaaaaaipccabaaaaaaaaaaaaceaaaaaaaaaiadp
aaaaaaaaaaaaaaaaaaaaiadpdoaaaaab"
}
}
 }
}
}