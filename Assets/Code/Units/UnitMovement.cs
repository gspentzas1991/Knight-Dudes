using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Helpers;
using Code.Models;
using Code.Scripts;
using UnityEngine;

namespace Code.Units
{
    public class UnitMovement
    {
        /// <summary>
        /// Makes actions with all the units in the EnemyUnits list
        /// </summary>
        public async Task MoveEnemyUnitsAsync(IEnumerable<Unit> enemyUnits,GridTile[,] tileGrid,UnitSelector unitSelector)
        {
            foreach (var enemy in enemyUnits)
            {
                //temporary solution to fix bug
                enemy.state = UnitState.Idle;
                await unitSelector.ChangeSelectedUnitAsync(enemy, tileGrid);
                var randomTile = unitSelector.SelectedUnitPathfindingData[Random.Range(0, unitSelector.SelectedUnitPathfindingData.Count)].DestinationGridTile;
                MoveUnitToTile(randomTile,enemy, unitSelector.SelectedUnitPathfindingData,unitSelector);
            }
        }
        
        /// <summary>
        /// Moves the selectedUnit to the selectedTile, if the selected tile is a valid move for that unit
        /// </summary>
        public void MoveUnitToTile(GridTile selectedGridTile,Unit selectedUnit, IReadOnlyList<TilePathfindingData> unitPathfindingData,UnitSelector unitSelector)
        {
            if (selectedGridTile.terrainType == TerrainType.Impassable || unitPathfindingData == null || ReferenceEquals(selectedUnit, null))
            {
                return;
            }
            //checks that the selected tile is a valid move for the unit
            var selectedTilePathfindingData = unitPathfindingData.FirstOrDefault(x => x.DestinationGridTile == selectedGridTile);
            if (selectedTilePathfindingData == null || selectedTilePathfindingData.MoveCost > selectedUnit.movement)
            {
                return;
            }
            var pathfindingDataList = PathfindingHelper.GetPathToTile(unitPathfindingData, selectedGridTile);
            //we move the unit reference from the starting tile to the selected tile
            unitPathfindingData[0].DestinationGridTile.currentUnit = null;
            selectedGridTile.currentUnit = selectedUnit;
            selectedUnit.BeginFollowingTilePath(pathfindingDataList);
            TileRenderingHelper.ChangeTileSprites(unitSelector.SelectedUnitPathfindingData.Select(x => x.DestinationGridTile), TileState.Idle);
            unitSelector.DeselectUnit();
        }
    }
}