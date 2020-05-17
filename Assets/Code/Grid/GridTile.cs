using Code.Units;
using UnityEngine;

namespace Code.Grid
{
    /// <summary>
    /// Represents a single tile on the grid
    /// </summary>
    public class GridTile : MonoBehaviour
    {
        public TerrainType TerrainType;
        public Unit CurrentUnit;
        public Vector3 PositionInGrid;
        #pragma warning disable 0649
        [SerializeField] private SpriteRenderer _gridSpriteRenderer;
        [SerializeField] private SpriteRenderer _gridCursorSpriteRenderer;
        #pragma warning restore 0649

        public void ChangeGridSprite(Sprite newGridSprite)
        {
            _gridSpriteRenderer.sprite = newGridSprite;
        }

        /// <summary>
        /// enables or disables the spriteRenderer of the cursor gameobject
        /// </summary>
        public void ChangeCursorRendererState(bool rendererState)
        {
            _gridCursorSpriteRenderer.enabled = rendererState;
        }
    }
}
