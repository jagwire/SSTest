Shader "Custom/PassThru" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue" = "Geometry"}
		Pass {
			GLSLPROGRAM
			#ifdef VERTEX
			attribute vec4 vPosition
			
			void main() {
				//gl_Position = vPosition;
			}
			#endif
			
			#ifdef FRAGMENT
			//precision mediump float;
			
			void main() {
				//gl_FragColor = vec4(0.0,0.0,0.0,1.0);
			}
			#endif
			ENDGLSL	
		}
	} 
}
