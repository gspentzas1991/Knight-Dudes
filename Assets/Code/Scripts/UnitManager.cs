using Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public Unit SelectedUnit;
    public GridManager GridManager;

    void Start()
    {
        var tileGrid = GridManager.GetTileGrid();
        //Tile selectedTile = tileGrid.Where(x=>x.transform.position == new Vector3(7,4,0)).FirstOrDefault();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && SelectedUnit.IsMoving == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag=="Tile")
                {
                    Debug.Log("This is a Player");
                    Tile selectedTile = hit.transform.gameObject.GetComponent<Tile>(); 
                    List<Tile> tilePath = CreateTilePathFromSelectedTile(selectedTile);
                    StartCoroutine(SelectedUnit.FollowTilePath(tilePath));
                }
                else
                {
                    Debug.Log("This isn't a Player");
                }
            }
        }

    }

    /// <summary>
    /// Calculates the tilepath to reach the selected tile
    /// </summary>
    public List<Tile> CreateTilePathFromSelectedTile(Tile selectedTile)
    {
        var tileGrid = GridManager.GetTileGrid();

        tileGrid.TryGetValue(((SelectedUnit.transform.position.x, SelectedUnit.transform.position.y)),out Tile nextTile);
        List<Tile> tilePath = new List<Tile>();
        do
        {
            int randomNumber = Random.Range(0, 2);
            if (randomNumber == 0 && selectedTile.transform.position.x != nextTile.transform.position.x)
            {
                if (selectedTile.transform.position.x > nextTile.transform.position.x)
                {
                    
                    nextTile = tileGrid[ (nextTile.transform.position.x + 1, nextTile.transform.position.y)];
                }
                else if (selectedTile.transform.position.x < nextTile.transform.position.x)
                {
                    nextTile = tileGrid[(nextTile.transform.position.x - 1, nextTile.transform.position.y)];

                }
            }
            else
            {
                if (selectedTile.transform.position.y > nextTile.transform.position.y)
                {
                    nextTile = tileGrid[(nextTile.transform.position.x, nextTile.transform.position.y+1)];
                }
                else if (selectedTile.transform.position.y < nextTile.transform.position.y)
                {
                    nextTile = tileGrid[(nextTile.transform.position.x, nextTile.transform.position.y-1)];

                }
            }
            tilePath.Add(nextTile);


        } while (selectedTile.transform.position != tilePath[tilePath.Count-1].transform.position);
        return tilePath;
    }
}
