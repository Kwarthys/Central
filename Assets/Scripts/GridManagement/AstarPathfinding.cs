using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarPathfinding
{
    private ASNode[] asNodes;

    private int gridWidth = 0;

    public void updateGrid(GridNode[] nodes, int gridWidth)
    {
        this.gridWidth = gridWidth;

        if(asNodes == null)
        {
            //init grid
            asNodes = new ASNode[nodes.Length];
            for(int i = 0; i < asNodes.Length; ++i)
            {
                asNodes[i] = new ASNode(nodes[i].x, nodes[i].y, nodes[i].index, nodes[i].isRoad);
            }
        }
        else
        {
            for (int i = 0; i < asNodes.Length; ++i)
            {
                ASNode n = asNodes[i];
                n.walkable = nodes[i].isRoad;
            }
        }
    }

    public Vector2Int[] findPath(GridNode gstart, GridNode gend)
    {
        if(gstart == gend)
        {
            Vector2Int[] r = new Vector2Int[1];
            r[0] = new Vector2Int(gstart.x, gstart.y);
            Debug.LogWarning("Start and finish are equal - AstartPathFinding");
            return r;
        }

        if(asNodes == null)
        {
            Debug.LogError("Astar grid not initialized");
            return null;
        }

        resetScores();

        List<Vector2Int> path;
        ASNode start = getNode(gstart);
        ASNode end = getNode(gend);

        List<ASNode> openList = new List<ASNode>();
        List<ASNode> closedList = new List<ASNode>();

        start.cost = 0;

        openList.Add(start);


        while(openList.Count > 0)
        {
            //pop
            ASNode toEvaluate = openList[0];
            openList.RemoveAt(0);

            //Debug.Log("Evaluating " + toEvaluate.pos + " " + toEvaluate.index + " s:" + toEvaluate.cost + " h:" + toEvaluate.hcost + " coming from " + toEvaluate.previousIndex);

            float newCost = toEvaluate.cost + 1;

            //add Neigbhours and update costs
            ASNode[] neighbours = getNeighbours(toEvaluate);
            for (int i = 0; i < neighbours.Length; ++i)
            {
                ASNode n = neighbours[i];

                if (n == end)
                {
                    //found
                    n.previousIndex = toEvaluate.index;

                    //trouver chemin a l'envers
                    path = computePath(start, n);
                    return path.ToArray();
                }

                float tempHCost = newCost + ASNode.distance(n, end);

                if(!closedList.Contains(n))
                {
                    if(!(openList.Contains(n) && n.cost <= newCost))
                    {
                        n.cost = newCost;
                        n.hcost = tempHCost;
                        n.previousIndex = toEvaluate.index;

                        if (openList.Contains(n)) openList.Remove(n);
                        
                        addToListHSorted(openList, n);
                    }
                }
            }

            closedList.Add(toEvaluate);
        }


        Debug.LogError("cannot find path");
        return null;
    }

    private List<Vector2Int> computePath(ASNode start, ASNode end)
    {
        ASNode evaluating = end;
        List<Vector2Int> path = new List<Vector2Int>();

        while (evaluating != start)
        {
            path.Add(evaluating.pos);
            evaluating = asNodes[evaluating.previousIndex];
        }

        path.Add(evaluating.pos);
        return path;
    }

    private void addToListHSorted(List<ASNode> nodes, ASNode toAdd)
    { 
        for(int i = 0; i < nodes.Count; ++i)
        {
            if(nodes[i].hcost > toAdd.hcost)
            {
                nodes.Insert(i, toAdd);
                return;
            }
        }

        nodes.Add(toAdd);
        return;
    }

    private ASNode[] getNeighbours(ASNode node)
    {
        List<ASNode> ns = new List<ASNode>();

        ASNode candidate;

        if(node.index > 0)
        {
            candidate = asNodes[node.index - 1];
            if (candidate.walkable)
            {
                ns.Add(candidate);
            }
        }

        if(node.index + 1 < asNodes.Length)
        {
            candidate = asNodes[node.index + 1];
            if (candidate.walkable)
            {
                ns.Add(candidate);
            }
        }

        if(node.index - gridWidth >= 0)
        {
            candidate = asNodes[node.index - gridWidth];
            if(candidate.walkable)
            {
                ns.Add(candidate);
            }
        }

        if(node.index + gridWidth + 1 < asNodes.Length)
        {
            candidate = asNodes[node.index + gridWidth];
            if(candidate.walkable)
            {
                ns.Add(candidate);
            }
        }

        return ns.ToArray();
    }



    private void resetScores()
    {
        for (int i = 0; i < asNodes.Length; ++i)
        {
            ASNode n = asNodes[i];
            n.cost = float.MaxValue;
            n.hcost = float.MaxValue;
        }
    }

    private ASNode getNode(GridNode gn)
    {
        return asNodes[gn.index];
    }
}

public class ASNode
{
    public Vector2Int pos;
    public int index;

    public float cost;
    public float hcost;

    public bool walkable;

    public int previousIndex;

    public ASNode(int x, int y, int index, bool walkable)
    {
        this.pos = new Vector2Int(x, y);
        this.index = index;

        this.cost = float.MaxValue;
        this.hcost = float.MaxValue;

        this.walkable = walkable;

        this.previousIndex = -1;
    }

    public static float distance(ASNode a, ASNode b)
    {
        return Vector2Int.Distance(a.pos, b.pos);
    }

    /* changed from struct to class, that is no longer needed
    public static bool operator == (ASNode a, ASNode b) => a.pos.x == b.pos.x && a.pos.y == b.pos.y;

    public static bool operator != (ASNode a, ASNode b) => a.pos.x != b.pos.x || a.pos.y != b.pos.y;
    */
}
