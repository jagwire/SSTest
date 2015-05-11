Shader "Custom/NewShader" {
	SubShader {
		Tags { "Queue"="Transparent"  "RenderType"="Transparent"}
		Pass {
			Material {
				Diffuse(1,1,1,0.1)
				
			}
			Lighting On
			Cull Back
		}
	}
	
	 
	FallBack "Diffuse"
}
