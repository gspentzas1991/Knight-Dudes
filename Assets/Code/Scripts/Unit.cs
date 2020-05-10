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
    /// Sets the animator's values and flips the sprite depending on the movement direction
    /// </summary>
    private void SetMovementAnimation(Vector3 movementTarget)
    {
        if (transform.position.x > movementTarget.x)
        {
            _spriteRenderer.flipX = true;
        }
        else if (transform.position.x < movementTarget.x)
        {
            _spriteRenderer.flipX = false;
        }
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
