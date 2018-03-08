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

    private int index;
    private float diameter;

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
        getIndex();
        getDiameter();

        Vector3 mousePosition = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            distance);
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.position = objectPosition;
    }

    public void Update()
    {
        ClampPosition();
    }


    private void ClampPosition()
    {
        getIndex();
        getDiameter();

        Vector3 objectPosition = transform.position;
        //changes object position but restricts the position to be inside of the borders
        float minX = BorderLeft.transform.position.x + diameter / 2 + (BorderLeft.GetComponent<Renderer>().bounds.size.x / 2);
        float maxX = BorderRight.transform.position.x - diameter / 2 - (BorderRight.GetComponent<Renderer>().bounds.size.x / 2);

        float minY = BorderBottom.transform.position.y + diameter / 2 + (BorderBottom.GetComponent<Renderer>().bounds.size.y / 2);
        float maxY = BorderTop.transform.position.y - diameter / 2 - (BorderTop.GetComponent<Renderer>().bounds.size.y / 2);

        objectPosition.x = MyMaths.Clamp(objectPosition.x, minX, maxX);
        objectPosition.y = MyMaths.Clamp(objectPosition.y, minY, maxY);

        transform.position = objectPosition;
    }

    private void getIndex()
    {
        index = gameObject.GetComponent<newCollisionsController>().particleIndex;
    }
    private void getDiameter()
    {
        diameter = newParticle.ParticleInstances[index].diameter;
    }

}
