using System.Collections.Generic;
using Assets.Scripts.Common;
using FridgeScripts;
using UnityEngine;

namespace Managers
{
    public class CartManager : Singleton<CartManager>
    {
        public List<GameObject> shoppingList;
        public FridgePlacementManipulator manipulator;

        public void UpdateList()
        {
            shoppingList = manipulator.placedFridges;
        }
    }
}