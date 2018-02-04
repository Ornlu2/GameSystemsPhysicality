using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hook : MonoBehaviour
{

    public Transform cam;
    private RaycastHit hit;
    private Rigidbody rb;
    private CameraMouseRotation camera;
    private PlayerMovement Pmovement;
    public GameObject Reticle;
    private LineRenderer rope;
    public bool anchorSet = false;
    private GameObject GrappleHookPointOBJ;
    public bool attached = false;
    public float momentum;
    public float speed;
    private float step;
    private Vector3 Hitpoint;
    public GameObject GrappleHookPoint;
    public bool hitCollisionPoint;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        camera = Camera.main.GetComponent<CameraMouseRotation>();
        Pmovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        Reticle = GameObject.Find("ReticleHook1");

        rope = GetComponent<LineRenderer>();

        Reticle.SetActive(false);
        Cursor.visible = false;
       
    }
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GrapplingHook();

    }

    void GrapplingHook()
    {

        if (Physics.Raycast(cam.position, cam.forward, out hit,50f))
        {
            Reticle.SetActive(true);
        }
        else if (hit.transform == null)
        {
            Reticle.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(cam.position, cam.forward, out hit,50f))
            {
               if( hit.collider.gameObject.tag != "Ground")
                {
                    Hitpoint = hit.point;
                    Reticle.SetActive(true);
                    anchorSet = true;
                }
               
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
            float distance = Vector3.Distance(GameObject.Find(name: "GrapplingHook1Anchor").transform.position, Hitpoint);
            rb.isKinematic = true;

            Debug.Log(Hitpoint);
            GrappleHookPoint.transform.position = Vector3.MoveTowards(GrappleHookPoint.transform.position, Hitpoint, 10f);
            //Debug.Log(distance);
            if (distance <= 0.25)
            {
                attached = true;
            }
        }
        if (!anchorSet)
        {
            // float distance = Vector3.Distance(rb.transform.position, GameObject.Find(name: "GrapplingHook1Anchor").transform.position);



            GrappleHookPoint.transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z), 15f);


        }
        if (attached)
        {
            //Debug.Log(hit.transform.position);
            float distance = Vector3.Distance(rb.transform.position, GameObject.Find(name: "GrapplingHook1Anchor").transform.position);
            Reticle.SetActive(false);
            camera.xSpeed = 0;
            camera.ySpeed = 0;

            Pmovement.CanMove = false;
            //Debug.Log(distance);

            step = momentum * Time.deltaTime;



            if (distance > 1 && hitCollisionPoint == false)
            {
                momentum += Time.deltaTime * speed;

                rb.transform.position = Vector3.MoveTowards(rb.transform.position, GameObject.Find(name: "GrapplingHook1Anchor").transform.position, step);

            }
            else if (distance < 1)
            {
                momentum = 0;
                hitCollisionPoint = true;
            }
            if (attached && hitCollisionPoint == true)
            {
                camera.xSpeed = 250f;
                camera.ySpeed = 140f;

                Pmovement.grounded = false;

            }



            if (Input.GetAxis("Mouse ScrollWheel") < 0 && hitCollisionPoint == true)
            {
                Physics.Raycast(rb.position, -transform.up, out hit);

                if (hit.distance < 2)
                {
                    Debug.Log("hit ground");
                    anchorSet = false;
                    attached = false;
                    hitCollisionPoint = false;
                    rb.isKinematic = false;
                }


                rb.position = new Vector3(rb.position.x, rb.position.y - 1f, rb.position.z);


            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && rb.position.y < Hitpoint.y - 1f && hitCollisionPoint == true)
            {

                rb.position = new Vector3(rb.position.x, rb.position.y + 0.25f, rb.position.z);

            }
        }

        if (!attached)
        {
            hitCollisionPoint = false;
        }


        if (attached == false && momentum >= 0)
        {
            Pmovement.CanMove = true;
            momentum -= Time.deltaTime * 100;
            step = 0;

            camera.xSpeed = 250f;
            camera.ySpeed = 140f;
        }
    }



}
