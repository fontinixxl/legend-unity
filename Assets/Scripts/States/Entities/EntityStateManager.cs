using UnityEngine;
public abstract class EntityStateManager : MonoBehaviour
{
    // Physics
    protected Rigidbody2D _rigidbody2d;
    protected Collider2D _collider;
    [HideInInspector] public Rigidbody2D Rigidbody2d { get { return _rigidbody2d; } }
    [HideInInspector] public Collider2D Collider { get { return _collider; } }

    // Animator
    protected Animator _animator;
    [HideInInspector] public Animator Animator { get { return _animator; } }

    // States
    protected BaseState _currentState;
    public BaseState CurrentState { get { return _currentState; } }

    [SerializeField]
    protected float walkSpeed = 3.0f;
    public float WalkSpeed { get { return walkSpeed; } }

    protected virtual void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        _currentState.Update();
    }

    protected virtual void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    public void TransitionToState(BaseState state)
    {
        _currentState = state;
        _currentState.EnterState();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState.OnCollisionEnter2D(collision);
    }
}
