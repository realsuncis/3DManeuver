using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pendulum : MonoBehaviour
{
    GameObject hook1;
    GameObject hook2;
    float ropeLength;
    Vector3 suspensionPoint;
    // Start is called before the first frame update
    void Start()
    {
        hook1 = GameObject.FindGameObjectWithTag("node");
        hook2 = GameObject.FindGameObjectWithTag("node1");
        base.gameObject.GetComponent<Rigidbody>().AddForce(base.gameObject.transform.forward * 4, ForceMode.VelocityChange);
        
        Vector3 hook1ToObject = base.gameObject.transform.position - hook1.transform.position;
        Vector3 hookToHook = hook2.transform.position - hook1.transform.position;
        float lerpFactor = Vector3.Project(hook1ToObject, hookToHook.normalized).magnitude / hookToHook.magnitude;
        suspensionPoint = Vector3.Lerp(hook1.transform.position, hook2.transform.position, lerpFactor);
        ropeLength = (base.gameObject.transform.position - suspensionPoint).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

        
        Vector3 equalibriumPoint = Vector3.down;
        Vector3 stringDirection = base.gameObject.transform.position - suspensionPoint;

        float angle = Vector3.Angle(equalibriumPoint, stringDirection);
        Vector3 tension = -stringDirection.normalized  * (base.gameObject.GetComponent<Rigidbody>().mass *Physics.gravity.magnitude * Mathf.Cos(Mathf.Deg2Rad*angle));
        Vector3 centripetalForce = -stringDirection.normalized * (base.gameObject.GetComponent<Rigidbody>().mass * (float)Math.Pow(base.gameObject.GetComponent<Rigidbody>().velocity.magnitude, 2)/stringDirection.magnitude);
        Vector3 totalTension = tension + centripetalForce;


        base.gameObject.GetComponent<Rigidbody>().AddForce(totalTension, ForceMode.Force);
        //Debug.Log(base.gameObject.GetComponent<Rigidbody>().velocity.magnitude);

        //base.gameObject.transform.position = suspensionPoint + stringDirection.normalized * ropeLength;

        Debug.DrawLine(base.gameObject.transform.position, base.gameObject.transform.position - stringDirection, Color.black);
        Debug.DrawLine(base.gameObject.transform.position, base.gameObject.transform.position + totalTension, Color.blue);
        if (tension.magnitude > centripetalForce.magnitude)
        {
            Debug.DrawLine(base.gameObject.transform.position, base.gameObject.transform.position + tension, Color.yellow);
            Debug.DrawLine(base.gameObject.transform.position, base.gameObject.transform.position + centripetalForce, Color.magenta);
        }
        else
        {
            Debug.DrawLine(base.gameObject.transform.position, base.gameObject.transform.position + centripetalForce, Color.magenta);
            Debug.DrawLine(base.gameObject.transform.position, base.gameObject.transform.position + tension, Color.yellow);
        }
        Debug.DrawLine(base.gameObject.transform.position, base.gameObject.transform.position + base.gameObject.GetComponent<Rigidbody>().velocity, Color.green);
        

    }
}
