using System;
using UnityEngine;

namespace Aaron.Toolkit.Transforms
{
    public static class EasingFunction
    {
        private const float elastic_const = 20.943951023931955f;
        private const float elastic_const2 = 0.075f;
        private const float back_const = 1.70158f;
        private const float back_const2 = 2.5949095f;
        private const float bounce_const = 0.36363636363636365f;
        
        private static readonly float expo_offset = Mathf.Pow(2.0f, -10.0f);
        private static readonly float elastic_offset_full = Mathf.Pow(2.0f, -11.0f);
        private static readonly float elastic_offset_half = Mathf.Pow(2.0f, -10.0f) * Mathf.Sin(8.901179185171081f);
        private static readonly float elastic_offset_quarter = Mathf.Pow(2.0f, -10.0f) * Mathf.Sin(3.665191429188092f);
        private static readonly float in_out_elastic_offset = Mathf.Pow(2.0f, -10.0f) * Mathf.Sin(12.391837689159738f);
        
        public static float Evaluate(Easing easing, float time)
        {
            switch (easing)
            {
                #region Quadratic

                case Easing.Out:
                case Easing.OutQuad:
                return time * (2.0f - time);

                case Easing.In:
                case Easing.InQuad:
                return time * time;

                case Easing.InOutQuad:
                if (time < 0.5f)
                    return time * time * 2.0f;

                return (time -= 1.0f) * time * -2.0f + 1.0f;

                #endregion

                #region Cubic

                case Easing.InCubic:
                return time * time * time;

                case Easing.OutCubic:
                return (time -= 1.0f) * time * time + 1.0f;

                case Easing.InOutCubic:
                if (time < 0.5f)
                    return time * time * time * 4.0f;

                return (time -= 1.0f) * time * time * 4.0f + 1.0f;

                #endregion

                #region Quartic

                case Easing.InQuart:
                return time * time * time * time;

                case Easing.OutQuart:
                return 1.0f - (time -= 1.0f) * time * time * time;

                case Easing.InOutQuart:
                if (time < 0.5f)
                    return time * time * time * time * 8.0f;

                return (time -= 1.0f) * time * time * time * -8.0f + 1.0f;

                #endregion

                #region Quintic

                case Easing.InQuint:
                return time * time * time * time * time;

                case Easing.OutQuint:
                return (time -= 1.0f) * time * time * time * time + 1.0f;

                case Easing.InOutQuint:
                if (time < 0.5f)
                    return time * time * time * time * time * 16.0f;

                return (time -= 1.0f) * time * time * time * time * 16.0f + 1.0f;

                #endregion

                #region Sine
                
                case Easing.InSine:
                return 1.0f - Mathf.Cos(time * Mathf.PI * 0.5f);

                case Easing.OutSine:
                return Mathf.Sin(time * Mathf.PI * 0.5f);

                case Easing.InOutSine:
                return 0.5f - 0.5f * Mathf.Cos(Mathf.PI * time);

                #endregion

                #region Exponential

                case Easing.InExpo:
                return Mathf.Pow(2.0f, 10.0f * (time - 1.0f)) + expo_offset * (time - 1.0f);

                case Easing.OutExpo:
                return -Mathf.Pow(2.0f, -10.0f * time) + 1.0f + expo_offset * time;

                case Easing.InOutExpo:
                if (time < 0.5)
                    return 0.5f * (Mathf.Pow(2.0f, 20.0f * time - 10.0f) + expo_offset * (2.0f * time - 1.0f));

                return 1.0f - 0.5f * (Mathf.Pow(2.0f, -20.0f * time + 10.0f) + expo_offset * (-2.0f * time + 1.0f));

                #endregion

                #region Circular

                case Easing.InCirc:
                return 1.0f - Mathf.Sqrt(1.0f - time * time);

                case Easing.OutCirc:
                return Mathf.Sqrt(1.0f - (time -= 1.0f) * time);

                case Easing.InOutCirc:
                if ((time *= 2.0f) < 1.0f)
                    return 0.5f - 0.5f * Mathf.Sqrt(1.0f - time * time);

                return 0.5f * Mathf.Sqrt(1.0f - (time -= 2.0f) * time) + 0.5f;

                #endregion

                #region Elastic

                case Easing.InElastic:
                return -Mathf.Pow(2.0f, -10.0f + 10.0f * time) * Mathf.Sin((0.925f - time) * elastic_const) + elastic_offset_full * (1.0f - time);

                case Easing.OutElastic:
                return Mathf.Pow(2.0f, -10.0f * time) * Mathf.Sin((time - elastic_const2) * elastic_const) + 1.0f - elastic_offset_full * time;

                case Easing.OutElasticHalf:
                return Mathf.Pow(2.0f, -10.0f * time) * Mathf.Sin((0.5f * time - elastic_const2) * elastic_const) + 1.0f - elastic_offset_half * time;

                case Easing.OutElasticQuarter:
                return Mathf.Pow(2.0f, -10.0f * time) * Mathf.Sin((0.25f * time - elastic_const2) * elastic_const) + 1.0f - elastic_offset_quarter * time;

                case Easing.InOutElastic:
                if ((time *= 2.0f) < 1.0f)
                    return -0.5f * (Mathf.Pow(2.0f, -10.0f + 10.0f * time) * Mathf.Sin((0.8875f - time) * elastic_const / 1.5f) - in_out_elastic_offset * (1.0f - time));
                return 0.5f * (Mathf.Pow(2.0f, -10.0f * (time -= 1.0f)) * Mathf.Sin((time - 0.1125f) * elastic_const / 1.5f) - in_out_elastic_offset * time) + 1.0f;

                #endregion

                #region Back

                case Easing.InBack:
                return time * time * (2.70158f * time - back_const);

                case Easing.OutBack:
                return (time -= 1.0f) * time * (2.70158f * time + back_const) + 1.0f;

                case Easing.InOutBack:
                if ((time *= 2.0f) < 1.0f)
                    return 0.5f * time * time * (3.5949095f * time - back_const2);

                return 0.5f * ((time -= 2.0f) * time * (3.5949095f * time + back_const2) + 2.0f);

                #endregion

                #region Bounce

                case Easing.InBounce:
                time = 1.0f - time;

                if (time < bounce_const)
                    return 1.0f - 7.5625f * time * time;

                if (time < 0.727272f)
                    return 1.0f - (7.5625f * (time -= 0.5454545454545454f) * time + 0.75f);

                if (time < 0.9090909090909092f)
                    return 1.0f - (7.5625f * (time -= 0.8181818181818182f) * time + 0.9375f);

                return 1.0f - (7.5625f * (time -= 0.9545454545454546f) * time + 0.984375f);

                case Easing.OutBounce:
                if (time < bounce_const)
                    return 7.5625f * time * time;
                
                if (time < 0.7272727272727273f)
                    return 7.5625f * (time -= 0.5454545454545454f) * time + 0.75f;
                
                if (time < 0.9090909090909092f)
                    return 7.5625f * (time -= 0.8181818181818182f) * time + 0.9375f;
                
                return 7.5625f * (time -= 0.9545454545454546f) * time + 0.984375f;
                
                case Easing.InOutBounce:
                if (time < 0.5f)
                    return 0.5f - 0.5f * Evaluate(Easing.OutBounce, 1.0f - time * 2.0f);
                
                return Evaluate(Easing.OutBounce, (time - 0.5f) * 2.0f) * 0.5f + 0.5f;

                #endregion

                #region Out^10

                case Easing.OutPow10:
                return (time -= 1.0f) * Mathf.Pow(time, 10.0f) + 1.0f;

                #endregion
                
                #region None

                default:
                return time;

                #endregion
            }
        }
    }

    // http://easings.net
    public enum Easing
    {
        None                    = 0,
        Out                     = 1,
        In                      = 2,
        InQuad                  = 3,
        OutQuad                 = 4,
        InOutQuad               = 5,
        InCubic                 = 6,
        OutCubic                = 7,
        InOutCubic              = 8,
        InQuart                 = 9,
        OutQuart                = 10,
        InOutQuart              = 11,
        InQuint                 = 12,
        OutQuint                = 13,
        InOutQuint              = 14,
        InSine                  = 15,
        OutSine                 = 16,
        InOutSine               = 17,
        InExpo                  = 18,
        OutExpo                 = 19,
        InOutExpo               = 20,
        InCirc                  = 21,
        OutCirc                 = 22,
        InOutCirc               = 23,
        InElastic               = 24,
        OutElastic              = 25,
        OutElasticHalf          = 26,
        OutElasticQuarter       = 27,
        InOutElastic            = 28,
        InBack                  = 29,
        OutBack                 = 30,
        InOutBack               = 31,
        InBounce                = 32,
        OutBounce               = 33,
        InOutBounce             = 34,
        OutPow10                = 35
    }
}
