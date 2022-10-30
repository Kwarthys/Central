using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDescriptor : MonoBehaviour
{
    public int sizeX;
    public int sizeY;

    public bool contiuous = false;
    public bool isRoad = false;

    public int rotation = 0;

    public GameObject buildingPrefab;

    public void rotate()
    {
        rotation += 90;
        if (rotation == 360) rotation = 0;
    }
}
