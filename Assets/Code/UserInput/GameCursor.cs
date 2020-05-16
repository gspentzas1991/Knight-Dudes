using System;
using Code.Grid;
using UnityEngine;

namespace Code.UserInput
{
    /// <summary>
    /// This is the object that the user moves with his mouse or keyboard, and it's used to calculate the
    /// hovered grid tile. The camera should always follow this object
    /// </summary>
    public class GameCursor : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] private GridCursor gridCursor;
        [SerializeField] private float cursorMovementSpeed;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GridManager gridManager;
        #pragma warning restore 0649

        private void Start()
        {
            Cursor.visible = false;
        }
        
        // Update is called once per frame
        void Update()
        {
            MovementDetection();
        }

        /// <summary>
        /// Moves the transform of the camera according to user input
        /// </summary>
        private void MovementDetection()
        {
            //When using a mouse, the transform updates along with the mouse
            if (gridCursor.controlWithMouse)
            {  
                var mouseWScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f);
                var mousePos = mainCamera.ScreenToWorldPoint(mouseWScreenPosition);
                transform.position = mousePos;
            }
            //When using a keyboard, the transform updates with the WASD
            else
            {
                var newPositionOffset = new Vector3();
                if (Input.GetKey(KeyCode.W))
                {
                    newPositionOffset.y = cursorMovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    newPositionOffset.y = -cursorMovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    newPositionOffset.x = cursorMovementSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    newPositionOffset.x = -cursorMovementSpeed * Time.deltaTime;
                }
                transform.position += newPositionOffset;
                
                //moves the gridCursor by one ever time a button is clicked
                /*Vector3 cursorMove = new Vector3();
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    cursorMove.y = 1;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    cursorMove.y = -1;
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    cursorMove.x = -1;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    cursorMove.x = 1;
                }

                var cursorPosition = cursorMove + gridCursor.cursorTile.transform.position;
                var tile = gridManager.TileGrid[(int)cursorPosition.x,(int)cursorPosition.y];
                gridCursor.ChangeCursorTile(tile);
                transform.position = gridCursor.cursorTile.transform.position;*/
            }
        }
    }
}
