using UnityEngine;

namespace Kralt.Editor.Extensions
{
    public static class LayerMaskExtensions
    {
        public static int LayerIndex(this LayerMask layer)
        {
            int maskValue = layer.value;
            int index = 0;
            
            while (maskValue > 1)
            {
                maskValue >>= 1;
                index++;
            }
            
            return index;
        }
    }
}
