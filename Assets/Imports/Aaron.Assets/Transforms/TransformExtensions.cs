using System;
using System.Collections;
using UnityEngine;

namespace Aaron.Toolkit.Transforms
{
    /// <summary>
    /// Ugly ass script
    /// </summary>
    public static class TransformExtensions
    {
        public static IEnumerator GenericTween(float duration, Action<float> interpolation)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);
                interpolation(t);
                elapsed += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            
            interpolation(1f);
        }

        #region Transform

        public static IEnumerator MoveTo(this Transform transform, Vector3 target, float duration = 1f, Easing easing = Easing.None)
        {
            Vector3 start = transform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.position = Vector3.LerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.position = target;
        }

        public static IEnumerator LocalMoveTo(this Transform transform, Vector2 target, float duration = 1f, Easing easing = Easing.None)
        {
            Vector2 start = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.localPosition = Vector2.LerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.localPosition = target;
        }

        public static IEnumerator ScaleTo(this Transform transform, Vector2 target, float duration = 1f, Easing easing = Easing.None)
        {
            Vector2 start = transform.localScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.localScale = Vector2.LerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.localScale = target;
        }
        
        public static IEnumerator RotateTo(this Transform transform, Quaternion target, float duration = 1f, Easing easing = Easing.None)
        {
            Quaternion start = transform.rotation;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.rotation = Quaternion.SlerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.rotation = target;
        }

        public static IEnumerator RotateTo(this Transform transform, Vector3 target, float duration = 1f, Easing easing = Easing.None)
        {
            Quaternion start = transform.rotation;
            Quaternion targetQuat = Quaternion.Euler(target);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.rotation = Quaternion.SlerpUnclamped(start, targetQuat, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.rotation = targetQuat;
        }

        public static IEnumerator LocalRotateTo(this Transform transform, Quaternion target, float duration = 1f, Easing easing = Easing.None)
        {
            Quaternion start = transform.localRotation;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.localRotation = Quaternion.SlerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.localRotation = target;
        }

        public static IEnumerator LocalRotateTo(this Transform transform, Vector3 target, float duration = 1f, Easing easing = Easing.None)
        {
            Quaternion start = transform.localRotation;
            Quaternion targetQuat = Quaternion.Euler(target);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.localRotation = Quaternion.SlerpUnclamped(start, targetQuat, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.localRotation = targetQuat;
        }

        #endregion

        #region Rect Transform

        public static IEnumerator MoveTo(this RectTransform transform, Vector3 target, float duration = 1f, Easing easing = Easing.None)
        {
            Vector3 start = transform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.position = Vector3.LerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }

            transform.position = target;
        }

        public static IEnumerator LocalMoveTo(this RectTransform transform, Vector2 target, float duration = 1f, Easing easing = Easing.None)
        {
            Vector2 start = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.localPosition = Vector2.LerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }

            transform.localPosition = target;
        }

        public static IEnumerator AnchoredMoveTo(this RectTransform transform, Vector2 target, float duration = 1f, Easing easing = Easing.None)
        {
            Vector2 start = transform.anchoredPosition;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.anchoredPosition = Vector2.LerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.anchoredPosition = target;
        }

        public static IEnumerator SizeDeltaTo(this RectTransform transform, Vector2 target, float duration = 1f, Easing easing = Easing.None)
        {
            Vector2 start = transform.sizeDelta;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.sizeDelta = Vector2.LerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.sizeDelta = target;
        }
        
        public static IEnumerator ScaleTo(this RectTransform transform, Vector2 target, float duration = 1f, Easing easing = Easing.None)
        {
            Vector2 start = transform.localScale;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.localScale = Vector2.LerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.localScale = target;
        }

        public static IEnumerator RotateTo(this RectTransform transform, Quaternion target, float duration = 1f, Easing easing = Easing.None)
        {
            Quaternion start = transform.rotation;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.rotation = Quaternion.SlerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.rotation = target;
        }

        public static IEnumerator RotateTo(this RectTransform transform, Vector3 target, float duration = 1f, Easing easing = Easing.None)
        {
            Quaternion start = transform.rotation;
            Quaternion targetQuat = Quaternion.Euler(target);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.rotation = Quaternion.SlerpUnclamped(start, targetQuat, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.rotation = targetQuat;
        }

        public static IEnumerator LocalRotateTo(this RectTransform transform, Quaternion target, float duration = 1f, Easing easing = Easing.None)
        {
            Quaternion start = transform.localRotation;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.localRotation = Quaternion.SlerpUnclamped(start, target, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.localRotation = target;
        }
        
        public static IEnumerator LocalRotateTo(this RectTransform transform, Vector3 target, float duration = 1f, Easing easing = Easing.None)
        {
            Quaternion start = transform.localRotation;
            Quaternion targetQuat = Quaternion.Euler(target);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return new WaitForEndOfFrame();
                float t = Mathf.Clamp01(elapsed / duration);
                transform.localRotation = Quaternion.SlerpUnclamped(start, targetQuat, EasingFunction.Evaluate(easing, t));
                elapsed += Time.deltaTime;
            }
            
            transform.localRotation = targetQuat;
        }

        #endregion
    }
}
