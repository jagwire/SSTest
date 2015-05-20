Shader "Custom/TestKernel" {
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D waterHeights;
			uniform sampler2D terrainHeights;
			uniform float x;
			
			float terrain(float2 uv) {
				return tex2D(terrainHeights, uv);
			}
			
			float water(float2 uv) {
				return tex2D(waterHeights, uv);
			}
			
			float flux(float2 uv1, float2 uv2) {
				Max(0,x*(terrain(uv1)+water(uv1)-terrain(uv2)-water(uv2));
			}
			
            float4 frag(v2f_img i) : SV_Target {
            
            	float2 up = float2(i.uv.x, i.uv.y+1);
            	float2 down = float(i.uv.x, i.uv.y-1);
            
            
                return float4(0.0,1.0,0.0,1.0);
            }
            ENDCG
        }
    }
}
