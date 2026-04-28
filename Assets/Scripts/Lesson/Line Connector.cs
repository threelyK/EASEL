using UnityEngine;

public class LineConnector : MonoBehaviour
{
    [SerializeField] private Transform objectA;
    [SerializeField] private Transform objectB;
    [SerializeField] private float width;
    
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // Update line positions every frame
        lineRenderer.SetPosition(0, objectA.position);
        lineRenderer.SetPosition(1, objectB.position);
    }
}
