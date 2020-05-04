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
    public Unit SelectedUnit;
    public GridManager _GridManager;
    private List<TilePathfindingData> SelectedUnitPathfindingData;
    [SerializeField]
    private Sprite SelectedGridTileSprite;
    [SerializeField]
    private Sprite DefaultGridTileSprite;
    [SerializeField]
    private Sprite UnitPathGridTileSprite;

    void Update()
    {
        //click gameobject detection
        if (Input.GetMouseButtonDown(0) && SelectedUnit.IsMoving == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag=="Tile" && hit.transform.position!=SelectedUnit.transform.position)
                {
                    Tile selectedTile = hit.transform.gameObject.GetComponent<Tile>(); 
                    if (selectedTile.TerrainType != TerrainType.Impassable && SelectedUnitPathfindingData!=null && SelectedUnit.IsMoving != true)
                    {
                        var selectedTilePathfindingData = SelectedUnitPathfindingData.FirstOrDefault(x => x.DestinationTile.gameObject == hit.transform.gameObject);
                        if (selectedTilePathfindingData!=null && selectedTilePathfindingData.MoveCost <= SelectedUnit.MovementSpeed)
                        {
                            var pathfindingDataList = PathfindingHelper.GetTilepathToTile(SelectedUnitPathfindingData, selectedTile).Select(x => x.DestinationTile).ToList();
                            _GridManager.ChangeTileListSprites(SelectedUnitPathfindingData.Select(x=>x.DestinationTile).ToList(), DefaultGridTileSprite);
                            SelectedUnitPathfindingData = null;
                            StartCoroutine(SelectedUnit.FollowTilePath(pathfindingDataList));
                        }

                    }
                }
                else if (hit.transform.position == SelectedUnit.transform.position)
                {
                    StartCoroutine(CalculatePossibleMovementsForUnit(SelectedUnit));
                }
            }
        }
        //hover gameobject detection
        else if(SelectedUnitPathfindingData != null)
        {
            _GridManager.ChangeTileListSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile).ToList(), SelectedGridTileSprite);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var selectedTilePathfindingData = SelectedUnitPathfindingData.FirstOrDefault(x => x.DestinationTile.gameObject == hit.transform.gameObject);
                if (selectedTilePathfindingData!=null)
                {
                    var pathfindingDataList = PathfindingHelper.GetTilepathToTile(SelectedUnitPathfindingData, selectedTilePathfindingData.DestinationTile).Select(x => x.DestinationTile).ToList();
                    _GridManager.ChangeTileListSprites(pathfindingDataList, UnitPathGridTileSprite);
                }
            }

        }

    }

    /// <summary>
    /// Calculates the paths to all available tiles for the unit in a new thread, fills the SelectedUnitPathfindingData list and renders the available tile moves on the grid
    /// </summary>
    private IEnumerator CalculatePossibleMovementsForUnit(Unit selectedUnit)
    {
        Vector3 selectedUnitPosition = selectedUnit.transform.position;
        Thread newThread = new Thread(() => SelectedUnitPathfindingData = PathfindingHelper.CalculatePathfindingForAvailableMoves(_GridManager.TileGrid,selectedUnitPosition, selectedUnit.MovementSpeed));
        newThread.Start();
        while (newThread.IsAlive)
        {
            yield return null;
        }
        _GridManager.ChangeTileListSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile).ToList(), SelectedGridTileSprite);
    }
}
