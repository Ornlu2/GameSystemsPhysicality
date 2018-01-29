using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed  ;            // Player's speed when walking.
    public float rotationSpeed ;
    public float jumpHeight;        // How high Player jumps
    RaycastHit hit;

    Rigidbody rb;
    public bool CanMove = true;
    public bool grounded = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        Physics.Raycast(rb.position, -transform.up, out hit);

    }

    void FixedUpdate()
    {
        if (CanMove == true)
        {
            if(grounded)
            {
                Move();
                jump();

            }
           
        }
        //Debug.Log(hit.distance);

        if (hit.distance <1.5)
        {
            grounded = true;
            Debug.Log("grounded");
        }
    }

    void Move()
    {


        var translationVertical = Input.GetAxis("Vertical");
        translationVertical = translationVertical * moveSpeed;
        translationVertical *= Time.deltaTime;

        var translationHorizontal = Input.GetAxis("Horizontal");
        translationHorizontal = translationHorizontal * moveSpeed;
        translationHorizontal *= Time.deltaTime;

        var cameraRelative = Camera.main.transform.TransformDirection(0, 0, -1);
        rb.position -= new Vector3((cameraRelative.x * translationVertical), 0, (cameraRelative.z * translationVertical));
        cameraRelative = Camera.main.transform.TransformDirection(-1, 0, 0);

        rb.position -= new  Vector3((cameraRelative.x * translationHorizontal), 0, (cameraRelative.z * translationHorizontal));
    }
    void jump()
    {
        if ( hit.distance <= 1.1f  && Input.GetKeyUp(KeyCode.Space) == true)
        {
            if (hit.transform.gameObject.tag == "Ground")
            {
                grounded = false;
                rb.AddForce(0, jumpHeight, 0);
                Debug.Log("jumping");
            }
            else if (hit.transform.gameObject.tag =="Platform")
            {
                grounded = false;
                rb.AddForce(0, jumpHeight, 0);
                Debug.Log("jumping");
            }
          
        }
        

    }
    
}