using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook2 : MonoBehaviour
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
        Reticle = GameObject.Find("ReticleHook2");

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

        if (Input.GetMouseButtonDown(1))
        {

            if (Physics.Raycast(cam.position, cam.forward, out hit))
            {
               Hitpoint= hit.point ;
                Reticle.SetActive(true);
                anchorSet = true;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            anchorSet = false;
            attached = false;
            rb.isKinematic = false;
            rb.velocity = cam.forward * momentum / 2;
        }

        if (anchorSet)
        {
            float distance = Vector3.Distance(GameObject.Find(name: "GrapplingHook2Anchor").transform.position, Hitpoint);
            rb.isKinematic = true;

            Debug.Log(Hitpoint);
            GrappleHookPoint.transform.position = Vector3.MoveTowards(GrappleHookPoint.transform.position, Hitpoint, 1f);
            Debug.Log(distance);
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
            float distance = Vector3.Distance(rb.transform.position, GameObject.Find(name: "GrapplingHook2Anchor").transform.position);
            Reticle.SetActive(false);
            camera.xSpeed = 0;
            camera.ySpeed = 0;

            Pmovement.CanMove = false;
            //Debug.Log(distance);

            momentum += Time.deltaTime * speed;
            step = momentum * Time.deltaTime;



            if (distance > 1 && hitCollisionPoint == false)
            {
                rb.transform.position = Vector3.MoveTowards(rb.transform.position, GameObject.Find(name: "GrapplingHook2Anchor").transform.position, step);
                momentum += Time.deltaTime * speed;

            }
            else if (distance < 1)
            {
                hitCollisionPoint = true;
                momentum = 0;
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
