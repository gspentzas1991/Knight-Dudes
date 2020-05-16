using System.Linq;
using Code.UserInput;
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
        public bool cursorTileChanged;
        #pragma warning disable 0649
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameCursor gameCursor;
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
            //fires a raycast from the camera to the mouse position to detect hits
            var screenPointRaySource = mainCamera.WorldToScreenPoint(gameCursor.transform.position);
            var hoveredTile = DetectHoveredTile(gridManager.TileGrid,screenPointRaySource);
            //Hovers over tile
            if (!ReferenceEquals(hoveredTile, null))
            {
                cursorTileChanged = ChangeCursorTile(hoveredTile);
            }
        }
                 
        /// <summary>
        /// Returns the tile the mouse is currently hovering over
        /// </summary>
        private GridTile DetectHoveredTile(GridTile[,] tileGrid, Vector3 screenPointRaySource)
        {
            var ray = mainCamera.ScreenPointToRay(screenPointRaySource);
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
        public bool ChangeCursorTile(GridTile selectedTile)
        {
            if (selectedTile == cursorTile) return false;
            // Changes the hoveredTile to be the cursor, and the previous hovered tile to not be
            // ReSharper disable once UseNullPropagation
            if (!ReferenceEquals(cursorTile,null))
            {
                cursorTile.ChangeCursorRendererState(false);
            }      
            selectedTile.ChangeCursorRendererState(true); 
            cursorTile = selectedTile;
            return true;
        }
        

    }
}