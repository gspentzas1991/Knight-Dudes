using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitProfileUI : MonoBehaviour
{
    private Unit UnitToDisplay = null;
    [SerializeField]
    private Text UnitName = null;
    [SerializeField]
    private Image UnitImage = null;
    [SerializeField]
    private Text UnitHealth = null;

    public void SetUnitToDisplay(Unit unitToDisplay)
    {
        UnitToDisplay = unitToDisplay;
        UnitName.text = unitToDisplay.Name;
        UnitImage.sprite = unitToDisplay.ProfileImage;
        UnitHealth.text = string.Format("HP: {0}/{1}",unitToDisplay.CurrentHealth,unitToDisplay.MaxHealth);

    }
}
