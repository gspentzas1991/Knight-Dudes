using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts
{
    public class Tile : MonoBehaviour
    {
        public TerrainType terrainType;
        public Unit currentUnit;
        public Vector3 positionInGrid;
        [SerializeField]
        private SpriteRenderer gridSpriteRenderer = null;
        [SerializeField]
        private SpriteRenderer gridCursorSpriteRenderer = null;

        public void ChangeGridSprite(Sprite newGridSprite)
        {
            gridSpriteRenderer.sprite = newGridSprite;
        }

        public void ChangeCursorRendererState(bool rendererState)
        {
            gridCursorSpriteRenderer.enabled = rendererState;
        }
    }
}
