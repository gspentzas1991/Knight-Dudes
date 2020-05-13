using System.Collections.Generic;
using Code.Scripts;
using Code.Units;
using UnityEngine;

namespace Code.Turns
{
    public class TurnManager
    {
        private int TurnCounter;
        public void EndTurn(IEnumerable<Unit> allUnits,UnitSelector unitSelector)
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