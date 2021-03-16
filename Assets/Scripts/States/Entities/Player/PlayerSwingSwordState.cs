using UnityEngine;

public class PlayerSwingSwordState : BaseState
{
    private PlayerStateManager _player;
    //private BoxCollider2D _hitBox;
    private float _elapsedTime;
    private readonly float _animDuration = 0.33f;

    public PlayerSwingSwordState(EntityStateManager player)
    {
        _player = (PlayerStateManager)player;
        //_hitBox = _player.GetComponentInChildren<BoxCollider2D>();
    }

    public override void EnterState()
    {
        _elapsedTime = 0f;
        _player.Animator.SetTrigger("SwingSword");
    }


    public override void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _animDuration)
            _player.TransitionToState(_player.IdleState);

    }
}
