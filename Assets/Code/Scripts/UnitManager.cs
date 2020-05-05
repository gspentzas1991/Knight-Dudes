using Enums;
using Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

/// <summary>
/// Keeps track of 
/// </summary>
public class UnitManager : MonoBehaviour
{
    private Unit SelectedUnit;
    [SerializeField]
    private GridManager _GridManager;
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
    void Update()
    {
        //click gameobject detection
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (SelectedUnit == null)
                {
                    if (hit.transform.gameObject.tag == "Unit")
                    {
                        Unit clickedUnit = hit.transform.gameObject.GetComponent<Unit>();
                        if (clickedUnit.State==UnitState.Idle)
                        {
                            SelectUnit(clickedUnit);
                        }
                    }
                }
                else if (SelectedUnit.State == UnitState.Selected && hit.transform.gameObject.tag == "Tile" && hit.transform.position != SelectedUnit.transform.position)
                {
                    MoveUnitToTile(hit.transform.gameObject.GetComponent<Tile>());
                }
            }
        }
        //right click detection
        else if (Input.GetMouseButtonDown(1))
        {
            EndTurn();
        }
        //hover gameobject detection
        else if(SelectedUnit !=null && SelectedUnit.State == UnitState.Selected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //we run path rendering on hover, when not hovering over the selected unit, and when the hoveredTile changes
                if (hit.transform.position != SelectedUnit.transform.position && hit.transform.gameObject != CurrentHoveredTile)
                {
                    HoverOverUnitMoveTileHandler(hit.transform.gameObject);
                }

            }

        }

    }

    private void HoverOverUnitMoveTileHandler(GameObject hoveredTile)
    {
        _GridManager.ChangeTileListSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile), SelectedGridTileSprite);
        CurrentHoveredTile = hoveredTile;
        var selectedTilePathfindingData = SelectedUnitPathfindingData.FirstOrDefault(x => x.DestinationTile.gameObject == hoveredTile);
        if (selectedTilePathfindingData != null)
        {
            var pathfindingDataList = PathfindingHelper.GetTilepathToTile(SelectedUnitPathfindingData, selectedTilePathfindingData.DestinationTile).Select(x => x.DestinationTile);
            _GridManager.ChangeTileListSprites(pathfindingDataList, UnitPathGridTileSprite);
        }
    }

    /// <summary>
    /// Deselects the previous unit, and selects a new one
    /// </summary>
    private void SelectUnit(Unit unitToSelect)
    {
        Unit previousSelectedUnit = SelectedUnit;
        //reset the state of the previous selected unit
        if (previousSelectedUnit != null)
        {
            previousSelectedUnit.State = UnitState.Idle;
        }
        SelectedUnit = unitToSelect;
        SelectedUnit.State = UnitState.Selected;
        StartCoroutine(CalculatePossibleMovementsForUnit(SelectedUnit));
    }

    /// <summary>
    /// Moves the selectedUnit to the selectedTile
    /// </summary>
    private void MoveUnitToTile(Tile selectedTile)
    {
        if (selectedTile.TerrainType != TerrainType.Impassable && SelectedUnitPathfindingData != null && SelectedUnit.State != UnitState.Moving)
        {
            var selectedTilePathfindingData = SelectedUnitPathfindingData.FirstOrDefault(x => x.DestinationTile.gameObject == selectedTile.gameObject);
            if (selectedTilePathfindingData != null && selectedTilePathfindingData.MoveCost <= SelectedUnit.Movement)
            {
                var pathfindingDataList = PathfindingHelper.GetTilepathToTile(SelectedUnitPathfindingData, selectedTile).Select(x => x.DestinationTile);
                _GridManager.ChangeTileListSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile), DefaultGridTileSprite);
                StartCoroutine(SelectedUnit.FollowTilePath(pathfindingDataList));
                SelectedUnitPathfindingData = null;
                SelectedUnit = null;
            }

        }
    }

    /// <summary>
    /// Calculates the paths to all available tiles for the unit in a new thread, fills the SelectedUnitPathfindingData list and renders the available tile moves on the grid
    /// </summary>
    private IEnumerator CalculatePossibleMovementsForUnit(Unit selectedUnit)
    {
        Vector3 selectedUnitPosition = selectedUnit.transform.position;
        Thread newThread = new Thread(() => SelectedUnitPathfindingData = PathfindingHelper.CalculatePathfindingForAvailableMoves(_GridManager.TileGrid,selectedUnitPosition, selectedUnit.Movement));
        newThread.Start();
        while (newThread.IsAlive)
        {
            yield return null;
        }
        _GridManager.ChangeTileListSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile), SelectedGridTileSprite);
    }

    private void EndTurn()
    {
        foreach(Unit unit in PlayerUnits)
        {
            unit.ResetUnitTurnValues();
        }
    }
}
