using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.Scripts
{
    public class UnitProfileGui : MonoBehaviour
    {
        private Unit UnitToDisplay = null;
        [SerializeField]
        private Text unitName = null;
        [SerializeField]
        private Image unitImage = null;
        [SerializeField]
        private Text unitHealth = null;

        public void SetUnitToDisplay(Unit unitToDisplay)
        {
            UnitToDisplay = unitToDisplay;
            unitName.text = unitToDisplay.unitName;
            unitImage.sprite = unitToDisplay.profileImage;
            unitHealth.text = $"HP: {unitToDisplay.currentHealth}/{unitToDisplay.maxHealth}";

        }
    }
}
