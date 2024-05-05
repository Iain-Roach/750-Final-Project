using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeSphereInserter : MonoBehaviour
{
    public QuadTreeComponent quadTree;
    public float radius = 0.5f;
    public int value = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var insertionPoint = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Deleting: " + insertionPoint.origin);
            quadTree.QuadTree.InsertCircle(insertionPoint.origin, radius, 0);

        }
        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Inserting: " + insertionPoint.origin);
            quadTree.QuadTree.InsertCircle(insertionPoint.origin, radius, value);
        }
    }
}
