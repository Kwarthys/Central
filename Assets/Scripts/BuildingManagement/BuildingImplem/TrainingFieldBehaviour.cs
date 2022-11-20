using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingFieldBehaviour : Building
{
    // Start is called before the first frame update
    void Awake()
    {
        this.workingplace = true;
    }
    public override List<GameObject> getMenuComponentToInstanciate()
    {
        throw new System.NotImplementedException();
    }
}
