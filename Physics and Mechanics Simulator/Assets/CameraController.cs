using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    //Contains reference to the object the camera must follow
    public static GameObject followTarget;
    //Position which camera is moving towards
    public Vector3 TargetPosition;
    //Index value of target to follow
    public static int targetIndex;


    //Reference to dropbox where target comes from in UI
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

    //Does the camera exist in the scene.
    public static bool CameraExists;

    //Ran when object attached to script is initialized.
    public void Start()
    {
        //if a camera does not exist then one must be instatiated.
        if (CameraExists == false)
        {
            CameraExists = true;
            //Prevents camera being destroyed when loading between scenes.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroys the game object because a camera already exists
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
    //Gets the target which the user has selected for the camera to follow
    private void GetFollowTarget()
    {
        //Dropbox is from UI
        targetIndex = DropBoxTarget.value-1;
        followTarget = SimulateController.ParticleInstances[targetIndex];
    }
    //Controls movemenet when in freeRoam mode
    private void ControlFreeRoam()
    {
        //GetAxisRaw returns 1,0,-1 depending on the d
        float input_x = Input.GetAxisRaw("Horizontal");
        float input_y = Input.GetAxisRaw("Vertical");
        gameObject.transform.position += new Vector3(
                    input_x,
                    input_y,
                    0f).normalized * freeSpeed;
    }
    //Controls movement when particle lock on is selected
    //Camera to move towards target particle with a speed proportional to the particle following.
    private void ControlLockOn()
    {
        //Velocity is a percentage of the particle its following.Determined by the speed Mod.
        Vector3 Velocity = Particle.Instances[targetIndex].FinalVelocity + Vector3.one;
        moveSpeed = Velocity.magnitude * speedMod;

        //Position to move to + the buffer.
        //Buffer allows for an offset between camera and particle
        TargetPosition = new Vector3(
            followTarget.transform.position.x + buffer,
            followTarget.transform.position.y + buffer,
            transform.position.z);
        //moves to the target position in a period of time (moveSpeed * deltaT)
        transform.position = Vector3.Lerp(transform.position, TargetPosition, moveSpeed * Time.deltaTime);
    }
    //Controls the zoom of the camera
    private void ControlZoom()
    {
        // 1 for positive , 0 for no input , -1 for negative input
        float input = Input.GetAxis("Mouse ScrollWheel");
        if (input != 0)
        {
            float sign = -Mathf.Sign(input); //minus so backwards is zoom out while inwards is zoom in
            currentZoom += sign * zoomMod;
            currentZoom = MyMaths.Clamp(currentZoom, minZoom, maxZoom); //Clamps the currentZoom between minZoom and maxZoom
            Camera.main.orthographicSize = currentZoom; // Updates zoom
        }
    }
}
