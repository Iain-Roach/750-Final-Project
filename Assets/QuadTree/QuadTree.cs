using System;
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


public class QuadTree<T> where T : IComparable
{
    private QuadTreeNode<T> node;
    private int depth; // How small should the QuadTree Go?

    public event EventHandler QuadTreeUpdated;


    public QuadTree(Vector2 position, float size, int depth)
    {
        node = new QuadTreeNode<T>(position, size, depth);
        this.depth = depth;
        //node.Subdivide(depth);

    }

    public void Insert(Vector2 position, T data)
    {
        var leafNode = node.Subdivide(position, data, depth - 1); // Better performance but might not work properly:/ 
        // var leafNode = node.Subdivide(position, data, depth);  

        leafNode.Data = data;
        NotifyQuadTreeUpdate();
    }

    public void InsertCircle(Vector2 position, float radius, T data)
    {
        var leafNodes = new LinkedList<QuadTreeNode<T>>();
        node.CircleSubdivide(leafNodes, position, radius, data, depth - 1); // Better performance but might not work properly:/ 
        // var leafNode = node.Subdivide(position, data, depth);  
        
        
        NotifyQuadTreeUpdate();
    }

    private void NotifyQuadTreeUpdate()
    {
        if(QuadTreeUpdated != null)
        {
            QuadTreeUpdated(this, new EventArgs());
        }
    }


    public class QuadTreeNode<T> where T : IComparable
    {
        Vector2 position;
        float size;
        QuadTreeNode<T>[] subNodes;
        int depth;
        T data; //IList<T> value;

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

        public T Data
        {
            get { return data; }
            internal set { this.data = value; }
        }

        public QuadTreeNode(Vector2 position, float size, int depth, T data = default(T))
        {
            this.position = position;
            this.size = size;
            this.data = data;
            this.depth = depth;
        }

        public QuadTreeNode<T> Subdivide(Vector2 targetPosition, T data, int depth = 0)
        {
            if(depth == 0)
            {
                return this;
            }
            if(depth > 8)
            {
                return null;
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


                    subNodes[i] = new QuadTreeNode<T>(newPos, size * 0.5f, depth - 1);
                    //if (depth > 0 && subDivIndex == i)
                    //{
                    //    subNodes[i].Subdivide(targetPosition, data, depth - 1);

                    //}
                }
            }
            
            
            return subNodes[subDivIndex].Subdivide(targetPosition, data, depth - 1);
            
        }

        public void CircleSubdivide(LinkedList<QuadTreeNode<T>> selectedNodes, Vector2 targetPosition, float radius, T data, int depth = 0)
        {
            if (depth == 0)
            {
                this.Data = data;
                selectedNodes.AddLast(this);
                return;
            }

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

                    subNodes[i] = new QuadTreeNode<T>(newPos, size * 0.5f, depth - 1, Data);
                    
                    

                    //if (depth > 0 && subDivIndex == i)
                    //{
                    //    subNodes[i].Subdivide(targetPosition, data, depth - 1);

                    //}
                }
            }

            for (int i = 0; i < subNodes.Length; ++i)
            {
                if (subNodes[i].ContainedInCircle(targetPosition, radius))
                {
                    subNodes[i].CircleSubdivide(selectedNodes, targetPosition, radius, data, depth - 1);
                }
            }

            var shouldReduce = true;
            var initialValue = subNodes[0].Data;
            for(int i = 0; i < subNodes.Length; ++i)
            {
                shouldReduce &= ((initialValue.CompareTo(subNodes[i].Data) == 0));
                shouldReduce &= (subNodes[i].IsLeaf());
                
            }

            if(shouldReduce)
            {
                this.Data = initialValue;
                subNodes = null;
            }
        }

        public bool ContainedInCircle(Vector2 position, float radius)
        {
            Vector2 difference = this.Position - position;
            difference.x = Mathf.Max(0, Mathf.Abs(difference.x) - Size / 2);
            difference.y = Mathf.Max(0, Mathf.Abs(difference.y) - Size / 2);

            return difference.magnitude < radius;
        }

        public bool IsLeaf()
        {
            return Nodes == null;
        }

        public IEnumerable<QuadTreeNode<T>> GetLeafNodes()
        {
            if (IsLeaf())
            {
                yield return this;
            }
            else
            {
                if(Nodes != null)
                {
                    foreach (var node in Nodes)
                    {
                        foreach (var leaf in node.GetLeafNodes())
                        {
                            yield return leaf;
                        }
                    }
                }
            }
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

    public IEnumerable<QuadTreeNode<T>> GetLeafNodes()
    {
        return node.GetLeafNodes();
    }
}

