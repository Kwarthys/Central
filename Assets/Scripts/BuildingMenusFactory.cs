using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenusFactory : MonoBehaviour
{
    public List<Sprite> faces;

    public static BuildingMenusFactory factory;

    public GameObject panelButtonComponentPrefab;

    private void Awake()
    {
        factory = this;
    }

    public GameObject getPanelButtonPrefab()
    {
        return panelButtonComponentPrefab;
    }

    public Sprite getRandomFace()
    {
        return faces[(int)(Random.value * faces.Count)];
    }
}
