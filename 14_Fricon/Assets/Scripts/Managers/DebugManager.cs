using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : Singleton<DebugManager>
{
    [SerializeField] private Text selectedObjectText;
    public GameObject SelectedObject
    {
        set
        {
            if (value == null)
            {
                selectedObjectText.text = "Selected Object: None";
            }
            else
            {
                selectedObjectText.text = "Selected Object: " + value.name;
            }
        }
    }
}
