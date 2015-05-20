﻿Shader "Custom/TestKernel" {
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct vertOut {
                float4 pos:SV_POSITION;
                float4 scrPos;
            };

            vertOut vert(appdata_base v) {
                vertOut o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.scrPos = ComputeScreenPos(o.pos);
                return o;
            }

            fixed4 frag(vertOut i) : SV_Target {
                float2 wcoord = (i.scrPos.xy/i.scrPos.w);
                fixed4 color;

                if (fmod(20.0*wcoord.x,2.0)<1.0) {
                    color = fixed4(wcoord.xy,0.0,1.0);
                } else {
                    color = fixed4(0.3,0.3,0.3,1.0);
                }
                return color;
            }

            ENDCG
        }
    }
}
