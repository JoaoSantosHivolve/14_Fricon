using FridgeScripts;
using Grids;
using UnityEngine;
using UnityEngine.UI;

public class DisplayClick : MonoBehaviour
{
    public Button button;
    public PopulateDisplayGrid grid;
    public Fridge fridge;
    public ObjectDescriptionUiController controller;
    public Animator odAnimator;
    public Animator odescAnimator;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        grid.SetInteractable(false);
        odAnimator.Play("OD_Wait");
        odescAnimator.Play("ODESC_Enter");
        controller.Fridge = fridge;
    }
}