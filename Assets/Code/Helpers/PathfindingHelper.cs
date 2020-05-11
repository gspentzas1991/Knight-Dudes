using System.Collections.Generic;
using System.Linq;
using Code.Models;
using Code.Scripts;

namespace Code.Helpers
{
    public static class PathfindingHelper 
    {

        //The cost of movement through difficult terrain
        private static readonly Dictionary<TerrainType, int> TerrainMoveCost = new Dictionary<TerrainType, int>()
        {
            { TerrainType.Normal,1 },
            { TerrainType.Difficult,4 }
        };
        //The relative x-y coordinates for the 4 adjacent tiles of any tile
        private static readonly List<(int x, int y)> AdjacentTilesCoordinates = new List<(int x, int y)>()
        {
            (-1,0),
            (1,0),
            (0,-1),
            (0,1)
        };

        /// <summary>
        /// Takes a tile's pathfinding data, and calculates the adjacent tiles' pathfinding data
        /// </summary>
        private static IEnumerable<TilePathfindingData> CalculateAdjacentTilePathfindingData(Tile[,] tileGrid,TilePathfindingData sourceTilePathfindingData, IReadOnlyCollection<TilePathfindingData> analyzedTiles)
        {
            var adjacentTilesPathfindingData = new List<TilePathfindingData>();
            for (var i = 0; i < AdjacentTilesCoordinates.Count; i++)
            {
                var adjacentTileXCoordinate = (int)sourceTilePathfindingData.DestinationTile.positionInGrid.x + AdjacentTilesCoordinates[i].x;
                var adjacentTileYCoordinate = (int)sourceTilePathfindingData.DestinationTile.positionInGrid.y + AdjacentTilesCoordinates[i].y;
                //we make sure the tile we are checking is inside the grid
                if (adjacentTileXCoordinate < 0 || adjacentTileXCoordinate > tileGrid.GetLength(0) ||
                    adjacentTileYCoordinate < 0 || adjacentTileYCoordinate > tileGrid.GetLength(1))
                {
                    continue;
                }
                var tileToAnalyze = tileGrid[adjacentTileXCoordinate, adjacentTileYCoordinate];
                //we ignore impassable tiles, and tiles that we have already analyzed
                if (tileToAnalyze.terrainType == TerrainType.Impassable ||
                    analyzedTiles.Any(x => x.DestinationTile == tileToAnalyze))
                {
                    continue;
                }
                var tileMoveCost = sourceTilePathfindingData.MoveCost + TerrainMoveCost[tileToAnalyze.terrainType];
                adjacentTilesPathfindingData.Add(new TilePathfindingData(tileToAnalyze, sourceTilePathfindingData, tileMoveCost,0));
            }
            return adjacentTilesPathfindingData;
        }

        /// <summary>
        /// Calculates and returns the tilePathfindingData of every available move for the selected unit in the grid, using Dijkstra pathfinding algorithm
        /// </summary>
        public static List<TilePathfindingData> CalculatePathfindingForAvailableMoves(Tile[,] tileGrid, Tile startingTile, int moveCount)
        {
            var remainingTilesToAnalyze = new List<TilePathfindingData>();
            var analyzedTiles = new List<TilePathfindingData>();

            //pathfindingData for the starting tile
            var startingTilePathfindingData = new TilePathfindingData(startingTile,null,0,0);
            remainingTilesToAnalyze.Add(startingTilePathfindingData);

            //We check the adjacent tiles of the first tile in remainingTilesToAnalyze, then we remove that tile from the list and then we order the list by totalTilePathCost
            do
            {
                var tileToAnalyze = remainingTilesToAnalyze[0];
                var adjacentTilesPathfindingData = CalculateAdjacentTilePathfindingData(tileGrid, tileToAnalyze, analyzedTiles);
                foreach (var tilePathfindingData in adjacentTilesPathfindingData)
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
                analyzedTiles.Add(tileToAnalyze);
                remainingTilesToAnalyze.Remove(tileToAnalyze);
                remainingTilesToAnalyze = remainingTilesToAnalyze.OrderBy(x => x.TotalTilePathCost).ToList();
            } while (remainingTilesToAnalyze.Any(x => x.MoveCost <= moveCount)); //we stop the pathfinding when all our moves cost more than the unit's movementSpeed
            return analyzedTiles;
        }

        /// <summary>
        /// Returns a list with the fastest tile path a unit can take to reach the selected tile
        /// </summary>
        public static IEnumerable<TilePathfindingData> GetPathToTile(IEnumerable<TilePathfindingData> pathfindingList, Tile selectedTile)
        {
            var tileCursorInPathfindingList = pathfindingList.FirstOrDefault(x => x.DestinationTile == selectedTile); //the first tileCursor is our goal selectedTile
            if (tileCursorInPathfindingList == null)
            {
                return null;
            }
            //We resolve the tilePath, going backwards from the destination TilePathfindingData, until we reach the start
            var tilePath = new List<TilePathfindingData>();
            do
            {
                tilePath.Add(tileCursorInPathfindingList);
                tileCursorInPathfindingList = tileCursorInPathfindingList.ClosestSourceTilePathfindingData;
            } while (tileCursorInPathfindingList != null); 
            tilePath.Reverse();
            return tilePath;
        }
    }
}
