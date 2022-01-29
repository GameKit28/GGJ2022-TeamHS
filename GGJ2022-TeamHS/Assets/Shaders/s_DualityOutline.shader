Shader "Duality/DualColorOutline" {
 
    Properties
    {
        [MainTexture]
        _MainTex("Texture", 2D) = "white" {}
        [MainColor]
        _Color("Color", Color) = (1, 1, 1, 1)
        
        _OutlineWidth("Outline Width", Range(0, 10)) = 0.03
    }
 
    Subshader
    {
    Tags
    {
        "RenderType" = "Opaque"
    }
 
    CGPROGRAM
        //basic surface shader
        #pragma surface surf Standard fullforwardshadows
        
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR;
        };
 
        half4 _Color;
 
        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 t = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = t.rgb * IN.color.rgb;
            o.Smoothness = 0;
            o.Metallic = 0;
            o.Alpha = _Color.a * IN.color.a;
        }
    ENDCG

        
        // whatever, the default didnt work so here we are
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
   
}