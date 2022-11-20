using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public GameObject alertPrefab;
    public BuildingGridManager gridManager;

    public GridNode associatedNode;

    public Transform[] connectingPoints;
    public List<GridNode> connectingNodes = new List<GridNode>();

    public string buildingName = "";

    protected GameObject spawnedAlert = null;

    [SerializeField]
    protected bool isWorking = false;
    [SerializeField]
    protected bool roadConnected = false;

    public bool isItWorking() { return isWorking; }


    public bool workingplace { get; protected set; } = false;
    public bool restplace { get; protected set; } = false;

    public int slots;

    public List<Character> users = new List<Character>();

    public void initialize()
    {
        for(int i = 0; i < connectingPoints.Length; ++i)
        {
            GridNode node = gridManager.worldPosToNode(connectingPoints[i].position);
            if(node != null)
            {
                connectingNodes.Add(node);
            }
        }
    }

    public void checkRoadConnections()
    {
        foreach (GridNode n in connectingNodes)
        {
            if(n.isRoad)
            {
                if (n.road.roadConnected)
                {
                    roadConnected = true;
                }
            }
        }
    }

    public void checkWorking()
    {
        if (roadConnected) isWorking = true;

        if(isWorking)
        {
            despawnAlert();
        }
        else
        {
            spawnAlert();
        }
    }

    private void spawnAlert()
    {
        if (spawnedAlert == null)
        {
            spawnedAlert = Instantiate(alertPrefab, transform.position + new Vector3(0, 10, 0), Quaternion.identity, transform);
        }
    }

    private void despawnAlert()
    {
        if (spawnedAlert != null)
        {
            Destroy(spawnedAlert);
            spawnedAlert = null;
        }
    }

    public void setRoadConnected(bool status)
    {
        roadConnected = status;

        if (!status) isWorking = false;

        /*
        if(status)
        {
            despawnAlert();
        }
        else
        {
            spawnAlert();
        }
        */
    }

    public int getBuildingCapacity()
    {
        return slots;
    }

    public int getBuildingUserCount()
    {
        return users.Count;
    }

    public bool buildingHasFreeSlot()
    {
        return users.Count < slots;
    }

    public bool addUser(Character newUser)
    {
        if (!buildingHasFreeSlot()) return false;

        if (users.Contains(newUser)) return false;

        users.Add(newUser);

        return true;
    }

    public bool removeUser(Character user)
    {
        return users.Remove(user);
    }

    public abstract List<GameObject> getMenuComponentToInstanciate();
    public virtual void initializeMenuUIComponent(List<GameObject> instanciatedComponents) { }
}
