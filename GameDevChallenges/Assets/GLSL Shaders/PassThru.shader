Shader "Custom/PassThru" {
	SubShader {
		Pass {
			GLSLPROGRAM
			#ifdef VERTEX
			attribute vec4 vPosition
			
			void main() {
				gl_Position = vPosition;z
			}
			#endif
			
			#ifdef FRAGMENT
			precision mediump float;
			
			void main() {
				gl_FragColor = vec4(1.0,0.0,0.0,1.0);
			}
			#endif
			ENDGLSL	
		}
	} 
	FallBack "Diffuse"
}
