using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColourGuesser : MonoBehaviour
{
    public static Action<ProjectorColourEnum> OnBotColourGuess;
    
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    
    [SerializeField] private TMP_Text guessText;
    [SerializeField] private GameObject canvas;

    private const int DelayTime = 3000;

    private void Start()
    {
        canvas.SetActive(false);
    }

    private void OnEnable()
    {
        OnBotColourGuess += RunDisplayGuess;
    }

    private void OnDisable()
    {
        OnBotColourGuess -= RunDisplayGuess;
    }

    private void RunDisplayGuess(ProjectorColourEnum guess)
    {
        DisplayGuess(guess);
    }

    private async UniTaskVoid DisplayGuess(ProjectorColourEnum guess)
    {
        await UniTask.Delay(DelayTime);
        
        canvas.SetActive(true);

        switch (guess)
        {
            case ProjectorColourEnum.Red:
                guessText.text = "Guess: Red";
                image.sprite = sprites[0];
                break;
            case ProjectorColourEnum.Green:
                guessText.text = "Guess: Green";
                image.sprite = sprites[1];
                break;
            case ProjectorColourEnum.Blue:
                guessText.text = "Guess: Blue";
                image.sprite = sprites[2];
                break;
            case ProjectorColourEnum.Yellow:
                guessText.text = "Guess: Yellow";
                image.sprite = sprites[3];
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(guess), guess, null);
        }
        
        await UniTask.Delay(DelayTime);
        
        canvas.SetActive(false);
    }
}
