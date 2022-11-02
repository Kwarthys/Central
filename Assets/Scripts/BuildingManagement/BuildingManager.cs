using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private List<Building> facilities = new List<Building>();
    private List<Building> roads = new List<Building>();

    private List<Building> rootRoads = new List<Building>();

    private AstarPathfinding pathfinder = new AstarPathfinding();

    public void registerRootRoad(Building rootRoad)
    {
        roads.Remove(rootRoad);
        rootRoads.Add(rootRoad);
    }
    public void registerRootRoads(List<Building> rootRoads)
    {
        foreach(Building r in rootRoads)
        {
            rootRoads.Remove(r);
        }
        rootRoads.AddRange(rootRoads);
    }

    public BuildingGridManager gridManager;

    public void registerNewRoad(Building road)
    {
        if(!roads.Contains(road))
        {
            roads.Add(road);
        }
        else
        {
            Debug.LogWarning("Road already registered");
        }
    }

    public void registerNewFacility(Building facility)
    {
        if (!facilities.Contains(facility))
        {
            facilities.Add(facility);
            facility.checkRoadConnections();
            facility.checkWorking();
        }
        else
        {
            Debug.LogWarning("Facility already registered");
        }
    }

    public void recomputeRoadConnected()
    {
        resetAllRoadConnected();

        List<Building> toEvaluate = new List<Building>(rootRoads);
        List<Building> done = new List<Building>();

        int i = 0;

        while(toEvaluate.Count > 0)
        {
            //Evaluating the node
            Building road = toEvaluate[0];
            ++i;

            road.setRoadConnected(true);
            
            foreach(GridNode n in road.connectingNodes)
            {
                if(n.isRoad)
                {
                    if(!done.Contains(n.road) && !toEvaluate.Contains(n.road))
                    {
                        toEvaluate.Add(n.road);
                    }
                }
            }

            //Lists management
            done.Add(road);
            toEvaluate.Remove(road);
        }

        //Debug.Log("Evaluated " + i + " roads");

        foreach(Building b in roads)
        {
            b.checkWorking();
        }

        foreach(Building b in facilities)
        {
            b.checkRoadConnections();
            b.checkWorking();
        }




        /*** ASTAR TEST ***/
        
        pathfinder.updateGrid(gridManager.getGrid(), gridManager.gridSize.x);
        
        List<Building> allroads = new List<Building>(rootRoads);
        allroads.AddRange(roads);

        GridNode roadA = allroads[(int)(Random.value * (allroads.Count - 1))].associatedNode;
        GridNode roadB = allroads[(int)(Random.value * (allroads.Count - 1))].associatedNode;

        Debug.Log("Trying from " + roadA.x + " ," + roadA.y + " to " + roadB.x + " ," + roadB.y);
        
        Vector2Int[] path = pathfinder.findPath(roadA, roadB);
        
        Vector3 x = new Vector3(0,1,0);

        foreach(Vector2Int gridPos in path)
        {
            GridNode node = gridManager.getNodeAt(gridPos.x, gridPos.y);
            Debug.DrawLine(gridManager.gridNodeToWorld(node), gridManager.gridNodeToWorld(node) + x, Color.red, 5);

            x.y += 0.2f;
        }
    }

    private void resetAllRoadConnected()
    {
        foreach (Building r in roads)
        {
            r.setRoadConnected(false);
        }

        foreach (Building f in facilities)
        {
            f.setRoadConnected(false);
        }

        //gridManager.resetAllRoadConnected();
    }
}