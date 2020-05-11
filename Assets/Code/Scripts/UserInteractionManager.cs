using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Helpers;
using Code.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts
{
    public class UserInteractionManager : MonoBehaviour
    {
        private const int MaxRaycastHits = 2;
        private Unit SelectedUnit;
        private List<TilePathfindingData> SelectedUnitPathfindingData;
        private IEnumerable<Unit> AllUnits;
        private int TurnCounter = 1;
        private Tile PreviousHoveredTile;
        #pragma warning disable 0649
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Sprite selectedGridTileSprite;
        [SerializeField] private Sprite unitPathGridTileSprite ;
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
            var hoveredTile = DetectHoveredTile();
            Unit hoveredUnit = null;
            if (!ReferenceEquals(hoveredTile,null))
            {
                hoveredUnit = hoveredTile.currentUnit;
            }

            //Hovers over tile
            if (!ReferenceEquals(hoveredTile,null))
            {
                if (hoveredTile != PreviousHoveredTile)
                {                        
                    // Changes the hoveredTile to be the cursor, and the previous hovered tile to not be
                    // ReSharper disable once UseNullPropagation
                    if (!ReferenceEquals(PreviousHoveredTile,null))
                    {
                        PreviousHoveredTile.ChangeCursorRendererState(false);
                    }      
                    hoveredTile.ChangeCursorRendererState(true);      
                    if (SelectedUnitPathfindingData!=null)
                    {
                        var hoveredTilePathData = SelectedUnitPathfindingData.FirstOrDefault(x => x.DestinationTile == hoveredTile);
                        //If hovering within the selected unit's movement, renders the path, otherwise removes the rendered path
                        RenderPathForUnitToTile(hoveredTilePathData,SelectedUnitPathfindingData);
                    }
                    PreviousHoveredTile = hoveredTile;
                }

            }
            
            //Hovers over a unit
            if (!ReferenceEquals(hoveredUnit,null))
            {
                if (!unitProfile.gameObject.activeSelf)
                {
                    ShowUnitProfile(hoveredUnit);
                }
            }
            //Stopped hovering over a unit
            else if (unitProfile.gameObject.activeSelf)
            {
                unitProfile.gameObject.SetActive(false);
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
            SelectedUnitPathfindingData = await CalculateUnitAvailablePathsAsync(SelectedUnit);
            GridManager.ChangeTilesSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile), selectedGridTileSprite);
        }
        
        /// <summary>
        /// Returns the tile the mouse is hovering over
        /// </summary>
        private Tile DetectHoveredTile()
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
            return gridManager.TileGrid[(int)position.x,(int)position.y];
        }
        
        /// <summary>
        /// Renders a path from the unit to the selected tile
        /// </summary>
        private void RenderPathForUnitToTile(TilePathfindingData selectedTilePathfindingData, IReadOnlyCollection<TilePathfindingData> unitPathfindingData)
        {
            GridManager.ChangeTilesSprites(unitPathfindingData.Select(x => x.DestinationTile), selectedGridTileSprite);
            if (selectedTilePathfindingData == null)
            {
                return;
            }
            var pathfindingDataList = PathfindingHelper.GetPathToTile(unitPathfindingData, selectedTilePathfindingData.DestinationTile).Select(x => x.DestinationTile);
            GridManager.ChangeTilesSprites(pathfindingDataList, unitPathGridTileSprite);
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
            unitProfile.gameObject.SetActive(true);
            unitProfile.SetUnitToDisplay(hoveredUnit);
        }

        /// <summary>
        /// Calculates asynchronously the unit's pathfinding data
        /// </summary>
        private async Task<List<TilePathfindingData>> CalculateUnitAvailablePathsAsync(Unit selectedUnit)
        {
            var selectedUnitPosition = selectedUnit.transform.position;
            var unitTile = gridManager.TileGrid[(int)selectedUnitPosition.x, (int)selectedUnitPosition.y];
            #if UNITY_WEBGL && !UNITY_EDITOR 
                var unitPathfindingData = PathfindingHelper.CalculatePathfindingForAvailableMoves(gridManager.TileGrid, unitTile, selectedUnit.movement);
            #else
                var unitPathfindingData = await Task.Run(() => PathfindingHelper.CalculatePathfindingForAvailableMoves(gridManager.TileGrid, unitTile, selectedUnit.movement));
            #endif
            return unitPathfindingData;
        }

        /// <summary>
        /// Moves the selectedUnit to the selectedTile, if the selected tile is a valid move for that unit
        /// </summary>
        private void MoveUnitToTile(Tile selectedTile,Unit selectedUnit, IReadOnlyList<TilePathfindingData> unitPathfindingData)
        {
            if (selectedTile.terrainType == TerrainType.Impassable || unitPathfindingData == null || ReferenceEquals(selectedUnit, null))
            {
                return;
            }
            //checks that the selected tile is a valid move for the unit
            var selectedTilePathfindingData = unitPathfindingData.FirstOrDefault(x => x.DestinationTile == selectedTile);
            if (selectedTilePathfindingData == null || selectedTilePathfindingData.MoveCost > selectedUnit.movement)
            {
                return;
            }
            var pathfindingDataList = PathfindingHelper.GetPathToTile(unitPathfindingData, selectedTile).Select(x => x.DestinationTile);
            //we move the unit reference from the starting tile to the selected tile
            unitPathfindingData[0].DestinationTile.currentUnit = null;
            selectedTile.currentUnit = SelectedUnit;
            StartCoroutine(selectedUnit.FollowTilePath(pathfindingDataList));
            GridManager.ChangeTilesSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile), null);
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
                SelectedUnitPathfindingData=await CalculateUnitAvailablePathsAsync(enemy);
                var randomTile = SelectedUnitPathfindingData[Random.Range(0, SelectedUnitPathfindingData.Count)].DestinationTile;
                MoveUnitToTile(randomTile,enemy, SelectedUnitPathfindingData);
            }
            EndTurn();
        }
    }
}
