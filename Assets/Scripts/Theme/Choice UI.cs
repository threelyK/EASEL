using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _themeText;
    [SerializeField] private Image _image;

    [SerializeField] private Sprite _foodImg;
    [SerializeField] private Sprite _gamesImg;

    [SerializeField] private GameObject _menu; // Used to disable ray interactions
    
    [SerializeField] private GameObject _page1;
    [SerializeField] private GameObject _page2;
    
    private CanvasGroup _canvasGroup;
    
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        ThemeManager.OnThemeChange += UpdateUI;
    }

    private void OnDisable()
    {
        ThemeManager.OnThemeChange -= UpdateUI;
    }

    private void UpdateUI(ThemeType theme)
    {
        string choice;
        Sprite sprite;

        switch (theme)
        {
            case ThemeType.Food:
                choice = "Food";
                sprite = _foodImg;
                break;
            case ThemeType.Games:
                choice = "Games";
                sprite = _gamesImg;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
        }

        if (_themeText) _themeText.text = $"Theme: {choice}";
        if (_image) _image.sprite = sprite;
        
        if (_page1) _page1.SetActive(false);
        if (_page2) _page2.SetActive(true);
        
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0, 3f).SetEase(Ease.Linear);
        
        if (_menu) _menu.SetActive(false);
    }
}
