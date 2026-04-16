using System;
using UnityEngine;
using UnityEngine.UI;

public class LoopImage : MonoBehaviour
{
    public static Action<GameObject> OnImageNext;

    public Image image;
    public Sprite[] sprites;
    
    private int currentIndex;
    
    private void Start()
    {
        if (sprites is null)
        {
            Debug.LogError($"{gameObject} has no sprites");
        }
        else
        {
            image.sprite = sprites[currentIndex];
        }
    }

    private void OnEnable()
    {
        OnImageNext += DisplayNextImage;
    }

    private void OnDisable()
    {
        OnImageNext -= DisplayNextImage;
    }

    public void DisplayNextImage(GameObject loopImage)
    {
        // public so a Unity Button can call it
        if (gameObject != loopImage) return;

        if (currentIndex != sprites.Length)
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }
        
        image.sprite = sprites[currentIndex];
    }
}
