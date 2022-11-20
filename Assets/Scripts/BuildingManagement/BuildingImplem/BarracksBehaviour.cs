using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksBehaviour : Building
{
    public override List<GameObject> getMenuComponentToInstanciate()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        this.restplace = true;
    }
}
