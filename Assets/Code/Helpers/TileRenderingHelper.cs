using System;
using System.Collections.Generic;
using System.Linq;
using Code.Grid;
using Code.Models;
using UnityEngine;

namespace Code.Helpers
{
    public static class TileRenderingHelper
    {
        private static readonly Sprite SelectedTileSprite =  Resources.Load<Sprite>("GridTileSprites/AvailableMoveTileSprite");
        private static readonly Sprite ActiveTileSprite = Resources.Load<Sprite>("GridTileSprites/PathTileSprite") ;
        
        /// <summary>
        /// Changes the sprite of every tile in a list
        /// </summary>
        public static void ChangeTileSprites(IEnumerable<GridTile> tileList, TileState state)
        {
            var tileSprite = GetSpriteFromTileState(state);
            foreach (var tile in tileList)
            {
                tile.ChangeGridSprite(tileSprite);
            }
        }

        /// <summary>
        /// Returns a tileSprite depending on the tileState
        /// </summary>
        private static Sprite GetSpriteFromTileState(TileState state)
        {
            switch (state)
            {
                case TileState.Selected:
                    return SelectedTileSprite;
                case TileState.Active:
                    return ActiveTileSprite;
                case TileState.Idle:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
            
        /// <summary>
        /// Renders a path from the unit to the selected tile
        /// </summary>
        public static void RenderPathForUnitToTile(TilePathfindingData selectedTilePathfindingData, IEnumerable<TilePathfindingData> unitPathfindingData)
        {
            var tilePathfindingData = unitPathfindingData.ToList();
            ChangeTileSprites(tilePathfindingData.Select(x => x.DestinationGridTile), TileState.Selected);
            if (selectedTilePathfindingData == null)
            {
                return;
            }
            var pathfindingDataList = PathfindingHelper.GetPathToTile(tilePathfindingData, selectedTilePathfindingData.DestinationGridTile);
            ChangeTileSprites(pathfindingDataList, TileState.Active);
        }
    }
}