using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityMouseDrag : MonoBehaviour {
    //Value to prevent the Z dimention from changing
    float distance = 10.0f;

    //Ran when mouse is selected on the collider of attached object
    private void OnMouseDrag()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            {
                Vector3 mousePosition = new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    distance);
                //Moves object to the relative position in world as mouse is pointing to
                Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                transform.position = objectPosition;
            }
        }
    }
}
