using System;
using System.Collections.Generic;
using System.Linq;
using Code.Grid;
using Code.Models;
using Code.Units;
using UnityEngine;

namespace Code.Helpers
{
    /// <summary>
    /// Contains methods for rendering sprites on tiles
    /// </summary>
    public static class TileRenderingHelper
    {
        private static readonly Sprite SelectedTileSprite =  Resources.Load<Sprite>("GridTileSprites/AvailableMoveTileSprite");
        private static readonly Sprite ActiveTileSprite = Resources.Load<Sprite>("GridTileSprites/PathTileSprite") ;
        
        /// <summary>
        /// Changes the sprite of every tile in a list
        /// </summary>
        private static void ChangeTileSprites(IEnumerable<GridTile> tileList, TileState state)
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
        /// Renders a path from the unit to the selected tile with active tile sprites
        /// </summary>
        public static void RenderPathToTile(TilePathfindingData selectedTilePathfindingData, IEnumerable<TilePathfindingData> unitPathfindingData)
        {
            var tilePathfindingData = unitPathfindingData.ToList();
            ChangeTileSprites(tilePathfindingData.Select(x => x.DestinationGridTile), TileState.Selected);
            if (selectedTilePathfindingData == null) return;
            var pathfindingDataList = PathfindingHelper.GetPathToTile(tilePathfindingData, selectedTilePathfindingData.DestinationGridTile);
            ChangeTileSprites(pathfindingDataList, TileState.Active);
        }

        /// <summary>
        /// Renderers every tile the unit can move to, as a selected tile
        /// </summary>
        public static void RenderUnitAvailablePaths(Unit selectedUnit)
        {
            ChangeTileSprites(selectedUnit._pathfindingData.Select(x => x.DestinationGridTile), TileState.Selected);
        }

        /// <summary>
        /// UnRenders all the tiles that a unit could move to
        /// </summary>
        public static void UnRenderUnitPaths(Unit selectedUnit)
        {
            ChangeTileSprites(selectedUnit._pathfindingData.Select(x => x.DestinationGridTile), TileState.Idle);
        }
    }
}