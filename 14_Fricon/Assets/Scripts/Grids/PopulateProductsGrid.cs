using System.Collections.Generic;
using FridgeScripts;
using GoogleARCore.Examples.ObjectManipulation;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Grids
{
    public class PopulateProductsGrid : MonoBehaviour
    {
        public GameObject prefab;
        public RectTransform scrollView;
        public GridLayoutGroup grid;

        public FridgePlacementManipulator manipulator;
        private CartManager m_Manager;
        private List<GameObject> m_GridElements = new List<GameObject>();

        private void Awake()
        {
            m_Manager = CartManager.Instance;
        }

        public void PopulateCartProducts()
        {
            // Clear Grid
            ClearGrid();

            // Get display image size
            var width = scrollView.rect.width;
            grid.cellSize = new Vector2(width, width / 4);

            // Instantiate grid images
            var index = 1;
            foreach (var fridge in FridgeManager.Instance.fridges)
            {
                // Search in every color
                var materialsManager = FridgeMaterialsManager.Instance.fridgeColors;
                for (var i = 0; i < materialsManager.Count; i++)
                {
                    var count = 0;
                 
                    // Verify if the same fridge && color 
                    foreach (var placedFridge in m_Manager.shoppingList)
                        if (fridge.fridgeObject == placedFridge.GetComponent<Manipulator>().fridge.fridgeObject
                            && materialsManager[i].name == materialsManager[placedFridge.GetComponent<Manipulator>().materialController.selectedColor].name)
                            count++;

                    if(count > 0)
                    {
                        var newObj = Instantiate(prefab, transform);
                        newObj.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(width, width / 8);
                        newObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = index + ": " + fridge.name + "(" + materialsManager[i].name + ")";
                        newObj.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(width, width / 8);
                        newObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Quantity: " + count;

                        index++;
                        m_GridElements.Add(newObj);
                    }
                }
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