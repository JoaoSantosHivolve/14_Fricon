using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Grids
{
    public class PopulateObjectImagesGrid : MonoBehaviour
    {
        public GameObject prefab;
        public RectTransform scrollView;
        public GridLayoutGroup grid;

        public int imageCount;
        public ImageScrollController controller;
        public ImageCountDisplayController ballController;

        private List<GameObject> m_GridElements = new List<GameObject>();

        public void PopulateGrid(Sprite[] images)
        {
            // Clear grid before inserting new images.
            ClearGrid();

            // Set num images
            imageCount = images.Length;
            ballController.CreateBallDisplay(imageCount);
            controller.SetProperties(imageCount);
            ballController.UpdateDisplay(0);

            // Set grid properties
            var width = scrollView.rect.height;
            grid.cellSize = new Vector2(width, width);
            grid.spacing = new Vector2(width / 2, 0);
            var padding = (int)width / 2;
            grid.padding = new RectOffset(padding, padding, 0,0);

            foreach (var t in images)
            {
                var newObj = Instantiate(prefab, transform);
                newObj.GetComponent<Image>().sprite = t;

                m_GridElements.Add(newObj);
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
}