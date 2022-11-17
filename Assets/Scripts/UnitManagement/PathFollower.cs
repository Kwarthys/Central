using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public float speed;

    private int currentPathIndex = 0;
    private Vector3[] pathToFollow;

    public bool following { get; private set; }

    public void updateFollower()
    {
        if (following)
        {
            float distancePerFrame = Time.deltaTime * speed;

            float distanceToNext = Vector3.Distance(pathToFollow[currentPathIndex], transform.position);

            if (distancePerFrame > distanceToNext)
            {
                currentPathIndex++;

                if (currentPathIndex >= pathToFollow.Length)
                {
                    destinationReachedCallback();
                    following = false;
                    transform.position = pathToFollow[currentPathIndex - 1];
                }
            }

            if (following)
            {
                transform.position += distancePerFrame * (pathToFollow[currentPathIndex] - transform.position).normalized;
            }
        }
    }

    public void registerNewPathToFollow(Vector3[] path)
    {
        pathToFollow = path;
        resetPathFollowing();
    }

    private void resetPathFollowing()
    {
        currentPathIndex = 0;
        following = true;
    }


    public delegate void reachDestinationCallback();

    private reachDestinationCallback destinationReachedCallback;

    public void registerNewReachDestinationCallback(reachDestinationCallback c)
    {
        destinationReachedCallback = c;
    }
}
