using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    
    // Rigidbody
    private Rigidbody2D _rigidbody2d;
    [HideInInspector] public Rigidbody2D Rigidbody2d { get { return _rigidbody2d; } }
    
    // Animator
    Animator _animator;
    [HideInInspector] public Animator Animator { get { return _animator; } }

    // States
    private PlayerBaseState _currentState;
    public PlayerBaseState CurrentState { get { return _currentState; } }
    public readonly PlayerIdleState IdleState = new PlayerIdleState();
    public readonly PlayerWalkState WalkState = new PlayerWalkState();


    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        TransitionToState(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.Update();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    public void TransitionToState(PlayerBaseState state)
    {
        _currentState = state;
        _currentState.EnterState(this);
    }

    public void EnableKinematic(bool enable)
    {
        _rigidbody2d.isKinematic = enable;
    }
}
