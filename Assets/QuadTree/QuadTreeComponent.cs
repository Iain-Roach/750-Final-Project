using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeComponent : MonoBehaviour
{
    public float size = 5;
    public int depth = 2;

    public Transform[] points = new Transform[0];


    // Start is called before the first frame update
    void Start()
    {
        bool hello = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        var quadTree = new QuadTree<bool>(this.transform.position, size, depth);
        foreach(var point in points)
        {
            quadTree.Insert(point.position, true);
        }

        DrawNode(quadTree.GetRoot());
    }

    private Color minColor = new Color(1, 1, 1, 1);
    private Color maxColor = new Color(0, 0.5f, 1, 0.25f);

    private void DrawNode(QuadTree<bool>.QuadTreeNode<bool> node, int nodeDepth = 0)
    {
        if(node.IsLeaf())
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.blue;
            foreach (var subnode in node.Nodes)
            {
                if(subnode != null)
                {
                    DrawNode(subnode, nodeDepth + 1);
                }
            }
        }
        Gizmos.color = Color.Lerp(minColor, maxColor, nodeDepth / (float)depth);
        
        Gizmos.DrawWireCube(node.Position, new Vector3(1, 1, 0.1f) * node.Size);
    }
}
