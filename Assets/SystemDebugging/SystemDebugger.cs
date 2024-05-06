using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemDebugger : MonoBehaviour
{
    [SerializeField]
    GameObject debugPanel;
    [SerializeField]
    TextMeshProUGUI debugData;

    float fps;
    float updateTimer = 0.2f;


    private int debugLevel = 0;









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
                break;
            case 2:
                debugLevel = 1;
                break;
            case 3:
                debugLevel = 1;
                break;
            case 4:
                debugLevel = 1;
                break;
            case 5:
                debugLevel = 1;
                break;
            case 6:
                debugLevel = 1;
                break;
            case 7:
                debugLevel = 1;
                break;
            case 8:
                debugLevel = 1;
                break;
            case 9:
                debugLevel = 1;
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
}
