using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestControls : MonoBehaviour
{

    PlayerControls controls;

    Vector2 move;
    Vector2 rotate;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Jump.performed += ctx => Grow();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        //controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        //controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;
        //controls.Gameplay.Move.
    }

    void Grow()
    {
        Debug.Log("TEST");
    }

    private void Update()
    {
        Vector2 dpadInput = move;
        if(move != Vector2.zero)
        {
            Vector2 m = new Vector2(move.x, move.y) * Time.deltaTime;
            transform.Translate(m, Space.World);
            Debug.Log("DPad Input: " + dpadInput);
        }

        Vector2 rightStickInput = controls.Gameplay.Rotate.ReadValue<Vector2>();
        Vector2 r = new Vector2(-rightStickInput.y, -rightStickInput.x) * 100.0f * Time.deltaTime;
        transform.Rotate(r, Space.World);
        // Debug.Log("Right Stick Input: " + rightStickInput);
        //Vector2 m = new Vector2(-move.x, move.y) * Time.deltaTime;
        //Debug.Log(m);
        //transform.Translate(m, Space.World);
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
