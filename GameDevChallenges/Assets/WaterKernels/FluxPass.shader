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
			
			float flux(float2 uv1, float2 uv2) {
				return max(0, deltaTime*x*(terrain(uv1)+water(uv1)-terrain(uv2)-water(uv2)));		
			}
			
			float k(float4 f, float waterHeight) {
				return min(1,waterHeight/((f.x+f.y+f.z+f.w)*deltaTime));
			}
			
			float4 frag(v2f_img i) : SV_Target {
				
			}
			
			ENDCG
		}
	}
}
