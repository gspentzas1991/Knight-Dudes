using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Enums;

namespace Scripts
{
    public class GridManager : MonoBehaviour
    {
        public Vector2 GridSize;
        [SerializeField]
        private GameObject GridOutlinePrefab;
        [SerializeField]
        private Transform GridStartTransform;
        [SerializeField]
        private List<Tile> SpecialTilesList;

        private Dictionary<(float x, float y ), Tile> TileGrid = new Dictionary<(float x, float y), Tile>();
        private Tile SelectedTile;

        //placeholder for selecting units, to be deleted
        public Unit KnightGameObject;

        public Dictionary<(float x, float y), Tile> GetTileGrid()
        {
            return TileGrid;
        }

        // Start is called before the first frame update
        void Start()
        {
            GenerateGrid();
            //placeholder for selecting a unit with cursor
            //SelectedTile = TileGrid.Where(x => x.transform.position.x == -1 && x.transform.position.y == -6).FirstOrDefault();
            //SelectedTile.CurrentUnit = KnightGameObject;
            //ShowUnitMovementOnGrid(SelectedTile);
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Generates Tile GameObjects, and adds them on the TileGrid list
        /// </summary>
        private void GenerateGrid()
        {
            foreach(Tile specialTile in SpecialTilesList)
            {
                Vector3 tilePosition = specialTile.transform.position;
                TileGrid.Add((tilePosition.x, tilePosition.y), specialTile);
            }

            for (float i = GridStartTransform.position.x; i <= GridSize.x; i++)
            {
                for (float j = GridStartTransform.position.y; j <= GridSize.y; j++)
                {
                    if(!TileGrid.TryGetValue((i, j),out Tile existingTile))
                    {
                        var newTile = Instantiate(GridOutlinePrefab, new Vector3(i, j, 0), new Quaternion()).GetComponent<Tile>();
                        newTile.TerrainType = TerrainType.Normal;
                        TileGrid.Add((i, j), newTile);
                    }

                }
            }
        }
    }
}
