using System.Collections.Generic;
using System.Linq;
using Code.Grid;
using UnityEngine;

namespace Code.Helpers
{
    public static class TileGridExtender
    {
        //The relative x-y coordinates for the 4 adjacent tiles of any tile
        private static readonly List<(int x, int y)> AdjacentTilesCoordinates = new List<(int x, int y)>()
        {
            (-1,0),
            (1,0),
            (0,-1),
            (0,1)
        };
        
        /// <summary>
        /// Returns true if the coordinates point in a valid gridTile in the grid
        /// </summary>
        public static bool CoordinatesWithinGrid(this GridTile[,] tileGrid,Vector2 coordinates)
        {
            return !((int)coordinates.x < 0) && !((int)coordinates.x > tileGrid.GetLength(0)) 
                                             && !((int)coordinates.y < 0) && !((int)coordinates.y > tileGrid.GetLength(1));
        }

        /// <summary>
        /// Returns the adjacent tiles of the selected tile
        /// </summary>
        public static IEnumerable<GridTile> GetAdjacentGridTiles(this GridTile[,] tileGrid,GridTile selectedTile)
        {
            return
                from adjacentTileCoordinates in AdjacentTilesCoordinates.Select((t, i) => new Vector2
                {
                    x = selectedTile.PositionInGrid.x + AdjacentTilesCoordinates[i].x,
                    y = selectedTile.PositionInGrid.y + AdjacentTilesCoordinates[i].y
                })
                where tileGrid.CoordinatesWithinGrid(adjacentTileCoordinates)
                select tileGrid[(int) adjacentTileCoordinates.x, (int) adjacentTileCoordinates.y];
        }

        /// <summary>
        /// Returns the tiles whose distance away from the starting tile is any of the rangeList values
        /// </summary>
        public static IEnumerable<GridTile> GetGridTilesOfRange(this GridTile[,] tileGrid, GridTile startingTile,
            IEnumerable<int> rangeList)
        {
            var tileList = new List<GridTile>();
            foreach (var range in rangeList)
            {
                tileList.AddRange(tileGrid.GetGridTilesOfRange(startingTile, range));
            }
            return tileList;
        }
        
        /// <summary>
        /// Returns the tiles whose distance away from the starting tile is the range value
        /// </summary>
        public static IEnumerable<GridTile> GetGridTilesOfRange(this GridTile[,] tileGrid,GridTile startingTile, int range)
        {
            var tileList = new List<GridTile>();
            for (var x = -range; x <= range; x++)
            {
                for (var y = -range; y <= range; y++)
                {
                    var tilePosition = new Vector2(startingTile.PositionInGrid.x+x,startingTile.PositionInGrid.y+y);
                    if (!tileGrid.CoordinatesWithinGrid(new Vector2(tilePosition.x, tilePosition.y))) continue;
                    if (Mathf.Abs(x) + Mathf.Abs(y) == range)
                    {
                        tileList.Add(tileGrid[(int)tilePosition.x, (int)tilePosition.y]);
                    }
                }
            }
            return tileList;
        }
    }
}