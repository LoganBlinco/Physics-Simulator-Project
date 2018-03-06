using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityMotionTracker : MonoBehaviour {

    public GameObject marker;

	//Ran on initialization
	void Start () {
        //Creates reference to marker prefab
        marker = Resources.Load("marker") as GameObject;
        //Time until first spawn
        float Initialwait = 0.5f;
        //Time between spawns
        float period = 0.5f;
        //Method "PlaceMarker" is ran after InitialWait seconds and is repeated every period seconds
        InvokeRepeating("PlaceMarker", Initialwait, period);
	}
	//Instatiates a prefab of marker where the object currently is
    private void PlaceMarker()
    {
        if (newSimulateController.isSimulating == true)
        {
            GameObject temp = Instantiate(marker) as GameObject;
            temp.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                -2.0f);     
        }
    }

}
