Shader "Custom/TestingCompute" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MyVector("Some Vector", Vector) = (0,0,0,0)
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
