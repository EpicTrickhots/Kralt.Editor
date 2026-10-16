using UnityEngine;
using Kralt.Editor.InputHandling;
using Kralt.Editor.Rulesets;

namespace Kralt.Editor.Graphics.Gizmos
{
    public class TransformGizmo : KraltGizmo
    {
        [SerializeField]
        Renderer[] axes;
        
        private Vector3 initialObjectPosition, initialDragPoint, dragAxis;
        private Plane dragPlane;
        private Camera playerCamera;
        
        public bool IsDragging { get; private set; }
        
        private void Awake() => playerCamera = Camera.main;

        private void Update()
        {
            KeepGizmoScaleConstant();
            HandleGizmoInteraction();
            
            transform.position = ObjectManager.SelectedObject.transform.position;
        }
        
        private void HandleGizmoInteraction()
        {
            if(KraltInput.LmbUp)
                IsDragging = false;

            if (IsDragging)
            {
                UpdateTransformation();
                return;
            }
            
            foreach(Renderer r in axes)
            {
                r.material.SetFloat("_Hovered", 0f);
                r.material.SetFloat("_Selected", 0f);
            }
            
            Ray ray = playerCamera.ScreenPointToRay(KraltInput.MousePosition);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, KraltEditor.InstanceData.kraltGizmoLayer))
            {
                dragAxis = hitInfo.transform.GetComponent<TransformAxisGizmo>().axis;
                
                Renderer renderer = hitInfo.transform.GetComponent<Renderer>();
                renderer.material.SetFloat("_Hovered", 1f);

                if (KraltInput.LmbDown)
                {
                    renderer.material.SetFloat("_Selected", 1f);
                    InitializeTransformation();
                }
            }
        }

        private void InitializeTransformation()
        {
            initialObjectPosition = transform.position;

            // Create drag plane
            Vector3 cameraDirection = playerCamera.transform.right;
            Vector3 planeNormal = Vector3.Cross(cameraDirection, dragAxis).normalized;
            dragPlane = new Plane(planeNormal, transform.position);

            // Find where the mouse ray intersects with the drag plane
            Ray ray = playerCamera.ScreenPointToRay(KraltInput.MousePosition);
            
            if (dragPlane.Raycast(ray, out float d))
            {
                initialDragPoint = ray.GetPoint(d);
                IsDragging = true;
            }
        }
        
        private void UpdateTransformation()
        {
            Ray ray = playerCamera.ScreenPointToRay(KraltInput.MousePosition);

            if (dragPlane.Raycast(ray, out float d))
            {
                Vector3 currentDragPoint = ray.GetPoint(d);
                
                // Project hit point onto the selected axis
                float distance = Vector3.Dot(currentDragPoint - initialDragPoint, dragAxis);
                distance = KraltInput.KeyHeld(KraltInput.EDITOR_GRID_SNAP) ? Mathf.Round(distance) : distance;
                ObjectManager.SelectedObject.transform.position = initialObjectPosition + dragAxis * distance;
            }
        }
    }
}
