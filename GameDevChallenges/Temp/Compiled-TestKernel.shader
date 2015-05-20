// Compiled shader for Web Player, uncompressed size: 2.6KB

// Skipping shader variants that would not be included into build of current scene.

Shader "Custom/TestKernel" {
SubShader { 


 // Stats for Vertex shader:
 //       d3d11 : 4 math
 //        d3d9 : 4 math
 //      opengl : 2 math
 // Stats for Fragment shader:
 //       d3d11 : 1 math
 Pass {
  GpuProgramID 53976
Program "vp" {
SubProgram "opengl " {
// Stats: 2 math
"!!GLSL
#ifdef VERTEX

void main ()
{
  gl_Position = (gl_ModelViewProjectionMatrix * gl_Vertex);
}


#endif
#ifdef FRAGMENT
uniform vec4 _ScreenParams;
varying vec4 xlv_WPOS;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.zw = vec2(0.0, 1.0);
  tmpvar_1.xy = (xlv_WPOS.xy / _ScreenParams.xy);
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "d3d9 " {
// Stats: 4 math
Bind "vertex" Vertex
Matrix 0 [glstate_matrix_mvp]
"vs_3_0
dcl_position v0
dcl_position o0
dp4 o0.x, c0, v0
dp4 o0.y, c1, v0
dp4 o0.z, c2, v0
dp4 o0.w, c3, v0

"
}
SubProgram "d3d11 " {
// Stats: 4 math
Bind "vertex" Vertex
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
BindCB  "UnityPerDraw" 0
"vs_4_0
eefiecedcopkamamfcjbkgbpedonpmombldhgkleabaaaaaaleabaaaaadaaaaaa
cmaaaaaakaaaaaaaneaaaaaaejfdeheogmaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafjaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahaaaaaagaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apaaaaaafaepfdejfeejepeoaaeoepfcenebemaafeeffiedepepfceeaaklklkl
epfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaabaaaaaaadaaaaaa
aaaaaaaaapaaaaaafdfgfpfagphdgjhegjgpgoaafdeieefcniaaaaaaeaaaabaa
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
// Platform d3d9 had shader errors
//   <no keywords>
SubProgram "opengl " {
"!!GLSL"
}
SubProgram "d3d11 " {
// Stats: 1 math
ConstBuffer "UnityPerCamera" 144
Vector 96 [_ScreenParams]
BindCB  "UnityPerCamera" 0
"ps_4_0
eefiecedfjjbhpmffhiiaekcgdnchebngfdnaoneabaaaaaaamabaaaaadaaaaaa
cmaaaaaafmaaaaaajaaaaaaaejfdeheociaaaaaaabaaaaaaaiaaaaaacaaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapadaaaafhfaepfdaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcheaaaaaaeaaaaaaabnaaaaaa
fjaaaaaeegiocaaaaaaaaaaaahaaaaaagcbaaaaddcbabaaaaaaaaaaagfaaaaad
pccabaaaaaaaaaaaaoaaaaaidccabaaaaaaaaaaaegbabaaaaaaaaaaaegiacaaa
aaaaaaaaagaaaaaadgaaaaaimccabaaaaaaaaaaaaceaaaaaaaaaaaaaaaaaaaaa
aaaaaaaaaaaaiadpdoaaaaab"
}
}
 }
}
}