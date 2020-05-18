using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Grid;
using Code.Helpers;
using UnityEngine;

namespace Code.Units
{
    /// <summary>
    /// Handles the unit movement to either selected tiles or AI chosen tiles
    /// </summary>
    public class UnitMovementHandler : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField]
        private UnitSelector _unitSelector;
        #pragma warning restore 0649
        
        /// <summary>
        /// Makes actions with all the units in the EnemyUnits list
        /// </summary>
        public async Task MoveEnemyUnitsAsync(IEnumerable<Unit> enemyUnits,GridTile[,] tileGrid)
        {
            foreach (var enemy in enemyUnits)
            {
                await _unitSelector.ChangeSelectedUnitAsync(enemy, tileGrid);
                var randomTile = enemy.PathfindingData[Random.Range(0, enemy.PathfindingData.Count)].DestinationGridTile;
                MoveUnitToTile(randomTile,enemy);
            }
        }
        
        /// <summary>
        /// Moves the selectedUnit to the selectedTile, if the selected tile is a valid move for that unit
        /// </summary>
        public void MoveUnitToTile(GridTile selectedGridTile,Unit selectedUnit)
        {
            if (selectedGridTile.TerrainType == TerrainType.Impassable || ReferenceEquals(selectedUnit, null))
            {
                return;
            }
            //checks that the selected tile is a valid move for the unit
            var selectedTilePathfindingData = selectedUnit.PathfindingData.FirstOrDefault(x => x.DestinationGridTile == selectedGridTile);
            if (selectedTilePathfindingData == null || selectedTilePathfindingData.MoveCost > selectedUnit.Movement)
            {
                return;
            }
            var pathfindingDataList = PathfindingHelper.GetPathToTile(selectedUnit.PathfindingData, selectedGridTile);
            //we move the unit reference from the starting tile to the selected tile
            selectedUnit.PathfindingData[0].DestinationGridTile.CurrentUnit = null;
            selectedGridTile.CurrentUnit = selectedUnit;
            selectedUnit.BeginFollowingTilePath(pathfindingDataList);
            _unitSelector.DeselectUnit();
            selectedUnit.State = UnitState.OutOfActions;
            selectedUnit.PathfindingData = null;
        }
    }
}