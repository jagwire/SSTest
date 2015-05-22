﻿Shader "Custom/PassThru" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue" = "Geometry"}
		Pass {
			GLSLPROGRAM
			
			#ifdef VERTEX
			
			void main()
			{
				gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
			}
			#endif
			
			#ifdef FRAGMENT
			
			void main()
			{
				gl_FragColor = vec4(255.0,1.0,1.0,255.0);
			}
			
			#endif
			
			ENDGLSL	
		}
	} 
}
