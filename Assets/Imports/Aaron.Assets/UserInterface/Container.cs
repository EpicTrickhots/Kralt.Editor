using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Aaron.Toolkit.Graphics
{
    [RequireComponent(typeof(CanvasRenderer))]
    [AddComponentMenu("UI (Canvas)/Container")]
    public class Container : MaskableGraphic
    {
        [Serializable]
        public struct ContainerFlags
        {
            public float borderRadius;
            public float borderWidth;
            public Color borderColor;
            public float fWidthMul;
            [HideInInspector]
            public Color tint;
        }

        [SerializeField]
        protected ContainerFlags containerSettings;

        UIVertex CreateVertex(float x, float y, Color color, Vector4 uv0, Vector4 uv1, Vector4 uv2, Vector4 uv3)
        {
            UIVertex v = new()
            {
                position = new Vector2(x, y),
                color = color,
                uv0 = uv0,
                uv1 = uv1,
                uv2 = uv2,
                uv3 = uv3,
            };
            
            return v;
        }
        
        /// <summary>
        /// Pulses a color over this container.
        /// Likely inefficient because it calls SetVerticesDirty() for every frame of the animation.
        /// </summary>
        /// <param name="pulseColor">Color of the pulse</param>
        /// <param name="duration">Time in seconds of the pulse</param>
        /// <returns></returns>
        protected IEnumerator Pulse(Color pulseColor, float duration)
        {
            containerSettings.tint = pulseColor;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                containerSettings.tint = Color.LerpUnclamped(pulseColor, Color.clear, t);
                elapsed += Time.deltaTime;
                SetVerticesDirty();
            }
            
            containerSettings.tint = Color.clear;
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            Rect rect = rectTransform.rect;
            
            Vector4 uv2 = new(rect.size.x, rect.size.y, containerSettings.fWidthMul);
            Vector4 uv1 = new(Mathf.Min(containerSettings.borderRadius, Mathf.Min(uv2.x / 2f, uv2.y / 2f)), containerSettings.borderWidth);
            Vector4 uv3 = (Vector4)(containerSettings.borderColor + containerSettings.tint);
            
            vh.AddVert(CreateVertex(rect.xMin, rect.yMin, color + containerSettings.tint, new(0, 0), uv1, uv2, uv3));
            vh.AddVert(CreateVertex(rect.xMax, rect.yMin, color + containerSettings.tint, new(1, 0), uv1, uv2, uv3));
            vh.AddVert(CreateVertex(rect.xMax, rect.yMax, color + containerSettings.tint, new(1, 1), uv1, uv2, uv3));
            vh.AddVert(CreateVertex(rect.xMin, rect.yMax, color + containerSettings.tint, new(0, 1), uv1, uv2, uv3));

            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(0, 2, 3);
        }
    }
}
