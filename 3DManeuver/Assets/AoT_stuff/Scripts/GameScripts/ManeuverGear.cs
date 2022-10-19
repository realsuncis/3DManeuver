using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ManeuverGear : MonoBehaviour
{
    [SerializeField]
    GameObject hookPrefab;

    GameObject leftHook;
    GameObject rightHook;
    FPSController controller;
    private bool shouldSwing = false;
    private Vector3 virtualHook;
    private float leftSwingLength = 10f;
    private float rightSwingLength = 10f;
    private readonly float passiveReelAccel = 50f;

    // Start is called before the first frame update
    void Start()
    {
        hookPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/AoT_stuff/Prefabs/Hook.prefab", typeof(GameObject));
        controller = gameObject.GetComponent<FPSController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {   
        //determine in regards to which hook should the physics be calculated
        bool calculateRightHook = false;
        bool calculateLeftHook = false;
        if (leftHook != null)
        {
            if (leftHook.GetComponent<HookScript>().hasCollided) calculateLeftHook = true;
        }
        if (rightHook != null)
        {
            if (rightHook.GetComponent<HookScript>().hasCollided) calculateRightHook = true;
        }

        //swinging physics calculations
        if (calculateLeftHook)
        {
            //if current position is closer to hook than rope length and reel is pressed, shorten rope length
            Vector3 nextPositionToHook = (leftHook.transform.position - (gameObject.transform.position + gameObject.GetComponent<Rigidbody>().velocity * Time.fixedDeltaTime));
            shouldSwing = false;
            if (nextPositionToHook.magnitude > leftSwingLength) shouldSwing = true;
            else if (controller.jump == true)
            {
                leftSwingLength = (gameObject.transform.position - leftHook.transform.position).magnitude;             
            }

            //calculate and set player velocity by constraining position to a circle
            Vector3 newPosition = gameObject.transform.position + gameObject.GetComponent<Rigidbody>().velocity * Time.fixedDeltaTime;
            Vector3 constrainedPosition = leftHook.transform.position + (newPosition - leftHook.transform.position).normalized * leftSwingLength;
            if (shouldSwing)
            {
                gameObject.GetComponent<Rigidbody>().velocity = (constrainedPosition - gameObject.transform.position) / Time.fixedDeltaTime;
            }
        }
        if (calculateRightHook)
        {
            //if current position is closer to hook than rope length and reel is pressed, shorten rope length
            shouldSwing = false;
            Vector3 nextPositionToHook = (rightHook.transform.position - (gameObject.transform.position + gameObject.GetComponent<Rigidbody>().velocity * Time.fixedDeltaTime));
            if (nextPositionToHook.magnitude > rightSwingLength) shouldSwing = true;
            else if (controller.jump == true)
            {
                rightSwingLength = (gameObject.transform.position - rightHook.transform.position).magnitude;               
            }

            //calculate and set player velocity by constraining position to a circle
            Vector3 newPosition = gameObject.transform.position + gameObject.GetComponent<Rigidbody>().velocity * Time.fixedDeltaTime;
            Vector3 constrainedPosition = rightHook.transform.position + (newPosition - rightHook.transform.position).normalized * rightSwingLength;
            if (shouldSwing)
            {
                gameObject.GetComponent<Rigidbody>().velocity = (constrainedPosition - gameObject.transform.position) / Time.fixedDeltaTime;
            }
        }

    }

    internal void passiveReel()
    {
        if (rightHook != null)
        {
            Debug.Log("Left hook exists");
            if (rightHook.GetComponent<HookScript>().hasCollided)
            {
                Debug.Log("Has collided");
                Vector3 reelDirection = (rightHook.transform.position - gameObject.transform.position).normalized;
                gameObject.GetComponent<Rigidbody>().AddForce(reelDirection * passiveReelAccel, ForceMode.Force);
            }
        }
        if (leftHook != null)
        {
            
            if (leftHook.GetComponent<HookScript>().hasCollided)
            {
                
                Vector3 reelDirection = (leftHook.transform.position - gameObject.transform.position).normalized;
                gameObject.GetComponent<Rigidbody>().AddForce(reelDirection * passiveReelAccel, ForceMode.Force);
            }
        }
    }

    internal void destroyHook(HOOK_TYPE type)
    {
        if (type == HOOK_TYPE.LEFT_HOOK)
        {
            Destroy(leftHook);
        }
        if (type == HOOK_TYPE.RIGHT_HOOK)
        {
            Destroy(rightHook);
        }
    }

    internal void shootHook(Vector3 shootDirection, HOOK_TYPE type , float shootVelocity = 40f)
    {
        if (type == HOOK_TYPE.LEFT_HOOK)
        {
            leftHook = Instantiate(hookPrefab, gameObject.transform.position, Quaternion.identity);
            leftHook.GetComponent<HookScript>().gear = this;
            leftHook.GetComponent<HookScript>().type = HOOK_TYPE.LEFT_HOOK;

            leftHook.GetComponent<Rigidbody>().velocity = shootDirection * (shootVelocity + gameObject.GetComponent<Rigidbody>().velocity.magnitude);
        }

        if (type == HOOK_TYPE.RIGHT_HOOK)
        {
            rightHook = Instantiate(hookPrefab, gameObject.transform.position, Quaternion.identity);
            rightHook.GetComponent<HookScript>().gear = this;
            rightHook.GetComponent<HookScript>().type = HOOK_TYPE.RIGHT_HOOK;

            rightHook.GetComponent<Rigidbody>().velocity = shootDirection * (shootVelocity + gameObject.GetComponent<Rigidbody>().velocity.magnitude);
        }
    }

    internal void setSwingLength(float length, HOOK_TYPE type)
    {
        //set length straight away only if both hooks don't exist at the same time
        if (type == HOOK_TYPE.LEFT_HOOK)
        {
            leftSwingLength = length;
        }
        else if (type == HOOK_TYPE.RIGHT_HOOK)
        {
            rightSwingLength = length;
        }

    }

}
