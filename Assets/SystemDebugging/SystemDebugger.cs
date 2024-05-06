using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
                debugHeader.text = "Debugger Level 5: QuadTree Size";
                break;
            case 6:
                debugLevel = 6;
                debugHeader.text = "Debugger Level 6: QuadTree Time";
                break;
            case 7:
                debugLevel = 7;
                debugHeader.text = "Debugger Level 7: QuadTree Visualizer";
                break;
            case 8:
                debugLevel = 8;
                debugHeader.text = "Debugger Level 8: Number of GameObjects";
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
    
}
