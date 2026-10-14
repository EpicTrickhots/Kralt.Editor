using UnityEngine;
using Kralt.Editor.InputHandling;
using Kralt.Editor.Graphics;

namespace Kralt.Editor.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class EditorPlayerController : MonoBehaviour
    {
        [SerializeField, Header("Movement")]
        float damping = 10.0f;

        [SerializeField]
        int speedIndex = 0;

        [SerializeField]
        float[] speeds;

        [SerializeField, Header("Camera Settings")]
        float mouseSensitivity = 2.0f;
        
        Vector3 movement, targetMovement;
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
        }

        private void HandleMovement()
        {
            Vector3 forward = transform.forward * KInput.EditorMovementVector.y;
            Vector3 sideways = transform.right * KInput.EditorMovementVector.x;
            Vector3 vertical = Vector3.up * KInput.VerticalMovement;
            targetMovement = forward + sideways + vertical;
        }
        
        private void HandleCameraMovement()
        {
            if (CameraViewport.MouseOver && KInput.RmbDown) isLooking = true;
            
            if (KInput.RmbUp)
            {
                isLooking = false;
                targetMovement = Vector3.zero;
            }
            
            if (isLooking)
            {
                HandleMovement();
                
                Vector2 mouseDelta = KInput.MouseDelta;
                float x = mouseDelta.y * mouseSensitivity;
                float y = mouseDelta.x * mouseSensitivity;
                
                playerTargetRotation *= Quaternion.Euler(0.0f, y, 0.0f);
                cameraTargetRotation *= Quaternion.Euler(-x, 0.0f, 0.0f);
                cameraTargetRotation = ClampRotationAroundAxis(cameraTargetRotation);
                
                transform.localRotation = playerTargetRotation;
                playerCamera.localRotation = cameraTargetRotation;
                
                float scroll = KInput.ScrollDelta.y;

                if (scroll > 0.0f && speedIndex < speeds.Length - 1)
                    speedIndex++;
                else if (scroll < 0.0f && speedIndex > 0)
                    speedIndex--;
            }
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
