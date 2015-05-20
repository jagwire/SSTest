﻿Shader "Custom/TestKernel" {
    SubShader {
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            float4 vert(appdata_base v) : POSITION {
                return mul (UNITY_MATRIX_MVP, v.vertex);
            }

            fixed4 frag(float4 sp:WPOS) : SV_Target {
                return fixed4(sp.xy/_ScreenParams.xy,0.0,1.0);
            }

            ENDCG
        }
    }
}
