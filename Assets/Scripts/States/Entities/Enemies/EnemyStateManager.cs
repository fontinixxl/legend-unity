
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyStateManager : EntityStateManager
{
    private EnemyIdleState _idleState;
    private EnemyWalkState _walkState;
    private Vector2 _direction;
    public EnemyWalkState WalkState { get => _walkState; }
    public EnemyIdleState IdleState { get => _idleState; }
    public Vector2 Direction { get => _direction; set => _direction = value; }

    private void Awake()
    {
        _idleState = new EnemyIdleState(this);
        _walkState = new EnemyWalkState(this);

        _direction = Vector2.down;
        
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

    protected void OnCollisionExit2D(Collision2D collision)
    {
        _currentState.OnCollisionExit2D(collision);
    }
}
