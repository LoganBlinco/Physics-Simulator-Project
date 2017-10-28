using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    public static GameObject followTarget;
    public Vector3 TargetPosition;
    public static int targetIndex;

    public static Dropdown DropBoxTarget;

    public static float moveSpeed;
    public static float buffer;
    public static float speedMod = 0.9f;

    public static float freeSpeed = 1.0f;
    public static bool isFreeRoam = true;
    public static float currentZoom =8;
    public static float maxZoom = 40;
    public static float minZoom = 0.1f;
    public static float zoomMod = 1;

    public static bool CameraExists;

    public void Start()
    {
        if (CameraExists == false)
        {
            CameraExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Camera.main.orthographicSize = currentZoom;
    }
    private void Update()
    {
        if (isFreeRoam == true)
        {
            ControlFreeRoam();
        }
        else if (SimulateController.isSimulating == true)
        {
            GetFollowTarget();
            ControlLockOn();
        }
        ControlZoom();
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
        try
        {
            Vector3 Velocity = Particle.Instances[targetIndex].FinalVelocity + Vector3.one;
            moveSpeed = Velocity.magnitude * speedMod;
    
            TargetPosition = new Vector3(
                followTarget.transform.position.x + buffer,
                followTarget.transform.position.y + buffer,
                transform.position.z);
            //Unity specific method which moves the to the target position in a period of time
            transform.position = Vector3.Lerp(transform.position, TargetPosition, moveSpeed * Time.deltaTime);
        }
        catch
        {

        }

    }
}
