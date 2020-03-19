using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float Speed;
    private Animator animator;
    public bool IsMoving = false;

    private MovementDirection direction = MovementDirection.Up; 
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("Direction", (int)MovementDirection.Up);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FollowTilePath(List<Tile> tilePath)
    {
        IsMoving = true;
        foreach (var tile in tilePath)
        {
            do
            {
                MoveTowardsTile(tile);
                yield return new WaitForEndOfFrame();
            } while (transform.position != tile.transform.position);
            //} while (Mathf.Abs(transform.position.x - tile.transform.position.x)>=Speed*Time.deltaTime && 
            //Mathf.Abs(transform.position.y - tile.transform.position.y)>=Speed*Time.deltaTime);
        }
        //corrects the position to match a tile
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        IsMoving = false;
       
    }

    /// <summary>
    /// Moves the unity towards the selectedTile. If the unit is close enough to the tile, it snaps on it
    /// </summary>
    /// <param name="selectedTile"></param>
    private void MoveTowardsTile(Tile selectedTile)
    {
        float newPositionX = transform.position.x;
        float newPositionY = transform.position.y;
        //checks if we should move on the X or Y axis
        if (transform.position.x == selectedTile.transform.position.x)
        {
            if (transform.position.y < selectedTile.transform.position.y)
            {
                direction = MovementDirection.Up;
                newPositionY = transform.position.y + Speed * Time.deltaTime;
                if (newPositionY > selectedTile.transform.position.y)
                {
                    newPositionY = selectedTile.transform.position.y;
                }
            }
            else if(transform.position.y > selectedTile.transform.position.y)
            {
                direction = MovementDirection.Down;
                newPositionY = transform.position.y - Speed * Time.deltaTime;
                if (newPositionY < selectedTile.transform.position.y)
                {
                    newPositionY = selectedTile.transform.position.y;
                }
            }
        }
        else
        {
            if (transform.position.x < selectedTile.transform.position.x)
            {
                direction = MovementDirection.Right;
                newPositionX = transform.position.x + Speed * Time.deltaTime;
                if (newPositionX > selectedTile.transform.position.x)
                {
                    newPositionX = selectedTile.transform.position.x;
                }
            }
            else if (transform.position.x > selectedTile.transform.position.x)
            {
                direction = MovementDirection.Left;
                newPositionX = transform.position.x - Speed * Time.deltaTime;
                if (newPositionX < selectedTile.transform.position.x)
                {
                    newPositionX = selectedTile.transform.position.x;
                }
            }
        }
        animator.SetInteger("Direction", (int)direction);
        transform.position = new Vector3(newPositionX, newPositionY, transform.position.z);

    }
}
