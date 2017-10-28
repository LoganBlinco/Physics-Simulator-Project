using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    public static GameObject followTarget;
    public Vector3 TargetPosition;
    public static int targetIndex;


    //Reference to dropbox where target comes from
    public static Dropdown DropBoxTarget;

    public static float moveSpeed;
    //Gap left between camera and particle
    public static float buffer = 0;
    //Percentage of speed the camera will follow of a particle
    public static float speedMod = 0.9f;

    //Movement speed when in freeMove (left right)
    public static float freeSpeed = 1.0f;
    public static bool isFreeRoam = true;
    //Zoom program begins with
    public static float currentZoom =8;
    public static float maxZoom = 40;
    public static float minZoom = 0.1f;
    //Amount changed per scroll on wheel
    public static float zoomMod = 1;

    public static bool CameraExists;

    //Ran when object attached to script is initialized.
    public void Start()
    {
        if (CameraExists == false)
        {
            CameraExists = true;
            //Prevents camera being destroyed when loading between scenes.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //Sets start zoom
        Camera.main.orthographicSize = currentZoom;
    }

    //Called every frame
    private void Update()
    {
        if (isFreeRoam == true)
        {
            ControlFreeRoam();
        }
        //Only allow particle lock when simulating to reduce calculations required
        else if (SimulateController.isSimulating == true)
        {
            GetFollowTarget();
            ControlLockOn();
        }
        ControlZoom();
    }

    private void GetFollowTarget()
    {
        targetIndex = DropBoxTarget.value-1;
        followTarget = SimulateController.ParticleInstances[targetIndex];
    }
    private void ControlFreeRoam()
    {
        float input_x = Input.GetAxisRaw("Horizontal");
        float input_y = Input.GetAxisRaw("Vertical");
        gameObject.transform.position += new Vector3(
                    input_x,
                    input_y,
                    0f).normalized * freeSpeed;
    }
    private void ControlLockOn()
    {
        Vector3 Velocity = Particle.Instances[targetIndex].FinalVelocity + Vector3.one;
        moveSpeed = Velocity.magnitude * speedMod;

        TargetPosition = new Vector3(
            followTarget.transform.position.x + buffer,
            followTarget.transform.position.y + buffer,
            transform.position.z);
        //moves to the target position in a period of time (moveSpeed * deltaT)
        transform.position = Vector3.Lerp(transform.position, TargetPosition, moveSpeed * Time.deltaTime);
    }
    private void ControlZoom()
    {
        float input = Input.GetAxis("Mouse ScrollWheel");
        if (input != 0)
        {
            float sign = -Mathf.Sign(input); //minus so backwards is zoom out while inwards is zoom in
            currentZoom += sign * zoomMod;
            currentZoom = MyMaths.Clamp(currentZoom, minZoom, maxZoom);
            Debug.Log("Next zoom " + currentZoom);
            Camera.main.orthographicSize = currentZoom;
        }
    }
}
