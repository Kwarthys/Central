using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAdmissionBehaviour : Building
{

    private void Awake()
    {
        this.workingplace = true;
    }
    public override List<GameObject> getMenuComponentToInstanciate()
    {
        List<GameObject> prefabs = new List<GameObject>();

        GameObject prefab = BuildingMenusFactory.factory.getPanelButtonPrefab();
        for(int i = 0; i < 3; ++i)
        {
            prefabs.Add(prefab);
        }

        return prefabs;
    }

    public override void initializeMenuUIComponent(List<GameObject> instanciatedComponents)
    {
        for(int i = 0; i < instanciatedComponents.Count; ++i)
        {
            PanelButtonUIComponent panel = instanciatedComponents[i].GetComponent<PanelButtonUIComponent>();

            if(panel != null)
            {
                panel.initialize(new PanelButtonUIData("SOLDAT", BuildingMenusFactory.factory.getRandomFace(), onPanelClic));
            }
        }
    }

    private void onPanelClic()
    {
        Debug.Log("CLIC");
    }
}
