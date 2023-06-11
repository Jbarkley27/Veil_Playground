using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    public GameObject focalPoint;
    public float lookAtSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var direction = focalPoint.transform.position - transform.position;
        var rotation = Quaternion.LookRotation(direction);
        Quaternion lookRot = Quaternion.RotateTowards(transform.rotation, rotation, lookAtSpeed * Time.deltaTime);
        rb.MoveRotation(lookRot);
    }
}
