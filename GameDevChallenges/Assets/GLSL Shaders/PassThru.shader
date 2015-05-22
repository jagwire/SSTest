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
			
			void main()
			{
				gl_Position = vPosition;
			}
			#endif
			
			#ifdef FRAGMENT
			precision mediump float;
			
			void main()
			{
				vec4 c;
				c.xyz = 1.0;
				c.w = 1.0;
				
				
				gl_FragColor = c;
			}
			
			#endif
			
			ENDGLSL	
		}
	} 
}
