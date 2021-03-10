
public class PlayerStateManager : EntityStateManager
{
    private PlayerIdleState _idleState;
    private PlayerWalkState _walkState;
    private PlayerShiftState _shiftState;

    public PlayerIdleState IdleState { get => _idleState; }
    public PlayerWalkState WalkState { get => _walkState; }
    public PlayerShiftState ShiftState { get => _shiftState; }

    private void Awake()
    {
        _idleState = new PlayerIdleState(this);
        _walkState = new PlayerWalkState(this);
        _shiftState = new PlayerShiftState(this);
    }

    protected override void Start()
    {
        base.Start();
        TransitionToState(IdleState);
    }

    protected override void Update()
    {
        // TODO: consider using events for the shifting
        if (_currentState != ShiftState && DungeonManager.Instance.Shifting)
        {
            TransitionToState(ShiftState);
        }
        base.Update();
    }

}
