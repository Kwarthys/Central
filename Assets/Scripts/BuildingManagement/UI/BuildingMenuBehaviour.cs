using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingMenuBehaviour : MonoBehaviour
{
    public static BuildingMenuBehaviour instance;
    
    public BuildingManager buildingManager;

    public Transform itemsHolder;

    public TextMeshProUGUI buildingName;

    public Building associatedBuilding { get; private set; }

    private List<GameObject> instanciatedMenuItems = new List<GameObject>();

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of BuildingMenuBehaviour");
        }

        BuildingMenuBehaviour.instance = this;

        setState(false);
    }

    public void setState(bool state)
    {
        if(!state)
        {
            for(int i = 0; i < instanciatedMenuItems.Count; ++i)
            {
                Destroy(instanciatedMenuItems[i]);
            }

            instanciatedMenuItems.Clear();
        }

        gameObject.SetActive(state);
    }

    public void setTitle(string name)
    {
        buildingName.text = name;
    }

    public void setAssociatedBuilding(Building b)
    {
        setState(true);
        associatedBuilding = b;
        setTitle(b.buildingName);

        List<GameObject> componentPrefabs = b.getMenuComponentToInstanciate();

        instanciatedMenuItems = new List<GameObject>();

        for(int i = 0; i < componentPrefabs.Count; ++i)
        {
            //Debug.Log("Tryin to instanciate " + componentPrefabs[i]);
            instanciatedMenuItems.Add(Instantiate(componentPrefabs[i], itemsHolder));
        }

        b.initializeMenuUIComponent(instanciatedMenuItems);
    }

    public void OnDestroyBuildingClic()
    {
        if(associatedBuilding != null)
        {
            buildingManager.destroyBuilding(associatedBuilding);

            setState(false);
            associatedBuilding = null;
        }
    }
}
