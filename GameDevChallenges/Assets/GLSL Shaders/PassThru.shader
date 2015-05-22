Shader "Custom/PassThru" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue" = "Geometry"}
		Pass {
			GLSLPROGRAM
			
			#ifdef VERTEX
			
			varying vec4 position;
			
			void main()
			{
				position = gl_Vertex + vec4(0.5, 0.5, 0.5, 0.0);
				gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
			}
			#endif
			
			#ifdef FRAGMENT
			
			void main()
			{
				gl_FragColor = vec4(0.0,1.0,0.0,1.0);
			}
			
			#endif
			
			ENDGLSL	
		}
	} 
}
