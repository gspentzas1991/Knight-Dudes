using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts
{
    public class Unit : MonoBehaviour
    {
        private Animator Animator;
        public UnitState state = UnitState.Idle;
        private readonly Color DefaultSpriteColor = Color.white;
        private readonly Color OutOfActionsSpriteColor = Color.grey;
        private SpriteRenderer SpriteRenderer;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        #pragma warning disable 0649
        [SerializeField] public Sprite profileImage;
        [SerializeField] public string unitName;
        [SerializeField] public int currentHealth;
        [SerializeField] public int maxHealth;
        [SerializeField] public int movement;
        [SerializeField] private float movementSpeed;
        #pragma warning restore 0649

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
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
        public IEnumerator FollowTilePath(IEnumerable<GridTile> tilePath)
        {
            state = UnitState.Moving;
            Animator.SetBool(IsMoving,true);
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
            state = UnitState.OutOfActions;
            Animator.SetBool(IsMoving,false);
            SpriteRenderer.color = OutOfActionsSpriteColor;
        }

        /// <summary>
        /// Flips the sprite depending on the direction the unit is heading
        /// </summary>
        private void SetSpriteDirection(Vector3 movementTarget)
        {
            if (transform.position.x > movementTarget.x)
            {
                SpriteRenderer.flipX = true;
            }
            else if (transform.position.x < movementTarget.x)
            {
                SpriteRenderer.flipX = false;
            }
        }

        /// <summary>
        /// Resets a units turn values to their defaults 
        /// </summary>
        public void ResetUnitTurnValues()
        {
            state = UnitState.Idle;
            SpriteRenderer.color = DefaultSpriteColor;
        }
    }
}
