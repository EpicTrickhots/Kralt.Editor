using UnityEngine;

namespace Kralt.Editor.Graphics.Gizmos
{
    public class KraltGizmo : MonoBehaviour
    {
        [SerializeField]
        protected float gizmoScale = 1f;
        
        protected void KeepGizmoScaleConstant()
        {
            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            transform.localScale = distance * gizmoScale * Vector3.one;
        }
    }
}
