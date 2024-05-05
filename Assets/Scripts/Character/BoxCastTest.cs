using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCastTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Collider boxCollider;
    LayerMask layermask;
    public float magicDistNum = 0.0026f;
    void Start()
    {
        layermask = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool groundhit = Physics.BoxCast(boxCollider.bounds.center + Vector3.up, transform.localScale / 2, Vector3.down, Quaternion.identity, 1 + magicDistNum, ~layermask);
        if(groundhit)
        {
            Debug.Log("Ground Hit");
        }
    }
}
