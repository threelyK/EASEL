using System;
using UnityEngine;

public class ProjectorDemo : MonoBehaviour
{
    public static Action<ProjectorColourEnum> OnPColourChanged;
    
    private ProjectorColour _colour;
    private LoopImage _loopImage;

    private void Start()
    {
        _colour =  new ProjectorColour();
        _loopImage = GetComponent<LoopImage>();
    }

    [ContextMenu("Test Next Colour")]
    public void NextColour()
    {
        _colour.NextColour();
        
        LoopImage.OnImageNext?.Invoke(_loopImage.gameObject);
        OnPColourChanged?.Invoke(_colour.colour);
    }
}
