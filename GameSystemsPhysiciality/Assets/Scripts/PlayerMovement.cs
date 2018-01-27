using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    float moveSpeed = 6f;            // Player's speed when walking.
    float rotationSpeed = 6f;
    float jumpHeight = 10f;         // How high Player jumps

    Vector3 moveDirection;

    Rigidbody rb;
    public bool CanMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (CanMove == true)
        {
            Move();
        }
    }

    void Move()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hAxis, 0f, vAxis);
        rb.position += movement * moveSpeed * Time.deltaTime;
    }
}