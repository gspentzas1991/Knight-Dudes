using System;
using UnityEngine;

namespace Code.UserInput
{
    /// <summary>
    /// The game cursor is used to calculate the cursorTile.
    /// The object follows the camera and stays inside its viewport
    /// </summary>
    public class GameCursor : MonoBehaviour
    {
        private Controls PlayerInputActions;
        private Vector2 InputCursorMovement;
        public bool controlWithMouse = true;
        #pragma warning disable 0649
        [SerializeField] private float cursorMovementSpeed;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float viewportMargin; 
        #pragma warning restore 0649

        private void Awake()
        {
            PlayerInputActions= new Controls();
            PlayerInputActions.Gameplay.CursorControl.performed += ctx => InputCursorMovement = ctx.ReadValue<Vector2>();
            PlayerInputActions.Gameplay.CursorControl.canceled += ctx => InputCursorMovement = ctx.ReadValue<Vector2>();
            Cursor.visible = false;
        }

        private void Update()
        {
            MovementDetection();
        }

        /// <summary>
        /// Moves the transform of the camera according to user input
        /// </summary>
        private void MovementDetection()
        {
            Debug.Log(InputCursorMovement);
            var newPositionOffset = new Vector3();
            if (Mathf.Abs(InputCursorMovement.y)>0)
            {
                newPositionOffset.y =InputCursorMovement.y * cursorMovementSpeed * Time.deltaTime;
            }
            if (Mathf.Abs(InputCursorMovement.x)>0)
            {
                newPositionOffset.x =InputCursorMovement.x * cursorMovementSpeed * Time.deltaTime;
            }
            transform.position = KeepPositionWithinViewport(transform.position + newPositionOffset,viewportMargin);
        }

        private Vector3 KeepPositionWithinViewport(Vector3 position,float margin)
        {
            var viewPortCursorPosition = mainCamera.WorldToViewportPoint(position);
            //keeps the cursor within a margin of the viewPort
            viewPortCursorPosition.x = Mathf.Clamp(viewPortCursorPosition.x, margin, 1-margin);
            viewPortCursorPosition.y = Mathf.Clamp(viewPortCursorPosition.y, margin, 1-margin);
            return mainCamera.ViewportToWorldPoint(viewPortCursorPosition);
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
