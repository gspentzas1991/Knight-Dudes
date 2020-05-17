using System.Linq;
using Code.UserInput;
using UnityEngine;

namespace Code.Grid
{
    /// <summary>
    /// Detects and keeps track of the currently hovered gridTile, and shows the cursor sprite on that tile
    /// </summary>
    public class TileSelector : MonoBehaviour
    {
        public GridTile CursorTile;
        /// <summary>
        /// Becomes true on the frame that the cursor changes
        /// </summary>
        public bool CursorTileChanged;
        private Controls _playerInputActions;
        private const int MaxRaycastHits = 2;
        /// <summary>
        /// The tile that the game cursor is currently hovering over
        /// </summary>
        #pragma warning disable 0649
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private GameCursor _gameCursor;
        #pragma warning restore 0649

        private void Awake()
        {
            
            _playerInputActions=new Controls();
            _playerInputActions.Gameplay.NavigateGrid.started  += ctx => MoveCursorTile(ctx.ReadValue<Vector2>());
        }
        private void Start()
        {
            //initialize selected tile
            ChangeCursorTile(_gridManager.TileGrid[0, 0]);
        }
        private void Update()
        {
            DetectCursorTile();
        }

        /// <summary>
        /// Reads user inputs and changes the cursor position accordingly
        /// </summary>
        private void DetectCursorTile()
        {
            //fires a raycast from the camera to the mouse position to detect hits
            var screenPointRaySource = _mainCamera.WorldToScreenPoint(_gameCursor.transform.position);
            var hoveredTile = DetectHoveredTile(_gridManager.TileGrid,screenPointRaySource);
            //Hovers over tile
            if (!ReferenceEquals(hoveredTile, null))
            {
                CursorTileChanged = ChangeCursorTile(hoveredTile);
            }
        }

        /// <summary>
        /// Moves the cursor in the direction of the move input
        /// </summary>
        private void MoveCursorTile(Vector2 moveInput)
        {
            if (ReferenceEquals(CursorTile,null))
            {
                return;
            }

            var newTilePosition = new Vector2
            {
                x = CursorTile.PositionInGrid.x + (int)moveInput.x,
                y = CursorTile.PositionInGrid.y + (int)moveInput.y
            };
            if (!GridManager.CoordinatesWithinGrid(newTilePosition,_gridManager.TileGrid)) return;
            var newCursorTile = _gridManager.TileGrid[(int)newTilePosition.x,(int)newTilePosition.y];
            CursorTileChanged = ChangeCursorTile(newCursorTile);
            if (CursorTileChanged)
            {
                _gameCursor.MoveCursorOverTile(CursorTile);
            }
            
        }
                 
        /// <summary>
        /// Returns the tile the mouse is currently hovering over
        /// </summary>
        private GridTile DetectHoveredTile(GridTile[,] tileGrid, Vector3 screenPointRaySource)
        {
            var ray = _mainCamera.ScreenPointToRay(screenPointRaySource);
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
        private bool ChangeCursorTile(GridTile tile)
        {
            if (tile == CursorTile) return false;
            // Changes the hoveredTile to be the cursor, and the previous hovered tile to not be
            // ReSharper disable once UseNullPropagation
            if (!ReferenceEquals(CursorTile,null))
            {
                CursorTile.ChangeCursorRendererState(false);
            }      
            tile.ChangeCursorRendererState(true); 
            CursorTile = tile;
            return true;
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