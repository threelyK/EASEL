
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    private int count = 1;

    private void Start()
    {
        UpdateObjects();
    }

    [ContextMenu("Increase Count")]
    public void Increase()
    {
        if (count == objects.Length) return;
        count++;
        UpdateObjects();
    }

    [ContextMenu("Decrease Count")]
    public void Decrease()
    {
        if (count == 1) return;
        count--;
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
