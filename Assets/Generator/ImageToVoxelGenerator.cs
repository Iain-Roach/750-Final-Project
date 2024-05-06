using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageToVoxelGenerator : MonoBehaviour
{
    public Texture2D image;
    public QuadTreeComponent quadTree;

    public float threshold = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        int cells = (int)Mathf.Pow(2, quadTree.depth);
        for(int x = 0; x < cells; ++x)
        {
            for(int y = 0; y < cells; ++y)
            {
                Vector2 position = quadTree.transform.position;
                position.x += ((x - cells / 2) / (float)cells) * quadTree.size;
                position.y += ((y - cells / 2) / (float)cells) * quadTree.size;
                // quadTree.QuadTree.Insert(position, true);

                var pixel = image.GetPixelBilinear(x / (float)cells, y / (float)cells);
                if(pixel.r > threshold)
                {
                    quadTree.QuadTree.InsertCircle(position, 0.0001f,  1);
                }
                else
                {
                    quadTree.QuadTree.InsertCircle(position, 0.0001f, 0);
                }
            }
        }
    }

    private void GenerateOld()
    {
        //int cells = (int)Mathf.Pow(2, quadTree.depth);
        //for (int x = 0; x < cells; ++x)
        //{
        //    for (int y = 0; y < cells; ++y)
        //    {
        //        Vector2 position = quadTree.transform.position;
        //        position.x += ((x - cells / 2) / (float)cells) * quadTree.size;
        //        position.y += ((y - cells / 2) / (float)cells) * quadTree.size;
        //        // quadTree.QuadTree.Insert(position, true);

        //        var pixel = image.GetPixelBilinear(x / (float)cells, y / (float)cells);
        //        if (pixel.r > threshold)
        //        {
        //            quadTree.QuadTree.Insert(position, true);
        //        }
        //    }
        //}
    }
}
