using UnityEngine;

public abstract class EntityStateManager : MonoBehaviour
{
    // Physics
    protected Rigidbody2D _rigidbody2d;
    protected Collider2D _collider;
    protected KinematicTopDownController _kinematicController;
    [HideInInspector] public Rigidbody2D Rigidbody2d { get { return _rigidbody2d; } }
    [HideInInspector] public Collider2D Collider { get { return _collider; } }
    [HideInInspector] public KinematicTopDownController KinematicController { get => _kinematicController; }

    // Animator
    protected Animator _animator;
    [HideInInspector] public Animator Animator { get { return _animator; } }

    // States
    protected IState _currentState;
    public IState CurrentState { get { return _currentState; } }

    // Others
    [SerializeField]
    protected float walkSpeed = 3.0f;
    public float WalkSpeed { get { return walkSpeed; } }
    protected Vector2 _direction;
    public Vector2 Direction { get => _direction; set => _direction = value; }

    protected virtual void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _kinematicController = GetComponent<KinematicTopDownController>();
    }

    protected virtual void Update()
    {
        _currentState.Update();
    }

    protected virtual void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    public void TransitionToState(IState state)
    {
        _currentState = state;
        _currentState.EnterState();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState.OnCollisionEnter2D(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _currentState.OnCollisionExit2D(collision);
    }
}
