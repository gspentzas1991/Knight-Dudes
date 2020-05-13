using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Grid;
using Code.Helpers;
using Code.Models;

namespace Code.Units
{
    public class UnitSelector
    {
        public Unit SelectedUnit { get; private set; }
        public List<TilePathfindingData> SelectedUnitPathfindingData { get; private set; }
        public async Task ChangeSelectedUnitAsync(Unit unitToSelect, GridTile[,] tileGrid)
        {
            //Clicked on an idle player unit
            if (unitToSelect.state != UnitState.Idle || SelectedUnit==unitToSelect) return;
            DeselectUnit();
            SelectedUnit = unitToSelect;
            SelectedUnitPathfindingData = await PathfindingHelper.CalculateUnitAvailablePathsAsync(SelectedUnit.transform.position,tileGrid);
        }
        
        /// <summary>
        /// Deselects the selectedUnit and renders the default grid on its movement path
        /// </summary>
        public void DeselectUnit()
        {
            if (!ReferenceEquals(SelectedUnit,null))
            {
                SelectedUnit.state = UnitState.Idle;
                TileRenderingHelper.ChangeTileSprites(SelectedUnitPathfindingData.Select(x => x.DestinationGridTile), TileState.Idle);
            }
            SelectedUnit = null;
            SelectedUnitPathfindingData = null;
        }
    }
}