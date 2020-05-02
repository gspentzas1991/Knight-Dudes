using Enums;
using Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

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

    //The cost of movement through difficult terrain
    public Dictionary<TerrainType, int> TerrainMoveCost = new Dictionary<TerrainType, int>()
        {
            { TerrainType.Normal,1 },
            { TerrainType.Difficult,4 }
        };

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
                            var pathfindingDataList = GetTilePathToTile(SelectedUnitPathfindingData, selectedTile).Select(x => x.DestinationTile).ToList();
                            ChangeTileListSprites(SelectedUnitPathfindingData.Select(x=>x.DestinationTile).ToList(), DefaultGridTileSprite);
                            SelectedUnitPathfindingData = null;
                            StartCoroutine(SelectedUnit.FollowTilePath(pathfindingDataList));
                        }

                    }
                }
                else if (hit.transform.position == SelectedUnit.transform.position)
                {
                    StartCoroutine(CalculatePossibleMovements(SelectedUnit));
                }
            }
        }
        //hover gameobject detection
        else if(SelectedUnitPathfindingData != null)
        {
            ChangeTileListSprites(SelectedUnitPathfindingData.Select(x => x.DestinationTile).ToList(), SelectedGridTileSprite);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var selectedTilePathfindingData = SelectedUnitPathfindingData.FirstOrDefault(x => x.DestinationTile.gameObject == hit.transform.gameObject);
                if (selectedTilePathfindingData!=null)
                {
                    var pathfindingDataList = GetTilePathToTile(SelectedUnitPathfindingData, selectedTilePathfindingData.DestinationTile).Select(x => x.DestinationTile).ToList();
                    ChangeTileListSprites(pathfindingDataList, UnitPathGridTileSprite);
                }
            }

        }

    }

    /// <summary>
    /// Calculates the paths to all available tiles for the unit and fills the SelectedUnitPathfindingData list
    /// </summary>
    private IEnumerator CalculatePossibleMovements(Unit selectedUnit)
    {
        Vector3 selectedUnitPosition = selectedUnit.transform.position;
        Thread newThread = new Thread(() => SelectedUnitPathfindingData = CreateTilePathFromSelectedTile(selectedUnitPosition, selectedUnit.MovementSpeed));
        newThread.Start();
        while (newThread.IsAlive)
        {
            yield return null;
        }
        ChangeTileListSprites(SelectedUnitPathfindingData.Select(x=>x.DestinationTile).ToList(), SelectedGridTileSprite);

    }

    /// <summary>
    /// Changes the sprite of every tile in the list
    /// </summary>
    private void ChangeTileListSprites(List<Tile> tileList, Sprite newSprite)
    {
        foreach (var tile in tileList)
        {
            tile.ChangeGridSprite(newSprite);
        }
    }

    /// <summary>
    /// Calculates the pathfinding data for a tile
    /// </summary>
    private TilePathfindingData CalculatePathfingingDataForTile(Tile destinationTile,TilePathfindingData closestSourceTile,int moveCost)
    {
        TilePathfindingData nextTilePathfindingData = new TilePathfindingData();
        nextTilePathfindingData.DestinationTile = destinationTile;
        nextTilePathfindingData.ClosestSourceTilePathfindingData = closestSourceTile;
        nextTilePathfindingData.MoveCost = moveCost;
        //commented out because we don't need the heuturistic value anymore
        //nextTilePathfindingData.TransformDistanceFromGoal = Mathf.Abs(destinationTile.PositionInGrid.x - goalPosition.x) + Mathf.Abs(destinationTile.PositionInGrid.y - goalPosition.y);
        return nextTilePathfindingData;
    }

    /// <summary>
    /// Takes a tile's pathfinding data, and calculates the 4 adjustent's tiles pathfinding data
    /// </summary>
    /// <param name="sourceTilePathfindingData"></param>
    /// <param name="goalTile"></param>
    private List<TilePathfindingData> CalculateAdjustentTilePathfindingData(TilePathfindingData sourceTilePathfindingData,List<TilePathfindingData> analyzedTileList)
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
            var adjustenTileXCoordinate = sourceTilePathfindingData.DestinationTile.PositionInGrid.x + AdjustentTilesCoordinates[i].x;
            var adjustenTileYCoordinate = sourceTilePathfindingData.DestinationTile.PositionInGrid.y + AdjustentTilesCoordinates[i].y;
            //we make sure the tile we are checking is inside the grid
            if (Mathf.Abs(adjustenTileXCoordinate)<=Mathf.Abs(_GridManager.GridSize.x) && Mathf.Abs(adjustenTileYCoordinate) <= Mathf.Abs(_GridManager.GridSize.y))
            {
                Tile nextTile = tileGrid[(adjustenTileXCoordinate, adjustenTileYCoordinate)];
                //we ignore impassable tiles, and tiles that we have already analyzed
                if (nextTile.TerrainType != TerrainType.Impassable && !analyzedTileList.Any(x=>x.DestinationTile==nextTile))
                {
                    adjustentTilePathfindingDataList.Add(CalculatePathfingingDataForTile(nextTile, sourceTilePathfindingData, sourceTilePathfindingData.MoveCost + TerrainMoveCost[nextTile.TerrainType]));
                }
            }
        }
        return adjustentTilePathfindingDataList;
    }

    /// <summary>
    /// Calculates the tilepath to reach the selected tile with the A* pathfinding alogirthm
    /// </summary>
    private List<TilePathfindingData> CreateTilePathFromSelectedTile(Vector3 selectedUnitPosition, int selectedUnitMovementSpeed)
    {
        var tileGrid = _GridManager.GetTileGrid();
        Tile startingTile = tileGrid[(selectedUnitPosition.x, selectedUnitPosition.y)];

        List<TilePathfindingData> pathfindingList = new List<TilePathfindingData>();
        List<TilePathfindingData> analyzedTileList = new List<TilePathfindingData>();

        //initialization of the startingTilePathfindingData
        var startPathfingingData = new TilePathfindingData();
        startPathfingingData.DestinationTile = startingTile;
        startPathfingingData.ClosestSourceTilePathfindingData = null;
        startPathfingingData.MoveCost = 0;
        startPathfingingData.TransformDistanceFromGoal = 0;
        pathfindingList.Add(startPathfingingData);
        TilePathfindingData tileToCalculate = null;

        //We check the four connectedTiles of the tile on the top of the pathfindingList, after that we remove that tile from the list and then we order the list by totalTilePathCost
        do
        {
            tileToCalculate = pathfindingList[0];
            var adjustentTilesPathfindingData = CalculateAdjustentTilePathfindingData(tileToCalculate, analyzedTileList);
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
        } while (pathfindingList.Any(x=>x.MoveCost<= selectedUnitMovementSpeed)); //we stop the pathfinding when all our moves cost more than the unit's movementSpeed
        return analyzedTileList;
    }

    /// <summary>
    /// Returns a list with the tiles of the fastest path a unit must go through to reach the selectedTile
    /// </summary>
    /// <param name="pathfindingList"></param>
    /// <param name="selectedTile"></param>
    /// <returns></returns>
    public List<TilePathfindingData> GetTilePathToTile (List<TilePathfindingData> pathfindingList, Tile selectedTile)
    {
        var tileCursorInPathfindingList = pathfindingList.FirstOrDefault(x => x.DestinationTile == selectedTile);
        if (tileCursorInPathfindingList != null)
        {
            //We resolve the tilePath, going backwards from the destination TilePathfindingData, until we reach the start
            List<TilePathfindingData> tilePath = new List<TilePathfindingData>();
            do
            {
                tilePath.Add(tileCursorInPathfindingList);
                tileCursorInPathfindingList = tileCursorInPathfindingList.ClosestSourceTilePathfindingData;
            } while (tileCursorInPathfindingList != null);

            tilePath.Reverse();
            return tilePath;
        }
        else
        {
            return null;
        }
    }
}
