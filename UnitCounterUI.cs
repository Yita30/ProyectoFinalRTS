using UnityEngine;
using UnityEngine.UI;

public class UnitCounterUI : MonoBehaviour
{
    public Text counterText; // Arr�stralo desde el inspector
    public string playerUnitTag = "PlayerUnit"; // Aseg�rate de que tus unidades tengan este tag

    void Update()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(playerUnitTag);
        counterText.text = "Unidades: " + units.Length;
    }
}
