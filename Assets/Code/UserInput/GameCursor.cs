using Code.Grid;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.UserInput
{
    /// <summary>
    /// The game cursor is used to calculate the cursorTile.
    /// The object follows the camera and stays inside its viewport
    /// </summary>
    public class GameCursor : MonoBehaviour
    {
        private Controls _playerInputActions;
        private Vector2 _inputCursorMovement;
        #pragma warning disable 0649
        [SerializeField] private float _cursorMovementSpeed;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _viewportMargin; 
        #pragma warning restore 0649

        private void Awake()
        {
            Cursor.visible = false;
            _playerInputActions= new Controls();
            _playerInputActions.Gameplay.CursorControl.performed += ctx =>_inputCursorMovement=ctx.ReadValue<Vector2>();
            _playerInputActions.Gameplay.CursorControl.canceled += ctx => _inputCursorMovement=ctx.ReadValue<Vector2>();
        }

        private void Update()
        {
            MovementDetection();
        }

        /// <summary>
        /// Moves the cursor over the tile, and stops being the child of the camera
        /// </summary>
        public void MoveCursorOverTile(GridTile tile)
        {
            var cursorTransform = transform;
            cursorTransform.position = tile.transform.position;
            cursorTransform.SetParent(null);
        }
        
        /// <summary>
        /// Moves the transform of the camera according to user input
        /// </summary>
        private void MovementDetection()
        {
            var newPositionOffset = new Vector3
            {
                y = _inputCursorMovement.y * _cursorMovementSpeed * Time.deltaTime,
                x = _inputCursorMovement.x * _cursorMovementSpeed * Time.deltaTime
            };
            transform.position = KeepPositionWithinViewport(transform.position + newPositionOffset,_viewportMargin);
        }

        private Vector3 KeepPositionWithinViewport(Vector3 position,float margin)
        {
            var viewPortCursorPosition = _mainCamera.WorldToViewportPoint(position);
            //keeps the cursor within a margin of the viewPort
            viewPortCursorPosition.x = Mathf.Clamp(viewPortCursorPosition.x, margin, 1-margin);
            viewPortCursorPosition.y = Mathf.Clamp(viewPortCursorPosition.y, margin, 1-margin);
            return _mainCamera.ViewportToWorldPoint(viewPortCursorPosition);
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
