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
			uniform float deltaTime;
			
			float terrain(float2 uv) {
				return tex2D(terrainHeights, uv);
			}
			
			float water(float2 uv) {
				return tex2D(waterHeights, uv);
			}
			
			float flux(float2 uv1, float2 uv2) {
				return max(0,deltaTime*x*(terrain(uv1)+water(uv1)-terrain(uv2)-water(uv2)));
			}
			
			float k(float4 f, float waterHeight) {
				return min(1, waterHeight / ((f.x + f.y + f.z + f.w)*deltaTime));
			}
			
            float4 frag(v2f_img i) : SV_Target {
            
            	float2 up = float2(i.uv.x, i.uv.y+1);
            	float2 down = float2(i.uv.x, i.uv.y-1);
            	float2 left = float2(i.uv.x-1, i.uv.y);
            	float2 right = float2(i.uv.x+1, i.uv.y);
				
        		float up_flux = flux(i.uv, up);
        		float down_flux = flux(i.uv, down);
        		float left_flux = flux(i.uv, left);
        		float right_flux = flux(i.uv, right);           	
            	
            	//if up_flux + down_flux + left_flux + right_flux > water(i.uv)
            	
            	// calculate k
            	float m = k(float4(up_flux, down_flux, left_flux, right_flux), water(i.uv));
            	
            	up_flux *= m;
            	down_flux *= m;
            	left_flux *= m;
            	right_flux *= m;
            	
                return float4(up_flux, down_flux, left_flux, right_flux);
            }
            ENDCG
        }
        
        Pass {
	        CGPROGRAM
	            #pragma vertex vert_img
	            #pragma fragment frag
	            #pragma target 3.0

	            #include "UnityCG.cginc"

				uniform sampler2D flux;
				
				
				
				
	           float4 frag(v2f_img i) : SV_Target {
	            	return float4(0.0, 0.0, 0.0, 0.0);
	           	//get left flow to the right
	           	//get right flow to the left
	           	//get up flow  down
	           	//get bottom flow up
	           	
	           	//add all those up
	           	
	           	 //get up flux
	           	 //get down flux
	           	 //get left flux
	           	 //get right flux
	           	 //add all these up
	           	   
	           	   //subtract first sum from second sum
	           	   
	           	   //return this as r value in float4
	           	  
	           	    
	           }
	            ENDCG
        }
        
    }
}
