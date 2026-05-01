using UnityEngine;

public class Snapper : MonoBehaviour
{
    [SerializeField] private Transform _snapPoint;
    [SerializeField] private bool isLessonZone;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LessonOrb"))
        {
            var orb = other.GetComponent<LessonOrb>();
            orb.snapTarget = _snapPoint;

            if (!isLessonZone) return;
            orb.inLessonZone = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LessonOrb"))
        {
            other.GetComponent<LessonOrb>().inLessonZone = false;
            LessonOrb.OnLessonHide?.Invoke(other.gameObject.GetInstanceID());
        }
    }
}
