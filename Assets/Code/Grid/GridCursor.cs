using System.Linq;
using UnityEngine;

namespace Code.Grid
{
    /// <summary>
    /// Keeps track of the currently hovered gridTile, and shows the cursor sprite that tile
    /// </summary>
    public class GridCursor
    {
        private GridTile PreviousHoveredGridTile;
        
        /// <summary>
        /// Hides the cursor on the previous GridTile, and shows it on the hoveredTile
        /// </summary>
        public bool HoveredTileChanged(GridTile hoveredTile)
        {
            if (hoveredTile == PreviousHoveredGridTile) return false;
            // Changes the hoveredTile to be the cursor, and the previous hovered tile to not be
            // ReSharper disable once UseNullPropagation
            if (!ReferenceEquals(PreviousHoveredGridTile,null))
            {
                PreviousHoveredGridTile.ChangeCursorRendererState(false);
            }      
            hoveredTile.ChangeCursorRendererState(true); 
            PreviousHoveredGridTile = hoveredTile;
            return true;
        }
        

    }
}