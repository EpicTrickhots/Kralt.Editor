Shader "Custom/TransformGizmo"
{
    Properties
    {
        _AxisColor("Axis Color", Color) = (1,1,1,1)
        [Toggle(_USE_FEATURE_ON)] _Hovered("Hovered", Float) = 0
        [Toggle(_USE_FEATURE_ON)] _Selected("Selected", Float) = 0
        _BaseGizmoScale("Gizmo Scale Multiplier", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        
        Pass
        {
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct appdata
            {
                float4 position : POSITION;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
            };
            
            float4 _AxisColor;
            float _Hovered;
            float _Selected;
            float _BaseGizmoScale;
            
            v2f vert(appdata i)
            {
                v2f o;
                
                /*
                float3 worldPivot = TransformObjectToWorld(float3(0.0, 0.0, 0.0));
                float dist = distance(worldPivot, _WorldSpaceCameraPos);
                float3 scaledLocalPos = i.position.xyz * dist * _BaseGizmoScale;
                float3 worldPos = TransformObjectToWorld(scaledLocalPos);
                */
                
                o.position = TransformObjectToHClip(i.position.xyz);
                
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                if(_Selected) return float4(1, 1, 0, 0);
                
                return _AxisColor - 0.3 * _Hovered;
            }
            
            ENDHLSL
        }
    }
}
