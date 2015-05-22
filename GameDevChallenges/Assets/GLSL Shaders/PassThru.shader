Shader "Custom/PassThru" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue" = "Geometry"}
		Pass {
			GLSLPROGRAM
			
			#ifdef VERTEX
			
			attribute vec4 vPosition;
			
			void main()
			{
				gl_Position = vPosition;
			}
			#endif
			
			#ifdef FRAGMENT
			
			void main()
			{
				vec4 c;
				c.x = 1.0;
				c.y = 1.0;
				c.z = 1.0;
				c.w = 1.0;
				
				
				gl_FragColor = c;
			}
			
			#endif
			
			ENDGLSL	
		}
	} 
}
