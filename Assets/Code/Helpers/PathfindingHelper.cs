using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Grid;
using Code.Models;
using UnityEngine;

namespace Code.Helpers
{
    /// <summary>
    /// Contains methods for pathfinding
    /// </summary>
    public static class PathfindingHelper 
    {

        //The cost of movement through difficult terrain
        private static readonly Dictionary<TerrainType, int> TerrainMoveCost = new Dictionary<TerrainType, int>()
        {
            { TerrainType.Normal,1 },
            { TerrainType.Difficult,4 }
        };
        
        /// <summary>
        /// Calculates asynchronously the unit's pathfinding data for all its moves
        /// </summary>
        public static async Task<List<TilePathfindingData>> CalculateUnitAvailablePathsAsync(Vector3 unitPosition, GridTile[,] tileGrid)
        {
            var selectedTile = tileGrid[(int)unitPosition.x, (int)unitPosition.y];
            #if UNITY_WEBGL && !UNITY_EDITOR 
                var unitPathfindingData = PathfindingHelper.CalculatePathfindingForAvailableMoves(tileGrid, selectedTile, selectedTile.currentUnit.movement);
            #else
                var unitPathfindingData = await Task.Run(() => CalculatePathfindingForAvailableMoves(tileGrid, selectedTile, selectedTile.CurrentUnit.Movement));
            #endif
            return unitPathfindingData;
        }

        /// <summary>
        /// Takes a tile's pathfinding data, and calculates the adjacent tiles' pathfinding data
        /// </summary>
        private static IEnumerable<TilePathfindingData> CalculateAdjacentTilePathfindingData(GridTile[,] tileGrid,TilePathfindingData sourceTilePathfindingData, IReadOnlyCollection<TilePathfindingData> analyzedTiles)
        {
            return (from adjacentTile in tileGrid.GetAdjacentGridTiles(sourceTilePathfindingData.DestinationGridTile)
                where (adjacentTile.TerrainType != TerrainType.Impassable &&
                       (ReferenceEquals(adjacentTile.CurrentUnit, null) ||
                        adjacentTile.CurrentUnit.Faction != UnitFaction.Monster)) && analyzedTiles.All(x => x.DestinationGridTile != adjacentTile)
                let tileMoveCost = sourceTilePathfindingData.MoveCost + TerrainMoveCost[adjacentTile.TerrainType]
                select new TilePathfindingData(adjacentTile, sourceTilePathfindingData, tileMoveCost, 0)).ToList();
        }

        /// <summary>
        /// Calculates and returns the tilePathfindingData of every available move for the selected unit in the grid, using Dijkstra pathfinding algorithm
        /// </summary>
        private static List<TilePathfindingData> CalculatePathfindingForAvailableMoves(GridTile[,] tileGrid, GridTile startingGridTile, int moveCount)
        {
            var remainingTilesToAnalyze = new List<TilePathfindingData>();
            var analyzedTiles = new List<TilePathfindingData>();

            //pathfindingData for the starting tile
            var startingTilePathfindingData = new TilePathfindingData(startingGridTile,null,0,0);
            remainingTilesToAnalyze.Add(startingTilePathfindingData);

            //We check the adjacent tiles of the first tile in remainingTilesToAnalyze, then we remove that tile from the list and then we order the list by totalTilePathCost
            do
            {
                var tileToAnalyze = remainingTilesToAnalyze[0];
                var adjacentTilesPathfindingData = CalculateAdjacentTilePathfindingData(tileGrid, tileToAnalyze, analyzedTiles);
                foreach (var tilePathfindingData in adjacentTilesPathfindingData)
                {
                    var existingTilePathfindingData = remainingTilesToAnalyze.FirstOrDefault(x => x.DestinationGridTile == tilePathfindingData.DestinationGridTile);
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
        public static IEnumerable<GridTile> GetPathToTile(IEnumerable<TilePathfindingData> pathfindingList, GridTile selectedGridTile)
        {
            var tileCursorInPathfindingList = pathfindingList.FirstOrDefault(x => x.DestinationGridTile == selectedGridTile); //the first tileCursor is our goal selectedTile
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
            return tilePath.Select(x=>x.DestinationGridTile);
        }
    }
}
