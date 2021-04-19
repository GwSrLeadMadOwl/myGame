using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class povCamera : MonoBehaviour
{
    public float rotSpeed = 500f;
    public Transform cam;
    float camHorAngle;
    float camVerAngle;

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //if(Input.GetAxis("Mouse X") != 0)
        horizontalRot();
        //if (Input.GetAxis("Mouse Y") != 0)
            verticalRot();
    }

    void horizontalRot()
    {
        camVerAngle += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        camVerAngle = Mathf.Clamp(camVerAngle, -40, 40);
        transform.rotation = Quaternion.AngleAxis(camVerAngle, Vector3.up);
    }

    void verticalRot()
    {
        camHorAngle += Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
        camHorAngle = Mathf.Clamp(camHorAngle, -35, 10);
        cam.transform.localRotation = Quaternion.AngleAxis(camHorAngle, Vector3.left);
    }
}
