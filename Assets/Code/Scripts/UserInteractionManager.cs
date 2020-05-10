using Enums;
using Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class UserInteractionManager : MonoBehaviour
{
    private Unit SelectedUnit;
    [SerializeField]
    private GridManager _GridManager = null;
    private List<TilePathfindingData> SelectedUnitPathfindingData;
    [SerializeField]
    private Sprite SelectedGridTileSprite = null;
    [SerializeField]
    private Sprite DefaultGridTileSprite = null;
    [SerializeField]
    private Sprite UnitPathGridTileSprite = null;
    private GameObject CurrentHoveredTile;
    [SerializeField]
    private List<Unit> PlayerUnits = new List<Unit>();
    [SerializeField]
    private List<Unit> EnemyUnits = new List<Unit>();
    private int TurnCounter = 1;
    [SerializeField]
    private UnitProfileUI UnitProfile = null;
    private Unit HoveredUnit = null;


    async void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            //click gameobject detection
            if (Input.GetMouseButtonDown(0))
            {
                //clicked on a new unit
                if (hit.transform.gameObject.tag == "Unit")
                {
                    Unit clickedUnit = hit.transform.gameObject.GetComponent<Unit>();
                    if (clickedUnit.State==UnitState.Idle && PlayerUnits.Any(x=>x==clickedUnit))
                    {
                        if (SelectedUnit!=null)
                        {
                            ResetSelectedUnit();
                        }
                        SelectUnit(clickedUnit);
                        SelectedUnitPathfindingData = await CalculateUnitAvailablePathsAsync(SelectedUnit);
                        _GridManager.ChangeTileListSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile), SelectedGridTileSprite);
                    }
                }
                //clicked on a tile to move selected unit
                else if (SelectedUnit!=null && SelectedUnit.State == UnitState.Selected && hit.transform.gameObject.tag == "Tile" && hit.transform.position != SelectedUnit.transform.position)
                {
                    MoveUnitToTile(hit.transform.gameObject.GetComponent<Tile>(),SelectedUnit,SelectedUnitPathfindingData);
                    ResetSelectedUnit();
                }
            }
            //right click detection
            else if (Input.GetMouseButtonDown(1))
            {
                StartEnemyTurnAsync();
            }
            //hover on tile within selectedUnit's available movement
            else if(SelectedUnit != null && SelectedUnit.State == UnitState.Selected && SelectedUnitPathfindingData!=null)
            {
                //we run path rendering on hover when the hoveredTile changes
                if (hit.transform.gameObject != CurrentHoveredTile)
                {
                    RenderPathForUnitToTile(hit.transform.gameObject,SelectedUnitPathfindingData);
                }

            }
            //hover over a unit
            if (hit.transform.tag=="Unit")
            {
                Unit CurrentHoveredUnit = hit.transform.gameObject.GetComponent<Unit>();
                if (CurrentHoveredUnit != HoveredUnit)
                {
                    ShowUnitProfile(CurrentHoveredUnit);
                }
            }
            //stopped hovering over a unit
            else if (HoveredUnit!=null)
            {
                HideUnitProfile();
            }
        }

    }

    private void RenderPathForUnitToTile(GameObject hoveredTile, List<TilePathfindingData> unitPathfindingdata)
    {
        CurrentHoveredTile = hoveredTile;
        _GridManager.ChangeTileListSprites(unitPathfindingdata.Select(x => x.DestinationTile), SelectedGridTileSprite);
        var selectedTilePathfindingData = unitPathfindingdata.FirstOrDefault(x => x.DestinationTile.gameObject == hoveredTile);
        if (selectedTilePathfindingData != null)
        {
            var pathfindingDataList = PathfindingHelper.GetTilepathToTile(unitPathfindingdata, selectedTilePathfindingData.DestinationTile).Select(x => x.DestinationTile);
            _GridManager.ChangeTileListSprites(pathfindingDataList, UnitPathGridTileSprite);
        }
    }

    /// <summary>
    /// Deselects the previous unit, selects a new one and renders its possible movements
    /// </summary>
    private void SelectUnit(Unit unitToSelect)
    {
        SelectedUnit = unitToSelect;
        SelectedUnit.State = UnitState.Selected; 
    }

    /// <summary>
    /// Deselects the selectedUnit and renders the default grid on its movement path
    /// </summary>
    private void ResetSelectedUnit()
    {
        if (SelectedUnit.State==UnitState.Selected)
        {
            SelectedUnit.State = UnitState.Idle;
            _GridManager.ChangeTileListSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile), DefaultGridTileSprite);
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
        UnitProfile.gameObject.SetActive(true);
        UnitProfile.SetUnitToDisplay(hoveredUnit);
    }

    /// <summary>
    /// Hides the unit profile on the UI
    /// </summary>
    private void HideUnitProfile()
    {
        HoveredUnit = null;
        UnitProfile.gameObject.SetActive(false);
    }

    /// <summary>
    /// Calculates asyncronously the unit's pathfinding data
    /// </summary>
    private async Task<List<TilePathfindingData>> CalculateUnitAvailablePathsAsync(Unit selectedUnit)
    {
        Vector3 selectedUnitPosition = selectedUnit.transform.position;

        #if UNITY_WEBGL
            var unitPathfindingData = PathfindingHelper.CalculatePathfindingForAvailableMoves(_GridManager.TileGrid, selectedUnitPosition, selectedUnit.Movement);
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
        if (selectedTile.TerrainType != TerrainType.Impassable && unitPathfindingData != null)
        {
            var selectedTilePathfindingData = unitPathfindingData.FirstOrDefault(x => x.DestinationTile.gameObject == selectedTile.gameObject);
            if (selectedTilePathfindingData != null && selectedTilePathfindingData.MoveCost <= selectedUnit.Movement)
            {
                var pathfindingDataList = PathfindingHelper.GetTilepathToTile(unitPathfindingData, selectedTile).Select(x => x.DestinationTile);
                _GridManager.ChangeTileListSprites(unitPathfindingData.Select(x => x.DestinationTile), DefaultGridTileSprite);
                StartCoroutine(selectedUnit.FollowTilePath(pathfindingDataList));
            }

        }
    }

    private void EndTurn()
    {
        foreach(Unit unit in PlayerUnits)
        {
            unit.ResetUnitTurnValues();
        }
        foreach (Unit unit in EnemyUnits)
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
        foreach (Unit enemy in EnemyUnits)
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
