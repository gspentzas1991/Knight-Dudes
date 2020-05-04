using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Enums;

namespace Scripts
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private Vector2 GridSize = new Vector2();
        [SerializeField]
        private GameObject GridOutlinePrefab = null;
        [SerializeField]
        private Transform GridStartTransform = null;
        [SerializeField]
        private List<Tile> SpecialTilesList = null;
        public Dictionary<(float x, float y), Tile> TileGrid { get; private set; } = new  Dictionary<(float x, float y), Tile>();

        // Start is called before the first frame update
        void Start()
        {
            GenerateGrid();
        }

        /// <summary>
        /// Generates Tile GameObjects, and adds them on the TileGrid list
        /// </summary>
        private void GenerateGrid()
        {
            foreach(Tile specialTile in SpecialTilesList)
            {
                Vector3 tilePosition = specialTile.transform.position;
                specialTile.PositionInGrid = tilePosition;
                TileGrid.Add((tilePosition.x, tilePosition.y), specialTile);
            }

            for (float i = 0; i <= GridSize.x; i++)
            {
                for (float j = 0; j <= GridSize.y; j++)
                {
                    Vector3 nextTilePosition = GridStartTransform.position + new Vector3(i,j,0);
                    if(!TileGrid.TryGetValue((nextTilePosition.x, nextTilePosition.y),out Tile existingTile))
                    {
                        var newTile = Instantiate(GridOutlinePrefab, nextTilePosition, new Quaternion()).GetComponent<Tile>();
                        newTile.TerrainType = TerrainType.Normal;
                        newTile.PositionInGrid = newTile.transform.position;
                        TileGrid.Add((nextTilePosition.x, nextTilePosition.y), newTile);
                    }

                }
            }
        }

        /// <summary>
        /// Changes the sprite of every tile in the list
        /// </summary>
        public void ChangeTileListSprites(IEnumerable<Tile> tileList, Sprite newSprite)
        {
            foreach (var tile in tileList)
            {
                tile.ChangeGridSprite(newSprite);
            }
        }
    }
}
