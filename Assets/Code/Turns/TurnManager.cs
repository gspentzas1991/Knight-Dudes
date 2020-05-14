using System.Collections.Generic;
using Code.Units;
using UnityEngine;

namespace Code.Turns
{
    public class TurnManager : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField]
        private UnitSelector unitSelector;
        #pragma warning restore 0649
        
        private int TurnCounter;
        public void EndTurn(IEnumerable<Unit> allUnits)
        {
            unitSelector.DeselectUnit();
            foreach(var unit in allUnits)
            {
                unit.ResetUnitTurnValues();
            }
            Debug.Log("finished turn "+TurnCounter);
            TurnCounter++;
        }
    }
}