using System;
using UnityEngine;

namespace FridgeScripts
{
    [Serializable]
    public struct Fridge
    {
        public string name;
        public GameObject fridgeObject;
        public Sprite display;
        public Sprite[] images;
        public FridgeDescriptions descriptions;
    }

    [Serializable]
    public struct FridgeDescriptions
    {
        [TextArea(0, 10)] public string description;
        [TextArea(0, 10)] public string generalInfo;
    }

    [Serializable]
    public struct FridgeColor
    {
        public string name;
        public Color squareColor;
        public Material material;
    }
}