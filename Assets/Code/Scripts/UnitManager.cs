using Enums;
using Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public Unit SelectedUnit;
    public GridManager _GridManager;

    //The cost of movement through difficult terrain
    public Dictionary<TerrainType, int> TerrainMoveCost = new Dictionary<TerrainType, int>()
        {
            { TerrainType.Normal,1 },
            { TerrainType.Difficult,4 }
        };

    void Start()
    {
        var tileGrid = _GridManager.GetTileGrid();
        //Tile selectedTile = tileGrid.Where(x=>x.transform.position == new Vector3(7,4,0)).FirstOrDefault();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SelectedUnit.IsMoving == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag=="Tile")
                {
                    Debug.Log("This is a Player");
                    Tile selectedTile = hit.transform.gameObject.GetComponent<Tile>(); 
                    if (selectedTile.TerrainType != TerrainType.Impassable)
                    {
                        List<Tile> tilePath = CreateTilePathFromSelectedTile(selectedTile);
                        StartCoroutine(SelectedUnit.FollowTilePath(tilePath));
                    }
                }
                else
                {
                    Debug.Log("This isn't a Player");
                }
            }
        }

    }


    /// <summary>
    /// Shows on the grid the available moves of the unit on the selectedTile
    /// </summary>
    /// <param name="selectedTile"></param>
    private void ShowUnitMovementOnGrid()
    {
        Debug.Log(SelectedUnit.name);
        Debug.Log(SelectedUnit.transform);
    }

    /// <summary>
    /// Calculates the pathfinding data for a tile
    /// </summary>
    private TilePathfindingData CalculatePathfingingData(Tile destinationTile,Vector3 goalPosition,TilePathfindingData closestSourceTile,int moveCost)
    {
        TilePathfindingData nextTilePathfindingData = new TilePathfindingData();
        nextTilePathfindingData.DestinationTile = destinationTile;
        nextTilePathfindingData.ClosestSourceTilePathfindingData = closestSourceTile;
        nextTilePathfindingData.MoveCost = moveCost;
        nextTilePathfindingData.TransformDistanceFromGoal = Mathf.Abs(destinationTile.transform.position.x - goalPosition.x)
                                                + Mathf.Abs(destinationTile.transform.position.y - goalPosition.y);
        return nextTilePathfindingData;
    }

    /// <summary>
    /// Takes a tile's pathfinding data, and calculates the 4 adjustent's tiles pathfinding data
    /// </summary>
    /// <param name="sourceTilePathfindingData"></param>
    /// <param name="goalTile"></param>
    private List<TilePathfindingData> CalculateAdjustentTilePathfindingData(TilePathfindingData sourceTilePathfindingData,Tile goalTile,List<TilePathfindingData> analyzedTileList)
    {
        var tileGrid = _GridManager.GetTileGrid();
        List<TilePathfindingData> adjustentTilePathfindingDataList = new List<TilePathfindingData>();
        List<(float x, float y)> AdjustentTilesCoordinates = new List<(float x, float y)>();
        AdjustentTilesCoordinates.Add((-1, 0));
        AdjustentTilesCoordinates.Add((1, 0));
        AdjustentTilesCoordinates.Add((0, -1));
        AdjustentTilesCoordinates.Add((0, 1));
        for (var i = 0; i < AdjustentTilesCoordinates.Count; i++)
        {
            var adjustenTileXCoordinate = sourceTilePathfindingData.DestinationTile.transform.position.x + AdjustentTilesCoordinates[i].x;
            var adjustenTileYCoordinate = sourceTilePathfindingData.DestinationTile.transform.position.y + AdjustentTilesCoordinates[i].y;
            //we make sure the tile we are checking is inside the grid
            if (Mathf.Abs(adjustenTileXCoordinate)<=Mathf.Abs(_GridManager.GridSize.x) && Mathf.Abs(adjustenTileYCoordinate) <= Mathf.Abs(_GridManager.GridSize.y))
            {
                Tile nextTile = tileGrid[(adjustenTileXCoordinate, adjustenTileYCoordinate)];
                //we ignore impassable tiles, and tiles that we have already analyzed
                if (nextTile.TerrainType != TerrainType.Impassable && !analyzedTileList.Any(x=>x.DestinationTile==nextTile))
                {
                    adjustentTilePathfindingDataList.Add(CalculatePathfingingData(nextTile, goalTile.transform.position, sourceTilePathfindingData, sourceTilePathfindingData.MoveCost + TerrainMoveCost[nextTile.TerrainType]));
                }
            }
        }
        return adjustentTilePathfindingDataList;
    }

    /// <summary>
    /// Calculates the tilepath to reach the selected tile with the A* pathfinding alogirthm
    /// </summary>
    public List<Tile> CreateTilePathFromSelectedTile(Tile selectedTile)
    {
        var tileGrid = _GridManager.GetTileGrid();
        Tile startingTile = tileGrid[(SelectedUnit.transform.position.x, SelectedUnit.transform.position.y)];

        List<TilePathfindingData> pathfindingList = new List<TilePathfindingData>();
        List<TilePathfindingData> analyzedTileList = new List<TilePathfindingData>();

        //initialization of the startingTilePathfindingData
        var startPathfingingData = CalculatePathfingingData(startingTile,selectedTile.transform.position, null, 0);
        pathfindingList.Add(startPathfingingData);
        TilePathfindingData tileToCalculate = null;

        //We check the four connectedTiles of the tile on the top of the pathfindingList, after that we remove that tile from the list and then we order the list by totalTilePathCost
        do
        {
            tileToCalculate = pathfindingList[0];
            var adjustentTilesPathfindingData = CalculateAdjustentTilePathfindingData(tileToCalculate, selectedTile, analyzedTileList);
            foreach (var tilePathfindingData in adjustentTilesPathfindingData)
            {
                var existingTilePathfindingData = pathfindingList.FirstOrDefault(x=>x.DestinationTile == tilePathfindingData.DestinationTile);
                //If we find a faster way to get to a tile that is already on the pathfindingList, we replace it with the new path
                if (existingTilePathfindingData != null && existingTilePathfindingData.MoveCost>tilePathfindingData.MoveCost)
                {
                    pathfindingList.Remove(existingTilePathfindingData);
                    pathfindingList.Add(tilePathfindingData);
                }
                //if the destinationTile is not on the pathfindingList, we add it
                else if(existingTilePathfindingData == null)
                {
                    pathfindingList.Add(tilePathfindingData);
                }
            }
            //after calculating all the adjustent tiles, we remove the sourceTile from the list, unless the tile we checked was the goal tile
            analyzedTileList.Add(tileToCalculate);
            pathfindingList.Remove(tileToCalculate);
            pathfindingList = pathfindingList.OrderBy(x => x.TotalTilePathCost).ToList();
        } while (pathfindingList[0].DestinationTile != selectedTile);

        //We resolve the tilePath, going backwards from the destination TilePathfindingData, until we reach the start
        List<Tile> tilePath = new List<Tile>();
        TilePathfindingData tileCursorInPathfindingList = pathfindingList[0];
        do
        {
            tilePath.Add(tileCursorInPathfindingList.DestinationTile);
            tileCursorInPathfindingList = tileCursorInPathfindingList.ClosestSourceTilePathfindingData;
        } while (tileCursorInPathfindingList != null);

        tilePath.Reverse();
        return tilePath;
    }
}
