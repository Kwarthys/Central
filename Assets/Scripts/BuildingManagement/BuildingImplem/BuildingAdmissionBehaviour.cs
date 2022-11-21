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
                panel.initialize(new PanelButtonUIData(pickARandomNameAndDescription(), BuildingMenusFactory.factory.getRandomFace(), onPanelClic));
            }
        }
    }

    private void onPanelClic()
    {
        Debug.Log("CLIC");

        GameController.instance.characterManager.spawnACharacter(this.connectingPoints[0].position);
    }

    private string[] names = { "Jean", "Jacques", "Marcel", "Michel", "Benoit", "Manon", "Michelle", "Gabrielle", "Gollum" };
    private string[] descripts = { "Super fort", "Super naze", "Pas mal", "pas ouf" };

    private string pickARandomNameAndDescription()
    {
        return $"{getRandomFrom(names)} : {getRandomFrom(descripts)}";
    }

    private string getRandomFrom(string[] list)
    {
        return list[(int)(Random.value * list.Length)];
    }
}
