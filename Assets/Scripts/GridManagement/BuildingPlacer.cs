using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public Camera laCamera;
    public LayerMask groundMask;
    public BuildingGridManager gridManager;
    public GameObject buildingAlertPrefab;
    public Transform buildingHolder;
    public BuildingManager buildingManager;
    public PlayerInteractionDetector interactor;

    private GameObject placingPrefab = null;

    public GameObject housePrefab;
    public GameObject roadPrefab;

    private bool placing = false;
    private bool placingSecondHandle = false;

    private GridNode firstNodeHandle = null;

    private List<Transform> ghosts = new List<Transform>();

    private Transform flyingBuilding;
    private BuildingDescriptor flyingDescriptor;


    private static BuildingPlacer instance;
    public static void enterBuildingMode(GameObject ghostPrefab)
    {
        instance.startBuilding(ghostPrefab);
    }

    private void Awake()
    {
        BuildingPlacer.instance = this;
    }

    private void startBuilding(GameObject ghostPrefab)
    {
        placingPrefab = ghostPrefab;
        flyingBuilding = Instantiate(ghostPrefab).transform;
        flyingDescriptor = flyingBuilding.GetComponent<BuildingDescriptor>();

        interactor.switchBuilderMenu(false);

        placing = true;
    }

    private void Start()
    {
        /*** INSTANCIATE STARTING ROAD ***/
        GridNode roadStart = gridManager.getNodeAt(0, 20);
        GridNode roadEnd = gridManager.getNodeAt(49, 20);

        BuildingDescriptor roadDescriptor = roadPrefab.GetComponent<BuildingDescriptor>();

        List<GridNode> path = gridManager.getGridPath(roadStart, roadEnd);
        gridManager.reservePath(roadDescriptor, path);
        
        foreach (GridNode n in path)
        {
            Building b = prepareBuildingForLife(n, Instantiate(roadDescriptor.buildingPrefab, gridManager.gridNodeToWorld(n), Quaternion.identity, buildingHolder));
            buildingManager.registerRootRoad(b);
        }

        buildingManager.recomputeRoadConnected();
    }

    // Update is called once per frame
    void Update()
    {



        /*

        if (!placing)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                placing = true;
                placingPrefab = roadPrefab;

            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                placing = true;
                placingPrefab = housePrefab;
            }

            if(placing && placingPrefab != null)
            {
                flyingBuilding = Instantiate(placingPrefab).transform;
                flyingDescriptor = flyingBuilding.GetComponent<BuildingDescriptor>();
            }
        }
        else
        */
        if(placing)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                flyingDescriptor.rotate();

                flyingBuilding.Rotate(new Vector3(0, 90, 0));
            }


            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.B))
            {
                resetAllPlacement();
            }
            else
            {
                bool placed = false;

                if (Input.GetMouseButtonDown(0))
                {
                    if(!flyingDescriptor.contiuous)
                    {
                        placed = placeBuilding();
                    }
                    else if(placingSecondHandle)
                    {
                        placed = placeContinuous();
                    }
                    else
                    {
                        placingSecondHandle = placeFirstHandle();
                    }
                }

                if(placed)
                {
                    resetAllPlacement();
                }
                else if(!placingSecondHandle)
                {
                    checkGridUnderMouse();
                }
                else
                {
                    checkContinousUnderMouse();
                }
            }
        }
    }

    private void resetAllPlacement()
    {
        Destroy(flyingBuilding.gameObject);
        flyingBuilding = null;
        placing = false;

        placingSecondHandle = false;
        firstNodeHandle = null;

        foreach (Transform t in ghosts)
        {
            Destroy(t.gameObject);
        }
        ghosts.Clear();
    }

    private bool placeContinuous()
    {
        GridNode centerNode = getNodeUnderMouse();

        if (centerNode == null) return false;
        if (!centerNode.free) return false;

        List<GridNode> path = gridManager.getGridPath(firstNodeHandle, centerNode);

        if (!gridManager.reservePath(flyingDescriptor, path)) return false;

        foreach(GridNode n in path)
        {
            prepareBuildingForLife(n, Instantiate(flyingDescriptor.buildingPrefab, gridManager.gridNodeToWorld(n), Quaternion.identity, buildingHolder));            
        }

        if(flyingDescriptor.isRoad)
        {
            buildingManager.recomputeRoadConnected();
        }

        return true;
    }

    private Building prepareBuildingForLife(GridNode n, List<GridNode> footprint, GameObject newlySpanwed)
    {
        Building building = newlySpanwed.GetComponent<Building>();
        building.alertPrefab = buildingAlertPrefab;
        building.gridManager = gridManager;
        building.initialize();

        building.footprint = footprint;

        if (n.isRoad)
        {
            n.road = building;
            buildingManager.registerNewRoad(building);
        }
        else
        {
            buildingManager.registerNewFacility(building);
        }

        return building;
    }

    private Building prepareBuildingForLife(GridNode n, GameObject newlySpanwed)
    {
        List<GridNode> footprint = new();
        footprint.Add(n);
        return prepareBuildingForLife(n, footprint, newlySpanwed);
    }

    private bool placeFirstHandle()
    {
        GridNode centerNode = getNodeUnderMouse();

        if (centerNode == null) return false;
        if (!centerNode.free) return false;

        firstNodeHandle = centerNode;

        return true;
    }


    private bool placeBuilding()
    {
        GridNode centerNode = getNodeUnderMouse();

        if (centerNode == null) return false;
        if (!centerNode.free) return false;

        if(gridManager.tryReserveSpot(flyingDescriptor, centerNode, out List<GridNode> footprint))
        {
            prepareBuildingForLife(centerNode, footprint, Instantiate(flyingDescriptor.buildingPrefab, gridManager.gridNodeToWorld(centerNode) + computePlacementOffset(flyingDescriptor), Quaternion.Euler(0, flyingDescriptor.rotation, 0), buildingHolder));

            return true;
        }
        else
        {
            return false;
        }        
    }

    private GridNode getNodeUnderMouse()
    {
        Ray ray = laCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, groundMask))
        {
            lastPointHit = hit.point;
            GridNode gridNode = gridManager.worldPosToNode(hit.point);

            return gridNode;
        }

        return null;
    }

    private Vector3 lastPointHit = new Vector3(-1000,-1000,-1000);

    private Vector3 computePlacementOffset(BuildingDescriptor descriptor)
    {
        Vector3 posOffset = new Vector3(0, 0, 0);
        if (descriptor.sizeX % 2 == 0) posOffset.x = -0.5f * gridManager.gridNodeSize;
        if (descriptor.sizeY % 2 == 0) posOffset.z = -0.5f * gridManager.gridNodeSize;

        return posOffset;
    }

    private void checkContinousUnderMouse()
    {
        GridNode gridNode = getNodeUnderMouse();

        if (gridNode == null) return;

        if (gridNode == firstNodeHandle) return;

        List<GridNode> path = gridManager.getGridPath(firstNodeHandle, gridNode);

        //adjusting ghosts number
        while(path.Count < ghosts.Count)
        {
            Destroy(ghosts[ghosts.Count - 1].gameObject);
            ghosts.RemoveAt(ghosts.Count - 1);
        }

        while(path.Count > ghosts.Count)
        {
            ghosts.Add(Instantiate(placingPrefab).transform);
        }
        //----

        for(int i = 0; i < path.Count; ++i)
        {
            ghosts[i].position = gridManager.gridNodeToWorld(path[i]);
        }
    }

    private void checkGridUnderMouse()
    {
        bool canPlace = false;
        GridNode gridNode = getNodeUnderMouse();

        if (gridNode != null)
        {
            if (gridManager.isPlacementFree(flyingDescriptor, gridNode))
            {
                canPlace = true;

                flyingBuilding.position = gridManager.gridNodeToWorld(gridNode) + computePlacementOffset(flyingDescriptor);
            }
        }

        if (!canPlace)
        {
            flyingBuilding.position = lastPointHit;
        }
        
    }
}
