using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelButtonUIComponent : MonoBehaviour
{
    public TextMeshProUGUI tmproText;

    public Image image;

    public Button button;

    public delegate void onClicButton();

    public void initialize(PanelButtonUIData data)
    {
        button.onClick.AddListener(() => data.action());
        tmproText.text = data.text;
        this.image.sprite = data.image;
    }
}

public class PanelButtonUIData
{
    public string text;

    public Sprite image;

    public PanelButtonUIComponent.onClicButton action;


    public GameObject componentPrefab;

    public PanelButtonUIData(string description, Sprite sprite, PanelButtonUIComponent.onClicButton action)
    {
        text = description;
        image = sprite;
        this.action = action;
    }
}
