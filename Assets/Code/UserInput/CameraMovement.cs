using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.UserInput
{
    public class CameraMovement : MonoBehaviour
    {
        private Controls PlayerInputActions;
        private Vector2 InputCameraControl;
        private float InputZoom;
        #pragma warning disable 0649
        [SerializeField] private float cameraMovementSpeed;
        [SerializeField] private float zoomSpeed;
        [SerializeField] private float edgeScrollingOffset;
        [SerializeField] private Vector3 maximumPosition;
        [SerializeField] private Vector3 minimumPosition;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform gameCursorTransform;
        #pragma warning restore 0649

        private void Awake()
        {
            PlayerInputActions = new Controls();
            PlayerInputActions.Gameplay.CameraControl.performed += ctx => InputCameraControl = ctx.ReadValue<Vector2>();
            PlayerInputActions.Gameplay.CameraZoomControl.performed += ctx => InputZoom = ctx.ReadValue<float>();
            PlayerInputActions.Gameplay.CameraControl.canceled += ctx => InputCameraControl = ctx.ReadValue<Vector2>();
            PlayerInputActions.Gameplay.CameraZoomControl.canceled += ctx => InputZoom = ctx.ReadValue<float>();
        }
        // Update is called once per frame
        private void LateUpdate()
        {
            MovementDetection();
        }

        /// <summary>
        /// Moves the transform of the camera according to user input
        /// </summary>
        private void MovementDetection()
        {
            var cursorPosition = mainCamera.WorldToViewportPoint(gameCursorTransform.position);
            var newPositionOffset = new Vector3
            {
                x = InputCameraControl.x * cameraMovementSpeed * Time.deltaTime,
                y = InputCameraControl.y * cameraMovementSpeed * Time.deltaTime,
                z = InputZoom * zoomSpeed * Time.deltaTime
            };
            if (cursorPosition.y>1-edgeScrollingOffset)
            {
                newPositionOffset.y = cameraMovementSpeed * Time.deltaTime;
            }
            else if (cursorPosition.y < edgeScrollingOffset)
            {
                newPositionOffset.y = -cameraMovementSpeed * Time.deltaTime;
            }
            if (cursorPosition.x > 1 - edgeScrollingOffset)
            {
                newPositionOffset.x = cameraMovementSpeed * Time.deltaTime;
            }
            else if (cursorPosition.x < edgeScrollingOffset)
            {
                newPositionOffset.x = -cameraMovementSpeed * Time.deltaTime;
            }
            transform.position = ClampPosition(transform.position + newPositionOffset);
        }

        /// <summary>
        /// Keeps the vector3 position between the minimum and maximum values
        /// </summary>
        private Vector3 ClampPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, minimumPosition.x, maximumPosition.x);
            position.y = Mathf.Clamp(position.y, minimumPosition.y, maximumPosition.y);
            position.z = Mathf.Clamp(position.z, minimumPosition.z, maximumPosition.z);
            return position;
        }
        private void OnEnable()
        {
            PlayerInputActions.Enable();
        }

        private void OnDisable()
        {
            PlayerInputActions.Disable();
        }
    }
}
