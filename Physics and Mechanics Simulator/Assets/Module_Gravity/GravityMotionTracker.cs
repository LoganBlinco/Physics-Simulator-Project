using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityMotionTracker : MonoBehaviour {

    public GameObject marker;

	// Use this for initialization
	void Start () {
        marker = Resources.Load("marker") as GameObject;
        float Initialwait = 0.5f;
        float period = 0.5f;
        InvokeRepeating("PlaceMarker", Initialwait, period);
	}
	
    private void PlaceMarker()
    {
        if (GravitySimulationController.isSimulating == true)
        {
            GameObject temp = Instantiate(marker) as GameObject;
            //temp.transform.position = transform.position;
            temp.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                -2.0f);     
            Debug.Log("Transform created");
        }


    }

}
