﻿Shader "Custom/TestKernel" {
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

			uniform sampler2D _MainTex;

            float4 frag(v2f_img i) : SV_Target {
                return float4(0.0,1.0,0.0,1.0);
            }
            ENDCG
        }
    }
}
