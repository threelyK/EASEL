using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuddyBot : MonoBehaviour
{
    // Bot player interacts with throughout gameplay
    public static Action<BotComponentType> OnBotComponentAdd;
    
    public bool hasPerception;
    public bool hasTools;
    public bool hasMemoryLearning;
    public bool hasReasoning;

    [SerializeField] private GameObject _lArm;
    [SerializeField] private GameObject _rArm;
    [SerializeField] private GameObject _eyes;

    private int colourGuesses;
    
    private void Start()
    {
        _lArm.SetActive(false);
        _rArm.SetActive(false);
        _eyes.SetActive(false);
    }

    private void OnEnable()
    {
        OnBotComponentAdd += AddComponent;
        ProjectorColour.OnPColourChange += GuessColour;
    }

    private void OnDisable()
    {
        OnBotComponentAdd -= AddComponent;
        ProjectorColour.OnPColourChange -= GuessColour;
    }

    private void AddComponent(BotComponentType type)
    {
        switch (type)
        {
            // TODO: Add nicer animations rather than instant pop in
            case BotComponentType.Perception:
                hasPerception = true;
                AddEyes();
                break;
            case BotComponentType.Tools:
                hasTools = true;
                AddArms();
                SpinArms();
                break;
            case BotComponentType.MemoryLearning:
                hasMemoryLearning = true;
                break;
            case BotComponentType.Reasoning:
                hasReasoning = true;
                break;
            default:
                Debug.LogError("Unknown bot component type");
                break;
        }

        if (!hasPerception || !hasTools || !hasMemoryLearning || !hasReasoning) return;
        Debug.Log("Robot complete");
        PlayBootupAnimation();
    }

    private void GuessColour(ProjectorColourEnum newColour)
    {
        var colours = new [] 
        {
            ProjectorColourEnum.Red,
            ProjectorColourEnum.Blue,
            ProjectorColourEnum.Green,
            ProjectorColourEnum.Yellow
        };

        ProjectorColourEnum guess;
        
        if (colourGuesses > 5 && hasPerception)
        {
            guess = newColour; // Always guesses correctly
        }
        else
        {
            guess = colours[Random.Range(0, 4)];
        }
        
        ColourGuesser.OnBotColourGuess?.Invoke(guess);
        colourGuesses++;
    }

    private void AddEyes()
    {
        _eyes.SetActive(true);
    }
    
    private void AddArms()
    {
        _lArm.SetActive(true);
        _rArm.SetActive(true);
    }
    

    public void SpinArms()
    {
        // play animation
    }

    private void PlayBootupAnimation()
    {
        // play animation for after robot is complete
    }
    
    

}
