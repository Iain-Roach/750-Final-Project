using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTreeComponent : MonoBehaviour
{
    public float size = 5;
    public int depth = 2;

    private QuadTree<bool> quadTree;

    public QuadTree<bool> QuadTree { get { return quadTree; } }

    // Start is called before the first frame update
    void Awake()
    {
       quadTree = new QuadTree<bool>(this.transform.position, size, depth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if(quadTree != null)
        {
            DrawNode(quadTree.GetRoot());
        }
       
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
