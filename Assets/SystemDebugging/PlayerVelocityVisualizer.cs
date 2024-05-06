using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVelocityVisualizer : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public LineRenderer inputRenderer;
    [SerializeField]
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lineRenderer.enabled)
        {
            UpdateLineRenderer(player.GetComponent<Rigidbody>().velocity);
        }
        
        if(inputRenderer.enabled)
        {
            UpdateInputRenderer();
        }
        
    }

    void UpdateLineRenderer(Vector3 velocity)
    {
        Vector3[] positions = { player.transform.position, player.transform.position + velocity };
        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(positions);
    }

    void UpdateInputRenderer()
    {
        Vector3[] positions = { player.transform.position, player.transform.position + player.GetComponent<SpaceShipController>().MoveInput };
        inputRenderer.positionCount = 2;
        inputRenderer.SetPositions(positions);
    }
}
