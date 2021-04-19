using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCTRL1 : MonoBehaviour {

    public float run = 4;
    public float walk = 2;
    public float jumpForce = 6;

    public GameObject player;
    //public GameObject camera;

    public float camSpeed = 20.0f;
    private Vector3 camPos;
    private float originFOV;
    public float zoomInScale = 20.0f;
    public float fieldOfViewSpeed = 20.0f;

    public bool isHidding = false;
    public bool zoomInZone = false;
    public bool zoomOutZone = false;

    bool right = true;
    Rigidbody rb;
    public bool ground = true;

    // Animator anim;

    private void Awake()
    {
        //camPos = camera.transform.localPosition;
        camPos = Camera.main.transform.localPosition;
        originFOV = Camera.main.fieldOfView;
    }

    void Start () {
        rb = GetComponent<Rigidbody>();
        // anim = GetComponent<Animator>();
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && !ground)
        {
            ground = true;
            rb.AddForce(Physics.gravity, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Collumn")
        {
            isHidding = true;
        }
        if(collider.gameObject.tag == "Zoom In")
        {
            zoomInZone = true;
        }
        if (collider.gameObject.tag == "Zoom Out")
        {
            zoomOutZone = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Collumn")
        {
            isHidding = false;
        }
        if (collider.gameObject.tag == "Zoom In")
        {
            zoomInZone = false;
        }
        if (collider.gameObject.tag == "Zoom Out")
        {
            zoomOutZone = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && ground)
        {
            ground = false;
            rb.AddForce(Physics.gravity, ForceMode.Impulse); //????
        }
    }

    void Update () {
        //moving the camera
        var camY = Input.GetAxis("Rightstick Vertical") * camSpeed * Time.deltaTime;
        var camX = Input.GetAxis("Rightstick Horizontal") * camSpeed * Time.deltaTime;

        if (!zoomInZone)
        {
            Camera.main.transform.Translate(-camX, camY, 0);
        } else
        {
            Camera.main.transform.Translate(-camX / 5, camY / 5, 0);
        }


        //limit camera movements
        Vector3 clampedCamera = Camera.main.transform.localPosition;
        clampedCamera.y = Mathf.Clamp(clampedCamera.y, 0, 2.0f);
        clampedCamera.x = Mathf.Clamp(clampedCamera.x, -2.0f, 2.0f);
        Camera.main.transform.localPosition = clampedCamera;

        //Field Of View scalling
        if (-Input.GetAxis("Rightstick Horizontal") > .2 && isHidding || -Input.GetAxis("Rightstick Horizontal") < -.2 && isHidding)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomInScale, fieldOfViewSpeed * Time.deltaTime);
        }

        if (Input.GetAxis("Rightstick Vertical") == 0 && Input.GetAxis("Rightstick Horizontal") == 0)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(clampedCamera, camPos, camSpeed / 2 * Time.deltaTime);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, originFOV, fieldOfViewSpeed * Time.deltaTime);
        }

        //Camera actions in zone
        if (zoomInZone)
        {
            Camera.main.fieldOfView = zoomInScale;
            if (right)
            {
                Vector3 maxXPos = new Vector3(.3f, .5f, -2);
                Camera.main.transform.localPosition = Vector3.Lerp(clampedCamera, maxXPos, camSpeed / 2 * Time.deltaTime);
            } else
            {
                Vector3 maxXPos = new Vector3(-.3f, .5f, -2);
                Camera.main.transform.localPosition = Vector3.Lerp(clampedCamera, maxXPos, camSpeed / 2 * Time.deltaTime);
            }
            
        }

        if (zoomOutZone)
        {
            Vector3 maxXPos = new Vector3(0, 1, -10);
            Camera.main.transform.localPosition = Vector3.Lerp(clampedCamera, maxXPos, camSpeed / 2 * Time.deltaTime);
        }


        //Camera movement system

        //Vertical
        if (Input.GetAxis("Rightstick Vertical") == 1 && !zoomInZone && !zoomOutZone)
        {
            Vector3 maxYPos = new Vector3(0, 2, -7);
            Camera.main.transform.localPosition = Vector3.Lerp(clampedCamera, maxYPos, camSpeed / 2 * Time.deltaTime);
        }
        if (Input.GetAxis("Rightstick Vertical") == -1 && !zoomInZone && !zoomOutZone)
        {
            Vector3 minYPos = new Vector3(0, 0, -7);
            Camera.main.transform.localPosition = Vector3.Lerp(clampedCamera, minYPos, camSpeed / 2 * Time.deltaTime);
        }

        //Horizontal
        if (-Input.GetAxis("Rightstick Horizontal") == 1 && !zoomInZone && !zoomOutZone && Input.GetAxis("Horizontal") == 0)
        {
            Vector3 maxXPos = new Vector3(2, 1, -7);
            Camera.main.transform.localPosition = Vector3.Lerp(clampedCamera, maxXPos, camSpeed / 2 * Time.deltaTime);
        }
        if (-Input.GetAxis("Rightstick Horizontal") == -1 && !zoomInZone && !zoomOutZone && Input.GetAxis("Horizontal") == 0)
        {
            Vector3 minXPos = new Vector3(-2, 1, -7);
            Camera.main.transform.localPosition = Vector3.Lerp(clampedCamera, minXPos, camSpeed / 2 * Time.deltaTime);
        }

        if(Input.GetAxis("Horizontal") > .5 && !zoomInZone && !zoomOutZone && -Input.GetAxis("Rightstick Horizontal") == 0)
        {
            Vector3 maxXPos = new Vector3(2, 1, -7);
            Camera.main.transform.localPosition = Vector3.Lerp(clampedCamera, maxXPos, camSpeed / 2 * Time.deltaTime);
        }
        if(Input.GetAxis("Horizontal") < -.5 && !zoomInZone && !zoomOutZone && -Input.GetAxis("Rightstick Horizontal") == 0)
        {
            Vector3 minXPos = new Vector3(-2, 1, -7);
            Camera.main.transform.localPosition = Vector3.Lerp(clampedCamera, minXPos, camSpeed / 2 * Time.deltaTime);
        }

        //jumping
        if (ground && Input.GetButtonDown("Jump") || ground && Input.GetAxisRaw("Vertical") > .2)
        {
            // anim.SetBool("Jump", true);
            ground = false;
            rb.velocity = new Vector3(0, jumpForce, 0); //need to update for falling velocity!
            rb.AddForce(rb.velocity, ForceMode.Impulse); //it must add more gravity to rigidbody! Somehow it's works, maybe...
        }
    }

    void FixedUpdate()
    {

        //movement
        float move = Input.GetAxis("Horizontal");
        // anim.SetFloat("Speed", Mathf.Abs(move));
        Vector3 movement1 = transform.right * move;
        movement1 *= walk * Time.deltaTime;
        transform.localPosition += movement1;
        if (Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Rightstick Horizontal") == 0)
        {
            //Vector3 maxXPos = new Vector3(2, 1, -7);
            //cam.transform.localPosition = Vector3.Lerp(cla)
        }

        //     if (Input.GetKeyDown(KeyCode.LeftShift))//add more speed(2+4=6)
        //     {
        //         // anim.SetBool("Run", true);
        //         Vector3 movement2 = transform.right * move;
        //         movement2 *= run * Time.deltaTime;
        //         transform.localPosition += movement2;
        //     }else if(Input.GetKeyUp(KeyCode.LeftShift))
        //         // anim.SetBool("Run", false);
        // }


        if (move > 0 && !right && -Input.GetAxis("Rightstick Horizontal") >= 0)
        {
            right = !right;
            Flip();
        }
        if (move < 0 && right && -Input.GetAxis("Rightstick Horizontal") <= 0)
        {
            right = !right;
            Flip();
        }
    }

    //crap
    private void CameraSystem(float x, float y, float z)
    {
        Vector3 maxXPos = new Vector3(x, y, z);
        //Camera.main.transform.localPosition = Vector3.Lerp(clampedCamera, maxXPos, camSpeed / 2 * Time.deltaTime);
    }

    private void Flip()
    {
        Vector3 scale = player.transform.localScale;
        scale.x *= -1;
        player.transform.localScale = scale;
    }
}
