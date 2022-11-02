using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGridManager : MonoBehaviour
{
    private GridNode[] grid;
    public GridNode[] getGrid() { return grid; }

    public float gridNodeSize = 1;

    public Vector2Int gridSize;

    public Vector3 gridOffset = Vector3.zero;

    /*
    public void resetAllRoadConnected()
    {
        foreach(GridNode n in grid)
        {
            n.roadConnected = false;
        }
    }*/

    // Start is called before the first frame update
    void Awake()
    {
        initializeGrid(gridSize.x, gridSize.y);
    }

    private void initializeGrid(int sizeX, int sizeY)
    {
        grid = new GridNode[sizeY * sizeX];

        gridOffset *= gridNodeSize;

        for(int j = 0; j < sizeY; ++j)
        {
            for(int i = 0; i < sizeX; ++i)
            {
                int index = j * sizeX + i;
                grid[index] = new GridNode(i, j, index);


                if (index == 0) grid[index].free = false;
                if (index == 10) grid[index].free = false;
                if (index == 99) grid[index].free = false;
                if (index == 200) grid[index].free = false;
                if (index == 300) grid[index].free = false;
                if (index == 450) grid[index].free = false;
            }
        }
    }

    public Vector3 gridNodeToWorld(GridNode node)
    {
        return new Vector3(node.x * gridNodeSize, 0, node.y * gridNodeSize) + gridOffset + new Vector3(gridNodeSize / 2, 0, gridNodeSize / 2);
    }

    public GridNode worldPosToNode(Vector3 worldPos)
    {
        worldPos -= gridOffset;// + new Vector3(-gridNodeSize/2, 0, -gridNodeSize/2);

        string debug = worldPos.ToString();

        worldPos = worldPos / gridNodeSize;

        debug += " -> " + worldPos.ToString();

        int x = (int)worldPos.x;
        int y = (int)worldPos.z;

        debug += " -> " + x + ", " + y;

        //Debug.Log(debug);

        return getNodeAt(x, y);
    }


    public GridNode getNodeAt(int x, int y)
    {
        if(x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y)
        {
            return grid[x + y * gridSize.x];
        }
        else
        {
            return null;
        }
    }

    public bool reservePath(BuildingDescriptor descriptor, List<GridNode> path)
    {
        if (!pathIsFree(path)) return false;

        bool isRoad = descriptor.isRoad;

        foreach (GridNode n in path)
        {
            n.free = false;
            if (isRoad) n.isRoad = true;
        }

        return true;
    }


    public bool pathIsFree(List<GridNode> path)
    {
        foreach(GridNode n in path)
        {
            if (!n.free) return false;
        }

        return true;
    }


    public List<GridNode> getGridPath(GridNode from, GridNode to)
    {
        List<GridNode> path = new List<GridNode>();
        path.Add(from);

        int dx = to.x - from.x;
        int dy = to.y - from.y;

        if (Mathf.Abs(dx) > Mathf.Abs(dy))
        {
            int coef = dx < 0 ? -1 : 1;

            for (int i = 1; i < Mathf.Abs(dx) + 1; ++i)
            {
                path.Add(getNodeAt(from.x + i * coef, from.y));
            }

            coef = dy < 0 ? -1 : 1;
            for (int i = 1; i < Mathf.Abs(dy) + 1; ++i)
            {
                path.Add(getNodeAt(to.x, from.y + i * coef));
            }
        }
        else
        {
            int coef = dy < 0 ? -1 : 1;
            for (int i = 1; i < Mathf.Abs(dy) + 1; ++i)
            {
                path.Add(getNodeAt(from.x, from.y + i * coef));
            }

            coef = dx < 0 ? -1 : 1;
            for (int i = 1; i < Mathf.Abs(dx) + 1; ++i)
            {
                path.Add(getNodeAt(from.x + i * coef, to.y));
            }
        }

        return path;
    }


    public bool reserveSpot(BuildingDescriptor descriptor, GridNode centerNode)
    {
        RectInt footprint = getBuildingFootPrint(descriptor, centerNode);
        bool free = isPlacementFree(footprint);

        if (!free) return false;

        for (int j = footprint.y; j < footprint.y + footprint.height; ++j)
        {
            for (int i = footprint.x; i < footprint.x + footprint.width; ++i)
            {
                getNodeAt(i, j).free = false;
            }
        }

        return true;
    }


    public RectInt getBuildingFootPrint(BuildingDescriptor descriptor, GridNode centerNode)
    {
        Vector2Int centerNodePos = new Vector2Int(centerNode.x, centerNode.y);

        Vector2Int buildingCenter = new Vector2Int(descriptor.sizeX / 2, descriptor.sizeY / 2);

        bool rotated = descriptor.rotation == 90 || descriptor.rotation == 270;

        if (rotated)
        {
            int tmpx = buildingCenter.x;
            buildingCenter.x = buildingCenter.y;
            buildingCenter.y = tmpx;
        }

        Vector2Int buildingLowerLeft = centerNodePos - buildingCenter;

        RectInt footprint = new RectInt(buildingLowerLeft.x, buildingLowerLeft.y, descriptor.sizeX, descriptor.sizeY);

        if(rotated)
        {
            footprint.height = descriptor.sizeX;
            footprint.width = descriptor.sizeY;
        }

        return footprint;
    }

    public bool isPlacementFree(RectInt footprint)
    {
        for (int j = footprint.y; j < footprint.y + footprint.height; ++j)
        {
            for (int i = footprint.x; i < footprint.x + footprint.width; ++i)
            {
                GridNode n = getNodeAt(i, j);

                if (n == null) return false;

                if (!n.free) return false;
            }
        }

        return true;
    }

    public bool isPlacementFree(BuildingDescriptor descriptor, GridNode centerNode)
    {
        if (!centerNode.free) return false;

        return isPlacementFree(getBuildingFootPrint(descriptor, centerNode));
    }

    private void OnDrawGizmosSelected()
    {
        if (grid == null) return;

        foreach (GridNode g in grid)
        {
            if (g.free)
            {
                Gizmos.color = Color.green;
            }
            else if(g.isRoad)
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawWireCube(gridNodeToWorld(g), new Vector3(gridNodeSize, 0.1f, gridNodeSize) * 0.9f);
        }
    }

    public List<GridNode> getNodeNeihbours(GridNode n)
    {
        GridNode[] nodes = new GridNode[4];

        nodes[0] = getNodeAt(n.x + 1, n.y);
        nodes[1] = getNodeAt(n.x - 1, n.y);
        nodes[2] = getNodeAt(n.x, n.y + 1);
        nodes[3] = getNodeAt(n.x, n.y - 1);

        List<GridNode> ns = new List<GridNode>();

        foreach(GridNode c in nodes)
        {
            if(c != null)
            {
                ns.Add(c);
            }
        }

        return ns;
    }
}


public class GridNode
{
    public int x;
    public int y;
    public int index;
    public bool free = true;
    public bool isRoad = false;
    public Building road = null;

    public GridNode(int x, int y, int index)
    {
        this.x = x;
        this.y = y;
        this.index = index;
    }
}