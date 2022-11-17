using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBody : MonoBehaviour
{
    public Character character;

    public PathFollower follower;

    public bool isFollowing(){return follower.following;}

    public void updateBody()
    {
        follower.updateFollower();
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
