using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.UserInput
{
    public class CameraMovement : MonoBehaviour
    {
        private Controls _playerInputActions;
        private Vector2 _inputCameraControl;
        private float _inputZoom;
        #pragma warning disable 0649
        [SerializeField] private float _cameraMovementSpeed;
        [SerializeField] private float _zoomSpeed;
        [SerializeField] private float _edgeScrollingOffset;
        [SerializeField] private Vector3 _maximumPosition;
        [SerializeField] private Vector3 _minimumPosition;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _gameCursorTransform;
        #pragma warning restore 0649

        private void Awake()
        {
            _playerInputActions = new Controls();
            _playerInputActions.Gameplay.CameraControl.performed += ctx => _inputCameraControl = ctx.ReadValue<Vector2>();
            _playerInputActions.Gameplay.CameraZoomControl.performed += ctx => _inputZoom = ctx.ReadValue<float>();
            _playerInputActions.Gameplay.CameraControl.canceled += ctx => _inputCameraControl = ctx.ReadValue<Vector2>();
            _playerInputActions.Gameplay.CameraZoomControl.canceled += ctx => _inputZoom = ctx.ReadValue<float>();
        }
        
        private void LateUpdate()
        {
            //if the cursor is not a child of the camera, the camera will follow it until it reaches the cursor
            if (_gameCursorTransform.parent==transform)
            {
                MovementDetection();
            }
            else
            {
                FollowGameCursor();
            }
            
        }

        /// <summary>
        /// Moves the transform of the camera according to user input
        /// </summary>
        private void MovementDetection()
        {
            var newPositionOffset = new Vector3
            {
                x = _inputCameraControl.x * _cameraMovementSpeed * Time.deltaTime,
                y = _inputCameraControl.y * _cameraMovementSpeed * Time.deltaTime,
                z = _inputZoom * _zoomSpeed * Time.deltaTime
            };
            newPositionOffset = EdgeScrollingHandler(newPositionOffset);
            transform.position = ClampPosition(transform.position + newPositionOffset);
        }

        /// <summary>
        /// If the game cursor is not a child of the camera, the camera will
        /// follow it and make it its child when it reaches the cursor
        /// </summary>
        private void FollowGameCursor()
        {
            var cameraPosition = transform.position;
            var cursorPosition = _gameCursorTransform.position;
            cursorPosition.z = cameraPosition.z;
            transform.position = Vector3.MoveTowards(cameraPosition, cursorPosition, _cameraMovementSpeed/2 * Time.deltaTime);
            if (transform.position != cursorPosition) return;
            _gameCursorTransform.SetParent(transform);
        }

        /// <summary>
        /// Keeps the vector3 position between the minimum and maximum values
        /// </summary>
        private Vector3 ClampPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, _minimumPosition.x, _maximumPosition.x);
            position.y = Mathf.Clamp(position.y, _minimumPosition.y, _maximumPosition.y);
            position.z = Mathf.Clamp(position.z, _minimumPosition.z, _maximumPosition.z);
            return position;
        }

        /// <summary>
        /// If the mouse is within the margins of edge scrolling, edge scrolling is applied to the newPositionOffset
        /// </summary>
        private Vector3 EdgeScrollingHandler(Vector3 newPositionOffset)
        {
            var cursorPosition = _mainCamera.WorldToViewportPoint(_gameCursorTransform.position);
            if (cursorPosition.y>1-_edgeScrollingOffset)
            {
                newPositionOffset.y = _cameraMovementSpeed * Time.deltaTime;
            }
            else if (cursorPosition.y < _edgeScrollingOffset)
            {
                newPositionOffset.y = -_cameraMovementSpeed * Time.deltaTime;
            }
            if (cursorPosition.x > 1 - _edgeScrollingOffset)
            {
                newPositionOffset.x = _cameraMovementSpeed * Time.deltaTime;
            }
            else if (cursorPosition.x < _edgeScrollingOffset)
            {
                newPositionOffset.x = -_cameraMovementSpeed * Time.deltaTime;
            }
            return newPositionOffset;
        }
        private void OnEnable()
        {
            _playerInputActions.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.Disable();
        }
    }
}
