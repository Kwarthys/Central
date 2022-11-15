using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBody : MonoBehaviour
{
    public Character character;

    /*** path follower ***/ //path following will most likely become its own component, refining it here for now

    private int currentPathIndex = 0;
    private Vector3[] pathToFollow;

    public float speed;

    private bool following = false;

    private void Update()
    {
        if(following)
        {
            float distancePerFrame = Time.deltaTime * speed;

            float distanceToNext = Vector3.Distance(pathToFollow[currentPathIndex], transform.position);

            if(distancePerFrame > distanceToNext)
            {
                currentPathIndex++;

                if(currentPathIndex >= pathToFollow.Length)
                {
                    Debug.Log("Has Arrived !");
                    following = false;
                    transform.position = pathToFollow[currentPathIndex - 1];
                }
            }

            if(following)
            {
                transform.position = distancePerFrame * (pathToFollow[currentPathIndex] - transform.position).normalized;
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
}
