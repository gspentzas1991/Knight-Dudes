using UnityEngine;

namespace Code.UserInput
{
    /// <summary>
    /// The game cursor is used to calculate the cursorTile.
    /// The object follows the camera and stays inside its viewport
    /// </summary>
    public class GameCursor : MonoBehaviour
    { 
        private const float ViewportMargin=0.05f; 
        public bool controlWithMouse = true;
        #pragma warning disable 0649
        [SerializeField] private float cursorMovementSpeed;
        [SerializeField] private Camera mainCamera;
        #pragma warning restore 0649

        private void Start()
        {
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
            Vector3 newCursorPosition;
            //When using a mouse, the transform updates along with the mouse
            if (controlWithMouse)
            {  
                var mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z);
                var mousePos = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
                newCursorPosition = mousePos;
            }
            //When using a keyboard, the transform updates with the WASD
            else
            {
                var newPositionOffset = new Vector3();
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    newPositionOffset.y = cursorMovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    newPositionOffset.y = -cursorMovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    newPositionOffset.x = cursorMovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    newPositionOffset.x = -cursorMovementSpeed * Time.deltaTime;
                }
                newCursorPosition = transform.position + newPositionOffset;
            }
            transform.position = KeepPositionWithinViewport(newCursorPosition,ViewportMargin);
        }

        private Vector3 KeepPositionWithinViewport(Vector3 position,float margin)
        {
            var viewPortCursorPosition = mainCamera.WorldToViewportPoint(position);
            //keeps the cursor within a margin of the viewPort
            viewPortCursorPosition.x = Mathf.Clamp(viewPortCursorPosition.x, margin, 1-margin);
            viewPortCursorPosition.y = Mathf.Clamp(viewPortCursorPosition.y, margin, 1-margin);
            return mainCamera.ViewportToWorldPoint(viewPortCursorPosition);
        }
    }
}
