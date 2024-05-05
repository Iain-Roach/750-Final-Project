using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreTestControls : MonoBehaviour
{
    PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Jump.performed += ctx => Jump();
    }

    void Jump()
    {
        Debug.Log("Test");
    }


}
