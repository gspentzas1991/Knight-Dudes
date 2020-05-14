using System.Collections;
using System.Collections.Generic;
using Code.Grid;
using Code.Models;
using UnityEngine;

namespace Code.Units
{
    public class Unit : MonoBehaviour
    {
        public UnitState state = UnitState.Idle;
        private readonly Color DefaultSpriteColor = Color.white;
        private readonly Color OutOfActionsSpriteColor = Color.grey;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        /// <summary>
        /// Pathfinding data for all available moves the unit can make from its current position
        /// </summary>
        public List<TilePathfindingData> pathfindingData;
        #pragma warning disable 0649
        [SerializeField] public Sprite profileImage;
        [SerializeField] public string unitName;
        [SerializeField] public int currentHealth;
        [SerializeField] public int maxHealth;
        [SerializeField] public int movement;
        [SerializeField] private float movementSpeed;
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        #pragma warning restore 0649

        private void Awake()
        {
        }

        /// <summary>
        /// Begins the unit's followTilePath Coroutine
        /// </summary>
        /// <param name="tilePath"></param>
        public void BeginFollowingTilePath(IEnumerable<GridTile> tilePath)
        {
            StartCoroutine((FollowTilePath(tilePath)));
        }
        
        /// <summary>
        /// Moves the unit along every tile on the tilePath list
        /// </summary>
        private IEnumerator FollowTilePath(IEnumerable<GridTile> tilePath)
        {
            animator.SetBool(IsMoving,true);
            foreach (var tile in tilePath)
            {
                SetSpriteDirection(tile.transform.position);
                do
                {
                    transform.position = Vector3.MoveTowards(transform.position, tile.transform.position, movementSpeed * Time.deltaTime);
                    if (transform.position!=tile.transform.position)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                } while (transform.position != tile.transform.position);
            }
            animator.SetBool(IsMoving,false);
            spriteRenderer.color = OutOfActionsSpriteColor;
        }

        /// <summary>
        /// Flips the sprite depending on the direction the unit is heading
        /// </summary>
        private void SetSpriteDirection(Vector3 movementTarget)
        {
            if (transform.position.x > movementTarget.x)
            {
                spriteRenderer.flipX = true;
            }
            else if (transform.position.x < movementTarget.x)
            {
                spriteRenderer.flipX = false;
            }
        }

        /// <summary>
        /// Resets a units turn values to their defaults 
        /// </summary>
        public void ResetUnitTurnValues()
        {
            state = UnitState.Idle;
            spriteRenderer.color = DefaultSpriteColor;
            pathfindingData = null;
        }
    }
}
