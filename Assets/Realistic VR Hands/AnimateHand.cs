using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SpatialTracking;

namespace Keenan_XR
{
    public class AnimateHand : MonoBehaviour
    {
        Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void UpdateHandAnimation(TrackedPoseDriver.TrackedPose pose, bool trigger, bool grip)
        {
             if (GetComponent<TrackedPoseDriver>().poseSource == pose)
            {
                animator.SetBool("Trigger Pressed", trigger);
                animator.SetBool("Grip Pressed", grip);
            }
        }
    }
}