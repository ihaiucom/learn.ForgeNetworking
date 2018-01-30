#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "GOK/Hero"
{
	Properties
	{
	    _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Diffuse (RGB)", 2D) = "grey" {}
        [Toggle] _Light_Tex ("Light Tex On?", Float) = 0
        _LightTex ("LightTex (RGB)", 2D) = "grey" {}
        _LightScale ("LightScale", Float) = 2
        [Toggle] _Hurt_Effect ("Hurt Effect On?", Float) = 0
        _HurtColor ("HurtColor", Color) = (0,0,0,0)
        [Toggle] _Effect_Tex ("Effect Tex On?", Float) = 0
        _EffectTex ("EffectTex (RGB)", 2D) = "white" {}
        _EffectFactor ("EffectFactor", Range(0,1)) = 0.85
        _EffectTexScale ("EffectTexScale", Float) = 1
        // 在GOK里，_RimColor 是一个全局变量，所有Shader共用
        [Toggle] _Rim_Color("Rim Color On?", Float) = 1
        _RimColor ("Rim Color", Color) = (0,0,0,0)        
        [HideInInspector]  _PlayerId ("Player ID", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma shader_feature _LIGHT_TEX_ON 
			#pragma shader_feature _HURT_EFFECT_ON
			#pragma shader_feature _EFFECT_TEX_ON
			#pragma shader_feature _RIM_COLOR_ON
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			// #pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				#if _RIM_COLOR_ON || _LIGHT_TEX_ON
				float3 viewNormal : TEXCOORD1;
				#endif
				#if _RIM_COLOR_ON
				float3 viewDir : TEXCOORD2;
				#endif							
				float4 vertex : SV_POSITION;				
			};
			
            fixed4 _Color;
			sampler2D _MainTex;
			
			// Picking
			float _PlayerId;
			
            #if _LIGHT_TEX_ON
			sampler2D _LightTex;
			float _LightScale;
			#endif
			
			#if _EFFECT_TEX_ON
			sampler2D _EffectTex;
			float _EffectTexScale;
			float _EffectFactor;
			#endif
			
			#if _HURT_EFFECT_ON
            fixed4 _HurtColor;
            #endif
            
            #if _RIM_COLOR_ON
			fixed4 _RimColor;
			#endif
			
			
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				
				#if _RIM_COLOR_ON || _LIGHT_TEX_ON
				o.viewNormal = mul(UNITY_MATRIX_MV, v.normal);
				#endif
				
				#if _RIM_COLOR_ON				
				o.viewDir = mul(UNITY_MATRIX_V, (_WorldSpaceCameraPos - (mul(unity_ObjectToWorld, v.vertex).xyz * 1.0))).xyz;
				#endif
								
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// Main Texture
				fixed3 col = tex2D(_MainTex, i.uv).xyz * _Color;
				
				#if _RIM_COLOR_ON || _LIGHT_TEX_ON
				fixed3 viewNormal = normalize(i.viewNormal);
				#endif				
				
		        #if _LIGHT_TEX_ON
		        float2 uvLight = viewNormal * 0.5 + 0.5;
		        fixed3 lightColor = tex2D(_LightTex, uvLight).xyz;
		        col = col * lightColor * _LightScale;
		        #endif
		        
		        #if _EFFECT_TEX_ON
		        fixed3 eff = tex2D(_EffectTex, i.uv).xyz * _EffectTexScale;
		        col = lerp(col, eff, _EffectFactor);
		        #endif
		        				
		        #if _RIM_COLOR_ON
				fixed3 rim = clamp (dot (normalize(i.viewDir), viewNormal), 0.0, 1.0);
				col += _RimColor.xyz * pow (1.0 - rim, _RimColor.w);
				#endif
				
				#if _HURT_EFFECT_ON
				col += _HurtColor.xyz;
				#endif				
								
				return float4(col, _PlayerId);
			}
			ENDCG
		}
	}
}
