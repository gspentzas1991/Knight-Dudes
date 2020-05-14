using System.Threading.Tasks;
using Code.Grid;
using Code.Helpers;
using UnityEngine;

namespace Code.Units
{
    /// <summary>
    /// Keeps track of the currently selected unit and its pathfinding data
    /// </summary>
    public class UnitSelector : MonoBehaviour
    {
        public Unit SelectedUnit { get; private set; }
        //public List<TilePathfindingData> SelectedUnitPathfindingData { get; private set; }
        public async Task ChangeSelectedUnitAsync(Unit unitToSelect, GridTile[,] tileGrid)
        {
            //Clicked on an idle player unit
            if (unitToSelect.state != UnitState.Idle || SelectedUnit==unitToSelect) return;
            DeselectUnit();
            SelectedUnit = unitToSelect;
            if (SelectedUnit.pathfindingData==null)
            {
                SelectedUnit.pathfindingData = await PathfindingHelper.CalculateUnitAvailablePathsAsync(SelectedUnit.transform.position,tileGrid);
            }
        }
        
        /// <summary>
        /// Deselects the selectedUnit and renders the default grid on its movement path
        /// </summary>
        public void DeselectUnit()
        {
            if (ReferenceEquals(SelectedUnit, null)) return;
            SelectedUnit.state = UnitState.Idle;
            TileRenderingHelper.UnRenderUnitPaths(SelectedUnit);
            SelectedUnit = null;
        }
    }
}