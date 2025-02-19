using TMPro;
using UnityEngine;

public class RoundCounter : MonoBehaviour
{
    public TMP_Text _roundsSurvived;

    void OnEnable()
    {
        _roundsSurvived.text = "Waves survived: " + PlayerStats._wavesSurvived.ToString();
    }
}
