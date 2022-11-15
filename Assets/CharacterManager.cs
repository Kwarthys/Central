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
        foreach(CharacterBody cb in characters)
        {
            cb.character.live();
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            spawnACharacter();
        }
    }

    private void spawnACharacter()
    {
        Character c = new Character(this);

        buildingManager.assignCharacterToHouse(c);

        CharacterBody body = Instantiate(characterBodyPrefab, c.house.connectingPoints[0].position, Quaternion.identity, charactersHolder).GetComponent<CharacterBody>();

        if(body != null)
        {
            body.character = c;
            c.associatedBody = body;
            characters.Add(body);
        }
    }

    public Vector3[] requestPathTo(Building b, Character c)
    {
        return buildingManager.requestPathFromTo(c.associatedBody.transform.position, b);
    }
}
