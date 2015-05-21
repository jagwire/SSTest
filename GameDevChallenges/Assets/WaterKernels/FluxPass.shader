Shader "Custom/FluxPass" {
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"	
			
			uniform sampler2D waterHeights;
			uniform sampler2D terrainHeights;
			uniform float x;
			uniform float deltaTime;
				
			float terrain(float2 uv) {
				return tex2D(terrainHeights, uv);
			}
				
			float water(float2 uv) {
				return tex2D(waterHeights, uv);
			}	
				
			ENDCG
		}
	}
}
