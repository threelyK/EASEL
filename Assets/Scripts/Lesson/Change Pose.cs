using UnityEngine;

public class ChangePose : MonoBehaviour
{
    [SerializeField] private AnimationClip[] animationClips;
    [SerializeField] private Animator _animator;
    private int currentPose = 0;
    
    void Start()
    {
        if (animationClips.Length == 0)
        {
            Debug.LogError("No animation clips found");
            return;
        }
        
        _animator.Play(animationClips[0].name);
    }

    [ContextMenu("Next Pose")]
    public void NextPose()
    {
        currentPose++;
        if (currentPose >= animationClips.Length) currentPose = 0;
        _animator.Play(animationClips[currentPose].name);
    } 
}
