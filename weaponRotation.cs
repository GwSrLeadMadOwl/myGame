using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponRotation : MonoBehaviour
{
    public float aimingSpeed = 0.01f;
    public GameObject aimTarget;

    Vector3 aimTransPos;

    Quaternion rotGoal;
    Vector3 direction;

    public bool isHidding = false;
    public bool zoomInZone = false;
    public bool zoomOutZone = false;

    private void Awake()
    {
        aimTransPos = aimTarget.transform.localPosition;
    }

    private void Update()
    {
        var aimY = -Input.GetAxis("Rightstick Vertical") * aimingSpeed * 200 * Time.deltaTime;
        var aimX = -Input.GetAxis("Rightstick Horizontal") * aimingSpeed * 200 * Time.deltaTime;

        //limit aim movements
        Vector3 clampedAim = aimTarget.transform.localPosition;
        clampedAim.y = Mathf.Clamp(clampedAim.y, -2.0f, 4.0f);
        clampedAim.x = Mathf.Clamp(clampedAim.x, -5.0f, 5.0f);
        aimTarget.transform.localPosition = clampedAim;

        //raturn aim to the origin
        if (Input.GetAxis("Rightstick Vertical") == 0 && Input.GetAxis("Rightstick Horizontal") == 0)
        {
            aimTarget.transform.localPosition = Vector3.Lerp(clampedAim, aimTransPos, aimingSpeed * 50 * Time.deltaTime);
        }

        Vector3 aimPos = Camera.main.WorldToScreenPoint(aimTarget.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(aimPos);
        RaycastHit hit;

        if (aimTarget.transform.localPosition.x < .8 && aimTarget.transform.localPosition.x > -.8 && aimTarget.transform.localPosition.y > -1 && aimTarget.transform.localPosition.y < 1.2)
        {
            aimTarget.transform.Translate(aimX, 0, aimY);

            aimTarget.SetActive(false);

            Quaternion idle = Quaternion.Euler(0, 0, -90);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, idle, aimingSpeed);
        }
        else
        {
            aimTarget.SetActive(true);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy")))
            {
                //print("Piu piu");
                direction = (hit.point - transform.position).normalized;
                aimTarget.transform.Translate(0, 0, 0);
            }
            else
            {
                //print("I'm looking at nothing");
                direction = (aimTarget.transform.position - transform.position).normalized;
                aimTarget.transform.Translate(aimX, 0, aimY);
            }

            //fixed offset rotation
            rotGoal = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, aimingSpeed);
        }

    }


    //public float aimingSpeed = 500f;
    //float armAngle;

    //void Update(){
    //    if(Input.GetAxis("Rightstick Vertical") > 0 || Input.GetAxis("Rightstick Vertical") < 0)
    //    {
    //        armAngle += Input.GetAxis("Rightstick Vertical") * aimingSpeed * Time.deltaTime;
    //        armAngle = Mathf.Clamp(armAngle, -45, 45);
    //        transform.localRotation = Quaternion.AngleAxis(armAngle, Vector3.forward);
    //    }
    //    else
    //    {
    //        armAngle = 0;
    //        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0f, 0f, 0f), aimingSpeed/100 * Time.deltaTime);
    //    }

    //}
}