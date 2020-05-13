using System.Collections.Generic;
using System.Linq;
using Code.Grid;
using Code.GUI;
using Code.Helpers;
using Code.Turns;
using Code.Units;
using UnityEngine;

namespace Code.Scripts
{
    /// <summary>
    /// Handles the user input and calls the appropriate code
    /// </summary>
    public class UserInteractionManager : MonoBehaviour
    {
        private IEnumerable<Unit> AllUnits;
        private readonly GridCursor GridCursor = new GridCursor();
        private readonly UnitSelector UnitSelector = new UnitSelector();
        private readonly UnitMovement UnitMovement = new UnitMovement();
        private readonly TurnManager TurnManager = new TurnManager();
        private const int MaxRaycastHits = 2;
        #pragma warning disable 0649
        [SerializeField] private GridManager gridManager;
        [SerializeField] private List<Unit> playerUnits;
        [SerializeField] private List<Unit> enemyUnits;
        [SerializeField] private UnitProfileGui unitProfile;
        [SerializeField] private Camera mainCamera;
        #pragma warning restore 0649



        private void Start()
        {
            AllUnits= playerUnits.Concat(enemyUnits);
            //currentUnit initialization in tiles
            foreach (var unit in AllUnits)
            {
                var position = unit.transform.position;
                gridManager.TileGrid[(int)position.x, (int)position.y].currentUnit=unit;
            }
        }
        private async void Update()
        {
            //fires a raycast from the camera to the mouse position to detect hits
            var hoveredTile = DetectHoveredTile(gridManager.TileGrid);
            Unit hoveredUnit = null;
            if (!ReferenceEquals(hoveredTile,null))
            {
                hoveredUnit = hoveredTile.currentUnit;
            }
            unitProfile.ShowUnitProfile(hoveredUnit);
            //Hovers over tile
            if (!ReferenceEquals(hoveredTile, null))
            {
                var tileChanged = GridCursor.HoveredTileChanged(hoveredTile);   
                if (tileChanged && UnitSelector.SelectedUnitPathfindingData!=null)
                {
                    //If hovering within the selected unit's movement, renders the path, otherwise removes the rendered path  
                    var hoveredTilePathData = UnitSelector.SelectedUnitPathfindingData.FirstOrDefault(x => x.DestinationGridTile == hoveredTile);
                    TileRenderingHelper.RenderPathForUnitToTile(hoveredTilePathData,UnitSelector.SelectedUnitPathfindingData);
                }
            }
            //Hovers over a unit
            if (!ReferenceEquals(hoveredUnit,null))
            {
                
            }
            //Left click detection
            if (Input.GetMouseButtonDown(0))
            {
                //Clicked on a unit
                if (!ReferenceEquals(hoveredUnit,null))
                {
                    if (playerUnits.All(x => x != hoveredUnit)) return;
                    //clicked on a player unit
                    await UnitSelector.ChangeSelectedUnitAsync(hoveredUnit,gridManager.TileGrid);
                    TileRenderingHelper.ChangeTileSprites(UnitSelector.SelectedUnitPathfindingData.Select(x => x.DestinationGridTile), TileState.Selected);
                }
                //Clicked on an empty tile
                else if (!ReferenceEquals(hoveredTile,null))
                {
                    UnitMovement.MoveUnitToTile(hoveredTile,UnitSelector.SelectedUnit,UnitSelector.SelectedUnitPathfindingData,UnitSelector);
                }
            }
            //Right click detection
            else if (Input.GetMouseButtonDown(1))
            {
                await UnitMovement.MoveEnemyUnitsAsync(enemyUnits,gridManager.TileGrid,UnitSelector);
                TurnManager.EndTurn(AllUnits,UnitSelector);
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
    }
}
