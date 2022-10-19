using UnityEngine;
using System.Collections;

public class HookScript : MonoBehaviour
{

    // Use this for initialization
    public bool hasCollided = false;
    public HOOK_TYPE type;
    public ManeuverGear gear;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            base.gameObject.transform.parent = other.transform;
            Destroy(gameObject.GetComponent<Rigidbody>());
            hasCollided = true;
            gear.setSwingLength((gameObject.transform.position - gear.gameObject.transform.position).magnitude, type);
        }
    }

    private void Update()
    {
        //draw wire between hook and player
        gameObject.GetComponent<LineRenderer>().SetPosition(0, gear.gameObject.transform.position);
        gameObject.GetComponent<LineRenderer>().SetPosition(1, gameObject.transform.position);
    }
}
