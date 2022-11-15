using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public BuildingManager buildingManager;

    private List<Character> characters = new List<Character>();

    // Start is called before the first frame update
    void Start()
    {
        spawnACharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnACharacter()
    {
        Character test = new Character();

        buildingManager.assignCharacterToHouse(test);
    }
}
