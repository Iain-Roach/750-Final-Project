using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctTreeComponent : MonoBehaviour
{
    public float size = 5;
    public int depth = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        var octTree = new OctTree<int>(this.transform.position, size, depth);

        DrawNode(octTree.GetRoot());
    }

    private Color minColor = new Color(1, 1, 1, 1);
    private Color maxColor = new Color(0, 0.5f, 1, 0.25f);

    private void DrawNode(OctTree<int>.OctTreeNode<int> node, int nodeDepth = 0)
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
                DrawNode(subnode, nodeDepth + 1);
            }
        }
        Gizmos.color = Color.Lerp(minColor, maxColor, nodeDepth / (float)depth);
        Gizmos.DrawWireCube(node.Position, Vector3.one * node.Size);
    }
}
