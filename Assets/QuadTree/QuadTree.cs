using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuadTreeIndex
{
    TopLeft = 0,        // 00,
    TopRight = 1,       // 01,
    BottomLeft = 2,     // 10,
    BottomRight = 3,    // 11,
}


public class QuadTree<T> 
{
    private QuadTreeNode<T> node;
    private int depth; // How small should the QuadTree Go?

   
    

    public QuadTree(Vector2 position, float size, int depth)
    {
        node = new QuadTreeNode<T>(position, size);
        this.depth = depth;
        //node.Subdivide(depth);

    }


    public void Insert(Vector2 position, T value)
    {
        node.Subdivide(position, value, depth);
    }


    public class QuadTreeNode<T>
    {
        Vector2 position;
        float size;
        QuadTreeNode<T>[] subNodes;
        T value; //IList<T> value;

        public IEnumerable<QuadTreeNode<T>> Nodes 
        { 
            get { return subNodes; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public float Size
        {
            get { return size; }
        }

        public QuadTreeNode(Vector2 position, float size)
        {
            this.position = position;
            this.size = size;
        }

        public void Subdivide(Vector2 targetPosition, T type, int depth = 0)
        {
            if(depth > 8)
            {
                return;
            }
            var subDivIndex = GetIndexOfPosition(targetPosition, position);
            if (subNodes == null)
            {
                subNodes = new QuadTreeNode<T>[4];
                
                for (int i = 0; i < subNodes.Length; ++i)
                {
                    Vector2 newPos = position;
                    if ((i & 2) == 2)
                    {
                        newPos.y -= size * 0.25f;
                    }
                    else
                    {
                        newPos.y += size * 0.25f;
                    }

                    if ((i & 1) == 1)
                    {
                        newPos.x += size * 0.25f;
                    }
                    else
                    {
                        newPos.x -= size * 0.25f;
                    }


                    subNodes[i] = new QuadTreeNode<T>(newPos, size * 0.5f);
                    //if (depth > 0 && subDivIndex == i)
                    //{
                    //    subNodes[i].Subdivide(targetPosition, value, depth - 1);

                    //}
                }
            }

            if (depth > 0)
            {
                subNodes[subDivIndex].Subdivide(targetPosition, value, depth - 1);

            }
 
            

            
        }

        public bool IsLeaf()
        {
            return subNodes == null;
        }
    }

    private static int GetIndexOfPosition(Vector2 lookupPosition, Vector2 nodePosition)
    {
        int index = 0;

        index |= lookupPosition.y < nodePosition.y ? 2 : 0;
        index |= lookupPosition.x > nodePosition.x ? 1 : 0;
        

        return index;
    }


    public QuadTreeNode<T> GetRoot()
    {
        return node;
    }
}

