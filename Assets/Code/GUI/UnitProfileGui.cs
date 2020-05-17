using Code.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI
{
    public class UnitProfileGui : MonoBehaviour
    {
        private Unit _displayedUnit;
        #pragma warning disable 0649
        [SerializeField] private Text _unitName;
        [SerializeField] private Image _unitImage;
        [SerializeField] private Text _unitHealth;
        #pragma warning restore 0649

        private void SetUnitToDisplay(Unit unitToDisplay)
        {
            _displayedUnit = unitToDisplay;
            _unitName.text = unitToDisplay.UnitName;
            _unitImage.sprite = unitToDisplay.ProfileImage;
            _unitHealth.text = $"HP: {unitToDisplay.CombatController.CurrentHealth}/{unitToDisplay.CombatController.MaxHealth}";
        }

        /// <summary>
        /// Displays the hovered unit's profile in the UI, if the hovered unit changes
        /// </summary>
        public void ShowUnitProfile(Unit hoveredUnit)
        {
            if (hoveredUnit==_displayedUnit) return;
            if (!ReferenceEquals(hoveredUnit,null))
            {
                SetUnitToDisplay(hoveredUnit);
                gameObject.SetActive(true);
            }
            else
            {
                _displayedUnit = null;
                gameObject.SetActive(false);
            }
        }
    }
}
