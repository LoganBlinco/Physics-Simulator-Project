using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityMouseDrag : MonoBehaviour {

    float distance = 10.0f;

    private void OnMouseDrag()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            {
                Vector3 mousePosition = new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    distance);
                Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                transform.position = objectPosition;
            }
        }
    }
}
