using UnityEngine;
using Kralt.Editor.InputHandling;
using Kralt.Editor.Graphics;
using Kralt.Editor.Rulesets;

namespace Kralt.Editor.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class EditorPlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] float damping = 10.0f;
        [SerializeField] int speedIndex = 0;
        [SerializeField] float[] speeds;
        
        [Header("Camera")]
        [SerializeField] float mouseSensitivity = 2.0f;
        
        [Header("Selection")]
        [SerializeField] Transform selectionGhost;
        [SerializeField] float maxPlacementDistance;
        [SerializeField] float selectionClickThreshold;
        
        Vector3 movement, targetMovement;
        Vector2 lmbDownPos;
        Transform playerCamera;
        Quaternion playerTargetRotation, cameraTargetRotation;
        CharacterController characterController;
        bool isLooking = false;
        
        private void Awake()
        {
            playerCamera = Camera.main.transform;
            characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            transform.localRotation = Quaternion.identity;
            playerCamera.localRotation = Quaternion.identity;
            
            playerTargetRotation = transform.localRotation;
            cameraTargetRotation = playerCamera.localRotation;
        }

        private void Update()
        {
            HandleCameraMovement();
            
            movement = Vector3.Lerp(movement, targetMovement, damping * Time.deltaTime);
            characterController.Move(speeds[speedIndex] * Time.deltaTime * movement);
            
            if((KraltEditor.IsEditMode || CameraViewport.MouseOver) && KraltInput.KeyDown(KraltInput.EDITOR_TOGGLE_EDIT_MODE) && ObjectManager.SelectedObject != null)
            {
                KraltEditor.InstanceData.ToggleEditorMode();
            }
        }
        
        private void HandleMovement()
        {
            Vector3 forward = transform.forward * KraltInput.EditorMovementVector.y;
            Vector3 sideways = transform.right * KraltInput.EditorMovementVector.x;
            Vector3 vertical = Vector3.up * KraltInput.VerticalMovement;
            targetMovement = forward + sideways + vertical;
        }
        
        private void HandleCameraMovement()
        {
            if (KraltEditor.IsObjectMode)
            {
                if (CameraViewport.MouseOver && KraltInput.RmbDown) isLooking = true;
                
                if (KraltInput.RmbUp)
                {
                    isLooking = false;
                    targetMovement = Vector3.zero;
                }
            }
            else if (KraltEditor.IsEditMode)
            {
                isLooking = true;
            }
            
            if (isLooking)
            {
                HandleMovement();
                
                Vector2 mouseDelta = KraltInput.MouseDelta;
                float x = mouseDelta.y * mouseSensitivity;
                float y = mouseDelta.x * mouseSensitivity;
                
                playerTargetRotation *= Quaternion.Euler(0.0f, y, 0.0f);
                cameraTargetRotation *= Quaternion.Euler(-x, 0.0f, 0.0f);
                cameraTargetRotation = ClampRotationAroundAxis(cameraTargetRotation);
                
                transform.localRotation = playerTargetRotation;
                playerCamera.localRotation = cameraTargetRotation;
                
                float scroll = KraltInput.ScrollDelta.y;

                if (scroll > 0.0f && speedIndex < speeds.Length - 1)
                    speedIndex++;
                else if (scroll < 0.0f && speedIndex > 0)
                    speedIndex--;
            }
            
            if (CameraViewport.MouseOver)
            {
                HandleObjectSelection();
            }
        }
        
        private void HandleObjectSelection()
        {
            if (KraltEditor.IsObjectMode)
            {
                selectionGhost.gameObject.SetActive(false);
                
                if (KraltInput.LmbDown)
                    lmbDownPos = KraltInput.MousePosition;
                
                if (KraltInput.LmbUp)
                {
                    float lmbDist = Vector2.Distance(KraltInput.MousePosition, lmbDownPos);
                    
                    if (lmbDist > selectionClickThreshold)
                        return;
                    
                    Ray ray = Camera.main.ScreenPointToRay(KraltInput.MousePosition);
                    
                    if (Physics.Raycast(ray, out RaycastHit hitInfo, 100.0f, KraltEditor.InstanceData.kraltObjectLayer))
                        ObjectManager.InstanceData.TrySelectObject(hitInfo.transform);
                    else
                        ObjectManager.InstanceData.DeselectAll();
                }
            }
            else
            {
                bool cast = Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hitInfo, maxPlacementDistance, KraltEditor.InstanceData.kraltObjectLayer);
                selectionGhost.gameObject.SetActive(cast);
                
                if (cast)
                {
                    Vector3 roundedPosition = RoundVectorToInt(hitInfo.point - hitInfo.normal * 0.1f);
                    selectionGhost.position = roundedPosition;
                    
                    KraltObject mesh = hitInfo.transform.GetComponent<KraltObject>();
                    
                    // Destroy block
                    if (KraltInput.LmbDown)
                    {
                        Vector3Int voxel = RoundVectorToInt(hitInfo.point - hitInfo.normal * 0.1f);
                        Vector3Int localObjectVoxelCoordinate = RoundVectorToInt(hitInfo.transform.position);
                        mesh.RemoveVoxel(voxel - localObjectVoxelCoordinate);
                    }
                    
                    // Place block
                    if (KraltInput.RmbDown)
                    {
                        Vector3Int voxel = RoundVectorToInt(hitInfo.point + hitInfo.normal * 0.1f);
                        Vector3Int localObjectVoxelCoordinate = RoundVectorToInt(hitInfo.transform.position);
                        mesh.AddVoxel(voxel - localObjectVoxelCoordinate);
                    }
                }
            }
        }
        
        public void UpdateCursorLock()
        {
            bool isLocked = KraltEditor.IsEditMode;
            Cursor.visible = !isLocked;
            Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;

            isLooking = false;
            targetMovement = Vector3.zero;
        }
        
        private Vector3Int RoundVectorToInt(Vector3 v)
        {
            int x = Mathf.RoundToInt(v.x);
            int y = Mathf.RoundToInt(v.y);
            int z = Mathf.RoundToInt(v.z);

            return new(x, y, z);
        }

        Quaternion ClampRotationAroundAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, -90.0f, 90.0f);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}
