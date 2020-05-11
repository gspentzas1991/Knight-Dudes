using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts
{
    public class UnitProfileGui : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] private Text unitName;
        [SerializeField] private Image unitImage;
        [SerializeField] private Text unitHealth;
        #pragma warning restore 0649

        public void SetUnitToDisplay(Unit unitToDisplay)
        {
            unitName.text = unitToDisplay.unitName;
            unitImage.sprite = unitToDisplay.profileImage;
            unitHealth.text = $"HP: {unitToDisplay.currentHealth}/{unitToDisplay.maxHealth}";
        }
    }
}
