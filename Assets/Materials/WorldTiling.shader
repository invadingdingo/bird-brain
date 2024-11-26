Shader "Custom/WorldTilingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tiling ("Tiling Size", Float) = 1
        _BlendSharpness ("Blend Sharpness", Float) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _Tiling;
            float _BlendSharpness;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Calculate world position and normal
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Normalize the world normal
                float3 normal = normalize(i.worldNormal);

                // Calculate the UVs for each planar axis
                float2 uvX = i.worldPos.yz * _Tiling; // YZ plane
                float2 uvY = i.worldPos.xz * _Tiling; // XZ plane
                float2 uvZ = i.worldPos.xy * _Tiling; // XY plane

                // Sample the texture for each axis
                fixed4 texX = tex2D(_MainTex, uvX);
                fixed4 texY = tex2D(_MainTex, uvY);
                fixed4 texZ = tex2D(_MainTex, uvZ);

                // Calculate blend weights based on the normal
                float3 blendWeights = pow(abs(normal), _BlendSharpness);
                blendWeights /= dot(blendWeights, float3(1.0, 1.0, 1.0)); // Normalize weights

                // Blend the textures
                fixed4 color = texX * blendWeights.x + texY * blendWeights.y + texZ * blendWeights.z;

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}