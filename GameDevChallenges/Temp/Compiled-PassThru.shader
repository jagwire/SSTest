// Compiled shader for Web Player, uncompressed size: 0.8KB

// Skipping shader variants that would not be included into build of current scene.

Shader "Custom/PassThru" {
SubShader { 
 Pass {
  GpuProgramID 58950
Program "vp" {
SubProgram "opengl " {
"!!GLSL
#ifndef SHADER_TARGET
    #define SHADER_TARGET 30
#endif
#ifndef SHADER_API_OPENGL
    #define SHADER_API_OPENGL 1
#endif
#ifndef SHADER_API_DESKTOP
    #define SHADER_API_DESKTOP 1
#endif
#define highp
#define mediump
#define lowp
#line 2
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif

#line 2
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif

			#ifdef VERTEX
			attribute vec4 vPosition
			
			void main() {
				gl_Position = vPosition;
			}
			#endif
			
			#ifdef FRAGMENT
			precision mediump float;
			
			void main() {
				gl_FragColor = vec4(0.0,0.0,0.0,1.0);
			}
			#endif
			
"
}
}
 }
}
}