using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{


    public Texture2D crosshairImage;
    private Transform gTransform;
    private Rigidbody gRigidbody;
    private Camera gCamera;
    private ManeuverGear playerGear;

    private readonly float maxSpeed = 10f;
    private readonly float speedFalloff = 0.92f; //0 = instant, 1 = dependant upon friction
    private readonly float accelerationSpeed = 10f; //acceleration from gas
    private readonly float maxHookDistance = 200f;


    private bool grounded = false;
    private bool moveForward = false;
    private bool moveLeft = false;
    private bool moveRight = false;
    private bool moveBack = false;
    public bool jump = false;
    private bool shootLeftHook = false;
    private bool shootRightHook = false;

    // Start is called before the first frame update
    void Start()
    {
        gTransform = gameObject.transform;
        gRigidbody = gameObject.GetComponent<Rigidbody>();
        gCamera = gameObject.GetComponentInChildren<Camera>();
        playerGear = gameObject.GetComponent<ManeuverGear>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) moveForward = true;
        else moveForward = false;

        if (Input.GetKey(KeyCode.A)) moveLeft = true;
        else moveLeft = false;

        if (Input.GetKey(KeyCode.D)) moveRight = true;
        else moveRight = false;

        if (Input.GetKey(KeyCode.S)) moveBack = true;
        else moveBack = false;

        if (Input.GetKey(KeyCode.LeftShift)) jump = true;
        else jump = false;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            shootLeftHook = true;
        }
        if (Input.GetKeyUp(KeyCode.Q)) playerGear.destroyHook(HOOK_TYPE.LEFT_HOOK);

        if (Input.GetKeyDown(KeyCode.E))
        {
            shootRightHook = true;
        }
        if (Input.GetKeyUp(KeyCode.E)) playerGear.destroyHook(HOOK_TYPE.RIGHT_HOOK);
    }

    private void FixedUpdate()
    {
        //shoot hooks
        if (shootLeftHook)
        {
            RaycastHit rayHit;
            Vector3 shootDirection;
            if (Physics.Raycast(gCamera.transform.position, gCamera.transform.forward, out rayHit, maxHookDistance)) shootDirection = (rayHit.point - transform.position).normalized;
            else shootDirection = ((gCamera.transform.position + gCamera.transform.forward * 2000f) - transform.position).normalized;
            playerGear.shootHook(shootDirection, HOOK_TYPE.LEFT_HOOK);
            shootLeftHook = false;
        }

        if (shootRightHook)
        {
            RaycastHit rayHit;
            Vector3 shootDirection;
            if (Physics.Raycast(gCamera.transform.position, gCamera.transform.forward, out rayHit, maxHookDistance)) shootDirection = (rayHit.point - transform.position).normalized;
            else shootDirection = ((gCamera.transform.position + gCamera.transform.forward * 2000f) - transform.position).normalized;
            playerGear.shootHook(shootDirection, HOOK_TYPE.RIGHT_HOOK);
            shootRightHook = false;
        }

        //check for grounded state
        if (Physics.Raycast(gTransform.position, Vector3.down, 1.05f)) grounded = true;
        else grounded = false;

        //if on ground, slow down
        if (grounded) gRigidbody.velocity *= speedFalloff;

        //player controls
        if (moveForward)
        {
            if (grounded) gRigidbody.AddForce(verticalClamp(gCamera.transform.forward).normalized, ForceMode.VelocityChange);
            else if (jump) gRigidbody.AddForce(verticalClamp(gCamera.transform.forward).normalized * accelerationSpeed, ForceMode.Acceleration);
        }
        if (moveLeft)
        {
            if (grounded) gRigidbody.AddForce(verticalClamp(-gCamera.transform.right).normalized, ForceMode.VelocityChange);
            else if (jump) gRigidbody.AddForce(verticalClamp(-gCamera.transform.right).normalized * accelerationSpeed, ForceMode.Acceleration);
        }
        if (moveRight)
        {
            if (grounded) gRigidbody.AddForce(verticalClamp(gCamera.transform.right).normalized, ForceMode.VelocityChange);
            else if (jump) gRigidbody.AddForce(verticalClamp(gCamera.transform.right).normalized * accelerationSpeed, ForceMode.Acceleration);
        }
        if (moveBack)
        {
            if (grounded) gRigidbody.AddForce(verticalClamp(-gCamera.transform.forward).normalized, ForceMode.VelocityChange);
            else if (jump) gRigidbody.AddForce(verticalClamp(-gCamera.transform.forward).normalized * accelerationSpeed, ForceMode.Acceleration);
        }

        //reel
        if (jump) playerGear.passiveReel();

        //jump
        if (grounded&&jump) gRigidbody.AddForce(Vector3.up*9.1f, ForceMode.VelocityChange);

        //limit player speed on ground
        //if (gRigidbody.velocity.magnitude > maxSpeed && grounded) gRigidbody.velocity = gRigidbody.velocity.normalized * maxSpeed;
    }

    //draw corsshair
    void OnGUI()
    {
        float xMin = (Screen.width / 2) - (crosshairImage.width / 2);
        float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
    }

    private Vector3 verticalClamp(Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);    
    }
}
