using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keenan_XR
{
    public class XR_Movement : MonoBehaviour
    {
        Camera cam;

        [SerializeField]
        [Range(0.0f, 10.0f)]
        float speed = 1;

        CharacterController controller;

        float gravity = -9.81f;
        Vector3 velocity;

        [SerializeField]
        GameObject GroundCheck;
        [SerializeField]
        LayerMask groundMask;
        bool isGrounded;

        // Start is called before the first frame update
        void Start()
        {
            cam = GetComponentInChildren<Camera>();
            controller = GetComponent<CharacterController>();
            XR_Input.leftJoyAxisDelegate += movePlayer;
        }

        // Update is called once per frame
        void Update()
        {
            isGrounded = Physics.CheckSphere(GroundCheck.transform.position, 1, groundMask, QueryTriggerInteraction.Ignore);

            //simulate gravity
            if (!isGrounded)
            {
                velocity.y += gravity;
                controller.Move(velocity * Time.deltaTime * Time.deltaTime);
            }
            else
            {
                //reset downward velocity
                ResetVelocity();
            }
        }

        void movePlayer(Vector2 input)
        {
            print(input.x + ", " + input.y);
            Vector3 move = (cam.transform.forward * input.y * speed) + (cam.transform.right * input.x * speed);

            controller.Move(move * Time.deltaTime);
        }

        void ResetVelocity()
        {
            velocity = new Vector3(0, 0, 0);
        }
    }
}