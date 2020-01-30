using System.Collections.Generic;
using Assets.Scripts.Common;
using FridgeScripts;
using UnityEngine;

public class FridgeMaterialsManager : Singleton<FridgeMaterialsManager>
{
    public List<FridgeColor> fridgeColors;

    public Material GetMaterial(int index)
    {
        return new Material(fridgeColors[index].material);
    }
}
