using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UnitTests : MonoBehaviour
{
    [SerializeField]
    GameObject character;

    [SerializeField]
    GameObject quadTree;

    [SerializeField]
    GameObject closeBG;

    [SerializeField]
    GameObject farBG;

    [SerializeField]
    TextMeshProUGUI debugData;

    [SerializeField]
    TextMeshProUGUI unitTestName;


    float fps;
    float updateTimer = 0.2f;

    private int unitLevel = 0;
    // Start is called before the first frame update
    void Awake()
    {
        string[] args = System.Environment.GetCommandLineArgs();

        foreach(string arg in args)
        {
            switch(arg)
            {
                case "/UT:1":
                    unitLevel = 1;
                    unitTestName.text = "Size of QuadTree: 200";
                    quadTree.GetComponent<QuadTreeComponent>().depth = 9;
                    quadTree.GetComponent<QuadTreeComponent>().size = 200;
                    quadTree.GetComponent<ImageToVoxelGenerator>().threshold = .5f;
                    break;
                case "/UT:2":
                    unitTestName.text = "Realtime destruction and creation: radius 9";
                    character.transform.position = new Vector3(9.2f, 0.0f, 0.0f);
                    unitLevel = 2;
                    break;
                case "/UT:3":
                    unitTestName.text = "Depth limit of QuadTree: 9";
                    unitLevel = 3;
                    quadTree.GetComponent<QuadTreeComponent>().depth = 9;
                    quadTree.GetComponent<QuadTreeComponent>().size = 5;
                    quadTree.GetComponent<ImageToVoxelGenerator>().threshold = .5f;
                    break;
            }
        }

        character.SetActive(true);
        
        quadTree.SetActive(true);
        closeBG.SetActive(true);
        farBG.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFPSDisplay();
        switch(unitLevel)
        {
            case 1:
                break;
            case 2:
                CreateAndDestroy();
                break;
            case 3:
                break;
        }
    }

    private void UpdateFPSDisplay()
    {
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0f)
        {
            fps = 1f / Time.unscaledDeltaTime;
            debugData.text = "FPS: " + Mathf.Round(fps).ToString();
            updateTimer = 0.2f;
        }
    }

    float radius = 9;
    bool create = false;
    float createAndDestroyTimer = 0.2f;
    private void CreateAndDestroy()
    {
        var insertionPoint = Vector3.zero;
        createAndDestroyTimer -= Time.deltaTime;
        {
            if(create)
            {
                quadTree.GetComponent<QuadTreeComponent>().QuadTree.InsertCircle(insertionPoint, radius, 1);
                create = !create;
            }
            else
            {
                quadTree.GetComponent<QuadTreeComponent>().QuadTree.InsertCircle(insertionPoint, radius, 0);
                create = !create;
            }

            createAndDestroyTimer = 0.2f;
        }
        //quadTree.GetComponent<QuadTreeComponent>().QuadTree.InsertCircle(insertionPoint, radius, 0);
        //quadTree.GetComponent<QuadTreeComponent>().QuadTree.InsertCircle(insertionPoint, radius, 1);
    }
}
