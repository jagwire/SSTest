Shader "Custom/PassThru" {
	SubShader {
		Pass {
			GLSLPROGRAM
			varying vec4 position;
			#ifdef VERTEX
			
			void main()
			{
				position = gl_Vertex + vec4(0.5, 0.5, 0.5, 0.0);
				gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
			}
			#endif
			
			#ifdef FRAGMENT
			void main()
			{
				gl_FragColor = position;
			}
			
			#endif
			
			ENDGLSL	
		}
	} 
}
