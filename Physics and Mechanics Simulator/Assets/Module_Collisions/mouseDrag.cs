using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mouseDrag : MonoBehaviour {
    //Value needed for convertion from screen to world point
	float distance = 10;

    //Stores references to the border gameobjects in scene
    private GameObject BorderLeft;
    private GameObject BorderRight;
    private GameObject BorderTop;
    private GameObject BorderBottom;

    //Ran when gameobject is first instatiated
    public void Start()
    {
        //GameObject.Find looks in the scene view for a gameobject with name ("NAME")
        BorderLeft = GameObject.Find("BorderLeft");
        BorderRight = GameObject.Find("BorderRight");
        BorderTop = GameObject.Find("BorderTop");
        BorderBottom = GameObject.Find("BorderBottom");

    }

    //Ran when user hovers and clicks over a collider
    private void OnMouseDrag()
	{
        Vector3 mousePosition = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            distance);
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //changes object position but restricts the position to be inside of the borders
        objectPosition.x = MyMaths.Clamp(objectPosition.x, BorderLeft.transform.position.x, BorderRight.transform.position.x);
        objectPosition.y = MyMaths.Clamp(objectPosition.y, BorderBottom.transform.position.y, BorderTop.transform.position.y);
        transform.position = objectPosition;
    }
}
