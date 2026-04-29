using TMPro;
using UnityEngine;

public class PointsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _tmpText;
    
    private void Start()
    {
        PointSystem.Instance.onPointChange += UpdateValueShown;
    }

    private void OnDestroy()
    {
        PointSystem.Instance.onPointChange -= UpdateValueShown;
    }

    private void UpdateValueShown()
    {
        _tmpText.text = $"You have {PointSystem.Instance._points} Points";
    }
}
