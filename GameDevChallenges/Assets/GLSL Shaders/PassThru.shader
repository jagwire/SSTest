Shader "Custom/PassThru" {
	SubShader {
		Pass {
			GLSLPROGRAM
			#ifdef VERTEX
			attribute vec4 position
			
			void main() {
				gl_Position = vPosition;
			}
			#endif
			
			#ifdef FRAGMENT
			#endif
			ENDGLSL	
		}
	} 
	FallBack "Diffuse"
}
