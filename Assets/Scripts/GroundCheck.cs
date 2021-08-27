using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField]
    LayerMask groundMask;
    
    // Update is called once per frame
    void Update()
    {
        bool isGrounded = Physics.CheckSphere(transform.position, 1, groundMask, QueryTriggerInteraction.Ignore);

        if (isGrounded)
            SendMessageUpwards("ResetVelocity");

    }
}
