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
            if(cb.isFollowing())
            {
                cb.updateBody();
            }
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
            c.setBody(body);
            characters.Add(body);
        }
    }

    public Vector3[] requestPathTo(Building b, Character c)
    {
        return buildingManager.requestPathFromTo(c.getBody().transform.position, b);
    }
}
