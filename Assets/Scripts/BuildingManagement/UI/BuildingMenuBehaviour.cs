using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingMenuBehaviour : MonoBehaviour
{
    public static BuildingMenuBehaviour instance;

    public Transform itemsHolder;

    public TextMeshProUGUI buildingName;

    private Building associatedBuilding;

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

        List<GameObject> instanciatedItems = new List<GameObject>();

        for(int i = 0; i < componentPrefabs.Count; ++i)
        {
            //Debug.Log("Tryin to instanciate " + componentPrefabs[i]);
            instanciatedItems.Add(Instantiate(componentPrefabs[i], itemsHolder));
        }

        b.initializeMenuUIComponent(instanciatedItems);
    }
}
