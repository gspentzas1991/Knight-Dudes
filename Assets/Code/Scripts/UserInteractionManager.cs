using System.Collections.Generic;
using System.Linq;
using Code.Grid;
using Code.GUI;
using Code.Helpers;
using Code.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts
{
    /// <summary>
    /// Handles the user input and calls the appropriate code
    /// </summary>
    public class UserInteractionManager : MonoBehaviour
    {
        private Unit SelectedUnit;
        private List<TilePathfindingData> SelectedUnitPathfindingData;
        private IEnumerable<Unit> AllUnits;
        private int TurnCounter = 1;
        private GridTile PreviousHoveredGridTile;
        private readonly GridCursor GridCursor = new GridCursor();
        private const int MaxRaycastHits = 2;
        #pragma warning disable 0649
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Sprite selectedGridTileSprite;
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
        private void Update()
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
                if (tileChanged && SelectedUnitPathfindingData!=null)
                {
                    //If hovering within the selected unit's movement, renders the path, otherwise removes the rendered path  
                    var hoveredTilePathData = SelectedUnitPathfindingData.FirstOrDefault(x => x.DestinationGridTile == hoveredTile);
                    TileRenderingHelper.RenderPathForUnitToTile(hoveredTilePathData,SelectedUnitPathfindingData);
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
                    ChangeSelectedUnitAsync(hoveredUnit);
                }
                //Clicked on an empty tile
                else if (!ReferenceEquals(hoveredTile,null))
                {
                    MoveUnitToTile(hoveredTile,SelectedUnit,SelectedUnitPathfindingData);
                }
            }
            //Right click detection
            else if (Input.GetMouseButtonDown(1))
            {
                StartEnemyTurnAsync();
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

        private async void ChangeSelectedUnitAsync(Unit unitToSelect)
        {
            //Clicked on an idle player unit
            if (unitToSelect.state != UnitState.Idle || SelectedUnit==unitToSelect || playerUnits.All(x => x != unitToSelect))
            {
                return;
            }
            if (!ReferenceEquals(SelectedUnit,null))
            {
                ResetSelectedUnit();
            }
            SelectUnit(unitToSelect);
            SelectedUnitPathfindingData = await PathfindingHelper.CalculateUnitAvailablePathsAsync(SelectedUnit.transform.position,gridManager.TileGrid);
            TileRenderingHelper.ChangeTileSprites(SelectedUnitPathfindingData.Select(x => x.DestinationGridTile), selectedGridTileSprite);
        }


        /// <summary>
        /// Deselects the previous unit, selects a new one and renders its possible movements
        /// </summary>
        private void SelectUnit(Unit unitToSelect)
        {
            SelectedUnit = unitToSelect;
            SelectedUnit.state = UnitState.Selected; 
        }

        /// <summary>
        /// Deselects the selectedUnit and renders the default grid on its movement path
        /// </summary>
        private void ResetSelectedUnit()
        {
            if (SelectedUnit.state==UnitState.Selected)
            {
                SelectedUnit.state = UnitState.Idle;
                TileRenderingHelper.ChangeTileSprites(SelectedUnitPathfindingData.Select(x => x.DestinationGridTile), null);
            }
            SelectedUnit = null;
            SelectedUnitPathfindingData = null;
        }

        /// <summary>
        /// Moves the selectedUnit to the selectedTile, if the selected tile is a valid move for that unit
        /// </summary>
        private void MoveUnitToTile(GridTile selectedGridTile,Unit selectedUnit, IReadOnlyList<TilePathfindingData> unitPathfindingData)
        {
            if (selectedGridTile.terrainType == TerrainType.Impassable || unitPathfindingData == null || ReferenceEquals(selectedUnit, null))
            {
                return;
            }
            //checks that the selected tile is a valid move for the unit
            var selectedTilePathfindingData = unitPathfindingData.FirstOrDefault(x => x.DestinationGridTile == selectedGridTile);
            if (selectedTilePathfindingData == null || selectedTilePathfindingData.MoveCost > selectedUnit.movement)
            {
                return;
            }
            var pathfindingDataList = PathfindingHelper.GetPathToTile(unitPathfindingData, selectedGridTile);
            //we move the unit reference from the starting tile to the selected tile
            unitPathfindingData[0].DestinationGridTile.currentUnit = null;
            selectedGridTile.currentUnit = SelectedUnit;
            StartCoroutine(selectedUnit.FollowTilePath(pathfindingDataList));
            TileRenderingHelper.ChangeTileSprites(SelectedUnitPathfindingData.Select(x => x.DestinationGridTile), null);
            ResetSelectedUnit();
        }

        private void EndTurn()
        {
            foreach(var unit in playerUnits)
            {
                unit.ResetUnitTurnValues();
            }
            foreach (var unit in enemyUnits)
            {
                unit.ResetUnitTurnValues();
            }
            Debug.Log("finished turn "+TurnCounter);
            TurnCounter++;
        }

        /// <summary>
        /// Makes actions with all the units in the EnemyUnits list
        /// </summary>
        private async void StartEnemyTurnAsync()
        {
            foreach (var enemy in enemyUnits)
            {
                SelectUnit(enemy);
                SelectedUnitPathfindingData=await PathfindingHelper.CalculateUnitAvailablePathsAsync(enemy.transform.position,gridManager.TileGrid);
                var randomTile = SelectedUnitPathfindingData[Random.Range(0, SelectedUnitPathfindingData.Count)].DestinationGridTile;
                MoveUnitToTile(randomTile,enemy, SelectedUnitPathfindingData);
            }
            EndTurn();
        }
    }
}
