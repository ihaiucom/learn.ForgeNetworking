Shader "S_Game_Hero/Hero_Battle" {
Properties {
  _Color ("Main Color", Color) = (1,1,1,1)

   _MainTex ("Diffuse (RGB)", 2D) = "grey" {}
   _LightTex ("LightTex (RGB)", 2D) = "grey" {}
   _LightScale ("LightScale", Float) = 2
   _HurtColor ("HurtColor", Vector) = (0,0,0,0)
   _EffectTex ("EffectTex (RGB)", 2D) = "white" {}
   _EffectFactor ("EffectFactor", Float) = 0.85
   _EffectTexScale ("EffectTexScale", Float) = 1
  [HideInInspector]  _PlayerId ("Player ID", Float) = 0
}


Category {
  Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
  Fog { Mode Off }

  SubShader {
    CGPROGRAM
    #pragma surface surf Lambert
    struct Input {
      float2 uv_MainTex;
      float2 uv2_LightTex;
    };
    sampler2D _MainTex;
    sampler2D _LightTex;
    fixed4 _Color;
    void surf (Input IN, inout SurfaceOutput o)
    {
      o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _Color;
      half4 lm = tex2D (_LightTex, IN.uv2_LightTex);
      o.Emission = lm.rgb*o.Albedo.rgb;
      o.Alpha = lm.a * _Color.a;
    }
    ENDCG
  }
FallBack "Legacy Shaders/Lightmapped/VertexLit"
}
}