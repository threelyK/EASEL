using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToActivate;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var obj in _objectsToActivate)
            {
                obj.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var obj in _objectsToActivate)
            {
                obj.SetActive(false);
            }
        }
    }

}
