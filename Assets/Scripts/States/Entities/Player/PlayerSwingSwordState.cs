using UnityEngine;

public class PlayerSwingSwordState : PlayerBaseState
{
    //private BoxCollider2D _hitBox;
    private float _elapsedTime;
    private readonly float _animDuration = 0.33f;

    public PlayerSwingSwordState(EntityStateManager entity) : base(entity) { }

    public override void EnterState()
    {
        _elapsedTime = 0f;
        player.Animator.SetTrigger("SwingSword");
    }

    public override void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= _animDuration)
            player.TransitionToState(player.IdleState);

    }
}
