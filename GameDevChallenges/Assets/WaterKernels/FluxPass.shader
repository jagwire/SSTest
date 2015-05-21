Shader "Custom/FluxPass" {
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"	
				
			ENDCG
		}
	}
}
