using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreePencil : MonoBehaviour
{

    public QuadTreeComponent quadTree;
    public int value = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var insertionPoint = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Input.GetMouseButton(0))
        {
            Debug.Log("Deleting: " + insertionPoint.origin);
            quadTree.QuadTree.Insert(insertionPoint.origin, 0);

        }
        else if(Input.GetMouseButton(1))
        {
            Debug.Log("Inserting: " + insertionPoint.origin);
            quadTree.QuadTree.Insert(insertionPoint.origin, value);
        }
    }
}
