using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPhysics : MonoBehaviour
{
    public Transform target;

    Rigidbody rb;

// Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(target.position);
    }
}