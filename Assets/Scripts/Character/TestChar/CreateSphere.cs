using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSphere : MonoBehaviour
{
    public QuadTreeComponent quadTree;


    public void Awake()
    {
        quadTree = GameObject.Find("QuadTree").GetComponent<QuadTreeComponent>();
        Destroy(gameObject, 5f);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "QuadTree")
        {
            var insertionPoint = transform.position;

            quadTree.QuadTree.InsertCircle(collision.contacts[0].point, 0.01f, 1);

        }
        Destroy(gameObject);
    }
}
