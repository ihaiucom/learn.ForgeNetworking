Shader "S_Game_Hero/Hero_Show2" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _MaskTex ("Mask (R,G,B)", 2D) = "white" {}
 _SpecColor ("Spec Color", Color) = (0,0,0,0)
 _SpecPower ("Spec Power", Range(1,128)) = 15
 _SpecMultiplier ("Spec Multiplier", Float) = 1
 _RampMap ("Ramp Map", 2D) = "white" {}
 _ShadowColor ("Shadow Color", Color) = (0,0,0,0)
 _LightTex ("轮廓光 (RGB)", 2D) = "white" {}
 _NormalTex ("Normal", 2D) = "bump" {}
 _NoiseTex ("Noise(RGB)", 2D) = "white" {}
 _Scroll2X ("Noise speed X", Float) = 1
 _Scroll2Y ("Noise speed Y", Float) = 0
 _NoiseColor ("Noise Color", Color) = (1,1,1,1)
 _MMultiplier ("Layer Multiplier", Float) = 2
 _ReflectTex ("Reflect(RGB)", 2D) = "white" {}
 _ReflectColor ("Reflect Color", Color) = (1,1,1,1)
 _ReflectPower ("Reflect Power", Range(0.1,5)) = 1
 _ReflectionMultiplier ("Reflection Multiplier", Float) = 2
 _Offset ("Height", Float) = 0.8
 _HeightColor ("Height Color", Color) = (0.5,0.5,0.5,1)
 _HeightLightCompensation ("Height Light Compensation", Float) = 1
}


Category {
  Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
  Fog { Mode Off }

  SubShader {
    CGPROGRAM
    #pragma surface surf Lambert
    struct Input {
      float2 uv_MainTex;
    };
    sampler2D _MainTex;
    void surf (Input IN, inout SurfaceOutput o)
    {
      o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb ;
      o.Emission = o.Albedo.rgb * 0.5;
    }
    ENDCG
  }
FallBack "Legacy Shaders/Lightmapped/VertexLit"
}
}