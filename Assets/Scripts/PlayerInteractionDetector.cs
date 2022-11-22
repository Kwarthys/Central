using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractionDetector : MonoBehaviour
{
    public Camera theCamera;

    public LayerMask interactibleLayers;

    public EventSystem eventSystem;

    public GameObject builderMenu;
    private bool builderMenuActive = false;

    private void Start()
    {
        builderMenu.SetActive(false);
    }

    public void switchBuilderMenu(bool state)
    {
        builderMenu.SetActive(state);
        builderMenuActive = state;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            //Toggle Building Menu
            if (builderMenuActive)
            {
                //deactivate menu
                builderMenu.SetActive(false);
                builderMenuActive = false;
            }
            else
            {
                //activate
                builderMenu.SetActive(true);
                builderMenuActive = true;
            }
        }

        if (Input.GetMouseButtonDown(0) && !builderMenuActive) //Ignore clics here while builder menu is open. Builder menu has OnClic events and handles itself
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                //clicked on an UI element
                return;
            }

            Ray ray = theCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit, 1000, interactibleLayers))
            {
                /*** looking for a building ***/
                Building b = hit.transform.GetComponentInParent<Building>();

                if(b != null)
                {
                    //Debug.Log("Found");
                    BaseMenuBehaviour.instance.setAssociatedInteractor(b);
                    return;
                }

                /*** looking for characters ? ***/

                CharacterBody c = hit.transform.GetComponentInParent<CharacterBody>();

                if(c != null)
                {
                    //open character menu
                    BaseMenuBehaviour.instance.setAssociatedInteractor(c);
                    return;
                }
            }

            BaseMenuBehaviour.instance.setState(false);
        }
    }
}
