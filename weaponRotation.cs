using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponRotation : MonoBehaviour
{
    public float aimingSpeed = 0.01f;
    public GameObject aimTarget;

    Quaternion rotGoal;
    Vector3 direction;

    public bool isHidding = false;
    public bool zoomInZone = false;
    public bool zoomOutZone = false;

    private void Update()
    {
        Vector3 aimPos = Camera.main.WorldToScreenPoint(aimTarget.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(aimPos);
        RaycastHit hit;

        if (aimTarget.transform.localPosition.x < .8 && aimTarget.transform.localPosition.x > -.8 && aimTarget.transform.localPosition.y > -1 && aimTarget.transform.localPosition.y < 1.2)
        {
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
            }
            else
            {
                //print("I'm looking at nothing");
                direction = (aimTarget.transform.position - transform.position).normalized;
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