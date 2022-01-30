Shader "Duality/DualCellShade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            float4 _MainTex_ST;

            float CellShade(float3 normal, float3 lightDir)
            {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));

                return floor(NdotL / 0.3); //dropping output to below 0 so it fragments shadows into solid parts
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal); //takes object normal and converts it to world space
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= CellShade(i.worldNormal, _WorldSpaceLightPos0.xyz); //takes the color of our texture and multiplies it by the function i made earlier
                return col;
            }
            ENDCG
        }
    }
}
