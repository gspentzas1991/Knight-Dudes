using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float AnimationSpeed;
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
            SetMovementAnimation(tile.transform.position);
            do
            {
                transform.position = Vector3.MoveTowards(transform.position, tile.transform.position, AnimationSpeed * Time.deltaTime);
                if (transform.position!=tile.transform.position)
                {
                    yield return new WaitForEndOfFrame();
                }
            } while (transform.position != tile.transform.position);
        }
        IsMoving = false;
       
    }

    /// <summary>
    /// Sets the animator's values depending on the unit's direction
    /// </summary>
    private void SetMovementAnimation(Vector3 movementTarget)
    {
        if (transform.position.x > movementTarget.x)
        {
            direction = MovementDirection.Left;
        }
        else if (transform.position.x < movementTarget.x)
        {

            direction = MovementDirection.Right;
        }
        else if (transform.position.y > movementTarget.y)
        {

            direction = MovementDirection.Down;
        }
        else if (transform.position.y < movementTarget.y)
        {

            direction = MovementDirection.Up;
        }
        animator.SetInteger("Direction", (int)direction);
    }
}
