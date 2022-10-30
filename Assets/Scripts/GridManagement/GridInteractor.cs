using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInteractor : MonoBehaviour
{
    public Camera laCamera;
    public LayerMask layerMask;

    public BuildingGridManager gridManager;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = laCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
            {
                Debug.DrawLine(hit.point, hit.point + transform.up, Color.black, 1);

                GridNode g = gridManager.worldPosToNode(hit.point);

                if(g != null)
                {
                    g.free = !g.free;
                }
            }
        }
    }
}
