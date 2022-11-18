using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBody : MonoBehaviour
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
}
