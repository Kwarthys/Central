using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelDropDownComponent : MonoBehaviour
{
    public TextMeshProUGUI tmproText;
    public TMP_Dropdown dropdown;

    public delegate void onNewOptionSelected(string data);

    private onNewOptionSelected callback;

    public void initialize(PanelDropDownData data)
    {
        this.tmproText.text = data.text;

        this.callback = data.callback;

        dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new();
        for (int i = 0; i < data.options.Count; ++i)
        {
            options.Add(new TMP_Dropdown.OptionData(data.options[i]));
        }
        dropdown.AddOptions(options);

        dropdown.onValueChanged.AddListener(delegate { onOptionSelect(); });
    }

    private void onOptionSelect()
    {
        Debug.Log($"{dropdown.options[dropdown.value].text} selected");
        callback.Invoke(dropdown.options[dropdown.value].text);
    }
}

public class PanelDropDownData
{
    public string text;
    public List<string> options;
    public PanelDropDownComponent.onNewOptionSelected callback;

    public PanelDropDownData(string text, List<string> options, PanelDropDownComponent.onNewOptionSelected callback)
    {
        this.text = text;
        this.options = options;
        this.callback = callback;
    }
}
