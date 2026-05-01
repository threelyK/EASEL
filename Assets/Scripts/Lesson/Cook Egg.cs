using UnityEngine;

public class CookEgg : MonoBehaviour
{
    [SerializeField] private Material cookedMat;
    private Renderer _renderer;
    
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    [ContextMenu("Cook Egg")]
    public void CookTheEgg()
    {
        _renderer.material = cookedMat;
    }
    
}
