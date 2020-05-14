using System;
using System.Linq;
using Code.Units;
using UnityEngine;

namespace Code.Grid
{
    /// <summary>
    /// Keeps track of the currently hovered gridTile, and shows the cursor sprite that tile
    /// </summary>
    public class GridCursor : MonoBehaviour
    {
        private const int MaxRaycastHits = 2;
        public GridTile cursorTile;
        public bool controlWithMouse = true;
        /// <summary>
        /// Becomes true on the frame that the cursor changes
        /// </summary>
        public bool cursorTileChanged = false;
        #pragma warning disable 0649
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Camera mainCamera;
        #pragma warning restore 0649

        private void Update()
        {
            ChangeCursorFromInput();
        }

        /// <summary>
        /// Reads user inputs and changes the cursor position accordingly
        /// </summary>
        private void ChangeCursorFromInput()
        {
            if (controlWithMouse)
            {
                //fires a raycast from the camera to the mouse position to detect hits
                var hoveredTile = DetectHoveredTile(gridManager.TileGrid);
                //Hovers over tile
                if (!ReferenceEquals(hoveredTile, null))
                {
                    cursorTileChanged = ChangeCursorTile(hoveredTile);

                }
            }
            else
            {
                var newCursorPosition = new Vector2Int((int) cursorTile.positionInGrid.x, (int) cursorTile.positionInGrid.y);
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    newCursorPosition.y += 1;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    newCursorPosition.y -= 1;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    newCursorPosition.x += 1;
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    newCursorPosition.x -= 1;
                }
                cursorTileChanged = ChangeCursorTile(gridManager.TileGrid[newCursorPosition.x,newCursorPosition.y]);
            }
        }
        
                 
        /// <summary>
        /// Returns the tile the mouse is currently hovering over
        /// </summary>
        private GridTile DetectHoveredTile(GridTile[,] tileGrid)
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var raycastHits = new RaycastHit[MaxRaycastHits];
            Physics.RaycastNonAlloc(ray, raycastHits);
            var hitTileTransform = raycastHits.FirstOrDefault(x=>!ReferenceEquals(x.transform, null) && x.transform.CompareTag("Tile")).transform;
            if (ReferenceEquals(hitTileTransform, null))
            {
                return null;
            }
            var position = hitTileTransform.position;
            return tileGrid[(int)position.x,(int)position.y];
        }

        /// <summary>
        /// Hides the cursor on the previous GridTile, and shows it on the hoveredTile
        /// Returns true if the hoveredTile changed from the previous tile
        /// </summary>
        private bool ChangeCursorTile(GridTile newHoveredTile)
        {
            if (newHoveredTile == cursorTile) return false;
            // Changes the hoveredTile to be the cursor, and the previous hovered tile to not be
            // ReSharper disable once UseNullPropagation
            if (!ReferenceEquals(cursorTile,null))
            {
                cursorTile.ChangeCursorRendererState(false);
            }      
            newHoveredTile.ChangeCursorRendererState(true); 
            cursorTile = newHoveredTile;
            return true;
        }
        

    }
}