using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private PlayerFPSInput PlayerFPSInput;
    public PlayerFPSInput.OnFootActions onFoot;

    private PlayerFPSLook look;

    private PlayerFPSMotor motor;
    

    private void Awake()
    {
        PlayerFPSInput = new PlayerFPSInput();
        onFoot = PlayerFPSInput.OnFoot;

        motor = GetComponent<PlayerFPSMotor>();
        look = GetComponent<PlayerFPSLook>();

        onFoot.Jump.performed += ctx => motor.Jump();

        onFoot.Crouch.performed += ctx => motor.Crouch();

        onFoot.Sprint.performed += ctx => motor.Sprint();
    }

    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProccesLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
