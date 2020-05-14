using System.Collections.Generic;
using System.Linq;
using Code.Grid;
using Code.GUI;
using Code.Helpers;
using Code.Turns;
using Code.Units;
using UnityEngine;

namespace Code.UserInput
{
    /// <summary>
    /// Handles the user input and calls the appropriate code
    /// </summary>
    public class UserInteractionManager : MonoBehaviour
    {
        private IEnumerable<Unit> AllUnits;
        private const int MaxRaycastHits = 2;
        #pragma warning disable 0649
        [SerializeField] private GridManager gridManager;
        [SerializeField] private List<Unit> playerUnits;
        [SerializeField] private List<Unit> enemyUnits;
        [SerializeField] private UnitProfileGui unitProfile;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GridCursor gridCursor;
        [SerializeField] private UnitSelector unitSelector;
        [SerializeField] private UnitMovementHandler unitMovementHandler;
        [SerializeField] private TurnManager turnManager;
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
                var tileChanged = gridCursor.HoveredTileChanged(hoveredTile);
                if (tileChanged && !ReferenceEquals(unitSelector.SelectedUnit, null))
                {
                    var selectedUnitPathfindingData = unitSelector.SelectedUnit.pathfindingData;
                    //If hovering within the selected unit's movement, renders the path, otherwise removes the rendered path  
                    var hoveredTilePathData = selectedUnitPathfindingData.FirstOrDefault(x => x.DestinationGridTile == hoveredTile);
                    TileRenderingHelper.RenderPathToTile(hoveredTilePathData,selectedUnitPathfindingData);
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
                    await unitSelector.ChangeSelectedUnitAsync(hoveredUnit,gridManager.TileGrid);
                    TileRenderingHelper.RenderUnitAvailablePaths(hoveredUnit);
                }
                //Clicked on an empty tile
                else if (!ReferenceEquals(hoveredTile,null))
                {
                    unitMovementHandler.MoveUnitToTile(hoveredTile,unitSelector.SelectedUnit);
                }
            }
            //Right click detection
            else if (Input.GetMouseButtonDown(1))
            {
                await unitMovementHandler.MoveEnemyUnitsAsync(enemyUnits,gridManager.TileGrid);
                turnManager.EndTurn(AllUnits);
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
