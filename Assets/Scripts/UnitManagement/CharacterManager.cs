using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public BuildingManager buildingManager;

    public GameObject characterBodyPrefab;
    public Transform charactersHolder;

    private List<CharacterBody> characters = new List<CharacterBody>();

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < characters.Count; ++i)
        {
            CharacterBody cb = characters[i];
            cb.character.live();
            cb.updateBody();
        }

        /***
        if(Input.GetKeyDown(KeyCode.U))
        {
            spawnACharacter();
        }
        ***/
    }

    public void spawnACharacter(Vector3 spawnPoint)
    {
        Character c = new Character(this);

        buildingManager.assignCharacterToHouse(c);

        CharacterBody body = Instantiate(characterBodyPrefab, spawnPoint, Quaternion.identity, charactersHolder).GetComponent<CharacterBody>();

        if (body != null)
        {
            body.character = c;
            c.setBody(body);
            characters.Add(body);
        }

        c.startStateMachine();
    }

    public Vector3[] requestPathTo(Building b, Character c)
    {
        return buildingManager.requestPathFromTo(c.getBody().transform.position, b);
    }

    public Building askForWorkPlace()
    {
        if(buildingManager.tryGetWorkplace(out Building workplace))
        {
            return workplace;
        }

        return null;
    }
}
