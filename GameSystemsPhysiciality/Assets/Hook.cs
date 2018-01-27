﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hook : MonoBehaviour {

    public Transform cam;
    private RaycastHit hit;
    private Rigidbody rb;
    private CameraMouseRotation camera;
    private PlayerMovement Pmovement;
    public GameObject Reticle;
    private LineRenderer rope;
    public bool anchorSet = false;

    public bool attached = false;
    public float momentum;
    public float speed;
    private float step;

    public GameObject GrappleHookPoint;
    public bool hitCollisionPoint;
    public Vector3 hitPoint;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        camera = Camera.main.GetComponent<CameraMouseRotation>();
        Pmovement = gameObject.GetComponent<PlayerMovement>();
        Reticle = GameObject.Find("Canvas");

        rope = GetComponent<LineRenderer>();

        Reticle.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {

        GrapplingHook();
        
    }

    void GrapplingHook()
    {

        if (Physics.Raycast(cam.position, cam.forward, out hit))
        {
            Reticle.SetActive(true);
        }
        else if (hit.transform == null)
        {
            Reticle.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(cam.position, cam.forward, out hit))
            {
                Instantiate(GrappleHookPoint, hit.point, GrappleHookPoint.transform.rotation,null);

                Reticle.SetActive(true);
                rb.isKinematic = true;
                anchorSet = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            anchorSet = false;
            attached = false;
            rb.isKinematic = false;
            rb.velocity = cam.forward * momentum / 2;
        }

        if (anchorSet)
        {
            attached = true;

        }

        if (attached)
        {
            Debug.Log(hit.transform.position);
            float distance = Vector3.Distance(rb.transform.position, GameObject.Find(name: "Grapple Hook Point(Clone)").transform.position);
            Reticle.SetActive(false);
            camera.xSpeed = 0;
            camera.ySpeed = 0;


            //Debug.Log(distance);

            momentum += Time.deltaTime * speed;
            step = momentum * Time.deltaTime;



            if (distance > 3 && hitCollisionPoint == false)
            {
                rb.transform.position = Vector3.MoveTowards(rb.transform.position, GameObject.Find(name: "Grapple Hook Point(Clone)").transform.position, step);

            }
            else if (distance < 4)
            {
                hitCollisionPoint = true;
            }
            if (attached && hitCollisionPoint == true)
            {
                camera.xSpeed = 250f;
                camera.ySpeed = 140f;
            }




            if (Input.GetAxis("Mouse ScrollWheel") < 0 && hitCollisionPoint == true)
            {
                rb.position = new Vector3(rb.position.x, rb.position.y - 0.25f, rb.position.z);
            }

        }

        if (!attached)
        {
            Destroy(GameObject.FindGameObjectWithTag("Hook1Anchor"));
            hitCollisionPoint = false;
        }


        if (attached == false && momentum >= 0)
        {
            momentum -= Time.deltaTime * 100;
            step = 0;

            camera.xSpeed = 250f;
            camera.ySpeed = 140f;
        }
    }



    void RopeUpdate()
    {
        rope.SetPosition(1, GameObject.Find("Grapple Hook (Clone)").transform.position);
        rope.SetPosition(0, gameObject.transform.position);
    }
}
