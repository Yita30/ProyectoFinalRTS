using UnityEngine;
using UnityEngine.UI;

public class UnitCounterUI : MonoBehaviour
{
    public Text counterText; // Arrástralo desde el inspector
    public string playerUnitTag = "PlayerUnit"; // Asegúrate de que tus unidades tengan este tag

    void Update()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag(playerUnitTag);
        counterText.text = "Unidades: " + units.Length;
    }
}
