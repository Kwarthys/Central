using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseMenuBehaviour : MonoBehaviour
{
    public static BaseMenuBehaviour instance;

    public Transform itemsHolder;
    public GameObject menu;

    public TextMeshProUGUI interactorName;

    public IMenuInteractor associatedMenuGameObject { get; private set; }

    private List<GameObject> instanciatedMenuItems = new List<GameObject>();

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of BaseMenuBehaviour");
        }

        BaseMenuBehaviour.instance = this;

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

        menu.SetActive(state);
    }

    public void setTitle(string name)
    {
        interactorName.text = name;
    }

    public void setAssociatedInteractor(IMenuInteractor interactor)
    {
        setState(true);
        associatedMenuGameObject = interactor;
        setTitle(interactor.getDisplayedName());

        List<GameObject> componentPrefabs = interactor.getMenuComponentToInstanciate();

        instanciatedMenuItems = new List<GameObject>();

        for(int i = 0; i < componentPrefabs.Count; ++i)
        {
            //Debug.Log("Tryin to instanciate " + componentPrefabs[i]);
            instanciatedMenuItems.Add(Instantiate(componentPrefabs[i], itemsHolder));
        }

        interactor.initializeMenuUIComponent(instanciatedMenuItems);
    }

    public void OnDestroyInteractorClic()
    {
        if(associatedMenuGameObject != null)
        {
            associatedMenuGameObject.delete();

            setState(false);
            associatedMenuGameObject = null;
        }
    }
}
