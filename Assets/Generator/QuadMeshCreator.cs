using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadMeshCreator : MonoBehaviour
{
    public bool generate = false;
    public QuadTreeComponent quadTree;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(generate)
        {
            GenerateMesh();
            generate = false;
        }
    }

    private void GenerateMesh()
    {

        foreach(var leaf in quadTree.QuadTree.GetLeafNodes())
        {
            // if
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.parent = quadTree.transform;
            go.transform.localPosition = leaf.Position;
            go.transform.localScale = Vector3.one * leaf.Size;
        }
        
    }
}
