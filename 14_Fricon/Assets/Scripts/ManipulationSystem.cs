//-----------------------------------------------------------------------
// <copyright file="ManipulationSystem.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

using FridgeScripts;
using GoogleARCore.Examples.ObjectManipulation;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manipulation system allows the user to manipulate virtual objects (select, translate,
/// rotate, scale and elevate) through gestures (tap, drag, twist, swipe).
/// Manipulation system also handles the current selected object and its visualization.
///
/// To enable it add one ManipulationSystem to your scene and one Manipulator as parent of each
/// of your virtual objects.
/// </summary>
public class ManipulationSystem : MonoBehaviour
{
    /// <summary>
    /// Gets the ManipulationSystem instance.
    /// </summary>
    private static ManipulationSystem s_Instance;
    public static ManipulationSystem Instance
    {
        get
        {
            if (s_Instance == null)
            {
                var manipulationSystems = FindObjectsOfType<ManipulationSystem>();
                if (manipulationSystems.Length > 0)
                {
                    s_Instance = manipulationSystems[0];
                }
                else
                {
                    Debug.LogError("No instance of ManipulationSystem exists in the scene.");
                }
            }

            return s_Instance;
        }
    }
    /// <summary>
    /// Gets the Drag gesture recognizer.
    /// </summary>
    private readonly DragGestureRecognizer m_DragGestureRecognizer = new DragGestureRecognizer();
    public DragGestureRecognizer DragGestureRecognizer
    {
        get
        {
            return m_DragGestureRecognizer;
        }
    }
    /// <summary>
    /// Gets the Pinch gesture recognizer.
    /// </summary>
    private PinchGestureRecognizer m_PinchGestureRecognizer = new PinchGestureRecognizer();
    public PinchGestureRecognizer PinchGestureRecognizer
    {
        get
        {
            return m_PinchGestureRecognizer;
        }
    }
    /// <summary>
    /// Gets the two finger drag gesture recognizer.
    /// </summary>
    private TwoFingerDragGestureRecognizer m_TwoFingerDragGestureRecognizer = new TwoFingerDragGestureRecognizer();
    public TwoFingerDragGestureRecognizer TwoFingerDragGestureRecognizer
    {
        get
        {
            return m_TwoFingerDragGestureRecognizer;
        }
    }
    /// <summary>
    /// Gets the Tap gesture recognizer.
    /// </summary>
    private TapGestureRecognizer m_TapGestureRecognizer = new TapGestureRecognizer();
    public TapGestureRecognizer TapGestureRecognizer
    {
        get
        {
            return m_TapGestureRecognizer;
        }
    }
    /// <summary>
    /// Gets the Twist gesture recognizer.
    /// </summary>
    private TwistGestureRecognizer m_TwistGestureRecognizer = new TwistGestureRecognizer();
    public TwistGestureRecognizer TwistGestureRecognizer
    {
        get
        {
            return m_TwistGestureRecognizer;
        }
    }

    private GameObject m_SelectedObject;
    public GameObject SelectedObject
    {
        get => m_SelectedObject;
        set
        {
            // Stop floating animation
            if (m_SelectedObject != null)
                m_SelectedObject.GetComponent<Manipulator>().materialController.transform.Find("[Fridge]").GetComponent<Animator>().SetBool(Selected, false);

            // Set new selected object
            m_SelectedObject = value;
                
            // Set selected animation
            if(value != null)
                m_SelectedObject.GetComponent<Manipulator>().materialController.transform.Find("[Fridge]").GetComponent<Animator>().SetBool(Selected, true);

            //// Set buttons interactable
            //confirmButton.gameObject.SetActive(value != null);
            //hudButton.gameObject.SetActive(value != null);
            //deleteButton.interactable = value != null;
            //colorButton1.interactable = value != null;
            //colorButton2.interactable = value != null;
            //colorButton3.interactable = value != null;
            //colorButton4.interactable = value != null;
            //
            //// Set color pallet colors
            //if (value != null)
            //{
            //    var colors = value.GetComponent<Manipulator>().fridge.colors;
            //    colorButton1.GetComponent<Image>().color = colors[0].squareColor;
            //    colorButton2.GetComponent<Image>().color = colors[1].squareColor;
            //    colorButton3.GetComponent<Image>().color = colors[2].squareColor;
            //    colorButton4.GetComponent<Image>().color = colors[3].squareColor;
            //}
            //// Set pallet with null colors
            //else
            //{
            //    colorButton1.GetComponent<Image>().color = Color.grey;
            //    colorButton2.GetComponent<Image>().color = Color.grey;
            //    colorButton3.GetComponent<Image>().color = Color.grey;
            //    colorButton4.GetComponent<Image>().color = Color.grey;
            //}
        }
    }

    public FridgePlacementManipulator manipulator;
    public Animator hudomAnimator;
    public TextMeshProUGUI hudomFridgeName;

    [Header("HUD Object Manipulation Buttons")]
    public Button hudButton;
    public Button confirmButton;
    public Button deleteButton;
    public Button colorButton1;
    public Button colorButton2;
    public Button colorButton3;
    public Button colorButton4;
    private static readonly int Selected = Animator.StringToHash("Selected");
    private static readonly int Activated = Animator.StringToHash("activated");

    public void Awake()
    {
        if (Instance != this)
        {
            Debug.LogWarning("Multiple instances of ManipulationSystem detected in the scene." +
                             " Only one instance can exist at a time. The duplicate instances" +
                             " will be destroyed.");
            DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        confirmButton.onClick.AddListener(Deselect);
    }
    public void Update()
    {
        DragGestureRecognizer.Update();
        PinchGestureRecognizer.Update();
        TwoFingerDragGestureRecognizer.Update();
        TapGestureRecognizer.Update();
        TwistGestureRecognizer.Update();

        // Detect object change
        if (m_SelectedObject == null)
        {
            hudomAnimator.SetBool(Activated, false);
            hudomFridgeName.text = "";

            // Set pallet with dull colors
            colorButton1.GetComponent<Image>().color = Color.grey;
            colorButton2.GetComponent<Image>().color = Color.grey;
            colorButton3.GetComponent<Image>().color = Color.grey;
            colorButton4.GetComponent<Image>().color = Color.grey;
        }
        else
        {
            hudomAnimator.SetBool(Activated, true);
            hudomFridgeName.text = m_SelectedObject.GetComponent<Manipulator>().fridge.name;

            // Set color pallet colors
            var manager = FridgeMaterialsManager.Instance;
            colorButton1.GetComponent<Image>().color = manager.fridgeColors[0].squareColor;
            colorButton2.GetComponent<Image>().color = manager.fridgeColors[1].squareColor;
            colorButton3.GetComponent<Image>().color = manager.fridgeColors[2].squareColor;
            colorButton4.GetComponent<Image>().color = manager.fridgeColors[3].squareColor;
        }
    }

    internal void Deselect()
    {
        // Vibrate
        Vibration.CreateOneShot(250, 15);

        SelectedObject = null;
    }
    internal void Select(GameObject target)
    {
        if (SelectedObject == target)
        {
            return;
        }

        Deselect();
        SelectedObject = target;
    }
        
    public void Delete()
    {
        if (SelectedObject == null)
        {
            return;
        }

        manipulator.placedFridges.Remove(SelectedObject);

        // Update shopping list
        CartManager.Instance.UpdateList();

        Destroy(SelectedObject.transform.parent.gameObject);
        Deselect();
    }
    public void ChangeMaterial_OnClick(int num)
    {
        var material = FridgeMaterialsManager.Instance.GetMaterial(num);
        m_SelectedObject.GetComponent<Manipulator>().materialController.SetMaterial(material, num);
    }
}