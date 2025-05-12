using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class handleGrab : XRGrabInteractable
{

    public Transform handler;
    public Rigidbody rb;

    public void EndGrab()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = handler.transform.position;
        transform.rotation = handler.transform.rotation;
    }
}