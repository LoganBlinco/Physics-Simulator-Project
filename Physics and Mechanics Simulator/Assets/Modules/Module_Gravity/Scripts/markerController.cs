using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class markerController : MonoBehaviour {
    float destroyTime = 60.0f;

    // Use this for initialization
    void Start () {
        //Runs method "DestroyObjects" after destroyTime seconds
        Invoke("DestroyObjects", destroyTime);
	} 
    //Destroys the object
    public void DestroyObjects()
    {
        Destroy(gameObject);
    }


}
