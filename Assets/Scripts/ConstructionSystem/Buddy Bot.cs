using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class BuddyBot : MonoBehaviour
{
    // Bot interacts with player throughout gameplay
    public static Action<BotComponentType> OnBotComponentAdd;
    
    public bool hasPerception;
    public bool hasTools;
    public bool hasMemoryLearning;
    public bool hasReasoning;

    [SerializeField] private GameObject _lArm;
    [SerializeField] private GameObject _rArm;

    // [SerializeField] private PlayableDirector _playableDirector;
    // [SerializeField] private Animation _addEyes;

    [SerializeField] private Animator _buddyBotAnimator;
    private int colourGuesses;
    
    private void Start()
    {
        _lArm.SetActive(hasTools);
        _rArm.SetActive(hasTools);
    }

    private void OnEnable()
    {
        OnBotComponentAdd += AddComponent;
        ProjectorDemo.OnPColourChanged += GuessColour;
    }

    private void OnDisable()
    {
        OnBotComponentAdd -= AddComponent;
        ProjectorDemo.OnPColourChanged -= GuessColour;
    }

    private void AddComponent(BotComponentType type)
    {
        switch (type)
        {
            case BotComponentType.Perception:
                hasPerception = true;
                AddEyes();
                break;
            case BotComponentType.Tools:
                hasTools = true;
                AddArms();
                break;
            case BotComponentType.MemoryLearning:
                hasMemoryLearning = true;
                AddMemory();
                break;
            case BotComponentType.Reasoning:
                hasReasoning = true;
                AddBrain();
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
        
        if (colourGuesses >= 5 && hasPerception)
        {
            guess = newColour; // Always guesses correctly
        }
        else
        {
            guess = colours[Random.Range(0, 4)];
        }
        
        GuessDisplayer.OnBotColourGuess?.Invoke(guess);
        colourGuesses++;
    }

    private void AddEyes()
    {
        _buddyBotAnimator.Play("Eyes On");
    }
    
    private void AddArms()
    {
        _buddyBotAnimator.Play("Arms Attach");
    }

    private void AddMemory()
    {
        _buddyBotAnimator.Play("Head Open B"); // Must use head animator not buddybot or botv3
    }

    private void AddBrain()
    {
        _buddyBotAnimator.Play("Head Open B");
    }

    private void PlayBootupAnimation()
    {
        // play animation for after robot is complete
    }
    
    

}
