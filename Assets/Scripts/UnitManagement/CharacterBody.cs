using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBody : MonoBehaviour, IMenuInteractor
{
    public Character character;

    public PathFollower follower;

    public int characterEnergy = 0;

    public bool isFollowing(){return follower.following;}

    public void updateBody()
    {
        characterEnergy = (int)character.energy;

        if(isFollowing())
        {
            follower.updateFollower();
        }
    }

    public void registerNewPathToFollow(Vector3[] path)
    {
        follower.registerNewPathToFollow(path);
    }
    public void registerNewReachDestinationCallback(PathFollower.reachDestinationCallback c)
    {
        follower.registerNewReachDestinationCallback(c);
    }

    public void delete()
    {
        throw new System.NotImplementedException();
    }

    public List<GameObject> getMenuComponentToInstanciate()
    {
        List<GameObject> prefabs = new List<GameObject>();

        prefabs.Add(BuildingMenusFactory.factory.getPanelDropdownPrefab());

        return prefabs;
    }

    public void initializeMenuUIComponent(List<GameObject> instanciatedComponents)
    {
        PanelDropDownComponent component = instanciatedComponents[0].GetComponent<PanelDropDownComponent>();

        List<string> roles = new();
        roles.Add("Soldier");
        roles.Add("Cook");

        component.initialize(new PanelDropDownData("Assign Role : ", roles, onRoleSelected));
    }

    private void onRoleSelected(string role)
    {
        Debug.Log(role);
    }

    public string getDisplayedName()
    {
        return $"Solider {character.name}";
    }
}
