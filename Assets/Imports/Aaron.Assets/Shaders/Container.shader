Shader "Jura/Container"
{
    Properties
    {
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv0 : TEXCOORD0;
                float3 uv1 : TEXCOORD1;
                float3 uv2 : TEXCOORD2;
                float4 uv3 : TEXCOORD3;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                float2 uv0 : TEXCOORD0;
                float3 uv1 : TEXCOORD1;
                float3 uv2 : TEXCOORD2;
                float4 uv3 : TEXCOORD3;
            };
            
            float CornerRadius(float2 p, float2 rectSize, float radius)
            {
                float2 halfSize = rectSize * 0.5;
                float2 q = abs(p) - halfSize + radius;
                return length(max(q, 0.0)) + min(max(q.x, q.y), 0.0) - radius;
            }
            
            v2f vert(appdata i)
            {
                v2f o;

                o.positionCS = UnityObjectToClipPos(i.positionOS);
                o.color = half4(UIGammaToLinear(i.color), i.color.a);
                o.uv0 = i.uv0;
                o.uv1 = i.uv1;
                o.uv2 = i.uv2;
                o.uv3 = i.uv3;
                
                return o;
            }
            
            float4 frag(v2f i) : SV_Target
            {
                float borderRadius = i.uv1.x;
                float borderWidth = i.uv1.y;
                
                float2 rectSize = i.uv2;
                float4 borderColor = i.uv3;
                
                float2 p = (i.uv0 - 0.5) * rectSize;
                float outerDistance = CornerRadius(p, rectSize, borderRadius);
                float innerDistance = outerDistance + borderWidth;
                
                float smoothing1 = fwidth(innerDistance) * i.uv2.z;
                float smoothing2 = fwidth(outerDistance) * i.uv2.z;
                
                float interpolationT = 1.0 - smoothstep(-smoothing1, smoothing1, innerDistance);
                float alpha = 1.0 - smoothstep(-smoothing2, smoothing2, outerDistance);
                
                if(borderWidth <= 1e-3)
                {
                    return float4(i.color.rgb, i.color.a * alpha);
                }
                
                float4 linearBorderColor = float4(UIGammaToLinear(borderColor.rgb), borderColor.a);
                float4 finalColor = lerp(linearBorderColor, i.color, interpolationT);
                
                return float4(finalColor.rgb, finalColor.a * alpha);
            }

            ENDCG
        }
    }
}
