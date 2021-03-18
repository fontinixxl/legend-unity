using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyStateManager : EntityStateManager
{
    private EnemyIdleState _idleState;
    private EnemyWalkState _walkState;
    private EnemyKnockedState _knockedState;
    private Vector2 _hitDir;

    public EnemyWalkState WalkState { get => _walkState; }
    public EnemyIdleState IdleState { get => _idleState; }
    public EnemyKnockedState KnockState { get => _knockedState; }
    public Vector2 HitDirection { get => _hitDir; }
    public float ThrustForce = 13.0f;

    private void Awake()
    {
        _idleState = new EnemyIdleState(this);
        _walkState = new EnemyWalkState(this);
        _knockedState = new EnemyKnockedState(this);

        _direction = Vector2.down;
        _hitDir = Vector2.zero;
        
    }
    protected override void Start()
    {
        base.Start();
        TransitionToState(_walkState);
    }

    protected override void Update()
    {
        _currentState.ProcessAI();
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we get hit by the player's weapon
        if (collision.CompareTag("Weapon"))
        {
            // Store the diretion the player hit us
            _hitDir = collision.gameObject.GetComponentInParent<PlayerStateManager>().Direction;
            TransitionToState(KnockState);
        }
    }
}
