using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

namespace Keenan_XR
{

    public class XR_Input : MonoBehaviour
    {
        List<UnityEngine.XR.InputDevice> leftHandControllers = new List<UnityEngine.XR.InputDevice>();
        List<UnityEngine.XR.InputDevice> rightHandControllers = new List<UnityEngine.XR.InputDevice>();

        bool leftTriggerOut = false;
        bool leftGripOut = false;

        bool rightTriggerOut = false;
        bool rightGripOut = false;

        Vector2 leftJoyOut;
        public delegate void LeftJoyAxisDelegate(Vector2 input);
        public static LeftJoyAxisDelegate leftJoyAxisDelegate;

        Vector2 rightJoyOut;

        AnimateHand[] handAnimators;

        // Start is called before the first frame update
        void Start()
        {
            var gameControllers = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand, gameControllers);

            var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandControllers);

            desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, rightHandControllers);

            handAnimators = GetComponentsInChildren<AnimateHand>();
        }

        // Update is called once per frame
        void Update()
        {
            foreach (UnityEngine.XR.InputDevice hand in leftHandControllers)
            {
                if (hand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton,
                                   out leftTriggerOut)
         && leftTriggerOut)
                {
                }
                if (hand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton,
                                   out leftGripOut)
         && leftGripOut)
                {
                }
                if (hand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis,
                                  out leftJoyOut))
                {
                    leftJoyAxisDelegate(leftJoyOut);
                }
            }

            foreach (UnityEngine.XR.InputDevice hand in rightHandControllers)
            {
                if (hand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton,
                                   out rightTriggerOut)
         && rightTriggerOut)
                {
                }
                if (hand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton,
                                   out rightGripOut)
         && rightGripOut)
                {
                }
            }

            foreach (AnimateHand handAnimator in handAnimators)
            {
                handAnimator.UpdateHandAnimation(TrackedPoseDriver.TrackedPose.LeftPose, leftTriggerOut, leftGripOut);
                handAnimator.UpdateHandAnimation(TrackedPoseDriver.TrackedPose.RightPose, rightTriggerOut, rightGripOut);
            }
        }
    }
}