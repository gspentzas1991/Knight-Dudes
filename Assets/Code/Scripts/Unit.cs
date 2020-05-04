using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float AnimationSpeed=0;
    private Animator animator;
    public int MovementSpeed;
    public UnitState State = UnitState.Idle;

    private MovementDirection direction = MovementDirection.Up; 
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("Direction", (int)MovementDirection.Up);
    }

    /// <summary>
    /// Moves the unit along every tile on the tilePath list
    /// </summary>
    public IEnumerator FollowTilePath(IEnumerable<Tile> tilePath)
    {
        State = UnitState.Moving;
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
        State = UnitState.Idle;
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
