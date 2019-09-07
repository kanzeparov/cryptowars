using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    
    public void SetPosition(Vector3 start, Vector3 end, float width)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    public void SetPosition(Vector3 start, Vector3 end)
    {
        var width = .5f;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
}
