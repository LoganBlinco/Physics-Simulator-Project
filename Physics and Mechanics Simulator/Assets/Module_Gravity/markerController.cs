using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markerController : MonoBehaviour {

    float destroyTime = 25.0f;


    // Use this for initialization
    void Start () {
        //Invoke("SpawnObject", destroyTime);
	} 

    public void SpawnObject()
    {
        Destroy(gameObject);
    }


}
