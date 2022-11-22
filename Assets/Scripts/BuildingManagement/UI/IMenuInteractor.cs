using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuInteractor
{
    public void delete();

    public List<GameObject> getMenuComponentToInstanciate();

    public void initializeMenuUIComponent(List<GameObject> instanciatedComponents);

    public string getDisplayedName();
}
