using System.Collections.Generic;
using Code.Units;
using UnityEngine;

namespace Code.Turns
{
    /// <summary>
    /// Handles logic regarding the change of turns
    /// </summary>
    public class TurnManager : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField]
        private UnitSelector _unitSelector;
        #pragma warning restore 0649
        
        private int _turnCounter;
        public void EndTurn(IEnumerable<Unit> allUnits)
        {
            _unitSelector.DeselectUnit();
            foreach(var unit in allUnits)
            {
                unit.ResetUnitTurnValues();
            }
            Debug.Log("finished turn "+_turnCounter);
            _turnCounter++;
        }
    }
}