using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputReader : MonoBehaviour
{
    public Vector2 Move { get; private set; }

    public bool SprintHeld { get; private set; }

    public event Action InteractPressed;
    public event Action ReloadPressed;

    PlayerInput pi;
    InputAction move, look, aim, sprint, interact, reload;

    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        var a = pi.actions;
        move = a.FindAction("Move", true);

        sprint = a.FindAction("Sprint", true);
        interact = a.FindAction("Interact", true);
        //reload   = a.FindAction("Reload",   true);
    }

    void OnEnable()
    {
        move.performed += ctx => Move = ctx.ReadValue<Vector2>();
        move.canceled += _ => Move = Vector2.zero;


        sprint.started += _ => SprintHeld = true;
        sprint.canceled += _ => SprintHeld = false;

        interact.performed += _ => InteractPressed?.Invoke();
        // reload.performed   += _ => ReloadPressed?.Invoke();
    }

    void OnDisable()
    {
        move.performed -= ctx => Move = ctx.ReadValue<Vector2>();
        move.canceled -= _ => Move = Vector2.zero;

        sprint.started -= _ => SprintHeld = true;
        sprint.canceled -= _ => SprintHeld = false;
        interact.performed -= _ => InteractPressed?.Invoke();
        //  reload.performed   -= _ => ReloadPressed?.Invoke();
    }
}