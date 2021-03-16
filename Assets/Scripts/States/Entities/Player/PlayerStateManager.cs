
using UnityEngine;

public class PlayerStateManager : EntityStateManager
{
    private PlayerIdleState _idleState;
    private PlayerWalkState _walkState;
    private PlayerShiftState _shiftState;
    private PlayerSwingSwordState _swingSwordState;

    public PlayerIdleState IdleState { get => _idleState; }
    public PlayerWalkState WalkState { get => _walkState; }
    public PlayerShiftState ShiftState { get => _shiftState; }
    public PlayerSwingSwordState SwingSwordState { get => _swingSwordState; }

    private void Awake()
    {
        _idleState = new PlayerIdleState(this);
        _walkState = new PlayerWalkState(this);
        _shiftState = new PlayerShiftState(this);
        _swingSwordState = new PlayerSwingSwordState(this);
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
        else if (_currentState != SwingSwordState && Input.GetKeyDown(KeyCode.Space)){
            TransitionToState(SwingSwordState);
        }

        base.Update();
    }

    // Triggerd by the Sword Hitbox collider (player's child gameobject)
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerEnterd Sword");
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemey!");
        }
    }

}
