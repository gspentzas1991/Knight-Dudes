using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Grid
{
    public class GridManager : MonoBehaviour
    {
        public GridTile[,] TileGrid { get; private set; }
        #pragma warning disable 0649
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private Tilemap _impassableTerrainTilemap;
        [SerializeField] private Tilemap _difficultTerrainTilemap;
        #pragma warning restore 0649

        // Start is called before the first frame update
        private void Awake()
        {
            TileGrid = new GridTile[_gridSize.x, _gridSize.y];
            GenerateGrid();
        }

        /// <summary>
        /// Generates Tile GameObjects, and adds them on the TileGrid list
        /// </summary>
        private void GenerateGrid()
        {
            AddTilesInGrid(_difficultTerrainTilemap,TerrainType.Difficult);
            AddTilesInGrid(_impassableTerrainTilemap, TerrainType.Impassable);
            for (var i = 0; i < _gridSize.x; i++)
            {
                for (var j = 0; j < _gridSize.y; j++)
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
            var newTile = Instantiate(_tilePrefab, tilePosition, new Quaternion()).GetComponent<GridTile>();
            newTile.TerrainType = tileType;
            newTile.PositionInGrid = newTile.transform.position;
            TileGrid[(int)tilePosition.x, (int)tilePosition.y] = newTile;
        }

    }
}
