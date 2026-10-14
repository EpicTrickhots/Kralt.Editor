using UnityEngine.EventSystems;
using UnityEngine;

namespace Aaron.Toolkit.Graphics.UserInterface
{
    [RequireComponent(typeof(RectTransform))]
    public class FlexBox : UIBehaviour
    {
        [Header("UI References")]
        [SerializeField] RectTransform primary;
        [SerializeField] RectTransform secondary;
        
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            
            
        }
    }
}
