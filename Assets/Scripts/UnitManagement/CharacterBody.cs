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
        throw new System.NotImplementedException();
    }

    public void initializeMenuUIComponent(List<GameObject> instanciatedComponents)
    {
        throw new System.NotImplementedException();
    }

    public string getDisplayedName()
    {
        return $"Solider {character.name}";
    }
}
