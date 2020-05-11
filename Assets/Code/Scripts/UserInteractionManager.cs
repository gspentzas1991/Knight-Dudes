using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Helpers;
using Code.Models;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using UnityEngine.Assertions;

namespace Code.Scripts
{
    public class UserInteractionManager : MonoBehaviour
    {
        private const int MaxRaycastHits = 2;
        private Unit SelectedUnit;
        [SerializeField]
        private GridManager gridManager = null;
        private List<TilePathfindingData> SelectedUnitPathfindingData;
        [SerializeField]
        private Sprite selectedGridTileSprite = null;
        [SerializeField]
        private Sprite unitPathGridTileSprite = null;
        [SerializeField]
        private List<Unit> playerUnits = new List<Unit>();
        [SerializeField]
        private List<Unit> enemyUnits = new List<Unit>();
        private int TurnCounter = 1;
        [SerializeField]
        private UnitProfileGui unitProfile = null;
        private Unit HoveredUnit = null;
        private Tile HoveredTile = null;
        [SerializeField] 
        private Camera mainCamera = null;

        private IEnumerable<Unit> AllUnits;

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
            var hitTile = DetectHoveredTile();
            Unit hitUnit = null;
            if (!ReferenceEquals(hitTile,null))
            {
                hitUnit = hitTile.currentUnit;
            }

