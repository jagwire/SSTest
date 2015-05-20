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
			
			
			
            float4 frag(v2f_img i) : SV_Target {
                return float4(0.0,1.0,0.0,1.0);
            }
            ENDCG
        }
    }
}
