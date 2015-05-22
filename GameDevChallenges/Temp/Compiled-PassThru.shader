// Compiled shader for Current graphics device, uncompressed size: 0.9KB

// Skipping shader variants that would not be included into build of current scene.

Shader "Custom/PassThru" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
}
SubShader { 
 Tags { "QUEUE"="Geometry" }
 Pass {
  Tags { "QUEUE"="Geometry" }
  GpuProgramID 23043
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
#line 6
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif

#line 6
#ifdef DUMMY_PREPROCESSOR_TO_WORK_AROUND_HLSL_COMPILER_LINE_HANDLING
#endif

			
			#ifdef VERTEX
			
			attribute vec4 vPosition
			
			void main()
			{
				gl_Position = vPosition;
			}
			#endif
			
			#ifdef FRAGMENT
			//precision mediump float;
			
			void main()
			{
				vec4 c;
				c.xyz = 1.0;
				c.w = 1.0;
				
				
				gl_FragColor = c;
			}
			
			#endif
			
			
"
}
}
 }
}
}