            //click gameobject detection
            if (Input.GetMouseButtonDown(0))
            {
                //clicked on a new unit
                if (!ReferenceEquals(hitUnit,null))
                {
                    if (hitUnit.state==UnitState.Idle && playerUnits.Any(x=>x==hitUnit))
                    {
                        if (SelectedUnit!=null)
                        {
                            ResetSelectedUnit();
                        }
                        SelectUnit(hitUnit);
                        SelectedUnitPathfindingData = await CalculateUnitAvailablePathsAsync(SelectedUnit);
                        GridManager.ChangeTilesSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile), selectedGridTileSprite);
                    }
                }
                //clicked on a tile to move selected unit
                else if (SelectedUnit!=null && SelectedUnit.state == UnitState.Selected && hitTile!=null && hitTile.transform.position != SelectedUnit.transform.position)
                {
                    MoveUnitToTile(hitTile,SelectedUnit,SelectedUnitPathfindingData);
                    GridManager.ChangeTilesSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile), null);
                    ResetSelectedUnit();
                }
            }
            //right click detection
            else if (Input.GetMouseButtonDown(1))
            {
                StartEnemyTurnAsync();
            }
            //hover on tile
            if (hitTile!=null)
            {
                if (hitTile != HoveredTile)
                {
                    var previousHoveredTile = HoveredTile;
                    HoveredTile = hitTile;
                    //changes the hoveredTile to be the cursor, and the previous tile to not be
                    if (previousHoveredTile!=null)
                    {
                        previousHoveredTile.ChangeCursorRendererState(false);
                    }
                    HoveredTile.ChangeCursorRendererState(true);
                    //hovered over a tile within the unit's movement
                    if (SelectedUnit != null && SelectedUnit.state == UnitState.Selected && SelectedUnitPathfindingData != null && SelectedUnitPathfindingData.Any(x => x.DestinationTile == HoveredTile))
                    {
                        RenderPathForUnitToTile(hitTile.gameObject, SelectedUnitPathfindingData);

                    }
                    //if we hover outside the units movement area, we stop rendering paths
                    else if (SelectedUnitPathfindingData!=null && !SelectedUnitPathfindingData.Any(x => x.DestinationTile == HoveredTile))
                    {
                        GridManager.ChangeTilesSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile), selectedGridTileSprite);
                    }
                }

            }
            
            //hover over a unit
            if (hitUnit!=null)
            {
                if (hitUnit != HoveredUnit)
                {
                    ShowUnitProfile(hitUnit);
                }
            }
            //stopped hovering over a unit
            else if (HoveredUnit!=null)
            {
                HideUnitProfile();
            }

        }
        
        /// <summary>
        /// Returns the tile the mouse is hovering over
        /// </summary>
        private Tile DetectHoveredTile()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var raycastHits = new RaycastHit[MaxRaycastHits];
            Physics.RaycastNonAlloc(ray, raycastHits);
            var hitTileTransform = raycastHits.FirstOrDefault(x=>x.transform.CompareTag("Tile")).transform;
            if (ReferenceEquals(hitTileTransform, null))
            {
                return null;
            }
            var position = hitTileTransform.position;
            return gridManager.TileGrid[(int)position.x,(int)position.y];
        }

        private void RenderPathForUnitToTile(GameObject hoveredTile, List<TilePathfindingData> unitPathfindingdata)
        {
            GridManager.ChangeTilesSprites(unitPathfindingdata.Select(x => x.DestinationTile), selectedGridTileSprite);
            var selectedTilePathfindingData = unitPathfindingdata.FirstOrDefault(x => x.DestinationTile.gameObject == hoveredTile);
            if (selectedTilePathfindingData != null)
            {
                var pathfindingDataList = PathfindingHelper.GetPathToTile(unitPathfindingdata, selectedTilePathfindingData.DestinationTile).Select(x => x.DestinationTile);
                GridManager.ChangeTilesSprites(pathfindingDataList, unitPathGridTileSprite);
            }
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
                GridManager.ChangeTilesSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile), null);
            }
            SelectedUnit = null;
            SelectedUnitPathfindingData = null;
        }

        /// <summary>
        /// Displays the unit profile on the UI
        /// </summary>
        private void ShowUnitProfile(Unit hoveredUnit)
        {
            HoveredUnit = hoveredUnit;
            unitProfile.gameObject.SetActive(true);
            unitProfile.SetUnitToDisplay(hoveredUnit);
        }

        /// <summary>
        /// Hides the unit profile on the UI
        /// </summary>
        private void HideUnitProfile()
        {
            HoveredUnit = null;
            unitProfile.gameObject.SetActive(false);
        }

        /// <summary>
        /// Calculates asyncronously the unit's pathfinding data
        /// </summary>
        private async Task<List<TilePathfindingData>> CalculateUnitAvailablePathsAsync(Unit selectedUnit)
        {
            var selectedUnitPosition = selectedUnit.transform.position;
            var unitTile = gridManager.TileGrid[(int)selectedUnitPosition.x, (int)selectedUnitPosition.y];
            #if UNITY_WEBGL
                var unitPathfindingData = PathfindingHelper.CalculatePathfindingForAvailableMoves(gridManager.TileGrid, unitTile, selectedUnit.movement);
            #else
                var unitPathfindingData = await Task.Run(() => PathfindingHelper.CalculatePathfindingForAvailableMoves(_GridManager.TileGrid, selectedUnitPosition, selectedUnit.Movement));
            #endif
            return unitPathfindingData;
        }

        /// <summary>
        /// Moves the selectedUnit to the selectedTile
        /// </summary>
        private void MoveUnitToTile(Tile selectedTile,Unit selectedUnit, List<TilePathfindingData> unitPathfindingData)
        {
            if (selectedTile.terrainType != TerrainType.Impassable && unitPathfindingData != null)
            {
                var selectedTilePathfindingData = unitPathfindingData.FirstOrDefault(x => x.DestinationTile.gameObject == selectedTile.gameObject);
                if (selectedTilePathfindingData != null && selectedTilePathfindingData.MoveCost <= selectedUnit.movement)
                {
                    var pathfindingDataList = PathfindingHelper.GetPathToTile(unitPathfindingData, selectedTile).Select(x => x.DestinationTile);
                    //we move the unit reference from the starting tile to the selected tile
                    unitPathfindingData[0].DestinationTile.currentUnit = null;
                    selectedTile.currentUnit = SelectedUnit;
                    StartCoroutine(selectedUnit.FollowTilePath(pathfindingDataList));
                }

            }
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
            TurnCounter++;
            Debug.Log("Round " + TurnCounter + " ended!");
        }

        /// <summary>
        /// Makes actions with all the units in the EnemyUnits list
        /// </summary>
        private async void StartEnemyTurnAsync()
        {
            Debug.Log("Enemy Turn for round" + TurnCounter + " started!");
            foreach (var enemy in enemyUnits)
            {
                SelectUnit(enemy);
                SelectedUnitPathfindingData=await CalculateUnitAvailablePathsAsync(enemy);
                var randomTile = SelectedUnitPathfindingData[Random.Range(0, SelectedUnitPathfindingData.Count)].DestinationTile;
                MoveUnitToTile(randomTile,enemy, SelectedUnitPathfindingData);
                ResetSelectedUnit();
            }
            EndTurn();
        }
    }
}
