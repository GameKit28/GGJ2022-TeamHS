Shader "Duality/DualColorOutline" {

    Properties
    {
        [MainTexture]
        _MainTex("Texture", 2D) = "white" {}
        [MainColor]
        _Color("Color", Color) = (1, 1, 1, 1)
        [MainBump]
        _BumpMap("Normal Map", 2D) = "bump" {}

        _Brightness("Brightness", Range(0,1)) = 0.3
        _CelCutoff("Max Distance", Float) = 5.0
        _OutlineThreshold("Outline Threshole", Range(0.01,0.99)) = 0.5
        _OutlineWidth("Outline Width", Range(0, 20)) = 2.0
        _OutlineIntens("Outline Texture Intensity", Range(0, 1)) = 1
        _OutlineTex("Outline Overlay Texture", 2D) = "black" {}

        _DissolveTexture("Dissolve Texutre", 2D) = "white" {}
        _Amount("Amount", Range(0,1)) = 0
    }

    Subshader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

            Pass
            {
                CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
                    // make fog work
                    #pragma multi_compile_fog

                    #include "UnityCG.cginc"

                    struct appdata
                    {
                        float4 vertex : POSITION;
                        float2 uv : TEXCOORD0;
                        float3 normal : NORMAL; //normal object space
                    };

                    struct v2f
                    {
                        float2 uv : TEXCOORD0;
                        float4 vertex : SV_POSITION;
                        half3 worldNormal : NORMAL; //adding world normals so we can... use world normals...
                    };

                    sampler2D _MainTex;
                    sampler2D _BumpMap;
                    float4 _MainTex_ST;
                    float _Brightness;


                    float CellShade(float3 normal, float3 lightDir)
                    {
                        float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));

                        return floor(NdotL / 0.3); //dropping output to below 0 so it fragments shadows into solid parts
                    }

                    v2f vert(appdata v)
                    {
                        v2f o;
                        o.vertex = UnityObjectToClipPos(v.vertex);
                        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                        o.worldNormal = UnpackNormal(tex2Dlod(_BumpMap, float4(v.uv, 1, 0))); //takes object normal and converts it to world space
                        o.worldNormal -= float3(0, 0, 0.5);
                        o.worldNormal = o.worldNormal + (UnityObjectToWorldNormal(v.normal));
                        return o;
                    }

                    fixed4 frag(v2f i) : SV_Target
                    {
                        // sample the texture
                        fixed4 col = tex2D(_MainTex, i.uv);
                        col *= CellShade(i.worldNormal, _WorldSpaceLightPos0.xyz)+_Brightness; //takes the color of our texture and multiplies it by the function i made earlier
                        return col;
                    }
                ENDCG
            }
            Pass
            {
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
                    float _OutlineThreshold;

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
                        fixed shadow = SHADOW_ATTENUATION(i); //this gives us the shadowing
                        fixed4 stepShade = fixed4(step(shadow, _OutlineThreshold), step(shadow, _OutlineThreshold), step(shadow, _OutlineThreshold), 1.0); //this is applying the shadow data to a vector4, clamped at the value of the threshold

                        return stepShade;
                        //return fixed4(step(shadow, _OutlineThreshold), step(shadow, _OutlineThreshold), step(shadow, _OutlineThreshold), 1.0);
                    }

            ENDCG
            }
            //outline pass, sorry mom
            Pass
            {
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
                    float _OutlineThreshold;

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
                        fixed shadow = SHADOW_ATTENUATION(i); //this gives us the shadowing
                        fixed4 stepShade = fixed4(step(shadow, _OutlineThreshold), step(shadow, _OutlineThreshold), step(shadow, _OutlineThreshold), 1.0); //this is applying the shadow data to a vector4, clamped at the value of the threshold

                        return stepShade;
                        //return fixed4(step(shadow, _OutlineThreshold), step(shadow, _OutlineThreshold), step(shadow, _OutlineThreshold), 1.0);
                    }

            ENDCG
            }
            // whatever, the default didnt work so here we are
            UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
            
            Pass
            {
                Tags { "RenderType" = "Opaque" }
                LOD 200
                Cull Off //Fast way to turn your material double-sided

                CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members uv_MainTex)
#pragma exclude_renderers d3d11
                    #pragma vertex vert
                    #pragma fragment frag
                    #pragma target 3.0

                    sampler2D _MainTex;

                    struct meshdata
                    {
                        float2 uv_MainTex;
                        float2 uv : TEXCOORD0;
                        float4 vertex : POSITION;
                    };

                    half _Glossiness;
                    half _Metallic;
                    fixed4 _Color;

                    struct v2f 
                    {
                        float4 vertex : POSITION;
                        float2 uv : TEXCOORD0;
                        float2 uv_MainTex;
                    };

                    //Dissolve properties
                    sampler2D _DissolveTexture;
                    half _Amount;

                    v2f vert(meshdata v) 
                    {
                        v2f o;

                        o.vertex = UnityObjectToClipPos(v.vertex);
                        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                        
                        return o;
                    }

                    fixed4 frag(v2f i) : SV_Target
                    {
                        
                        //Dissolve function
                        half dissolve_value = tex2D(_DissolveTexture, i.uv_MainTex).r;
                        fixed3 clipper = clamp(dissolve_value - _Amount);

                        //Basic shader function
                        fixed4 c = tex2D(_MainTex, i.uv_MainTex) * _Color;

                        return clipper;
                    }
                ENDCG
            }
                UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }

}