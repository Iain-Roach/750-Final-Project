using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;


public class SystemDebugger : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    GameObject debugPanel;
    [SerializeField]
    TextMeshProUGUI debugHeader;
    [SerializeField]
    TextMeshProUGUI debugData;

    float fps;
    float updateTimer = 0.2f;


    private int debugLevel = 0;

    [Header("Player")]
    [SerializeField] GameObject player;

    [Header("LineRenderers")]
    [SerializeField] PlayerVelocityVisualizer vV;
    [SerializeField] LineRenderer vLR;
    [SerializeField] LineRenderer iLR;

    [Header("QuadTree")]
    [SerializeField] QuadTreeComponent quadTree;
    [SerializeField] QuadMeshCreator quadCreator;




    private void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();

        foreach (string arg in args)
        {
            switch (arg)
            {
                case "/GD:1":
                    ActivateDebuggerLevel(1);
                    break;
                case "/GD:2":
                    ActivateDebuggerLevel(2);
                    break;
                case "/GD:3":
                    ActivateDebuggerLevel(3);
                    break;
                case "/GD:4":
                    ActivateDebuggerLevel(4);
                    break;
                case "/GD:5":
                    ActivateDebuggerLevel(5);
                    break;
                case "/GD:6":
                    ActivateDebuggerLevel(6);
                    break;
                case "/GD:7":
                    ActivateDebuggerLevel(7);
                    break;
                case "/GD:8":
                    ActivateDebuggerLevel(8);
                    break;
                case "/GD:9":
                    ActivateDebuggerLevel(9);
                    break;
                case "/UT:1":
                    SceneManager.LoadScene(1);
                    break;
                case "/UT:2":
                    SceneManager.LoadScene(1);
                    break;
                case "/UT:3":
                    SceneManager.LoadScene(1);
                    break;
                default:
                    ActivateDebuggerLevel(0);
                    break;
            }
        }
    }


    void ActivateDebuggerLevel(int level)
    {
        if(level > 0)
        {
            debugPanel.SetActive(true);
        }
        else
        {
            debugPanel.SetActive(false);
            return;
        }

        switch(level)
        {
            case 1:
                debugLevel = 1;
                debugHeader.text = "Debugger Level 1: FPS";
                break;
            case 2:
                debugLevel = 2;
                debugHeader.text = "Debugger Level 2: Position";
                break;
            case 3:
                debugLevel = 3;
                debugHeader.text = "Debugger Level 3: Velocity";
                debugData.text = "Velocity: Red";
                vV.enabled = true;
                vLR.enabled = true;
                break;
            case 4:
                debugLevel = 4;
                debugHeader.text = "Debugger Level 4: Input";
                debugData.text = "Input: Blue"; // add the vector data from spaceshipcontroller
                vV.enabled = true;
                iLR.enabled = true;
                break;
            case 5:
                debugLevel = 5;
                debugHeader.text = "Debugger Level 5: Number of GameObjects";
                break;
            case 6:
                debugLevel = 6;
                debugHeader.text = "Debugger Level 6: QuadTree Size";
                break;
            case 7:
                debugLevel = 7;
                debugHeader.text = "Debugger Level 7: QuadTree Time";
                break;
            case 8:
                debugLevel = 8;
                debugHeader.text = "Debugger Level 8: QuadTree Item Type";
                break;
            case 9:
                debugLevel = 9;
                debugHeader.text = "Debugger Level 9: Complete";
                break;
            default:
                debugLevel = 0;
                break;
        }
    }
    void ActicateUnitTest(int level)
    {
        switch(level)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
    private void Update()
    {
        switch(debugLevel)
        {
            case 1:
                UpdateFPSDisplay();
                break;
            case 2:
                UpdatePositionDisplay();
                break;
            case 3:
                UpdateVelocityDisplay();
                break;
            case 4:
                UpdateInputDisplay();
                break;
            case 5:
                UpdateGameobjectDisplay();
                break;
            case 6:
                UpdateQuadTreeSizeDisplay();
                break;
            case 7:
                UpdateQuadTreeUpdateTime();
                break;
            case 8:
                UpdateQuadTreeDataDisplay();
                break;
            case 9:
                UpdateMasterDisplay();
                break;
            default:
                break;
        }
    }
    private void UpdateFPSDisplay()
    {
        updateTimer -= Time.deltaTime;
        if(updateTimer <= 0f)
        {
            fps = 1f / Time.unscaledDeltaTime;
            debugData.text = Mathf.Round(fps).ToString();
            updateTimer = 0.2f;
        }
    }

    private void UpdatePositionDisplay()
    {
        updateTimer -= Time.deltaTime;
        if(updateTimer <= 0f)
        {
            debugData.text = "Position: " + player.transform.position + "\n" +
                " Rotation: " + player.transform.GetChild(0).transform.rotation.eulerAngles;
            updateTimer = 0.2f;
        }
    }

    private void UpdateVelocityDisplay()
    {
        Vector2 vel = player.GetComponent<Rigidbody>().velocity;
        debugData.text = "Velocity: Red\n" + "Velocity: " + vel;
    }

    private void UpdateInputDisplay()
    {
        Vector2 mI = player.GetComponent<SpaceShipController>().MoveInput;
        Vector2 rI = player.GetComponent<SpaceShipController>().RotateInput;


        
        // find a ay to get the sI and bI values to show up for longer

        
        bool sI = player.GetComponent<SpaceShipController>().GetShoot;
        bool bI = player.GetComponent<SpaceShipController>().GetBuild;
        debugData.text = "Input: Blue\n" + "LeftStick: " + mI + "\nRightStick: " + rI + "\nShoot: " + sI + "\nBuild: " + bI;
    }

    private void UpdateGameobjectDisplay()
    {
        float objectCount = GameObject.FindObjectsOfType<GameObject>().Length;
        debugData.text = "Number of gameobjects: " + objectCount;
    }

    private void UpdateQuadTreeSizeDisplay()
    {
        float quadTreeSize = quadTree.size;
        float quadTreeDepth = quadTree.depth;

        debugData.text = "Size: " + quadTreeSize + "\nDepth: " + quadTreeDepth;
    }

    private void UpdateQuadTreeUpdateTime()
    {
        float lastGenDuration = quadCreator.genTime;
        debugData.text = "Last Gen Duration (ms): " + lastGenDuration;
    }

    private void UpdateQuadTreeDataDisplay()
    {
        float data1 = 0;
        float data2 = 0;
        float data3 = 0;
        float empty = 0;

        foreach(var leaf in quadTree.QuadTree.GetLeafNodes())
        {
            if(leaf.Data == 0)
            {
                empty++;
            }
            else if (leaf.Data == 1)
            {
                data1++;
            }
            else if (leaf.Data == 2)
            {
                data2++;
            }
            else if(leaf.Data ==3)
            {
                data3++;
            }
        }

        debugData.text = "EmptyNodes: " + empty + "\ntype1: " + data1 + "\ntype2: " + data2 + "\ntype3: " + data3;
    }

    float lastFPS = 0.0f;

    private void UpdateMasterDisplay()
    {
        debugData.text = lastFPS.ToString();

        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0f)
        {
            fps = 1f / Time.unscaledDeltaTime;
            debugData.text = Mathf.Round(fps).ToString();
            updateTimer = 0.2f;
        }

        lastFPS = fps;


        debugData.text += "\nPosition: " + player.transform.position + "\n" +
               " Rotation: " + player.transform.GetChild(0).transform.rotation.eulerAngles;

        Vector2 vel = player.GetComponent<Rigidbody>().velocity;
        debugData.text += "\n" + "Velocity: " + vel;

        Vector2 mI = player.GetComponent<SpaceShipController>().MoveInput;
        Vector2 rI = player.GetComponent<SpaceShipController>().RotateInput;



        // find a ay to get the sI and bI values to show up for longer


        bool sI = player.GetComponent<SpaceShipController>().GetShoot;
        bool bI = player.GetComponent<SpaceShipController>().GetBuild;
        debugData.text += "\n" + "LeftStick: " + mI + "\nRightStick: " + rI + "\nShoot: " + sI + "\nBuild: " + bI;

        float objectCount = GameObject.FindObjectsOfType<GameObject>().Length;
        debugData.text += "\nNumber of gameobjects: " + objectCount;

        float quadTreeSize = quadTree.size;
        float quadTreeDepth = quadTree.depth;

        debugData.text += "\nQuadtree Size: " + quadTreeSize + "\nQuadtree Depth: " + quadTreeDepth;

        float lastGenDuration = quadCreator.genTime;
        debugData.text += "\nLast Gen Duration (ms): " + lastGenDuration;

        float data1 = 0;
        float data2 = 0;
        float data3 = 0;
        float empty = 0;

        foreach (var leaf in quadTree.QuadTree.GetLeafNodes())
        {
            if (leaf.Data == 0)
            {
                empty++;
            }
            else if (leaf.Data == 1)
            {
                data1++;
            }
            else if (leaf.Data == 2)
            {
                data2++;
            }
            else if (leaf.Data == 3)
            {
                data3++;
            }
        }

        debugData.text += "\nEmptyNodes: " + empty + "\ntype1: " + data1 + "\ntype2: " + data2 + "\ntype3: " + data3;
    }
}
