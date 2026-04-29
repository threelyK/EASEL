
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    private int count = 1;

    private void Start()
    {
        UpdateObjects();
    }

    public void Increase()
    {
        count = Mathf.Min(count + 1, objects.Length);
        UpdateObjects();
    }

    public void Decrease()
    {
        count = Mathf.Max(count - 1, 1);
        UpdateObjects();
    }

    private void UpdateObjects()
    {
        for (var i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i < count);
        }
    }
}
