using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private float MovementSpeed=0;
    private Animator animator;
    public int Movement;
    public UnitState State = UnitState.Idle;
    private MovementDirection direction = MovementDirection.Up;
    private Color DefaultSpriteColor = Color.white;
    private Color OutOfActionsSpriteColor = Color.grey;
    private SpriteRenderer _spriteRenderer = null;
    public Sprite ProfileImage = null;
    public string Name = null;
    public int CurrentHealth;
    public int MaxHealth;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("Direction", (int)direction);
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
                transform.position = Vector3.MoveTowards(transform.position, tile.transform.position, MovementSpeed * Time.deltaTime);
                if (transform.position!=tile.transform.position)
                {
                    yield return new WaitForEndOfFrame();
                }
            } while (transform.position != tile.transform.position);
        }
        State = UnitState.OutOfActions;
        SetMovementAnimation(transform.position);
        _spriteRenderer.color = OutOfActionsSpriteColor;
    }

    /// <summary>
    /// Sets the animator's values depending on the unit's direction
    /// </summary>
    private void SetMovementAnimation(Vector3 movementTarget)
    {
        if (transform.position.x > movementTarget.x)
        {
            direction = MovementDirection.Left;
            _spriteRenderer.flipX = true;
        }
        else if (transform.position.x < movementTarget.x)
        {
            direction = MovementDirection.Right;
            _spriteRenderer.flipX = false;
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
        animator.SetBool("IsMoving", State == UnitState.Moving);
    }

    /// <summary>
    /// Resets a units turn values to their defaults 
    /// </summary>
    public void ResetUnitTurnValues()
    {
        State = UnitState.Idle;
        SetMovementAnimation(transform.position);
        _spriteRenderer.color = DefaultSpriteColor;
    }
}
