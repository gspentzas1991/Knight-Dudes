using System.Collections.Generic;
using System.Linq;
using Code.Models;
using Code.Scripts;
using UnityEngine;

namespace Code.Helpers
{
    public static class TileRenderingHelper
    {
        private static readonly Sprite AvailableMoveTileSprite =  Resources.Load<Sprite>("GridTileSprites/AvailableMoveTileSprite");
        private static readonly Sprite PathTileSprite = Resources.Load<Sprite>("GridTileSprites/PathTileSprite") ;
        
        /// <summary>
        /// Changes the sprite of every tile in a list
        /// </summary>
        public static void ChangeTileSprites(IEnumerable<GridTile> tileList, Sprite newSprite)
        {
            foreach (var tile in tileList)
            {
                tile.ChangeGridSprite(newSprite);
            }
        }
            
        /// <summary>
        /// Renders a path from the unit to the selected tile
        /// </summary>
        public static void RenderPathForUnitToTile(TilePathfindingData selectedTilePathfindingData, IEnumerable<TilePathfindingData> unitPathfindingData)
        {
            var tilePathfindingData = unitPathfindingData.ToList();
            ChangeTileSprites(tilePathfindingData.Select(x => x.DestinationGridTile), AvailableMoveTileSprite);
            if (selectedTilePathfindingData == null)
            {
                return;
            }
            var pathfindingDataList = PathfindingHelper.GetPathToTile(tilePathfindingData, selectedTilePathfindingData.DestinationGridTile);
            ChangeTileSprites(pathfindingDataList, PathTileSprite);
        }
    }
}