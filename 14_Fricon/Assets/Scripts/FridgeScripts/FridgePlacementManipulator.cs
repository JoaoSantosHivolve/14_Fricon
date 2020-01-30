//-----------------------------------------------------------------------
// <copyright file="AndyPlacementManipulator.cs" company="Google">
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

using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.ObjectManipulation;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FridgeScripts
{
    public class FridgePlacementManipulator : Manipulator
    {
        public Fridge fridgeInfo;
        public GameObject FridgePrefab => fridgeInfo.fridgeObject;

        public Camera firstPersonCamera;
        public GameObject manipulatorPrefab;
        public List<GameObject> placedFridges = new List<GameObject>();

        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            if (gesture.TargetObject == null)
            {
                return true;
            }

            return false;
        }
        protected override void OnEndManipulation(TapGesture gesture)
        {
            // No need to check for manipulation if there is no object to be placed
            if (FridgePrefab == null)
                return;

            if (gesture.WasCancelled)
            {
                return;
            }

            // If gesture is targeting an existing object we are done.
            if (gesture.TargetObject != null)
            {
                return;
            }

            if(IsPointerOverUiObject(gesture))
                return;


            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;

            if (Frame.Raycast(
                gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(firstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    // Instantiate Andy model at the hit pose.
                    var fridgeObject = Instantiate(FridgePrefab, hit.Pose.position, hit.Pose.rotation);

                    // Instantiate manipulator.
                    var manipulator = Instantiate(manipulatorPrefab, hit.Pose.position, hit.Pose.rotation);

                    // Make Andy model a child of the manipulator.
                    fridgeObject.transform.parent = manipulator.transform;

                    // Create an anchor to allow ARCore to track the hit point as understanding of
                    // the physical world evolves.
                    var anchor = Session.CreateAnchor(hit.Pose, hit.Trackable);// hit.Trackable.CreateAnchor(hit.Pose);

                    // Make manipulator a child of the anchor.
                    manipulator.transform.parent = anchor.transform;

                    // Add the manipulator fridge
                    manipulator.GetComponent<Manipulator>().fridge = fridgeInfo;

                    // Set fridge default material
                    if (fridgeObject.GetComponent<FridgeMaterialController>())
                    {
                        manipulator.GetComponent<Manipulator>().materialController = fridgeObject.GetComponent<FridgeMaterialController>();
                        fridgeObject.GetComponent<FridgeMaterialController>().SetMaterial(FridgeMaterialsManager.Instance.GetMaterial(0), 0);
                    }

                    // Select the placed object.
                    manipulator.GetComponent<Manipulator>().Select();

                    // Add placed fridge to list
                    placedFridges.Add(manipulator.gameObject);

                    // Update shopping list
                    CartManager.Instance.UpdateList();
                }
            }
        }
        private static bool IsPointerOverUiObject(TapGesture gesture)
        {
            // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
            // the ray cast appears to require only eventData.position.
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(gesture.StartPosition.x, gesture.StartPosition.y)
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}