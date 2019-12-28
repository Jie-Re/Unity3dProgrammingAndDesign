using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTarget : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.name == "Plane")
                {
                    this.transform.position = hit.point;
                }
            }
        }
    }
}