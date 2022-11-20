using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionDetector : MonoBehaviour
{
    public Camera theCamera;

    public LayerMask interactibleLayers;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = theCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit, 1000, interactibleLayers))
            {
                /*** looking for a building ***/
                Building b = hit.transform.GetComponentInParent<Building>();

                if(b != null)
                {
                    //Debug.Log("Found");
                    BuildingMenuBehaviour.instance.setAssociatedBuilding(b);
                    return;
                }

                /*** looking for characters ? ***/
            }

            BuildingMenuBehaviour.instance.setState(false);
        }

    }
}
