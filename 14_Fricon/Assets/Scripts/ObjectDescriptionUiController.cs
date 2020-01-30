using FridgeScripts;
using Grids;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDescriptionUiController : MonoBehaviour
{
    [Header("Buttons")]
    public Button description;
    public Button generalInfo;

    [Space(10)] 
    [Header("Images Grid")]
    public PopulateObjectImagesGrid grid;
    public RectTransform gridTransform;

    [Space(10)] 
    [Header("Descriptions text")]
    public TextMeshProUGUI fridgeName;
    public TextMeshProUGUI descText;
    public RectTransform textTransform;

    [Space(10)]
    [Header("Select Fridge")]
    public FridgePlacementManipulator manipulator;
    public Button acceptFridge;

    private Fridge m_Fridge;
    public Fridge Fridge
    {
        set
        {
            // Prevent weird bug
            if (!value.Equals(m_Fridge))
            {
                m_Fridge = value;
                grid.PopulateGrid(m_Fridge.images);
                //gridTransform.rect.position.Set(0, 0);
                fridgeName.text = m_Fridge.name;
                descText.text = m_Fridge.descriptions.description;
            }
        }
    }
    private readonly Color m_Transparent = new Color(1, 1, 1, 0.5f);

    private void Start()
    {
        description.onClick.AddListener(Description_OnClick);
        generalInfo.onClick.AddListener(GeneralInfo_OnClick);
        acceptFridge.onClick.AddListener(SelectFridge);

        Description_OnClick();
    }

    private void SetTextDescription(string desc)
    {
        descText.text = desc;
        textTransform.rect.position.Set(0,0);
    }
    private void Description_OnClick()
    {
        SetTextDescription(m_Fridge.descriptions.description);
        description.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
        generalInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        description.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        generalInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = m_Transparent;
    }
    private void GeneralInfo_OnClick()
    {
        SetTextDescription(m_Fridge.descriptions.generalInfo);
        description.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        generalInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
        description.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = m_Transparent;
        generalInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
    }
    private void SelectFridge()
    {
        manipulator.fridgeInfo = m_Fridge;
    }
}