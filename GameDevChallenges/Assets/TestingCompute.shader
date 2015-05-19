Shader "Custom/TestingCompute" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
