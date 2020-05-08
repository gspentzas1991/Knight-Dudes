using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Enums;
using UnityEngine.Tilemaps;

namespace Scripts
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int GridSize = new Vector2Int();
        [SerializeField]
        private GameObject GridOutlinePrefab = null;
        [SerializeField]
        private Tilemap ImpassableTerrainTilemap = null;
        [SerializeField]
        private Tilemap DifficultTerrainTilemap = null;
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

            AddTileInGridListFromTilemap(DifficultTerrainTilemap,TerrainType.Difficult);
            AddTileInGridListFromTilemap(ImpassableTerrainTilemap, TerrainType.Impassable);


            for (int i = 0; i < GridSize.x; i++)
            {
                for (int j = 0; j < GridSize.y; j++)
                {
                    Vector3 nextTilePosition = new Vector3(i,j,0);
                    if(TileGrid[i,j]==null)
                    {
                        AddTileInGridList(nextTilePosition, TerrainType.Normal);
                    }

                }
            }
        }

        /// <summary>
        /// For every tile of the tilemap, adds a tile in the grid with the selected type
        /// </summary>
        public void AddTileInGridListFromTilemap(Tilemap tilemap, TerrainType tilesType)
        {
            foreach (var position in tilemap.cellBounds.allPositionsWithin)
            {
                Vector3 place = tilemap.GetCellCenterWorld(position);
                if (tilemap.HasTile(position))
                {
                    AddTileInGridList(place, tilesType);
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
