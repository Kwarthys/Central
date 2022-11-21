using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractionDetector : MonoBehaviour
{
    public Camera theCamera;

    public LayerMask interactibleLayers;

    public EventSystem eventSystem;

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

            if(eventSystem.currentSelectedGameObject == null)
            {
                //Did not clic on an UI element
                BuildingMenuBehaviour.instance.setState(false);
            }
        }
    }
}
