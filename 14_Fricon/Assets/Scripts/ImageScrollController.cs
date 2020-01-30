using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageScrollController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isEntered;
    public Scrollbar scrollbar;
    public TextMeshProUGUI debug;
    public ImageCountDisplayController display;

    public float startPos;
    public float endPos;

    private int m_CurrentImage;
    public int CurrentImage
    {
        get => m_CurrentImage;
        set
        {
            if (value < 0)
            {
                m_CurrentImage = 0;
            }
            else if (value >= imageCount)
            {
                m_CurrentImage = imageCount - 1;
            }
            else
            {
                m_CurrentImage = value;
            }

            display.UpdateDisplay(m_CurrentImage);
        }
    }
    public float value;
    public int imageCount;
    private float m_Threshold;

    private void Update()
    {
        if (isEntered)
            return;

        // Move right
        if (startPos > endPos)
        {
            startPos = 0;
            endPos = 0;

            CurrentImage++;
        }
        // Move left
        else if (startPos < endPos)
        {
            startPos = 0;
            endPos = 0;

            CurrentImage--;
        }

        scrollbar.value = Mathf.Lerp(scrollbar.value, CurrentImage * m_Threshold, 0.35f);
    }

    public void SetProperties(int num)
    {
        CurrentImage = 0;
        scrollbar.value = 0;
        imageCount = num;
        m_Threshold = 1.0f / (num - 1);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        startPos = Input.GetTouch(0).position.x;
        isEntered = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        endPos = Input.GetTouch(Input.touchCount - 1).position.x;
        isEntered = false;
    }
}