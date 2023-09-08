using UnityEngine;
using Cinemachine;

public class OrthoFit : MonoBehaviour
{
    [SerializeField] PolygonCollider2D target;
    [SerializeField] CinemachineVirtualCamera targetCam;

    [SerializeField] bool invalidate = false;
    [SerializeField] float padding = 0f;
    void Start()
    {
        Calculate();
    }

    void Update()
    {
        if (invalidate.Trigger())
            Calculate();
    }

    private void Calculate()
    {
        var size = target.bounds.size;
        float screenRatio = Screen.width / (float)Screen.height;
        float targetRatio = size.x / size.y;
        if (screenRatio >= targetRatio)
            targetCam.m_Lens.OrthographicSize = size.y / 2 + padding;
        else
        {
            float scaler = targetRatio / screenRatio;
            targetCam.m_Lens.OrthographicSize = size.y / 2 * scaler + padding;
        }
    }
}
