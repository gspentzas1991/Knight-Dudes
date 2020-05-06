using Enums;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathfindingHelper 
{

    //The cost of movement through difficult terrain
    private static Dictionary<TerrainType, int> TerrainMoveCost = new Dictionary<TerrainType, int>()
        {
            { TerrainType.Normal,1 },
            { TerrainType.Difficult,4 }
        };
    //The relative x-y coordinates for the 4 adjustent tiles of any tile
    private static List<(float x, float y)> AdjustentTilesCoordinates = new List<(float x, float y)>()
    {
        (-1,0),
        (1,0),
        (0,-1),
        (0,1)
    };

    /// <summary>
    /// Takes a tile's pathfinding data, and calculates the 4 adjustent's tiles pathfinding data
    /// </summary>
    public static List<TilePathfindingData> CalculateAdjustentTilePathfindingData(Dictionary<(float x, float y), Tile> tileGrid,TilePathfindingData sourceTilePathfindingData, List<TilePathfindingData> analyzedTileList)
    {
        List<TilePathfindingData> adjustentTilePathfindingDataList = new List<TilePathfindingData>();
        for (var i = 0; i < AdjustentTilesCoordinates.Count; i++)
        {
            var adjustenTileXCoordinate = sourceTilePathfindingData.DestinationTile.PositionInGrid.x + AdjustentTilesCoordinates[i].x;
            var adjustenTileYCoordinate = sourceTilePathfindingData.DestinationTile.PositionInGrid.y + AdjustentTilesCoordinates[i].y;
            //we make sure the tile we are checking is inside the grid
            if (tileGrid.TryGetValue((adjustenTileXCoordinate, adjustenTileYCoordinate), out Tile tileToCheck))
            {
                //we ignore impassable tiles, and tiles that we have already analyzed
                if (tileToCheck.TerrainType != TerrainType.Impassable && !analyzedTileList.Any(x => x.DestinationTile == tileToCheck))
                {
                    adjustentTilePathfindingDataList.Add(new TilePathfindingData(tileToCheck, sourceTilePathfindingData, sourceTilePathfindingData.MoveCost + TerrainMoveCost[tileToCheck.TerrainType],0));
                }
            }
        }
        return adjustentTilePathfindingDataList;
    }

    /// <summary>
    /// Calculates and returns the tilePathfindingData of every available move for the selected unit in the grid, using Dijkstra pathfinding alogirthm
    /// </summary>
    public static List<TilePathfindingData> CalculatePathfindingForAvailableMoves(Dictionary<(float x, float y), Tile> tileGrid, Vector3 selectedUnitPosition, int selectedUnitMovementSpeed)
    {
        Tile startingTile = tileGrid[(selectedUnitPosition.x, selectedUnitPosition.y)];

        List<TilePathfindingData> remainingTilesToAnalyze = new List<TilePathfindingData>();
        List<TilePathfindingData> analyzedTiles = new List<TilePathfindingData>();

        //initialization of the startingTilePathfindingData
        var startingTilePathfingingData = new TilePathfindingData(startingTile,null,0,0);
        remainingTilesToAnalyze.Add(startingTilePathfingingData);
        TilePathfindingData nextTileToAnalyze = null;

        //We check the four connectedTiles of the tile on the top of the pathfindingList, after that we remove that tile from the list and then we order the list by totalTilePathCost
        do
        {
            nextTileToAnalyze = remainingTilesToAnalyze[0];
            var adjustentTilesPathfindingData = CalculateAdjustentTilePathfindingData(tileGrid, nextTileToAnalyze, analyzedTiles);
            foreach (var tilePathfindingData in adjustentTilesPathfindingData)
            {
                var existingTilePathfindingData = remainingTilesToAnalyze.FirstOrDefault(x => x.DestinationTile == tilePathfindingData.DestinationTile);
                //If we find a faster way to get to a tile that is already on the remainingTilesToAnalyze, we replace it with the new pathfinding data
                if (existingTilePathfindingData != null && existingTilePathfindingData.MoveCost > tilePathfindingData.MoveCost)
                {
                    remainingTilesToAnalyze.Remove(existingTilePathfindingData);
                    remainingTilesToAnalyze.Add(tilePathfindingData);
                }
                //if the destinationTile is not on the remainingTilesToAnalyze list, we add it
                else if (existingTilePathfindingData == null)
                {
                    remainingTilesToAnalyze.Add(tilePathfindingData);
                }
            }
            analyzedTiles.Add(nextTileToAnalyze);
            remainingTilesToAnalyze.Remove(nextTileToAnalyze);
            remainingTilesToAnalyze = remainingTilesToAnalyze.OrderBy(x => x.TotalTilePathCost).ToList();
        } while (remainingTilesToAnalyze.Any(x => x.MoveCost <= selectedUnitMovementSpeed)); //we stop the pathfinding when all our moves cost more than the unit's movementSpeed
        return analyzedTiles;
    }

    /// <summary>
    /// Returns a list with the tiles of the fastest path a unit must go through to reach the selectedTile
    /// </summary>
    public static List<TilePathfindingData> GetTilepathToTile(List<TilePathfindingData> pathfindingList, Tile selectedTile)
    {
        var tileCursorInPathfindingList = pathfindingList.FirstOrDefault(x => x.DestinationTile == selectedTile); //the first tileCursor is our goal selectedTile
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
