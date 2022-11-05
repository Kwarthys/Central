using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Building", menuName = "Buildings")]
public class ThumbnailScriptable : ScriptableObject
{
    public string buildingName;
    public Sprite image;

    public GameObject buildingGhost;
}
