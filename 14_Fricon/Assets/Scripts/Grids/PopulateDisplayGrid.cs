using System.Collections.Generic;
using FridgeScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Grids
{
    public class PopulateDisplayGrid : MonoBehaviour
    {
        public GameObject prefab;
        public RectTransform scrollView;
        public GridLayoutGroup grid;
        public FridgeManager fridgeManager;
        public ObjectDescriptionUiController controller;

        public Animator odAnimator;
        public Animator odescAnimator;

        private List<DisplayClick> m_GridElements = new List<DisplayClick>();

        public void Start()
        {
            Populate();
        }

        private void Populate()
        {
            // Get display image size
            var width = scrollView.rect.width / 2f;
            grid.cellSize = new Vector2(width, width);
            grid.spacing = new Vector2(0, width / 6);
            grid.padding = new RectOffset(0,0,0,(int)width / 2);
            // Instantiate grid images
            foreach (var fridge in fridgeManager.fridges)
            {
                var newObj = Instantiate(prefab, transform);
                newObj.GetComponent<DisplayClick>().grid = this;
                newObj.GetComponent<DisplayClick>().fridge = fridge;
                newObj.GetComponent<DisplayClick>().controller = controller;
                newObj.GetComponent<DisplayClick>().odAnimator = odAnimator;
                newObj.GetComponent<DisplayClick>().odescAnimator = odescAnimator;
                newObj.GetComponent<Image>().sprite = fridge.display;
                newObj.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(width * 0.75f, width / 6);
                newObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fridge.name;

                m_GridElements.Add(newObj.GetComponent<DisplayClick>());
            }
        }

        public void SetInteractable(bool state)
        {
            foreach (var element in m_GridElements)
            {
                element.button.interactable = state;
            }
        }
    }
}
