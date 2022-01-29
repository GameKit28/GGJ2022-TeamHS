Shader "Duality/DualColorOutline" {
 
    Properties
    {
        [MainTexture]
        _MainTex("Texture", 2D) = "white" {}
        [MainColor]
        _Color("Color", Color) = (1, 1, 1, 1)
        
        _OutlineWidth("Outline Width", Range(0, 20)) = 2.0
        _OutlineIntens("Outline Texture Intensity", Range(0, 1)) = 1
        _OutlineTex("Outline Overlay Texture", 2D) = "black" {}
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

        //outline pass, sorry mom
        Pass{
            Tags{ "LightMode" = "ForwardBase" }
            Cull Front //so it's an outline and not just ... the mesh again but black and white lole
 
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                #include "Lighting.cginc"
                #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight //go fuck yourself, you get none
                #include "AutoLight.cginc"

                struct meshdata
                {
                    float4 position : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 pos : SV_POSITION;
                    SHADOW_COORDS(1) //add lighting data to texcoords
                };
        
                sampler2D _OutlineTex;
                float4 _OutlineTex_ST;
                half _OutlineWidth;
                fixed _OutlineIntens;

                v2f vert(meshdata v)
                {
                    v2f o;
                     
                    o.pos = UnityObjectToClipPos(v.position);

                    o.uv = TRANSFORM_TEX(v.uv, _OutlineTex);

                    float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal));
                    float2 offset = normalize(clipNormal.xy) / _ScreenParams.xy * _OutlineWidth * o.pos.w * 2;
                    float2 inset = normalize(clipNormal.xy) / _ScreenParams.xy * 1 * o.pos.w * 2;
 
                    o.pos.xy -= inset;
                    TRANSFER_SHADOW(o)
 
                    o.pos.xy += offset;
                    return o;
                }
 
                fixed4 frag(v2f i) : SV_TARGET
                {
                    fixed shadow = SHADOW_ATTENUATION(i);

                    return fixed4(step(shadow, 0.5), step(shadow, 0.5), step(shadow, 0.5), 1.0);
                }
 
        ENDCG
        }
        // whatever, the default didnt work so here we are
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
   
}