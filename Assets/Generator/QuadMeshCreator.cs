using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;

public class QuadMeshCreator : MonoBehaviour
{
    private readonly Color[] voxelColorCodes = new Color[]
    {
        Color.clear,
        new Color(67.0f/255.0f, 16.0f/255.0f, 14.0f/255.0f), 
        Color.green,
        Color.blue
    };

    public bool generate = false;
    public QuadTreeComponent quadTree;

    public Material voxelMaterial;

    private GameObject previousMesh;
    private bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(quadTree.QuadTree != null)
        {
            initialized = true;
            quadTree.QuadTree.QuadTreeUpdated += (obj, args) => { generate = true; };
        }
        if(generate)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var generatedMesh = GenerateMesh();
            stopwatch.Stop();

            if(previousMesh != null)
            {
                Destroy(previousMesh);
            }
            previousMesh = generatedMesh;
            UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds);
            generate = false;
        }
    }

    private GameObject GenerateMesh()
    {
        GameObject chunk = new GameObject();
        chunk.name = "Voxel Chunk";
        chunk.transform.parent = this.transform;
        chunk.transform.localPosition = Vector3.zero;



        var mesh = new Mesh();
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uvs = new List<Vector2>();
        var normals = new List<Vector3>();
        var colors = new List<Color>();

        foreach(var leaf in quadTree.QuadTree.GetLeafNodes().Where((node) => node.Data != 0))
        {

            // Vertice insert
            var upperLeft = new Vector3(leaf.Position.x - leaf.Size * 0.5f, leaf.Position.y + leaf.Size * 0.5f, 0.0f);
            var initialIndex = vertices.Count;

            vertices.Add(upperLeft);
            vertices.Add(upperLeft + Vector3.right * leaf.Size);
            vertices.Add(upperLeft + Vector3.down * leaf.Size);
            vertices.Add(upperLeft + Vector3.down * leaf.Size + Vector3.right * leaf.Size);

            // uvs
            uvs.Add(upperLeft);
            uvs.Add(upperLeft + Vector3.right * leaf.Size);
            uvs.Add(upperLeft + Vector3.down * leaf.Size);
            uvs.Add(upperLeft + Vector3.down * leaf.Size + Vector3.right * leaf.Size);

            // normals
            normals.Add(Vector3.back);
            normals.Add(Vector3.back);
            normals.Add(Vector3.back);
            normals.Add(Vector3.back);

            // triangles
            // top left, top right, bottom left
            triangles.Add(initialIndex);
            triangles.Add(initialIndex + 1);
            triangles.Add(initialIndex + 2);

            // top right, bottom left, bottom right
            triangles.Add(initialIndex + 3);
            triangles.Add(initialIndex + 2);
            triangles.Add(initialIndex + 1);


            colors.Add(voxelColorCodes[leaf.Data]);
            colors.Add(voxelColorCodes[leaf.Data]);
            colors.Add(voxelColorCodes[leaf.Data]);
            colors.Add(voxelColorCodes[leaf.Data]);
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);
        mesh.SetColors(colors);

        var meshFilter = chunk.AddComponent<MeshFilter>();  // Change to sprite renderer? for performance maybe
        var meshRenderer = chunk.AddComponent<MeshRenderer>();
        meshRenderer.material = voxelMaterial;

        meshFilter.mesh = mesh;

        MeshCollider col = chunk.AddComponent<MeshCollider>();
        col.bounds.Expand(new Vector3(0.0f, 0.0f, 1.0f));

        chunk.tag = "QuadTree";
        return chunk;
        //foreach(var leaf in quadTree.QuadTree.GetLeafNodes())
        //{
        //    if(leaf.Data)
        //    {
        //        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        //        go.transform.parent = quadTree.transform;
        //        go.transform.localPosition = leaf.Position;
        //        go.transform.localScale = Vector3.one * leaf.Size;
        //    }

        //}

    }
}
