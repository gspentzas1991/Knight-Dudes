using UnityEngine;

namespace Code.Scripts
{
    public class GridTile : MonoBehaviour
    {
        public TerrainType terrainType;
        public Unit currentUnit;
        public Vector3 positionInGrid;
        #pragma warning disable 0649
        [SerializeField] private SpriteRenderer gridSpriteRenderer;
        [SerializeField] private SpriteRenderer gridCursorSpriteRenderer;
        #pragma warning restore 0649

        public void ChangeGridSprite(Sprite newGridSprite)
        {
            gridSpriteRenderer.sprite = newGridSprite;
        }

        /// <summary>
        /// enables or disables the spriteRenderer of the cursor gameobject
        /// </summary>
        public void ChangeCursorRendererState(bool rendererState)
        {
            gridCursorSpriteRenderer.enabled = rendererState;
        }
    }
}
