using UnityEngine;

public class ProjectorDemo : MonoBehaviour
{
    private ProjectorColour _colour;
    private LoopImage _loopImage;

    private void Start()
    {
        _colour =  new ProjectorColour();
        _loopImage = GetComponent<LoopImage>();
    }

    public void NextColour()
    {
        _colour.NextColour();
        
        LoopImage.OnImageNext?.Invoke(_loopImage.gameObject);
    }
}
