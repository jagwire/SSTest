Shader "Custom/TestKernel" {
	
	SubShader {
		Pass {
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag
		
		#include "UnityCG.cginc"
		
		float4 frag(v2f_img i) {
			return float4(1.0, 0.0, 0.0, 1.0);
		}
		ENDCG
		}
	} 
	FallBack "Diffuse"
}
