using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Scripts
{
    public class GridManager : MonoBehaviour
    {
        public Tile[,] TileGrid { get; private set; }
        #pragma warning disable 0649
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Tilemap impassableTerrainTilemap;
        [SerializeField] private Tilemap difficultTerrainTilemap;
        #pragma warning restore 0649

        // Start is called before the first frame update
        private void Start()
        {
            TileGrid = new Tile[gridSize.x, gridSize.y];
            GenerateGrid();
        }

        /// <summary>
        /// Generates Tile GameObjects, and adds them on the TileGrid list
        /// </summary>
        private void GenerateGrid()
        {
            AddTilesInGrid(difficultTerrainTilemap,TerrainType.Difficult);
            AddTilesInGrid(impassableTerrainTilemap, TerrainType.Impassable);
            for (var i = 0; i < gridSize.x; i++)
            {
                for (var j = 0; j < gridSize.y; j++)
                {
                    var nextTilePosition = new Vector3(i,j,0);
                    if(TileGrid[i,j]==null)
                    {
                        AddTileInGrid(nextTilePosition, TerrainType.Normal);
                    }
                }
            }
        }

        /// <summary>
        /// For every tile of the tilemap, adds a tile in the grid with the selected type
        /// </summary>
        private void AddTilesInGrid(Tilemap tilemap, TerrainType tilesType)
        {
            foreach (var position in tilemap.cellBounds.allPositionsWithin)
            {
                var place = tilemap.GetCellCenterWorld(position);
                if (tilemap.HasTile(position))
                {
                    AddTileInGrid(place, tilesType);
                }
            }
        }



        /// <summary>
        /// Instantiates a grid GameObject and adds it to the tileGrid
        /// </summary>
        private void AddTileInGrid(Vector3 tilePosition, TerrainType tileType)
        {
            var newTile = Instantiate(tilePrefab, tilePosition, new Quaternion()).GetComponent<Tile>();
            newTile.terrainType = tileType;
            newTile.positionInGrid = newTile.transform.position;
            TileGrid[(int)tilePosition.x, (int)tilePosition.y] = newTile;
        }

        /// <summary>
        /// Changes the sprite of every tile in a list
        /// </summary>
        public static void ChangeTilesSprites(IEnumerable<Tile> tileList, Sprite newSprite)
        {
            foreach (var tile in tileList)
            {
                tile.ChangeGridSprite(newSprite);
            }
        }
    }
}
