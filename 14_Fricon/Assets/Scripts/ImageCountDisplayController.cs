using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCountDisplayController : MonoBehaviour
{
    public GameObject prefab;
    public RectTransform scrollView;
    public GridLayoutGroup grid;

    public Sprite selected;
    public Sprite notSelected;

    private List<GameObject> m_GridElements = new List<GameObject>();

    public void CreateBallDisplay(int count)
    {
        // Clear grid before inserting new images.
        ClearGrid();

        // Set grid properties
        var width = scrollView.rect.height / 10f;
        grid.cellSize = new Vector2(width, width);
        grid.spacing = new Vector2(width, 0);

        for (var i = 0; i < count; i++)
        {
            var newObj = Instantiate(prefab, transform);
            newObj.GetComponent<Image>().sprite = notSelected;

            m_GridElements.Add(newObj);
        }
    }
    public void UpdateDisplay(int index)
    {
        for (var i = 0; i < m_GridElements.Count; i++)
        {
            m_GridElements[i].GetComponent<Image>().sprite = index == i ? selected : notSelected;
        }
    }

    private void ClearGrid()
    {
        if (m_GridElements == null || m_GridElements.Count == 0)
            return;

        foreach (var element in m_GridElements)
            Destroy(element);

        m_GridElements = new List<GameObject>();
    }
}