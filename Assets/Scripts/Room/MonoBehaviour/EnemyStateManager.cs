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
        
    }
    protected override void Start()
    {
        base.Start();
        TransitionToState(_walkState);
    }

    // Called from Player when hit an NPC with the sword
    // Stores the player direction to apply thrust force (inversed) to NPC
    public void Hit(Vector2 direction)
    {
        _hitDir = direction;
        TransitionToState(KnockState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
