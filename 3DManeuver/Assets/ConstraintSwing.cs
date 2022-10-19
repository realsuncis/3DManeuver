using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintSwing : MonoBehaviour
{
    GameObject hookPoint;
    private bool shouldSwing = false;
    private float swingLength = 10f;

    // Start is called before the first frame update
    void Start()
    {
        hookPoint = GameObject.FindGameObjectWithTag("node");
        //gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.right * 4f, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        if ((hookPoint.transform.position - (gameObject.transform.position + gameObject.GetComponent<Rigidbody>().velocity * Time.fixedDeltaTime)).magnitude > swingLength) shouldSwing = true;
        else
        {
            swingLength = (gameObject.transform.position - hookPoint.transform.position).magnitude;
            shouldSwing = false;
        }

        Debug.DrawLine(base.gameObject.transform.position, hookPoint.transform.position, Color.magenta);

        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * 9.8f, ForceMode.Force);
        Vector3 newPosition = gameObject.transform.position + gameObject.GetComponent<Rigidbody>().velocity * Time.fixedDeltaTime;
        Vector3 constrainedPosition = hookPoint.transform.position + (newPosition - hookPoint.transform.position).normalized * swingLength;
        if (shouldSwing)
        {
            gameObject.GetComponent<Rigidbody>().velocity = (constrainedPosition - gameObject.transform.position)/Time.fixedDeltaTime;
            gameObject.transform.position = constrainedPosition;
        }

    }
}
