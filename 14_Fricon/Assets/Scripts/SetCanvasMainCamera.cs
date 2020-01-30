using UnityEngine;

public class SetCanvasMainCamera : MonoBehaviour
{
    private Canvas m_Canvas;

    private void Awake()
    {
        m_Canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        m_Canvas.worldCamera = Camera.main;
    }
}
