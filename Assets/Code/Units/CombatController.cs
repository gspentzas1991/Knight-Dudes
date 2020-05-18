using System.Collections.Generic;
using Code.Grid;
using UnityEngine;

namespace Code.Units
{
    public class CombatController : MonoBehaviour
    {
        public List<int> AttackRanges;
        public int CurrentHealth;
        public int MaxHealth;
        /// <summary>
        /// Tiles that are within the unit's attack range
        /// </summary>
        public IEnumerable<GridTile> AttackableTiles;
        
    }
}