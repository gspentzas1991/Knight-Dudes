using System.Collections;
using System.Collections.Generic;
using Code.Grid;
using Code.Models;
using UnityEngine;

namespace Code.Units
{
    /// <summary>
    /// Represents a Unit on the grid
    /// </summary>
    public class Unit : MonoBehaviour
    {
        public UnitState State = UnitState.Idle;
        public Sprite ProfileImage;
        public string UnitName;
        public int Movement;
        public CombatController CombatController;
        /// <summary>
        /// Pathfinding data for all available moves the unit can make from its current position
        /// </summary>
        public List<TilePathfindingData> _pathfindingData;
        private readonly Color _defaultSpriteColor = Color.white;
        private readonly Color _outOfActionsSpriteColor = Color.grey;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        #pragma warning disable 0649
        [SerializeField] private float _movementSpeed;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        #pragma warning restore 0649


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
            _animator.SetBool(IsMoving,true);
            foreach (var tile in tilePath)
            {
                SetSpriteDirection(tile.transform.position);
                do
                {
                    transform.position = Vector3.MoveTowards(transform.position, tile.transform.position, _movementSpeed * Time.deltaTime);
                    if (transform.position!=tile.transform.position)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                } while (transform.position != tile.transform.position);
            }
            _animator.SetBool(IsMoving,false);
            _spriteRenderer.color = _outOfActionsSpriteColor;
        }

        /// <summary>
        /// Flips the sprite depending on the direction the unit is heading
        /// </summary>
        private void SetSpriteDirection(Vector3 movementTarget)
        {
            if (transform.position.x > movementTarget.x)
            {
                _spriteRenderer.flipX = true;
            }
            else if (transform.position.x < movementTarget.x)
            {
                _spriteRenderer.flipX = false;
            }
        }

        /// <summary>
        /// Resets a units turn values to their defaults 
        /// </summary>
        public void ResetUnitTurnValues()
        {
            State = UnitState.Idle;
            _spriteRenderer.color = _defaultSpriteColor;
            _pathfindingData = null;
        }
    }
}
