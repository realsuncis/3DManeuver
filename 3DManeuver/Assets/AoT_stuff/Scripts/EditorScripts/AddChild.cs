using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddChild : MonoBehaviour
{
    bool canDo = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (canDo)
        {
            foreach (Transform child in transform)
            {
                if (!child.gameObject.GetComponent<BoxCollider>() && child.lossyScale.x > 0 && child.lossyScale.y > 0 && child.lossyScale.z > 0 /*&& child.gameObject.layer == LayerMask.NameToLayer("Untagged")*/)
                {
                    child.gameObject.AddComponent<BoxCollider>();
                    //child.gameObject.
                    child.gameObject.layer = LayerMask.NameToLayer("Ground");
                    child.gameObject.AddComponent<AddChild>();
                }
            }
            canDo = !canDo;
        }
    }
}
