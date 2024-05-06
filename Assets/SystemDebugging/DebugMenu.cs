using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DebugMenu : MonoBehaviour
{
    [SerializeField]
    ImageToVoxelGenerator gen;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Generate()
    {
        gen.Generate();
    }

}
