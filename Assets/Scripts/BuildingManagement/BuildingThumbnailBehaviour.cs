using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildingThumbnailBehaviour : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI buildingName;
    public Image buildingThumbnail;

    public ThumbnailScriptable descriptor;

    private void Start()
    {
        this.buildingName.text = descriptor.buildingName;
        this.buildingThumbnail.sprite = descriptor.image;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BuildingPlacer.enterBuildingMode(descriptor.buildingGhost);
    }
}
