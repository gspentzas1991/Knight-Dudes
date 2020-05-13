using Code.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI
{
    public class UnitProfileGui : MonoBehaviour
    {
        private Unit DisplayedUnit;
        #pragma warning disable 0649
        [SerializeField] private Text unitName;
        [SerializeField] private Image unitImage;
        [SerializeField] private Text unitHealth;
        #pragma warning restore 0649

        private void SetUnitToDisplay(Unit unitToDisplay)
        {
            DisplayedUnit = unitToDisplay;
            unitName.text = unitToDisplay.unitName;
            unitImage.sprite = unitToDisplay.profileImage;
            unitHealth.text = $"HP: {unitToDisplay.currentHealth}/{unitToDisplay.maxHealth}";
        }

        /// <summary>
        /// Displays the hovered unit's profile in the UI, if the hovered unit changes
        /// </summary>
        public void ShowUnitProfile(Unit hoveredUnit)
        {
            if (hoveredUnit==DisplayedUnit) return;
            if (!ReferenceEquals(hoveredUnit,null))
            {
                SetUnitToDisplay(hoveredUnit);
                gameObject.SetActive(true);
            }
            else
            {
                DisplayedUnit = null;
                gameObject.SetActive(false);
            }
        }
    }
}
