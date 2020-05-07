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
        private Vector2Int GridSize = new Vector2Int();
        [SerializeField]
        private GameObject GridOutlinePrefab = null;
        [SerializeField]
        private Transform GridStartTransform = null;
        [SerializeField]
        private List<Transform> DifficultTerrainTransforms = null;
        [SerializeField]
        private List<Transform> ImpassableTerrainTransforms = null;
        public Tile[,] TileGrid { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            TileGrid = new Tile[GridSize.x, GridSize.y];
            GenerateGrid();
        }

        /// <summary>
        /// Generates Tile GameObjects, and adds them on the TileGrid list
        /// </summary>
        private void GenerateGrid()
        {
            foreach(Transform specialTileTransform in DifficultTerrainTransforms)
            {
                AddTileInGridList(specialTileTransform.position, TerrainType.Difficult);
            }

            foreach (Transform specialTileTransform in ImpassableTerrainTransforms)
            {
                AddTileInGridList(specialTileTransform.position, TerrainType.Impassable);
            }


            for (int i = 0; i < GridSize.x; i++)
            {
                for (int j = 0; j < GridSize.y; j++)
                {
                    Vector3 nextTilePosition = GridStartTransform.position + new Vector3(i,j,0);
                    if(TileGrid[i,j]==null)
                    {
                        AddTileInGridList(nextTilePosition, TerrainType.Normal);
                    }

                }
            }
        }

        /// <summary>
        /// Instantiates a grid gameobject and it adds the tile to the tileGrid
        /// </summary>
        public void AddTileInGridList(Vector3 tilePosition, TerrainType tileType)
        {
            var newTile = Instantiate(GridOutlinePrefab, tilePosition, new Quaternion()).GetComponent<Tile>();
            newTile.TerrainType = tileType;
            newTile.PositionInGrid = newTile.transform.position;
            TileGrid[(int)tilePosition.x, (int)tilePosition.y] = newTile;
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
